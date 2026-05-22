using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Models
{
    public interface IBookingRepository
    {
        Hotel? Hotel { get; set; }
     
        public void CreateBooking(Booking booking);

        Booking? GetBooking(int id);

        Task<IEnumerable<Booking>> GetBookingsAsync(int hotelId);

        IEnumerable<Booking> GetAllBookings();
    }
}
