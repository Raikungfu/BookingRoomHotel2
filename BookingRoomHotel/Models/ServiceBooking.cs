using System.ComponentModel.DataAnnotations;
namespace BookingRoomHotel.Models
{
	public class ServiceBooking
	{
		[Key]
		public int ServiceBookingID { get; set; }

		public int BookingID { get; set; }
		public int ServiceID { get; set; }

		public int Quantity { get; set; }
		public int Subtotal { get; set; }
		public virtual Booking Booking { get; set; }
		public virtual Service Service { get; set; }
	}
}
