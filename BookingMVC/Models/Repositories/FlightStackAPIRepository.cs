using BookingMVC.Models.POCOs;
using System.Text.Json;

namespace BookingMVC.Models.Repositories
{
    public class FlightStackAPIRepository : IFlightStackAPIRepository
    {
        public async Task<RootObject> GetFutureFlightsAsync(Booking booking, int offset, CancellationToken token)
        {
              HttpClient client = new HttpClient();
            if(booking.Hotel == null)
            {
                return new RootObject();
            }    
                var response = await client.GetAsync($"https://api.aviationstack.com/v1/flightsFuture?access_key=a384dc4492c519059dd8792ccf98ed76&" +
                    $"iataCode={booking.Hotel.AirportAitaCode}&type=arrival&date={booking.Arrival.ToString("yyyy-MM-dd")}&limit=25&offset={offset}", token);

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);

                var flights = JsonSerializer.Deserialize<RootObject>(json, options);

                return flights ?? new RootObject();
        }
    }
}
