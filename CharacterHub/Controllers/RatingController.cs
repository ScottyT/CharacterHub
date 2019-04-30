using CharacterHub.Dtos;
using CharacterHub.Models;
using CharacterHub.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace CharacterHub.Controllers
{
	public class RatingController : Controller
	{
		
		private readonly CharacterContext _ctx;
		public RatingController()
		{
			_ctx = new CharacterContext();
		}

	    public JsonResult UpdateAverage(CharacterDto dto)
	    {
	        var characters = _ctx.Characters.ToList();
	        var character = characters.FirstOrDefault(c => c.Id == dto.Id);
	        if (character != null)
	        {
	            var average = _ctx.CharacterRatings.GroupBy(c => c.Id)
	                .Select(x => new
	                {
	                    GroupName = x.Key,
	                    GroupAverage = (double)(x.Sum(y => y.RatingNumber)) / x.Count()
	                }).ToList();
	            character.AverageRating = average.Single(x => x.GroupName == dto.Id).GroupAverage;
	        }

	        _ctx.SaveChanges();
	        return Json(new
	        {
	            data = character
	        });
	    }

        public JsonResult GetCharacters()
		{
		    var characters = _ctx.Characters.ToList();
           
		    //var character = characters.FirstOrDefault(c => c.Id);
		    if (!characters.Any())
            {
		        var average = _ctx.CharacterRatings.GroupBy(c => c.Id)
		            .Select(x => new
		            {
		                GroupName = x.Key,
		                GroupAverage = (double) (x.Sum(y => y.RatingNumber)) / x.Count()
		            }).ToList();
                foreach (var y in characters)
                {
                    y.AverageRating = average.Single(x => x.GroupName == y.Id).GroupAverage;
                }
                //character.AverageRating = average.Single(c => c.GroupName == id).GroupAverage;
		    }

		    _ctx.SaveChanges();
			return Json(new
			{
				data = characters
			}, JsonRequestBehavior.AllowGet);

		}


		//[HttpGet]
		//public JsonResult GetCharacterRatings()
		//{
		//	var characterRatings = _ctx.CharacterRatings.ToList();
		//	return Json(new
		//	{
		//		data = characterRatings
		//	}, JsonRequestBehavior.AllowGet);
		//}

		//public JsonResult RatingAdded(RatingViewModel model)
		//{
		//	var user = User.Identity.GetUserId();

		//	var characterRatings = new CharacterRating
		//	{
		//		RatingNumber = model.Rating,
		//		CharacterId = model.CharacterId,
		//		UserId = user,
		//		RatingTitle = model.RatingTitle
		//	};
		//	_ctx.CharacterRatings.Add(characterRatings);
		//	_ctx.SaveChanges();

			
		//	return Json(new
		//	{
		//		message = "Added Favorite!",
		//		data = characterRatings
		//	}, JsonRequestBehavior.AllowGet);
		//}
	}
}