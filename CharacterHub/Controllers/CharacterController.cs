using CharacterHub.Models;
using CharacterHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Net;
using CharacterHub.Dtos;

namespace CharacterHub.Controllers
{
	public class CharacterController : Controller
	{
		private readonly CharacterContext _ctx;
		public CharacterController()
		{
			_ctx = new CharacterContext();
		}
		[Authorize]
		public ActionResult Mine()
		{
			// A user's characters that they rated.  
			// Will have two lists for favorite and non-favorite. Favorite is >= 4  Non-favorite is <= 3
			// Each character will have a favorite button next to them.

			var user = User.Identity.GetUserId();
			var characterRatings = _ctx.CharacterRatings.Where(c => c.UserId == user).ToList();
			var characters = _ctx.Characters.ToList();
			var viewModels = new CharacterViewModel()
			{
				CharacterRatings = characterRatings,
				Characters = characters,
				Ratings = _ctx.Ratings.ToList()
			};
			var userCharacters = characters.Where(c => c.UserId == user);

            return View("Mine", viewModels);
		}

	    public ActionResult Detail(int id)
	    {
	        var character = _ctx.Characters.FirstOrDefault(c => c.Id == id);
            var viewModel = new CharacterViewModel()
            {
                Character = character
            };
	        return View("Detail", viewModel);
	    }
	    public void UpdateAverage(Character character, List<CharacterRating> cr, List<Character> characters)
	    {
	        var characterRatings = cr.ToList();
	        double averageForCharacter = 0;
	        if (cr.Exists(c => c.CharacterId == character.Id))
	        {
	            var characterRating = characterRatings.GroupBy(c => c.CharacterId)
	                .Select(x => new
	                {
	                    GroupName = x.Key,
	                    GroupAverage = (double)(x.Sum(y => y.RatingNumber)) / x.Count()
	                }).ToList();
	            averageForCharacter = characterRating.Single(c => c.GroupName == character.Id).GroupAverage;
            }
           
	        character.AverageRating = Math.Round(averageForCharacter, 2);
        }

		[Authorize]
		public ActionResult Create()
		{
			var viewModel = new CharacterFormViewModel
			{
				
			};
			return View("CharacterForm", viewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(CharacterFormViewModel viewModel)
		{
			var userId = User.Identity.GetUserId();
			if (!ModelState.IsValid)
			{
				return View("CharacterForm", viewModel);
			}

			var users = _ctx.Users.ToList();

            var character = new Character
            {
                FirstAppearance = viewModel.FirstAppearance,
                CharacterName = viewModel.CharacterName,
                Likes = viewModel.Likes,
                Dislikes = viewModel.Dislikes,
                UserId = User.Identity.GetUserId()

            };

            //var character = new CharacterDto
            //{
            //    CharacterName = viewModel.CharacterName,
            //    FirstAppearance = viewModel.FirstAppearance,
            //    Likes = viewModel.Likes,
            //    Dislikes = viewModel.Dislikes,
            //    User = User.Identity.GetUserId()
            //};

            _ctx.Characters.Add(character);

            var notification = new Notification
            {
                Character = character,
                DateNotified = DateTime.Now,
                Type = NotificationType.CharacterAdded
            };
            foreach (var user in users)
            {
                user.Notify(notification, character);
            }

            _ctx.SaveChanges();
			
			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public ActionResult Edit(int id)
		{

			var user = User.Identity.GetUserId();
			var character = _ctx.Characters.Single(c => c.Id == id);
			var viewModel = new CharacterFormViewModel
			{
				Id = character.Id,
				CharacterName = character.CharacterName,
				FirstAppearance = character.FirstAppearance,
				Likes = character.Likes,
				Dislikes = character.Dislikes
			};
			return View("CharacterForm", viewModel);
		}

		[Authorize]
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Update(CharacterFormViewModel viewModel)
		{
			if (!ModelState.IsValid)
			{
				return View("CharacterForm", viewModel);
			}

		    HttpPostedFileBase file = Request.Files["uploadImage"];
            
			var userId = User.Identity.GetUserId();
			var character = _ctx.Characters.FirstOrDefault(c => c.Id == viewModel.Id);
		    
			if (character != null)
			{
				character.CharacterName = viewModel.CharacterName;
				character.FirstAppearance = viewModel.FirstAppearance;
				character.Likes = viewModel.Likes;
				character.Dislikes = viewModel.Dislikes;
			    if (file != null && file.ContentLength > 0)
			    {
			        //viewModel.ImageData = new byte[viewModel.Image.ContentLength];
			        var fileName = Path.GetFileName(file.FileName);
			        if (fileName != null)
			        {
                        byte[] data = new byte[] {};
			            using (var br = new BinaryReader(file.InputStream))
			            {
			                data = br.ReadBytes(file.ContentLength);
			            }			           
			            character.ImageData = data;
			        }
			    }
            }

			var userFavorites = _ctx.CharacterRatings
				.Where(c => c.RatingNumber > 3).Select(c => c.User).ToList();
            var characters = _ctx.Characters.Where(c => c.UserId == userId).Select(c => c.User).ToList();


            var notification = new Notification
            {
                DateNotified = DateTime.Now,
                Type = NotificationType.CharacterUpdated,
                Character = character
            };

            foreach (var userFav in userFavorites)
            {
                userFav.Notify(notification, character);
            }

            _ctx.SaveChanges();
			return RedirectToAction("Index", "Home");
		}
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var user = User.Identity.GetUserId();
            var character = _ctx.Characters.Single(c => c.Id == id);
            var viewModel = new CharacterViewModel
            {
                Character = character
            };
            return View("Delete", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var character = _ctx.Characters.Find(id);
            var notification = _ctx.Notifications.Single(n => n.Character.Id == id);
            var viewModel = new CharacterViewModel
            {
                Character = character
            };
            try
            {
                
                if (character == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    _ctx.Characters.Remove(character);
                    _ctx.Notifications.Remove(notification);
                    _ctx.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception ex)
            {
                return View(viewModel);
            }
            
            //_ctx.UserNotifications.Remove(viewModel.UserNotification);
            

            //return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _ctx.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}