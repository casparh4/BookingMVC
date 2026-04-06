using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHotelRepository _hotelRepository;
       

        public BookingController(IBookingRepository bookingRepository, IHotelRepository hotelRepository)
        {
            _bookingRepository = bookingRepository;
            _hotelRepository = hotelRepository;
        }

        public IActionResult CreateBooking(int id)
        {

            _bookingRepository.Hotel = _hotelRepository.GetHotelById(id);
            if (_bookingRepository.Hotel == null)
            {
                return NotFound();
            }
            ViewBag.HotelId = _bookingRepository.Hotel.HotelId;
            ViewBag.HotelName = _bookingRepository.Hotel.Name;
            ViewBag.HotelImageURL = _bookingRepository.Hotel.ImageUrl;

            return View();
        }
        [HttpPost]
        public IActionResult CreateBooking(int id, Booking booking)
        {
            _bookingRepository.Hotel = _hotelRepository.GetHotelById(id);

            if(ModelState.IsValid)
            {
                _bookingRepository.CreateBooking(booking);
                return RedirectToAction("BookingConfirmation", 
                    new 
                    { 
                        id=booking.BookingId
                    });
            }

            return View(booking);
        }

        public IActionResult BookingConfirmation(int id)
        {

            var booking =_bookingRepository.GetBooking(id);
            return View(booking);

        }
        
    }
}
