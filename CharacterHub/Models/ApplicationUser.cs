using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace CharacterHub.Models
{
	public class ApplicationUser : IdentityUser
	{
		[Required]
		public string Name { get; set; }
        public ICollection<UserNotification> UserNotifications { get; set; }

        public ApplicationUser()
        {
            UserNotifications = new Collection<UserNotification>();
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
		{
			// Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
			var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
			// Add custom user claims here
			return userIdentity;
		}

        public void Notify(Notification notification, Character c)
        {
            var userNotification = new UserNotification(this.Name, notification, c) { };

            UserNotifications.Add(userNotification);
        }
    }
}