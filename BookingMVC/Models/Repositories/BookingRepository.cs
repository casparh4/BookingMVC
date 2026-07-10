using BookingMVC.Models.POCOs;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace BookingMVC.Models.Repositories
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

        public int CreateBooking(Booking booking)
        {
            booking.Hotel = Hotel;
           var days = booking.Leaving.Day - booking.Arrival.Day;
     
            if(Hotel!= null)
            {
                booking.TotalCost = days * Hotel.Price;
            }
            _dbContext.Add(booking);
            return _dbContext.SaveChanges();
        }

        public async Task<Booking?> GetBookingAsync(int id, CancellationToken token)
        {
            return await _dbContext.Bookings.Include(b=>b.Hotel).FirstOrDefaultAsync(B => B.BookingId == id, token);
        }

        public async Task<IEnumerable<Booking>?> GetBookingsAsync(int hotelId)
        {
            
            return await _dbContext.Bookings.Where(b => b.Hotel.HotelId == hotelId).ToListAsync();
        }

        public  IEnumerable<Booking> GetAllBookings()
        {
            return _dbContext.Bookings.Include(h => h.Hotel).ToList();
        }

        public async Task<bool> HotelIsAvailable(Booking newBooking)
        {
            int currentAmount = 0;
            int rooms = Hotel.Rooms;
            var currentBookings = await _dbContext.Bookings.Where(b=> b.Hotel.HotelId == Hotel.HotelId).ToListAsync();
            
            try
            {
                var parallelLoop = Parallel.ForEach(currentBookings, (booking, state) =>
                {
                    if (booking.Leaving > newBooking.Arrival && booking.Arrival < newBooking.Leaving)
                    {
                        Interlocked.Increment(ref currentAmount);
                        if (currentAmount >= rooms)
                        {
                            state.Stop();
                            return;
                        }
                        
                    }
                });
                return parallelLoop.IsCompleted; 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            //added room capacity for hotel, if this bool is true, add one to count and if overtakes capacity, return false
        }

    }
}
