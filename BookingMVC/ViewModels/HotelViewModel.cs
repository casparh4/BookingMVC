using BookingMVC.Models;

namespace BookingMVC.ViewModels
{
    public class HotelViewModel
    {
        public IEnumerable<Hotel> Hotels { get; } //read only property

      
        public HotelViewModel(IEnumerable<Hotel> hotels)
        {
            Hotels = hotels;
        }
    }
}
