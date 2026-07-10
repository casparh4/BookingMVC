using BookingMVC.Models.POCOs;
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

        public Hotel Hotel { get; set; } = default!;//dont want user to enter this, connect it via hotelid from book now button 

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
        
        public DateTime Arrival { get;set; } //make so val cannot be below DateTime.UTC.Now

        [Required(ErrorMessage = "required field")]
        [Display(Name = "Leaving Date")]
        [DataType(DataType.Date)]
        public DateTime Leaving { get; set; }//mkae sure this val is not below Arrival value

        [BindNever]
        public decimal TotalCost { get; set; }
    }
}
