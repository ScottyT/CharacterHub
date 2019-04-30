using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CharacterHub.Models
{
	public class CharacterRating
	{
		[Key]
		[Column(Order = 1)]
		public int Id { get; set; }
		[Column(Order = 2)]
		public int CharacterId { get; set; }
		[Column(Order = 3)]
		public string UserId { get; set; }
		[Column(Order = 4)]
		public double RatingNumber { get; set; }
		[Column(Order = 5)]
		public string RatingTitle { get; set; }
		public string Name { get; set; }
		public ApplicationUser User { get; set; }
	}
}