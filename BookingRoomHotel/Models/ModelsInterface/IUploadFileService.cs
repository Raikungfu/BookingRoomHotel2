namespace BookingRoomHotel.Models
{
    public interface IUploadFileService
    {
        public string uploadImage(IFormFile url, string path);
        List<Media> uploadListImage(IFormFileCollection images, string path);
    }
}
