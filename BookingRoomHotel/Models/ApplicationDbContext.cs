using Microsoft.EntityFrameworkCore;

namespace BookingRoomHotel.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApplicationDbContext()
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }

        public DbSet<Question> Question { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Media> Media { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Service> Service { get; set; }
        public DbSet<ServiceBooking> ServiceBooking { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            /*if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("DefaultConnection");
            }*/
        }
    }
}