using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Hotel
	{
		[Key]
		public int HotelID { get; set; }

		[Required]
		public string Name { get; set; }

		public string Address { get; set; }
		public string Description { get; set; }
		[Phone]
		public string PhoneNumber { get; set; }
		[Range(1, 5)]
		public int Rating { get; set; }
		public virtual ICollection<Room> Rooms { get; set; }
	}
}
