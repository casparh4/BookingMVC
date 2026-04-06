
using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Validation;

namespace BookingMVC.Controllers
{
    
    public class HotelController : Controller
    {

        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<IdentityUser> _userManager;
        public HotelController(IHotelRepository hotelRepository, UserManager<IdentityUser> userManager)
        {
            _hotelRepository = hotelRepository;
            _userManager = userManager;
        }

        public IActionResult Details(int Id) 
        {
            var hotel = _hotelRepository.GetHotelById(Id);
            
            Review NewReview = new Review();
            NewReview.HotelId = Id;
            if(hotel == null)
            {
                return NotFound();
            }
            var detailsViewModel = new DetailsViewModel(hotel, NewReview);

            return View(detailsViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(int Id, Review NewReview)//error in passing Id here
        {
            var hotel = _hotelRepository.GetHotelById(Id);
            if (hotel == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                NewReview.HotelId = hotel.HotelId;
                var incompleteDetailsViewModel = new DetailsViewModel(hotel, NewReview);
                
                ViewBag.ReviewCompletionMessage = "invalid review";
                return View(incompleteDetailsViewModel);

                //return RedirectToAction("Details", new { Id = Id});

            }
            

            var user = await _userManager.GetUserAsync(User);
            NewReview.FirstName = user.UserName;
            
            _hotelRepository.CreateReview(Id, NewReview);

           
            
            


            
            ViewBag.ReviewCompletionMessage = "thanks for the review!";

            var detailsViewModel = new DetailsViewModel(hotel, NewReview); //make a new viewmodel with just hotel and string

            return View(detailsViewModel); //redirect to a page with no review form and just the view message(once reviews are fixed)
        }
        
    }
}
