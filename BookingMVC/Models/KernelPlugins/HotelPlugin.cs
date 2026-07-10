using BookingMVC.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.SemanticKernel;
using System.ComponentModel;
using System.Text;

namespace BookingMVC.Models.KernelPlugins
{
    public class HotelPlugin
    {
        private readonly IHotelRepository _hotelRepository;
    public HotelPlugin(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        [KernelFunction]
        [Description("Get all hotels on the Hotel4You Website")]
        public string Hotels()
        {
            var hotels = _hotelRepository.GetAllHotels();
         
            var hotelStringBuilder = new StringBuilder();
            var reviewStringBuilder = new StringBuilder();

            foreach(var hotel in hotels)
            {
                if(hotel.Reviews!= null || hotel.Reviews.Count != 0)
                {
                    foreach (var review in hotel.Reviews)
                    {
                        reviewStringBuilder.Append($"rating: {review.Rating}, comments:{review.Comments}");
                    }
                    hotelStringBuilder.AppendLine($"""hotel name: {hotel.Name}, hotel ID: {hotel.HotelId} price per night: {hotel.Price}, rating: {hotel.Rating}, location:{hotel.Country} {hotel.City}, Description: {hotel.Description}, customer reviews: {reviewStringBuilder.ToString()}""");

                }
                else
                {
                    hotelStringBuilder.AppendLine($"""hotel name: {hotel.Name}, hotel ID: {hotel.HotelId} price per night: {hotel.Price}, rating: {hotel.Rating}, location:{hotel.Country} {hotel.City}, Description: {hotel.Description}""");

                }

            }
            return hotelStringBuilder.ToString();
        }
    }
}
