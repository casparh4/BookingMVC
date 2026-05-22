using AspNetCoreGeneratedDocument;
using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Images;
using System.IO;
using System.Reflection.Metadata;
using static System.Net.Mime.MediaTypeNames;

namespace BookingMVC.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly OpenAIClient _aiClient;
        private readonly IOptions<ModelSettings> _modelSettings;


        public AdminController(IHotelRepository hotelRepository, UserManager<IdentityUser> userManager, IBookingRepository bookingRepository, OpenAIClient aiClient, IOptions<ModelSettings> modelSettings)
        {
            _hotelRepository = hotelRepository;
            _userManager = userManager;
            _bookingRepository = bookingRepository;
            _aiClient = aiClient;
            _modelSettings = modelSettings;
        }
      
        
        public async Task<IActionResult> Index()
        {
            //var user = await _userManager.GetUserAsync(User);
            //string userString = User.ToString();
            ////Console.WriteLine($"USER!!!!::::{userString}");

            var hotels = _hotelRepository.GetAllHotels();
            HotelViewModel hotelViewModel = new HotelViewModel(hotels);
            
            return View(hotelViewModel);
        }
        public async Task<IActionResult> Add(Hotel? hotelForCreation)
        {
            if(hotelForCreation==null)
            {
                 hotelForCreation = new Hotel();
            }
           

            return View(hotelForCreation);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitAddition(Hotel hotelForCreation)
        {
            if(ModelState.IsValid)
            {
               await _hotelRepository.AddHotelAsync(hotelForCreation);
                TempData["HotelAdded"] = $"Hotel: {hotelForCreation.Name} was successfully added";
               return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ErrorMessage"] = "Model binding failed, please try again";
                return View(hotelForCreation);
            }

            
        }
        public async Task<IActionResult> Delete(int hotelId)
        {
            var hotel = _hotelRepository.GetHotelById(hotelId);
            if(hotel==null)
            {
                return NotFound();
            }
            else
            {
                return View(hotel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int hotelId)
        {
            var hotel = _hotelRepository.GetHotelById(hotelId);
            if (hotel == null)
            {
                return NotFound();
                throw new Exception($"Hotel was not deleted");
            }

            await _hotelRepository.DeleteHotelAsync(hotelId);

                TempData["HotelDeleted"] = $"Hotel: {hotel.Name} has been deleted successfully";

                return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Bookings(int hotelId)
        {
            var hotel = _hotelRepository.GetHotelById(hotelId);
            if(hotel ==null)
            {
                return NotFound();
            }

            var bookings = await _bookingRepository.GetBookingsAsync(hotelId);
            
            BookingsViewModel bookingsViewModel = new BookingsViewModel(bookings, hotel);
            return View(bookingsViewModel);

        }

        //[HttpPost]
        //public async Task<IActionResult> ConfirmBooking(int bookingId)
        //{
        //    var booking = _bookingRepository.GetBooking(bookingId);

        //    if(booking==null)
        //    {
        //        return NotFound();
        //    }

        //}

        [HttpPost]
        public async Task<IActionResult> GenerateDesc(Hotel hotel) //hotel.name isnt passed through here
        {
            ChatClient chatClient = _aiClient.GetChatClient(_modelSettings.Value.TextModelName);

            hotel.Description = string.Empty;


            if(!string.IsNullOrEmpty(hotel.Name) && !string.IsNullOrEmpty(hotel.Country) && !string.IsNullOrEmpty(hotel.City))
            {
                string prompt = $"Generate a description for a hotel named {hotel.Name} in {hotel.City},{hotel.Country} for a hotel booking website to make customers want to book with us";
                ChatCompletion completion = chatClient.CompleteChat(prompt);

                var generatedDesc = completion.Content[0].Text;
                hotel.Description = generatedDesc;
            }
            else if (!string.IsNullOrEmpty(hotel.Name))
            {
                string prompt = $"Generate a description for a hotel named {hotel.Name} for a hotel booking website to make customers want to book with us";
                ChatCompletion completion = chatClient.CompleteChat(prompt);

                var generatedDesc = completion.Content[0].Text;
                hotel.Description = generatedDesc;
            }

            return RedirectToAction(nameof(Add), hotel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateImage(Hotel hotel)
        {
            ImageClient imageClient = _aiClient.GetImageClient(_modelSettings.Value.ImageModelName);
            hotel.ImageUrl = string.Empty;
            if (!string.IsNullOrEmpty(hotel.Name) && !string.IsNullOrEmpty(hotel.Country) && !string.IsNullOrEmpty(hotel.City) && !string.IsNullOrEmpty(hotel.Description))
            {
                string prompt = $"""
                    Generate an image of a hotel for a booking website using these values.
                     Name: {hotel.Name},
                     location: {hotel.City} {hotel.Country},
                     Description: {hotel.Description},
                     size: 1024px x 1024px,
                     just one front facing image of the hotel itself to attract holiday-makers
                    """;

        
                 GeneratedImage generatedImage = imageClient.GenerateImage(prompt);



                BinaryData bytes = generatedImage.ImageBytes;
                   string imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images");
                Directory.CreateDirectory(imagesFolder);
                string fileName = $"{hotel.Name}.png";
                string filePath = Path.Combine(imagesFolder, fileName);

                await System.IO.File.WriteAllBytesAsync(filePath, bytes.ToArray());

                hotel.ImageUrl = $"Images/{fileName}";
            }

            return RedirectToAction(nameof(Add), hotel);

        }
    }
}
