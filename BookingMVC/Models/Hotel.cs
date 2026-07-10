using System.ComponentModel.DataAnnotations;

namespace BookingMVC.Models
{
    public class Hotel
    {
        public int HotelId { get; set; }
        [Required(ErrorMessage ="Required field")]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        [Required(ErrorMessage = "Required field")]
        public string Country { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required field")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Required field")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Required field")]
        public int Rooms { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string? AirportAitaCode { get; set; }

      

        public decimal Rating { get; set; }
        public List<Review> Reviews { get; set; } = new List<Review>();

        public string ReviewSummary { get; set; } = string.Empty;

    }
}
