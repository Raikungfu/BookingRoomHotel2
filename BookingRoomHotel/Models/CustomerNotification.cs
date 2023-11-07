using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.Models
{
    public class CustomerNotification
    {
        [Key]
        public int CustomerNotificationId { get; set; }

        [Required]
        public string Content { get; set; }
        public string CustomerId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
    }
}
