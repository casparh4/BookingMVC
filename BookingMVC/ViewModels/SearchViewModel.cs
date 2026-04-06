using BookingMVC.Models;
using System.Collections;

namespace BookingMVC.ViewModels
{
    public class SearchViewModel
    {
        public Search? Search { get; set; }
        public IEnumerable<Hotel> Hotels { get; }
        
        public SearchViewModel(Search search, IEnumerable<Hotel> hotels)
        {
            Search = search;
            Hotels = hotels;
        }
    }
}
