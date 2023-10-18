using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingRoomHotel.Models
{
    public class CustomerNotification
    {
        [Key]
        public int CustomerNotificationId { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public string Id { get; set; }
        public bool IsRead { get; set; }

        [ForeignKey("Id")]
        public Customer Customer { get; set; }
    }


}
