using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CharacterHub.Models
{
	public class Character
	{
        [Key]
		public int Id { get; set; }
		[Required]
		public string CharacterName { get; set; }
		public string FirstAppearance { get; set; }
		public string Likes { get; set; }
		public string Dislikes { get; set; }
		public ApplicationUser User { get; set; }
		public string UserId { get; set; }
        public double AverageRating { get; set; }
        public byte[] ImageData { get; set; }
	}
}