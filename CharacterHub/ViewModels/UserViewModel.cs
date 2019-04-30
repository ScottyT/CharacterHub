using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CharacterHub.Models;

namespace CharacterHub.ViewModels
{
	public class UserViewModel
	{
		public string UserId { get; set; }
		public string Username { get; set; }
		public string Email { get; set; }
	}
}