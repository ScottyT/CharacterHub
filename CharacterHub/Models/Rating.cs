using System.ComponentModel.DataAnnotations;

namespace CharacterHub.Models
{
	public class Rating
	{
		public byte Id { get; set; }
		public double RateNumber { get; set; }

		
		[StringLength(255)]
		public string RatingTitle { get; set; }
	}
}