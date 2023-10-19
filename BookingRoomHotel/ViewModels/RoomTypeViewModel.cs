using BookingRoomHotel.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.ViewModels
{
	public class ListRoomTypeViewModel
	{
		public List<RoomType> ListRoomTypes { get; set; }
		public int Count { get; set; }
	}

    public class RoomTypeDetail
    {
        public List<Media> ListMedia { get; set; }
        public RoomType RoomType { get; set; }
    }

	public class CreateRoomTypeViewModel
	{
        [Required]
        public string TypeName { get; set; }
        [Required]
        public int Bed { get; set; }
        [Required]
        public int Max { get; set; }
        [Required]
        public string View { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int Price { get; set; }
        [Required]
        public string Description1 { get; set; }
        public string? Description2 { get; set; }
		public string? Description3 { get; set; }
		public IFormFileCollection Images { get; set; }
        public string? VideoUrl { get; set; }
	}
}
