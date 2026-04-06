using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BookingMVC.Models
{
    public class BookingDbContext: IdentityDbContext
    {
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) //Pass these options up to the parent class (DbContext)
        {
        }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Review> Reviews { get; set; }
    }
}
