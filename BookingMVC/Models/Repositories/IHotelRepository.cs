using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace BookingMVC.Models.Repositories
{
    public interface IHotelRepository
    {
        
        IEnumerable<Hotel> GetAllHotels();
        Hotel? GetHotelById(int id);
       
        IEnumerable<Hotel> FilterHotels(Search search);

        void CreateReview(int Id,Review review);

        public Task<int> AddHotelAsync(Hotel hotelToAdd);
        public Task<int> DeleteHotelAsync(int hotelId);

        Task<int> UpdateReviewSummary(int id);

        string GetReviewsStringByHotelName(string hotelName);

        Hotel? GetHotelByName(string hotelName);
    }
}
