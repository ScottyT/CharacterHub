using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CharacterHub.Models
{
	public class UserNotification
	{
		[Key]
		[Column(Order = 1)]
		public string UserId { get; set; }
		[Key]
		[Column(Order = 2)]
		public int NotificationId { get; set; }
		[Column(Order = 3)]
		public string CharacterName { get; set; }
		[Column(Order = 4)]
		public int CharacterId { get; set; }
		public ApplicationUser User { get; set; }
		public Notification Notification { get; set; }
		public Character Character { get; set; }
		public bool IsRead { get; set; }
		protected UserNotification() { }

		public UserNotification(string user, Notification notification, Character c, string isread = "false")
		{
			if (user == null)
				throw new ArgumentNullException("user");
			if (notification == null)
				throw new ArgumentNullException("notification");
			if (c == null)
				throw new ArgumentNullException("c");
			UserId = user;
			Notification = notification;
            CharacterName = c.CharacterName;
			Character = c;
		    IsRead = Convert.ToBoolean(isread);
		}
	}
}