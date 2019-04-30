using CharacterHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharacterHub.ViewModels
{
	public class RatingViewModel
	{
		public int CharacterId { get; set; }
		public int Rating { get; set; }
		public string RatingTitle { get; set; }
	}
}