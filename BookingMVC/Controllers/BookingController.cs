using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookingMVC.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(IBookingRepository bookingRepository, IHotelRepository hotelRepository, UserManager<IdentityUser> userManager)
        {
            _bookingRepository = bookingRepository;
            _hotelRepository = hotelRepository;
            _userManager = userManager;
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
        public async Task<IActionResult> CreateBooking(int id, Booking booking)
        {
            _bookingRepository.Hotel = _hotelRepository.GetHotelById(id);

            var user = await _userManager.GetUserAsync(User);

            ModelState.Remove("booking.Email");
            if(user != null && user.UserName !=null)
            {
                booking.Email = user.UserName.ToString();
            }
            else
            {
                return View(booking);
            }
            if (ModelState.IsValid) //email is invalid here because bool only changed once booking is first gened
            {
                _bookingRepository.CreateBooking(booking); //pass user in here an do it repo level maybe??
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
