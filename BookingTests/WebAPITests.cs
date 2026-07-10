using BookingMVC.Controllers;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingTests
{
    public class WebAPITests
    {

        [Fact]
        public async Task FlightsAreDeserializedSuccessfullyFromJson()
        {
            //arrange
            var mockBookingRepository = RepositoryMocks.GetBookingRepository();
            var mockHotelRepository = RepositoryMocks.GetHotelRepository();
            var mockUserManager = RepositoryMocks.GetMockUserManager();
            var mockFlightsRepo = RepositoryMocks.GetFlightStackRepository();
            var bookingController = new BookingController(mockBookingRepository.Object, mockHotelRepository.Object, mockUserManager.Object, mockFlightsRepo.Object);

            //act
            var result = await bookingController.BookingConfirmation(0, true, 0);

            //assert

            var viewResult = Assert.IsType<ViewResult>(result);
            var detailsViewModel = Assert.IsAssignableFrom<FlightViewModel>(viewResult.Model);
        }
    }
}
