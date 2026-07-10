using BookingMVC.Models.POCOs;
using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Models.Repositories
{
    public interface IBookingRepository
    {
        Hotel? Hotel { get; set; }
     
        public int CreateBooking(Booking booking);

        Task<Booking?> GetBookingAsync(int id, CancellationToken token);

        Task<IEnumerable<Booking>> GetBookingsAsync(int hotelId);

        IEnumerable<Booking> GetAllBookings();
        Task<bool> HotelIsAvailable( Booking newBooking);



    }
}

