using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingRoomHotel.Models
{
    public class StaffNotification
    {
        [Key]
        public int StaffNotificationId { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }
        public string StaffId { get; set; }
        public string Title { get; set; }

    }

}
