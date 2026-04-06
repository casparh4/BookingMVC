using Microsoft.EntityFrameworkCore;

namespace BookingMVC.Models
{
    public interface IHotelRepository
    {
        
        IEnumerable<Hotel> GetAllHotels();
        Hotel? GetHotelById(int id);
       
        IEnumerable<Hotel> FilterHotels(Search search);

        void CreateReview(int Id,Review review);
    }
}
