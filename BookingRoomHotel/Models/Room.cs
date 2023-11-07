using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Room
	{
		[Key]
		public int RoomID { get; set; }

		public int? HotelID { get; set; }
		public int? RoomTypeID { get; set; }

		public int RoomNumber { get; set; }
		public string? RoomImage { get; set; }

		[Range(0, double.MaxValue)]
		public int Price { get; set; }

		public string Describe { get; set; }
		public virtual Hotel? Hotel { get; set; }
		public virtual ICollection<Booking>? Bookings { get; set; }

		public virtual RoomType? RoomType { get; set; }
	}
}
