using BookingMVC.Controllers;
using BookingMVC.Models;
using BookingMVC.Models.Repositories;
using BookingMVC.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace BookingTests
{
    public class BookingTests
    {


        [Fact]
        public async Task RedirectToBookingConfirmationIsSuccessAndBookingUserIsRecorded()
        {
            //arrange
            var mockBookingRepository = RepositoryMocks.GetBookingRepository();
            var mockHotelRepository = RepositoryMocks.GetHotelRepository();
            var mockUserManager = RepositoryMocks.GetMockUserManager();
            var mockFlightsRepo = RepositoryMocks.GetFlightStackRepository();
            var bookingController = new BookingController(mockBookingRepository.Object, mockHotelRepository.Object, mockUserManager.Object, mockFlightsRepo.Object);


            //act
            var booking = new Booking()
            {

                Arrival = new DateTime(2026, 6, 6),
                Leaving = new DateTime(2026, 7, 1),
                FirstName = "Unit",
                LastName = "Test",

            };
            var result = await bookingController.CreateBooking(1, booking);

            //assert
            var viewResult = Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("TESTING@UNIT", booking.Email);
        }
   
    }

  
}
