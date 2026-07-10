
using BookingMVC.Models;
using BookingMVC.Models.Repositories;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Validation;
using OpenAI;
using OpenAI.Chat;
using System.Linq.Expressions;
using System.Text;

namespace BookingMVC.Controllers
{
    
    public class HotelController : Controller
    {

        private readonly IHotelRepository _hotelRepository;
        private readonly UserManager<IdentityUser>? _userManager;
        private IHotelRepository @object;

        public HotelController(IHotelRepository hotelRepository, UserManager<IdentityUser>? userManager)
        {
            _hotelRepository = hotelRepository;
            _userManager = userManager;
        }

       

        [HttpPost]
        public async Task<IActionResult> UpdateReviewSummary(int id) 
        {
            await _hotelRepository.UpdateReviewSummary(id);

            return RedirectToAction(nameof(Details), new {id=id});
        }


        public async Task<IActionResult> Details(int id) 
        {
            var hotel = _hotelRepository.GetHotelById(id);
            if (hotel !=null)
            {
                Review NewReview = new Review();
                NewReview.HotelId = id;
                var detailsViewModel = new DetailsViewModel(hotel, NewReview);
                return View(detailsViewModel);
            }
            else
            {
                throw new Exception("Hotel not found");
            }
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
            }
            if(_userManager != null)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    NewReview.Email = user.UserName;
                }
            }
   

            _hotelRepository.CreateReview(Id, NewReview); // add a mock for this
            ViewBag.ReviewCompletionMessage = "thanks for the review!";
            var detailsViewModel = new DetailsViewModel(hotel, NewReview); //make a new viewmodel with just hotel and string
            return View(detailsViewModel); //redirect to a page with no review form and just the view message(once reviews are fixed)
        }
        
    }
}
