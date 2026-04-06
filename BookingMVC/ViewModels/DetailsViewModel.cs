using BookingMVC.Models;

namespace BookingMVC.ViewModels
{
    public class DetailsViewModel
    {
        public Hotel Hotel { get; }
        
        public Review NewReview { get; set; }


        public DetailsViewModel(Hotel hotel, Review review)
        {
            Hotel = hotel;
            NewReview = review;
            
        }
    }
}
