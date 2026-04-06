namespace BookingMVC.Models
{
    public interface IBookingRepository
    {
        Hotel? Hotel { get; set; }
     
        public void CreateBooking(Booking booking);

        Booking? GetBooking(int id);
    }
}
