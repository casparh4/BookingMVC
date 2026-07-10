using BookingMVC.Models;

namespace BookingMVC.ViewModels
{
    public class BookingsViewModel
    {
        public IEnumerable<Booking>? Bookings { get; }
        public Hotel Hotel { get; }
        public BookingsViewModel(IEnumerable<Booking>? bookings, Hotel hotel)
        {
            Bookings = bookings;
            Hotel = hotel;
        }
    }
}
