using AutoMapper;
using BookingRoomHotel.Models;

namespace BookingRoomHotel.ViewModels
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<CreateRoomTypeViewModel, RoomType>();
            CreateMap<BookViewModel, Booking>();
            CreateMap<CreateRoomViewModel, Room>();
        }
    }
}
