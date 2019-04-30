using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CharacterHub.Models;
using CharacterHub.ViewModels;

namespace CharacterHub.Controllers
{
	public class UsersController : Controller
	{
		private readonly CharacterContext _ctx;

		public UsersController()
		{
			_ctx = new CharacterContext();
		}

		public ActionResult Index()
		{
			var usersWithRoles = _ctx.Users.Select(user => new UserViewModel
			{
				UserId = user.Id,
				Username = user.Name,
				Email = user.Email
			}).ToList();
			return View(usersWithRoles);
		}
	}
}