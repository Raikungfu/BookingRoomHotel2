using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Booking
	{
		[Key]
		public int BookingID { get; set; }
		public int RoomID { get; set; }

		[DataType(DataType.Date)]
		public DateTime CheckInDate { get; set; }

		[DataType(DataType.Date)]
		public DateTime CheckOutDate { get; set; }
		public int TotalPrice { get; set; }

		public string Status { get; set; }

		public virtual Customer Customer { get; set; }
		public virtual Room Room { get; set; }
		public virtual ICollection<ServiceBooking> ServiceBookings { get; set; }
	}
}
