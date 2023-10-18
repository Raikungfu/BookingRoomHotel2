using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Media
	{
		[Key]
		public int MediaID { get; set; }
		public int? RoomTypeID { get; set; }

		[Required]
		public string URL { get; set; }

		public string For { get; set; }
		public string? Title { get; set; }
		[Required]
		public string? Type { get; set; }
	}

}
