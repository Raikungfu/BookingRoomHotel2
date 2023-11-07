using BookingRoomHotel.Models;
using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.ViewModels
{
    public class RoomViewModel
    {
        public List<Room> ListRoom { get; set; }
        public int Count { get; set; }
    }
    public class SearchListRoomViewModel
    {
        public RoomViewModel roomViewModel { get; set; }
        public SearchRoomViewModel searchRoomViewModel { get; set; }
        
    }
    public class CreateRoomViewModel
    {
        public int? RoomID { get; set; }
        [Required]
        public int? RoomTypeID { get; set; }
        [Required]
        public int? HotelID { get; set; }
        [Required]
        public int Bed { get; set; }
        [Required]
        public int RoomNumber { get; set; }
        [Required]
        [Range(0, double.MaxValue)]
        public int Price { get; set; }
        [Required]
        public string Describe { get; set; }
        public IFormFile? Image { get; set; }
    }
}
