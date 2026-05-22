
using BookingMVC.Models;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IOptions<ModelSettings> _modelSettings;
        private readonly OpenAIClient _aiClient;
        public HotelController(IHotelRepository hotelRepository, UserManager<IdentityUser> userManager, IOptions<ModelSettings> modelSettings, OpenAIClient aiClient)
        {
            _hotelRepository = hotelRepository;
            _userManager = userManager;
            _aiClient = aiClient;
            _modelSettings = modelSettings;
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
                if (hotel == null)
                {
                    return NotFound();
                }
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
            var user = await _userManager.GetUserAsync(User);
            if(user!= null)
            {
                NewReview.Email = user.UserName;
            }

            _hotelRepository.CreateReview(Id, NewReview);

            ViewBag.ReviewCompletionMessage = "thanks for the review!";

            var detailsViewModel = new DetailsViewModel(hotel, NewReview); //make a new viewmodel with just hotel and string

            return View(detailsViewModel); //redirect to a page with no review form and just the view message(once reviews are fixed)
        }
        
    }
}
