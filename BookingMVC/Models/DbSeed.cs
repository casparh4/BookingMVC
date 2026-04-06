using System.Diagnostics.Metrics;

namespace BookingMVC.Models
{
    public class DbSeed
    {
        public static void Seed(IApplicationBuilder applicationBuilder) //static because we dont need an object, just functuality, so have to call dbcontext from application builde4r
        {
            BookingDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<BookingDbContext>();

            if (!context.Hotels.Any())
            {
                context.Hotels.AddRange(new List<Hotel>()
                {
                    new Hotel
                    {
                      Name = "Grand Palace Hotel",
                      Description = "Luxury hotel in the heart of the city",
                      ImageUrl = "/Images/GrandPalace.png",
                      Country = "UK",
                      City = "London",
                      Price = 250m,
                      Rating = 5
                    },
                    new Hotel
                    {
                       Name = "Seaside Escape",
                       Description = "Hotel on the spanish coast",
                       ImageUrl = "/Images/SeasideEscape.png",
                       Country = "Spain",
                       City = "Barcelona",
                       Price = 180m,
                       Rating = 4
                    },
                    new Hotel
                    {
                        Name = "Mountain Retreat",
                        Description = "Peaceful stay in the mountains",
                        ImageUrl = "/Images/MountainRetreat.png",
                        Country = "Switzerland",
                        City = "Zermatt",
                        Price = 300m,
                        Rating = 5
                    },
                    new Hotel
                    {
                        Name = "City Budget Inn",
                        Description = "Affordable hotel for short stays",
                        ImageUrl = "/Images/CityBudget.png",
                        Country = "UK",
                        City = "Manchester",
                        Price = 75m,
                        Rating = 3
                    },
                    new Hotel
                    {
                       Name = "Business Suites",
                       Description = "Perfect for business travellers",
                       ImageUrl = "/Images/BusinessSuites.png",
                       Country = "USA",
                       City = "New York",
                       Price = 220m,
                       Rating = 4
                    },
                    new Hotel
                    {
                        Name = "Lakeside Hotel",
                        Description = "Beautiful views of the lake with modern rooms",
                        ImageUrl = "/Images/LakesideResort.png",
                        Country = "Canada",
                        City = "Toronto",
                       Price = 160m,
                       Rating = 4
                    },
                    new Hotel
                    {
                       Name = "Desert Oasis Resort",
                       Description = "Luxury resort in the desert",
                       ImageUrl = "Images/DesertOasis.png",
                       Country = "UAE",
                       City = "Dubai",
                       Price = 350m,
                       Rating = 5
                    },
                    new Hotel
                    {
                       Name = "Historic Charm Hotel",
                       Description = "Stay in a beautifully restored historic building",
                       ImageUrl = "Images/Historic.png",
                       Country = "Italy",
                       City = "Rome",
                       Price = 200m,
                       Rating = 4
                    }
                   
                });
                context.SaveChanges();
            }
        }
    }
}
