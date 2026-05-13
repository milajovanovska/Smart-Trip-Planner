using System.Threading.Tasks;
using Trip_Planner.Models;

namespace Trip_Planner.Services
{
    public class TripGeneratorService
    {
        public async Task<string> GenerateTripAsync(TripRequest request)
        {
            await Task.Delay(5000);

            return "Trip generated successfully!";
        }
    }
}