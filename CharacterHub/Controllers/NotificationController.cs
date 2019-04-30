using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using CharacterHub.Models;
using CharacterHub.Controllers;
using CharacterHub.Dtos;
using CharacterHub.ViewModels;
using Microsoft.AspNet.Identity;

namespace CharacterHub.Controllers
{
	public class NotificationController : ApiController, IEqualityComparer<UserNotification>
	{
		private readonly CharacterContext _ctx;
		public NotificationController()
		{
			_ctx = new CharacterContext();
		}

		public List<UserNotification> GetUserNotifications()
		{
			List<UserNotification> notifyList = null;
            if (_ctx.Characters.Any())
            {
                var usernotifications = _ctx.UserNotifications
                    .Include(c => c.Character)
                    .Include(c => c.User)
                    .Include(c => c.Notification)
                    .ToList();

                if (usernotifications.Any())
                {
                    notifyList = usernotifications.Distinct(new NotificationController()).Where(n => n.IsRead != true).ToList();
                }
            }
			

			return notifyList;
		}

		public bool Equals(UserNotification x, UserNotification y)
		{
			if (Object.ReferenceEquals(x, y)) return true;
			if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
				return false;

			return x.NotificationId == y.NotificationId;
		}

		public int GetHashCode(UserNotification obj)
		{
			return obj.NotificationId.GetHashCode();
		}
        [Route("api/notification")]
        [HttpPost]
        public IHttpActionResult ClickedOn(NotificationsDto dto)
        {           
            var notification = _ctx.Notifications.FirstOrDefault(n => n.Id == dto.NotificationId);
            var character = _ctx.Characters.FirstOrDefault(c => c.Id == dto.CharacterId);
            //var userId = character?.UserId;
            var userId = User.Identity.GetUserId();
            var notificationList = new UserNotification(userId, notification, character, "True")
            {
                CharacterId = dto.CharacterId,
                NotificationId = dto.NotificationId,
                IsRead = dto.IsRead
            };
            var updatedNotification = _ctx.UserNotifications.FirstOrDefault(n => n.CharacterId == dto.CharacterId);
            if (updatedNotification != null)
            {
                updatedNotification.IsRead = Convert.ToBoolean("True");
            }
            _ctx.SaveChanges();

            return Ok(new
            {
                data = notificationList
            });
        }
    }
}