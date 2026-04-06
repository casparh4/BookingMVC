using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace BookingMVC.Models
{
    public class Booking
    {
        [BindNever]
        public int BookingId { get; set; }

      
        public Hotel? Hotel { get; set; } //dont want user to enter this, connect it via hotelid from book now button 

        [Required(ErrorMessage ="required field")]
        [Display(Name = "First name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "required field")]
        [Display(Name = "Last name")]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])", //email formatting
            ErrorMessage = "The email address is not entered in a correct format")]
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
