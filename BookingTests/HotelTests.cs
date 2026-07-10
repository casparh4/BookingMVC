using BookingMVC;
using BookingMVC.Controllers;
using BookingMVC.Models;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OpenAI;

namespace BookingTests
{
    public class HotelTests
    {
 
        [Fact]
        public async Task HotelDetailsIsOfTypeViewResult_AndIsDetailsViewModel()
        {
            //arrange
            var mockHotelRepository = RepositoryMocks.GetHotelRepository();

           
            var hotelController = new HotelController(mockHotelRepository.Object, null);
            //act
            var result = await hotelController.Details(1);
            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var detailsViewModel = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.Model);

        }
        [Fact]
        public async Task PostedReviewHasCorrectHotelAndIsDetailsViewmodel()
        {
            //arrange
            var mockHotelRepository = RepositoryMocks.GetHotelRepository();
            var mockUserManager = RepositoryMocks.GetMockUserManager();
            var hotelController = new HotelController(mockHotelRepository.Object, mockUserManager.Object);
            //act
            var newReview = new Review()
            {
               FirstName = "Unit",
               LastName="Test",
               Rating=4,
               Comments="This is a unit test"

            };
            var result = await hotelController.Details(0, newReview);
            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var detailsViewModel = Assert.IsAssignableFrom<DetailsViewModel>(viewResult.Model);


            Assert.Equal("Grand Palace Hotel", detailsViewModel.Hotel.Name);
            Assert.Equal(0, detailsViewModel.Hotel.HotelId);
            
        }


    }
}
