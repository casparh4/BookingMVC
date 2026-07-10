using BookingMVC.Models;
using BookingMVC.Models.POCOs;
using BookingMVC.Models.Repositories;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BookingMVC.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<IdentityUser>? _userManager;
        private readonly IFlightStackAPIRepository _flightStack;
      

        public BookingController(IBookingRepository bookingRepository, IHotelRepository hotelRepository, UserManager<IdentityUser>? userManager, IFlightStackAPIRepository flightStack)
        {
            _bookingRepository = bookingRepository ?? throw new ArgumentNullException(nameof(_bookingRepository));
            _hotelRepository = hotelRepository ?? throw new ArgumentNullException(nameof(_hotelRepository));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(_userManager));
            _flightStack = flightStack ?? throw new ArgumentNullException(nameof(_flightStack));
 
        }


        public IActionResult CreateBooking(int id)
        {

            _bookingRepository.Hotel = _hotelRepository.GetHotelById(id);

            var booking = new Booking()
            {
                Hotel = _bookingRepository.Hotel ?? throw new Exception("this hotel is currently unavailable to book"),
                Arrival = DateTime.UtcNow,
                Leaving = DateTime.UtcNow
                
            };
           
            return View(booking);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateBooking( int hotelId, Booking booking)
        {
             var user = await _userManager.GetUserAsync(User) ?? throw new ArgumentNullException("user must be logged in ");

              _bookingRepository.Hotel = _hotelRepository.GetHotelById(hotelId) ?? throw new ArgumentNullException($"hotel could not be booked");
               booking.Email = user.UserName.ToString();
            
            
         
            if(!await _bookingRepository.HotelIsAvailable( booking))
            {
                return View("_Error", new ErrorViewModel() { Message = $"{_bookingRepository.Hotel.Name} is fully booked between {booking.Arrival}-{booking.Leaving}" });
            }

            if (_bookingRepository.CreateBooking(booking)>0) 
            {

                return RedirectToAction("BookingConfirmation",
                    new
                    {
                        id = booking.BookingId
                    });
            }

            return View(booking);
        }

        public async Task <IActionResult> BookingConfirmation(int id, bool showFlights, int offset = 0, CancellationTokenSource cancellationTokenSource = default)
        {
            try
            {
                cancellationTokenSource = new();
                cancellationTokenSource.CancelAfter(60000);

                var booking = await _bookingRepository.GetBookingAsync(id, cancellationTokenSource.Token);
                if (booking == null)
                {
                    return NotFound($"could not find requested booking from ID:{id}");
                }
                var flights = new RootObject();
                if (showFlights)
                {
                    flights = await _flightStack.GetFutureFlightsAsync(booking, offset, cancellationTokenSource.Token);

                }
                var viewModel = new FlightViewModel(booking, flights, offset);
                return View(viewModel);
            }
            catch(OperationCanceledException ex)
            {
                return View("_Error", new ErrorViewModel() { Message= ex.Message} );
            }
      
        } 

        [HttpPost]
        public async Task<IActionResult> BookingConfirmationWithFlights(int id, CancellationTokenSource cancellationTokenSource) //pass in booking id here instead
        {
            cancellationTokenSource = new();
            cancellationTokenSource.CancelAfter(6000);
            var booking = await _bookingRepository.GetBookingAsync(id, cancellationTokenSource.Token); // add token here
            if (booking == null)
            {
                throw new Exception();
            }
            return RedirectToAction(nameof(BookingConfirmation), new {id=id, showFlights=true });

        }
    }

}
    

