using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
    public class Staff
    {
        [Key]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your username")]
        [StringLength(maximumLength: 20, MinimumLength = 5, ErrorMessage = "Length between 5 - 20")]
        public string Id { get; set; }
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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your address")]
        public string Address { get; set; }
        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your password")]
        public string Pw { get; set; }
        [StringLength(maximumLength: 20, ErrorMessage = "Max Length 20")]
        public string Role { get; set; }
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "Error Status")]
        public string Status { get; set; }
    }
}