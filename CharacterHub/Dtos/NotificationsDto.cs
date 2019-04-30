using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CharacterHub.Models;

namespace CharacterHub.Dtos
{
    public class NotificationsDto
    {
        public string UserId { get; set; }
        public int NotificationId { get; set; }
        public int CharacterId { get; set; }
        public bool IsRead { get; set; }
    }
}