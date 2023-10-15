using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Rating
	{
		[Key]
		public int RatingID { get; set; }

		[Range(1, 5)]
		public int Stars { get; set; }

		[Required]
		[StringLength(500)]
		public string Comment { get; set; }
		public int BookingId { get; set; }
		public int HotelID { get; set; }
		public int CustomerID { get; set; }

		public DateTime DateCreated { get; set; }
	}

}
