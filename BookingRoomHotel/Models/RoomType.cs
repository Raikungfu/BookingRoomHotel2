using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{

	public class RoomType
	{
		[Key]
		public int RoomTypeID { get; set; }

		[Required]
		public string TypeName { get; set; }

		public string Description { get; set; }

		public virtual ICollection<Room> Rooms { get; set; }
	}
}
