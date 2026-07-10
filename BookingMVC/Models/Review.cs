using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BookingMVC.Models
{
    public class Review
    {
        [BindNever]
        public int ReviewId { get; set; }

        public int HotelId { get; set; }
        public Hotel? Hotel { get; set; }

        public string? Email { get; set; } 

        [Required]
        [Display(Name ="First Name ")]
        public string FirstName { get; set; } = string.Empty;
        [Required(ErrorMessage ="Required field")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } =string.Empty;

        [Required(ErrorMessage = "Required field")]
        public int Rating { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Comments { get; set; } = string.Empty;

    }
}
