using System.ComponentModel.DataAnnotations;

namespace BookingMVC.Models
{
    public class Search
    {
        [Display(Name ="Hotel name" )]
        public string? Name { get; set; }

        [Display(Name ="City")]

        public string? City { get; set; }

        [Display(Name ="Country")]

        public string? Country { get; set; }

        [Display(Name ="Rating")]
        public decimal? Rating { get; set; }
    }
}
