using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHotelRepository _hotelRepository;

        public HomeController(IHotelRepository hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }
        public IActionResult Index()
        {
            var allHotels = _hotelRepository.GetAllHotels();
            var hotelViewModel = new HotelViewModel(allHotels);
            return View(hotelViewModel);
        }

        


    
    }
}
