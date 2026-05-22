
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Conversations;
using System.Runtime.CompilerServices;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BookingMVC.Models
{
    public class HotelRepository : IHotelRepository
    {
        private readonly BookingDbContext _DbContext;
        private readonly OpenAIClient _aiClient;
        private readonly IOptions<ModelSettings> _modelSettings;


        public HotelRepository(BookingDbContext dbContext, OpenAIClient aiClient, IOptions<ModelSettings> modelSettings)
        {
            _DbContext = dbContext;
            _aiClient = aiClient;
            _modelSettings = modelSettings;

        }

        public IEnumerable<Hotel> GetAllHotels()
        {
            return _DbContext.Hotels;
        }

        public string GetReviewsStringByHotelName(string hotelName)
        {
            var hotel = _DbContext.Hotels.Include(h => h.Reviews).FirstOrDefault(h => h.Name == hotelName);
            if (hotel==null)
            {
                throw new Exception($"Hotel could not be found by name: {hotelName}");
            }

            StringBuilder sb = new StringBuilder();

            foreach(var review in hotel.Reviews)
            {
                sb.AppendLine($"reviewer: {review.FirstName} {review.LastName}, comments {review.Comments}, rating: {review.Rating}");
            }

            return sb.ToString();

        }
        public Hotel? GetHotelById(int id)
        {
            return _DbContext.Hotels.Include(H => H.Reviews).FirstOrDefault(H => H.HotelId == id);

            
        }

        

        

        public IEnumerable<Hotel> FilterHotels(Search search)
        {
            var hotels = _DbContext.Hotels.AsQueryable(); //asQuariable lets you buld the query step by step
            if (!string.IsNullOrEmpty(search.Name))
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
            if (search.Rating != null)
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
                if (review.Hotel.Reviews != null)
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

        public async Task<int> AddHotelAsync(Hotel hotelToAdd)
        {

            if (hotelToAdd != null)
            {
                await _DbContext.Hotels.AddAsync(hotelToAdd);
                return await _DbContext.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Hotel could not be added");
            }

        }

        public async Task<int> DeleteHotelAsync(int hotelId)
        {
            var hotelToDelete = await _DbContext.Hotels.FirstOrDefaultAsync(h => h.HotelId == hotelId);

            if (hotelToDelete == null)
            {
                throw new Exception("Invalid hotel ID or hotel has already been deleted");
            }
            else
            {
                _DbContext.Hotels.Remove(hotelToDelete); //That’s just in-memory state change. No I/ O.No waiting.No async needed.
                return await _DbContext.SaveChangesAsync();
            }
        }

        public async Task<int> UpdateReviewSummary(int id) //add a bool ot he detailsviewmodel to do onclick this metho
        {
            //store review count somewhere here so AI only updates summary when reviews are added to
            ChatClient chatClient = _aiClient.GetChatClient(_modelSettings.Value.TextModelName);

            var hotel = await _DbContext.Hotels.Include(h=>h.Reviews).FirstOrDefaultAsync(h => h.HotelId == id);

            var Reviews = hotel.Reviews;

            if (Reviews == null || Reviews.Count == 0)
            {
                hotel.ReviewSummary = "No Reviews";
                return await _DbContext.SaveChangesAsync();
            }
            else
            {
                StringBuilder Comments = new StringBuilder(); //change for a stringbuilder
                foreach (Review review in Reviews)
                {
                    string comment = $"Comments: {review.Comments}. Rating: {review.Rating}";
                    Comments.AppendLine(comment);

                }

                string prompt = $"""Summarise these reviews for this hotel in one or two sentences {Comments} """; //using RAG

                ChatCompletion completion = await chatClient.CompleteChatAsync(prompt);

                var updatedSummary = completion.Content[0].Text;

                hotel.ReviewSummary = updatedSummary;

                return await _DbContext.SaveChangesAsync();

            }

            
        }

        public Hotel? GetHotelByName(string hotelName)
        {
            return _DbContext.Hotels.FirstOrDefault(h => h.Name == hotelName);
        }
    }
}
