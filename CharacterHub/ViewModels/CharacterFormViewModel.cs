using CharacterHub.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using CharacterHub.Controllers;

namespace CharacterHub.ViewModels
{
	public class CharacterFormViewModel
	{
		public int Id { get; set; }
		[Required]
		[Display(Name = "Character Name")]
		public string CharacterName { get; set; }
		[Required]
		[Display(Name = "First Appearance")]
		public string FirstAppearance { get; set; }
		public string Likes { get; set; }
		public string Dislikes { get; set; }
	    [DataType(DataType.Upload)]
	    public HttpPostedFileBase Image { get; set; }
        public string ImageFileUrl { get; set; }

        public string Action
		{
			get
			{
				Expression<Func<CharacterController, ActionResult>> update = c => c.Update(this);
				Expression<Func<CharacterController, ActionResult>> create = c => c.Create(this);
				var action = (Id != 0) ? update : create;
				return (action.Body as MethodCallExpression).Method.Name;
			}
		}
	}
}