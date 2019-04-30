using CharacterHub.Dtos;
using CharacterHub.Models;
using CharacterHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace CharacterHub.Controllers
{
    public class CharactersController : ApiController
    {
        private readonly CharacterContext _ctx;
        public CharacterController _c;
        public CharactersController()
        {
            _ctx = new CharacterContext();
            _c = new CharacterController();
        }
        private static readonly Expression<Func<Character, CharacterDto>> AsCharacterDto =
            c => new CharacterDto
            {
                Id = c.Id,
                CharacterName = c.CharacterName,
                AverageRating = c.AverageRating
            };
        [HttpGet]
        public IQueryable<CharacterDto> GetAllCharacters()
        {
            // These use user id. 
            //var user = User.Identity.GetUserId();

            //var characters = _ctx.CharacterRatings.Where(c => c.UserId == user).ToList();
            var characters = _ctx.Characters.Select(AsCharacterDto);
            return characters;
        }

        [Route("api/characters/{id}")]
        [HttpGet]
        [ResponseType(typeof(CharacterDetailDto))]
        public IHttpActionResult GetCharacter(int id)
        {
            var character = _ctx.Characters.Select(c => new CharacterDetailDto()
            {
                Id = c.Id,
                CharacterName = c.CharacterName,
                FirstAppearance = c.FirstAppearance,
                Likes = c.Likes,
                Dislikes = c.Dislikes
            }).SingleOrDefault(c => c.Id == id);
            if (character == null)
            {
                return NotFound();
            }
            return Ok(character);
        }
        [Route("api/favorites")]
        [HttpGet]
        public IEnumerable<CharacterRating> GetFavCharacters()
        {
            var user = User.Identity.GetUserId();
            var characters = _ctx.CharacterRatings.Where(c => c.RatingNumber > 3 && c.UserId == user).ToList();
            return characters;
        }

        [Route("api/rating")]
        [HttpPost]
        public IHttpActionResult PostRating(CharacterDto dto)
        {
            var user = User.Identity.GetUserId();
            var favCharacters = _ctx.CharacterRatings.Where(c => c.RatingNumber > 3).ToList();

            var message = new MessageViewModel();
            var characters = _ctx.Characters.ToList();

            var currentCharacter = _ctx.Characters.Single(c => c.Id == dto.Id);

            var rating = new CharacterRating
            {
                CharacterId = dto.Id,
                RatingNumber = dto.RatingNumber,
                RatingTitle = dto.RatingTitle,
                UserId = user,
                Name = dto.CharacterName
            };

            using (var dbContexttransaction = _ctx.Database.BeginTransaction())
            {
                try
                {
                    // Saves the updated or new rating
                    var updateRatings =
                        _ctx.CharacterRatings.FirstOrDefault(c => c.CharacterId == dto.Id && c.UserId == user);
                    if (updateRatings != null)
                    {
                        message.IsSuccess = true;
                        updateRatings.RatingNumber = dto.RatingNumber;
                        updateRatings.RatingTitle = dto.RatingTitle;


                        if (dto.RatingNumber > 3 && favCharacters.Any(c => c.CharacterId == rating.CharacterId))
                        {
                            message.IsSuccess = false;
                            message.Message = "You already favorited this character!";
                        }
                    }

                    if (!_ctx.CharacterRatings.Any(c => c.CharacterId == dto.Id && c.UserId == user))
                    {
                        message.IsSuccess = true;
                        _ctx.CharacterRatings.Add(rating);
                    }

                    _ctx.SaveChanges();

                    // Updates the average rating
                    var updateRatingsList = _ctx.CharacterRatings.ToList();
                    _c.UpdateAverage(currentCharacter, updateRatingsList, characters);
                    _ctx.SaveChanges();

                    dbContexttransaction.Commit();
                }
                catch (Exception ex)
                {
                    var filePath = "~/App_Data/Error.txt";
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        writer.WriteLine("------");
                        writer.WriteLine("Date: " + DateTime.Now.ToString());
                        writer.WriteLine();
                        while (ex != null)
                        {
                            writer.WriteLine(ex.GetType().FullName);
                            writer.WriteLine("Message: " + ex.Message);
                            writer.WriteLine("StackTrace :" + ex.StackTrace);

                            ex = ex.InnerException;
                        }
                    }
                }
            }

            dto.AverageRating = currentCharacter.AverageRating;
          
            return Ok(new
            {
                characters = dto,
                messages = message
            });
        }
    }
}
