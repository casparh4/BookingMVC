using BookingMVC.Migrations;
using BookingMVC.Models;
using BookingMVC.Models.POCOs;
using BookingMVC.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace BookingTests
{
    internal class RepositoryMocks
    {

        public static Mock<IHotelRepository> GetHotelRepository()
        {
            var hotels = new List<Hotel>() {  new Hotel
                    {
                      Name = "Grand Palace Hotel",
                      Description = "Luxury hotel in the heart of the city",
                      ImageUrl = "/Images/GrandPalace.png",
                      Country = "UK",
                      City = "London",
                      Price = 250m,
                      Rating = 5,
                      AirportAitaCode = "LHR"
                    

                    },
                    new Hotel
                    {
                       Name = "Seaside Escape",
                       Description = "Hotel on the spanish coast",
                       ImageUrl = "/Images/SeasideEscape.png",
                       Country = "Spain",
                       City = "Barcelona",
                       Price = 180m,
                       Rating = 4,
                       AirportAitaCode = "BLA"
                      
                    },
                    new Hotel
                    {
                        Name = "Mountain Retreat",
                        Description = "Peaceful stay in the mountains",
                        ImageUrl = "/Images/MountainRetreat.png",
                        Country = "Switzerland",
                        City = "Zermatt",
                        Price = 300m,
                        Rating = 5,
                        AirportAitaCode = "ZRH"
                  
                    },
                    new Hotel
                    {
                        Name = "City Budget Inn",
                        Description = "Affordable hotel for short stays",
                        ImageUrl = "/Images/CityBudget.png",
                        Country = "UK",
                        City = "Manchester",
                        Price = 75m,
                        Rating = 3,
                        AirportAitaCode = "MAN"
                    },
                    new Hotel
                    {
                       Name = "Business Suites",
                       Description = "Perfect for business travellers",
                       ImageUrl = "/Images/BusinessSuites.png",
                       Country = "USA",
                       City = "New York",
                       Price = 220m,
                       Rating = 4,
                       AirportAitaCode = "NYC"
                    },
                    new Hotel
                    {
                        Name = "Lakeside Hotel",
                        Description = "Beautiful views of the lake with modern rooms",
                        ImageUrl = "/Images/LakesideResort.png",
                        Country = "Canada",
                        City = "Toronto",
                       Price = 160m,
                       Rating = 4,
                       AirportAitaCode = "YYZ"
                    },
                    new Hotel
                    {
                       Name = "Desert Oasis Resort",
                       Description = "Luxury resort in the desert",
                       ImageUrl = "Images/DesertOasis.png",
                       Country = "UAE",
                       City = "Dubai",
                       Price = 350m,
                       Rating = 5,
                       AirportAitaCode = "DST"
                    },
                    new Hotel
                    {
                       Name = "Historic Charm Hotel",
                       Description = "Stay in a beautifully restored historic building",
                       ImageUrl = "Images/Historic.png",
                       Country = "Italy",
                       City = "Rome",
                       Price = 200m,
                       Rating = 4,
                       AirportAitaCode = "FCO"
                    }};

            var mockHotelRepository = new Mock<IHotelRepository>();

            Review? capturedReview = null;
            
            mockHotelRepository.Setup(repo => repo.GetAllHotels()).Returns(hotels);
            mockHotelRepository.Setup(repo => repo.GetHotelById(It.IsAny<int>())).Returns((int id) => hotels[id]);//It.IsAny<T>() is a Moq matcher that means:"I don't care what value is passed here, match anything of type T."
            mockHotelRepository.Setup(repo => repo.CreateReview(It.IsAny<int>(), It.IsAny<Review>())).Callback<int, Review>((id, review) => { capturedReview = review; capturedReview.HotelId = id; capturedReview.Hotel = hotels[id];  });
            
            return mockHotelRepository;

        }

        public static Mock<IBookingRepository> GetBookingRepository()
        {

            var hotelrepo = GetHotelRepository();
            var bookings = new List<Booking>() {

                new Booking()
                {
                    Arrival = new DateTime(2026, 6, 15),
                    Leaving = new DateTime(2026, 6, 22),
                    Hotel = hotelrepo.Object.GetHotelById(1),
                    FirstName="Caspar",
                    LastName = "Huntley",
                    Email="caspar127@icloud.com",

                },
                new Booking()
{
    Arrival = new DateTime(2026, 7, 3),
    Leaving = new DateTime(2026, 7, 10),
    Hotel = hotelrepo.Object.GetHotelById(2),
    FirstName = "Emma",
    LastName = "Johnson",
    Email = "emma.johnson@example.com",
},

new Booking()
{
    Arrival = new DateTime(2026, 8, 12),
    Leaving = new DateTime(2026, 8, 19),
    Hotel = hotelrepo.Object.GetHotelById(3),
    FirstName = "James",
    LastName = "Wilson",
    Email = "james.wilson@example.com",
},

new Booking()
{
    Arrival = new DateTime(2026, 9, 5),
    Leaving = new DateTime(2026, 9, 12),
    Hotel = hotelrepo.Object.GetHotelById(4),
    FirstName = "Sophia",
    LastName = "Brown",
    Email = "sophia.brown@example.com",
},

new Booking()
{
    Arrival = new DateTime(2026, 10, 18),
    Leaving = new DateTime(2026, 10, 25),
    Hotel = hotelrepo.Object.GetHotelById(5),
    FirstName = "Oliver",
    LastName = "Taylor",
    Email = "oliver.taylor@example.com",
},

new Booking()
{
    Arrival = new DateTime(2026, 12, 20),
    Leaving = new DateTime(2026, 12, 27),
    Hotel = hotelrepo.Object.GetHotelById(6),
    FirstName = "Charlotte",
    LastName = "Davis",
    Email = "charlotte.davis@example.com",



            }

        };
  

            var mockBookingRepository = new Mock<IBookingRepository>();

            mockBookingRepository.Setup(repo => repo.GetAllBookings()).Returns(bookings);
            mockBookingRepository.Setup(repo => repo.GetBookingAsync(It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((int id, CancellationToken _) => bookings[id]);
            mockBookingRepository.Setup(repo => repo.CreateBooking(It.IsAny<Booking>())).Callback<Booking>(booking => bookings.Add(booking));
             return mockBookingRepository;
            

        }
        public static Mock<IFlightStackAPIRepository> GetFlightStackRepository()
        {
            var jsonFlights = """
                

                {"pagination":{"limit":25,"offset":0,"count":25,"total":276},"data":[{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"british airways","iataCode":"ba","icaoCode":"baw"},"flight":{"number":"5811","iataNumber":"ba5811","icaoNumber":"baw5811"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"srilankan airlines","iataCode":"ul","icaoCode":"alk"},"flight":{"number":"3521","iataNumber":"ul3521","icaoNumber":"alk3521"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"rwandair","iataCode":"wb","icaoCode":"rwd"},"flight":{"number":"1140","iataNumber":"wb1140","icaoNumber":"rwd1140"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"malaysia airlines","iataCode":"mh","icaoCode":"mas"},"flight":{"number":"4100","iataNumber":"mh4100","icaoNumber":"mas4100"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"virgin australia","iataCode":"va","icaoCode":"voz"},"flight":{"number":"6225","iataNumber":"va6225","icaoNumber":"voz6225"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"oman air","iataCode":"wy","icaoCode":"oma"},"flight":{"number":"6063","iataNumber":"wy6063","icaoNumber":"oma6063"},"codeshared":{"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}}},{"weekday":"7","departure":{"iataCode":"doh","icaoCode":"othh","terminal":"","gate":"b5","scheduledTime":"01:40"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"07:40"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-3dz(er)"},"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"21","iataNumber":"qr21","icaoNumber":"qtr21"}},{"weekday":"7","departure":{"iataCode":"auh","icaoCode":"omaa","terminal":"a","gate":"b16","scheduledTime":"02:10"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"06:55"},"aircraft":{"modelCode":"b78x","modelText":"boeing 787-10 dreamliner"},"airline":{"name":"garuda indonesia","iataCode":"ga","icaoCode":"gia"},"flight":{"number":"9050","iataNumber":"ga9050","icaoNumber":"gia9050"},"codeshared":{"airline":{"name":"etihad airways","iataCode":"ey","icaoCode":"etd"},"flight":{"number":"77","iataNumber":"ey77","icaoNumber":"etd77"}}},{"weekday":"7","departure":{"iataCode":"auh","icaoCode":"omaa","terminal":"a","gate":"b16","scheduledTime":"02:10"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"06:55"},"aircraft":{"modelCode":"b78x","modelText":"boeing 787-10 dreamliner"},"airline":{"name":"air new zealand","iataCode":"nz","icaoCode":"anz"},"flight":{"number":"4285","iataNumber":"nz4285","icaoNumber":"anz4285"},"codeshared":{"airline":{"name":"etihad airways","iataCode":"ey","icaoCode":"etd"},"flight":{"number":"77","iataNumber":"ey77","icaoNumber":"etd77"}}},{"weekday":"7","departure":{"iataCode":"auh","icaoCode":"omaa","terminal":"a","gate":"b16","scheduledTime":"02:10"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"06:55"},"aircraft":{"modelCode":"b78x","modelText":"boeing 787-10 dreamliner"},"airline":{"name":"etihad airways","iataCode":"ey","icaoCode":"etd"},"flight":{"number":"77","iataNumber":"ey77","icaoNumber":"etd77"}},{"weekday":"7","departure":{"iataCode":"dub","icaoCode":"eidw","terminal":"2","gate":"","scheduledTime":"06:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"a3","scheduledTime":"07:40"},"aircraft":{"modelCode":"a320","modelText":"airbus a320-214"},"airline":{"name":"british airways","iataCode":"ba","icaoCode":"baw"},"flight":{"number":"2072","iataNumber":"ba2072","icaoNumber":"baw2072"},"codeshared":{"airline":{"name":"aer lingus","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"202","iataNumber":"ei202","icaoNumber":"ein202"}}},{"weekday":"7","departure":{"iataCode":"dub","icaoCode":"eidw","terminal":"2","gate":"","scheduledTime":"06:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"a3","scheduledTime":"07:40"},"aircraft":{"modelCode":"a320","modelText":"airbus a320-214"},"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"8227","iataNumber":"qr8227","icaoNumber":"qtr8227"},"codeshared":{"airline":{"name":"aer lingus","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"202","iataNumber":"ei202","icaoNumber":"ein202"}}},{"weekday":"7","departure":{"iataCode":"dub","icaoCode":"eidw","terminal":"2","gate":"","scheduledTime":"06:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"a3","scheduledTime":"07:40"},"aircraft":{"modelCode":"a320","modelText":"airbus a320-214"},"airline":{"name":"aer lingus","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"202","iataNumber":"ei202","icaoNumber":"ein202"}},{"weekday":"7","departure":{"iataCode":"dxb","icaoCode":"omdb","terminal":"3","gate":"","scheduledTime":"01:50"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"c6","scheduledTime":"07:45"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-31h(er)"},"airline":{"name":"icelandair","iataCode":"fi","icaoCode":"ice"},"flight":{"number":"6043","iataNumber":"fi6043","icaoNumber":"ice6043"},"codeshared":{"airline":{"name":"emirates","iataCode":"ek","icaoCode":"uae"},"flight":{"number":"21","iataNumber":"ek21","icaoNumber":"uae21"}}},{"weekday":"7","departure":{"iataCode":"dxb","icaoCode":"omdb","terminal":"3","gate":"","scheduledTime":"01:50"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"c6","scheduledTime":"07:45"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-31h(er)"},"airline":{"name":"garuda indonesia","iataCode":"ga","icaoCode":"gia"},"flight":{"number":"8871","iataNumber":"ga8871","icaoNumber":"gia8871"},"codeshared":{"airline":{"name":"emirates","iataCode":"ek","icaoCode":"uae"},"flight":{"number":"21","iataNumber":"ek21","icaoNumber":"uae21"}}},{"weekday":"7","departure":{"iataCode":"dxb","icaoCode":"omdb","terminal":"3","gate":"","scheduledTime":"01:50"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"c6","scheduledTime":"07:45"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-31h(er)"},"airline":{"name":"qantas","iataCode":"qf","icaoCode":"qfa"},"flight":{"number":"8021","iataNumber":"qf8021","icaoNumber":"qfa8021"},"codeshared":{"airline":{"name":"emirates","iataCode":"ek","icaoCode":"uae"},"flight":{"number":"21","iataNumber":"ek21","icaoNumber":"uae21"}}},{"weekday":"7","departure":{"iataCode":"dxb","icaoCode":"omdb","terminal":"3","gate":"","scheduledTime":"01:50"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"c6","scheduledTime":"07:45"},"aircraft":{"modelCode":"b77w","modelText":"boeing 777-31h(er)"},"airline":{"name":"emirates","iataCode":"ek","icaoCode":"uae"},"flight":{"number":"21","iataNumber":"ek21","icaoNumber":"uae21"}},{"weekday":"7","departure":{"iataCode":"bhd","icaoCode":"egac","terminal":"","gate":"","scheduledTime":"07:00"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"dom","scheduledTime":"08:05"},"aircraft":{"modelCode":"at76","modelText":"atr 72-600"},"airline":{"name":"british airways","iataCode":"ba","icaoCode":"baw"},"flight":{"number":"8900","iataNumber":"ba8900","icaoNumber":"baw8900"},"codeshared":{"airline":{"name":"aer lingus regional","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"3610","iataNumber":"ei3610","icaoNumber":"ein3610"}}},{"weekday":"7","departure":{"iataCode":"bhd","icaoCode":"egac","terminal":"","gate":"","scheduledTime":"07:00"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"dom","scheduledTime":"08:05"},"aircraft":{"modelCode":"at76","modelText":"atr 72-600"},"airline":{"name":"qatar airways","iataCode":"qr","icaoCode":"qtr"},"flight":{"number":"8256","iataNumber":"qr8256","icaoNumber":"qtr8256"},"codeshared":{"airline":{"name":"aer lingus regional","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"3610","iataNumber":"ei3610","icaoNumber":"ein3610"}}},{"weekday":"7","departure":{"iataCode":"bhd","icaoCode":"egac","terminal":"","gate":"","scheduledTime":"07:00"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"dom","scheduledTime":"08:05"},"aircraft":{"modelCode":"at76","modelText":"atr 72-600"},"airline":{"name":"aer lingus regional","iataCode":"ei","icaoCode":"ein"},"flight":{"number":"3610","iataNumber":"ei3610","icaoNumber":"ein3610"}},{"weekday":"7","departure":{"iataCode":"zrh","icaoCode":"lszh","terminal":"1","gate":"","scheduledTime":"07:10"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"08:10"},"aircraft":{"modelCode":"","modelText":""},"airline":{"name":"helvetic airways","iataCode":"2l","icaoCode":"oaw"},"flight":{"number":"390","iataNumber":"2l390","icaoNumber":"oaw390"}},{"weekday":"7","departure":{"iataCode":"gva","icaoCode":"lsgg","terminal":"1","gate":"c55","scheduledTime":"07:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"08:15"},"aircraft":{"modelCode":"a359","modelText":"airbus a350-941"},"airline":{"name":"rwandair","iataCode":"wb","icaoCode":"rwd"},"flight":{"number":"1242","iataNumber":"wb1242","icaoNumber":"rwd1242"},"codeshared":{"airline":{"name":"ethiopian airlines","iataCode":"et","icaoCode":"eth"},"flight":{"number":"728","iataNumber":"et728","icaoNumber":"eth728"}}},{"weekday":"7","departure":{"iataCode":"gva","icaoCode":"lsgg","terminal":"1","gate":"c55","scheduledTime":"07:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"08:15"},"aircraft":{"modelCode":"a359","modelText":"airbus a350-941"},"airline":{"name":"rwandair","iataCode":"wb","icaoCode":"rwd"},"flight":{"number":"1236","iataNumber":"wb1236","icaoNumber":"rwd1236"},"codeshared":{"airline":{"name":"ethiopian airlines","iataCode":"et","icaoCode":"eth"},"flight":{"number":"728","iataNumber":"et728","icaoNumber":"eth728"}}},{"weekday":"7","departure":{"iataCode":"gva","icaoCode":"lsgg","terminal":"1","gate":"c55","scheduledTime":"07:30"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"","scheduledTime":"08:15"},"aircraft":{"modelCode":"a359","modelText":"airbus a350-941"},"airline":{"name":"ethiopian airlines","iataCode":"et","icaoCode":"eth"},"flight":{"number":"728","iataNumber":"et728","icaoNumber":"eth728"}},{"weekday":"7","departure":{"iataCode":"ams","icaoCode":"eham","terminal":"2","gate":"d6","scheduledTime":"08:00"},"arrival":{"iataCode":"man","icaoCode":"egcc","terminal":"2","gate":"a5","scheduledTime":"08:25"},"aircraft":{"modelCode":"e295","modelText":"embraer e195-e2"},"airline":{"name":"delta air lines","iataCode":"dl","icaoCode":"dal"},"flight":{"number":"9363","iataNumber":"dl9363","icaoNumber":"dal9363"},"codeshared":{"airline":{"name":"klm","iataCode":"kl","icaoCode":"klm"},"flight":{"number":"1029","iataNumber":"kl1029","icaoNumber":"klm1029"}}}]}
                
                """;

            var mockFlightsRepository = new Mock<IFlightStackAPIRepository>();
            mockFlightsRepository.Setup(repo => repo.GetFutureFlightsAsync(It.IsAny<Booking>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ReturnsAsync((CancellationToken _)=>JsonSerializer.Deserialize<RootObject>(jsonFlights)); //ReturnsAsync(value) automatically wraps value in a completed Task<T>.
            return mockFlightsRepository;
        }

        public static Mock<UserManager<IdentityUser>> GetMockUserManager()
        {
            var store = new Mock<IUserStore<IdentityUser>>();
            var mockUserManager = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
           
            mockUserManager.Setup(u => u.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(new IdentityUser()
            {
                Id = "1",
                UserName="TESTING@UNIT",
                Email="caspar127@icloud.com"

            });

            return mockUserManager;
        }
    }
}