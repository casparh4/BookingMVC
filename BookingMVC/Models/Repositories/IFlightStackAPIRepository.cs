using BookingMVC.Models.POCOs;
using System.Text.Json;

namespace BookingMVC.Models.Repositories
{
    public interface IFlightStackAPIRepository
    {
        public Task<RootObject> GetFutureFlightsAsync(Booking booking, int offset, CancellationToken token);
        
    }
}
