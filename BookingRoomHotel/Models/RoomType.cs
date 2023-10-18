using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{

	public class RoomType
	{
		[Key]
		public int RoomTypeID { get; set; }

		[Required]
		public string TypeName { get; set; }

        [Required]
        public int Price { get; set; }
        [Required]
        public int Max { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public string View { get; set; }
        [Required]
        public int Bed { get; set; }
        [Required]
        public string Description1 { get; set; }
		public string? Description2 { get; set; }
		public string? Description3 { get; set; }
		public virtual ICollection<Room>? Rooms { get; set; }
		public virtual ICollection<Media>? Media { get; set; }
	}
}
