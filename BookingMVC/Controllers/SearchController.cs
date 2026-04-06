using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly IHotelRepository _hotelRepository;

        public SearchController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public IActionResult Filter()
        {
            Search search = new Search();
            var allHotels = _hotelRepository.GetAllHotels();
            var SearchViewModel = new SearchViewModel(search, allHotels);
            return View(SearchViewModel);
        }
        [HttpPost]
        public IActionResult Filter(Search search)
        {
            var hotels =_hotelRepository.FilterHotels(search);
            var searchViewModel = new SearchViewModel(search, hotels);
            return View(searchViewModel);


        }
    }
}
