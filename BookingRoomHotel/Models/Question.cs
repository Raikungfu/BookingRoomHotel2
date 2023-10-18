using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
    public class Question
    {
        [Key] 
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your name")]
        public string Name { get; set; }
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your email")]
        public string Email { get; set; }
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your subject")]
        [StringLength(50, ErrorMessage = "Less than 50 characters")]
        public string Subject { get; set; }
        [DataType(DataType.Text)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your message")]
        [StringLength(300, ErrorMessage = "Less than 300 characters")]
        public string Message { get; set; }
        [DataType(DataType.Text)]
        public string? Response { get; set; }
        [DataType(DataType.Text)]
        [StringLength(10, ErrorMessage = "Error Status")]
        public string? Status { get; set; }
    }
}
