using Microsoft.EntityFrameworkCore;

namespace BookingMVC.Models
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly BookingDbContext _dbContext;

        public Hotel? Hotel { get; set; }
        public BookingRepository(IHotelRepository hotelRepository, BookingDbContext dbContext)
        {
            _hotelRepository = hotelRepository;
            _dbContext = dbContext;
        }

        public void CreateBooking(Booking booking)
        {
           var days = booking.Leaving.Day - booking.Arrival.Day;
            booking.Hotel = Hotel;
            if(Hotel!= null)
            {
                booking.TotalCost = days * Hotel.Price;
            }
            _dbContext.Add(booking);
            _dbContext.SaveChanges();
        }
        public Booking? GetBooking(int id)
        {
            return _dbContext.Bookings.Include(b=>b.Hotel).FirstOrDefault(B => B.BookingId == id);
        }


    }
}
