using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.ViewModels
{
    public class ProfileCustomerViewModel
    {
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name")]
        [StringLength(maximumLength: 30, MinimumLength = 6, ErrorMessage = "Length between 6 - 50")]
        public string Name { get; set; }

        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email")]
        public string Email { get; set; }

        [Phone]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your phone")]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your BirthDay")]
        public DateTime DateOfBirth { get; set; }

        [DataType(DataType.Text)]
        public string Address { get; set; }

        public IFormFile ImgAvt { get; set; }
        public IFormFile ImgIdentify1 { get; set; }
        public IFormFile ImgIdentify2 { get; set; }
    }

    public class ProfileView
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string ImgAvt { get; set; }
        public string ImgIdentify1 { get; set; }
        public string ImgIdentify2 { get; set; }
        public string Status { get; set; }
    }
}