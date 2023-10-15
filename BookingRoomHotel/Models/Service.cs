using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
	public class Service
	{
		[Key]
		public int ServiceID { get; set; }

		[Required]
		public string ServiceName { get; set; }
		public string Description { get; set; }
		[Range(0, double.MaxValue)]
		public int Price { get; set; }

		public virtual ICollection<Room> Rooms { get; set; }
	}
}
