using CharacterHub.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharacterHub.Dtos
{
	public class CharacterDto
	{
		public int Id { get; set; }
		public double RatingNumber { get; set; }
        public double AverageRating { get; set; }
		public string RatingTitle { get; set; }
		public string CharacterName { get; set; }        
		public string User { get; set; }
	}
}