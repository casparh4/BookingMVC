using BookingMVC.Models;
using BookingMVC.Models.POCOs;

namespace BookingMVC.ViewModels
{
    public class FlightViewModel
    {
       

        public Booking Booking { get; set; } = default!;

        public RootObject? Flights { get; set; } 

        public int Offset { get; set; }
 
        public FlightViewModel() { }
        public FlightViewModel(Booking booking, RootObject? flights, int offset)
        {
            Flights = flights;
            Booking = booking;
            Offset = offset;
           
        }
    }
}
