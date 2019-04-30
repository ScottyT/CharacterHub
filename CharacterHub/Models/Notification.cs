using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CharacterHub.Models
{
	public class Notification
	{
        [Key]
        [Column(Order = 1)]
		public int Id { get; set; }
        [Column(Order = 2)]
		public DateTime DateNotified { get; set; }
        [Column(Order = 3)]
		public NotificationType Type { get; set; }
		public Character Character { get; set; }
	}
}