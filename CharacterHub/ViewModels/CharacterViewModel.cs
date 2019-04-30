using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CharacterHub.Models;

namespace CharacterHub.ViewModels
{
	public class CharacterViewModel
	{
		public IEnumerable<Character> Characters { get; set; }
		public IEnumerable<Rating> Ratings { get; set; }
		public IEnumerable<CharacterRating> CharacterRatings { get; set; }
		public IEnumerable<UserNotification> UserNotifications { get; set; }
        public Character Character { get; set; }
        public UserNotification UserNotification { get; set; }
        public int RatingAverage { get; set; }
		public bool ShowActions { get; set; }
        public string Message { get; set; }
	}
}