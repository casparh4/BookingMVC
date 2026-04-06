
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingMVC.Models
{
    public class HotelRepository: IHotelRepository
    {
        private readonly BookingDbContext _DbContext;

        
        public HotelRepository(BookingDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _DbContext.Hotels;
        }

        public Hotel? GetHotelById(int id) 
        {
            return _DbContext.Hotels.Include(H=>H.Reviews).FirstOrDefault(H => H.HotelId == id);
        }


       
        public IEnumerable<Hotel> FilterHotels(Search search)
        {
            var hotels = _DbContext.Hotels.AsQueryable(); //asQuariable lets you buld the query step by step
            if(!string.IsNullOrEmpty(search.Name))
            {
                hotels = hotels.Where(h => h.Name.Contains(search.Name));
            }
            if (!string.IsNullOrEmpty(search.Country))
            {
                hotels = hotels.Where(h => h.Country.Contains(search.Country));
            }
            if (!string.IsNullOrEmpty(search.City))
            {
                hotels = hotels.Where(h => h.City.Contains(search.City));
            }
            if(search.Rating != null)
            {
                hotels = hotels.Where(h => h.Rating == search.Rating);
            }
            return hotels.ToList();
        }


        public void CreateReview(int Id, Review review)
        {
            review.HotelId = Id;
            review.Hotel = GetHotelById(Id);
            
           
            _DbContext.Reviews.Add(review);

            _DbContext.SaveChanges();



            
            
            if (review.Hotel != null)
            {
                var hotel = review.Hotel;
                if(review.Hotel.Reviews != null)
                {
                    decimal reviewAmount = hotel.Reviews.Count();
                    decimal ratingsSum = hotel.Reviews.Sum(r => r.Rating);
                    var newRating = ratingsSum / reviewAmount;
                    var roundedRating = Math.Round(newRating, 2);
                    hotel.Rating = roundedRating;
                    _DbContext.Hotels.Update(hotel);
                    _DbContext.SaveChanges();
                }
            }
            
        }

    }
}
