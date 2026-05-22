using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.ComponentModel.DataAnnotations;

namespace BookingMVC.Models
{
    
  
    public class Booking
    {


        [BindNever]
        public int BookingId { get; set; }

        public IdentityUser? User { get; set; }
      
        public Hotel? Hotel { get; set; } //dont want user to enter this, connect it via hotelid from book now button 

        [Required(ErrorMessage ="required field")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "required field")]
        [Display(Name = "Last name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage ="required field")]
        [Display(Name = "Arrival Date")]
        [DataType(DataType.Date)]
        public DateTime Arrival { get; set; }

        [Required(ErrorMessage = "required field")]
        [Display(Name = "Leaving Date")]
        [DataType(DataType.Date)]
        public DateTime Leaving { get; set; }

        [BindNever]
        public decimal TotalCost { get; set; }
    }
}
