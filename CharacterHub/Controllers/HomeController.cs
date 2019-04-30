using CharacterHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using CharacterHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace CharacterHub.Controllers
{
	public class HomeController : Controller
	{
		private readonly CharacterContext _ctx;
		public HomeController()
		{
			_ctx = new CharacterContext();
		}
 
        public ActionResult Index()
        {
           
            CharacterController c = new CharacterController();
            var user = User.Identity.GetUserId();
            var characters = _ctx.Characters.ToList();
            var characterRatings = _ctx.CharacterRatings
                .ToList();

            foreach (var character in characters)
            {
                c.UpdateAverage(character, characterRatings, characters);
                if (character.AverageRating <= 0 && characterRatings.All(x => x.CharacterId != character.Id))
                {

                }
                  
            }

            _ctx.SaveChanges();

            var viewModels = new CharacterViewModel()
            {
                CharacterRatings = characterRatings,
                Characters = characters,
                Ratings = _ctx.Ratings.ToList(),
                ShowActions = User.Identity.IsAuthenticated
            };

            return View(viewModels);
        }

        //[HttpPost]
        //public ActionResult Index(CharacterViewModel viewModel)
        //{
        //    var result = Submit(viewModel.CharacterForm);
        //    if (result)
        //    {
        //        ViewBag.SuccessMessage = "Your character was sucessfully added!";
        //        ViewBag.Success = true;
        //        return View("Default");
        //    }

        //    ViewBag.Success = false;
        //    ViewBag.ErrorMessage = "Something went wrong.";
        //    return View(viewModel);
        //}

        public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

        [Authorize]
		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		private bool Submit(CharacterFormViewModel vm)
		{
			if (!ModelState.IsValid)
			{
				return false;
			}
			return true;
		}
	}
}