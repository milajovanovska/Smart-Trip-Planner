using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trip_Planner.Models;

namespace Trip_Planner.Services
{
    public class TripGeneratorService
    {
        public async Task<string> GenerateTripAsync(TripRequest request)
        {
            await Task.Delay(2000);

            int totalDays = (request.EndDate - request.StartDate).Days;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"TRIP TO {request.Destination.ToUpper()}");
            sb.AppendLine("");

            sb.AppendLine($"Travel Style: {request.TravelStyle}");
            sb.AppendLine($"Budget: {request.Budget} €");
            sb.AppendLine($"Pace: {request.Pace}");
            sb.AppendLine("");

            sb.AppendLine("INTERESTS:");
            sb.AppendLine(string.Join(", ", request.Interests));
            sb.AppendLine("");

            sb.AppendLine("TRANSPORT:");
            sb.AppendLine(string.Join(", ", request.TransportPreferences));
            sb.AppendLine("");

            sb.AppendLine("DAY BY DAY PLAN");
            sb.AppendLine("");

            for (int i = 1; i <= totalDays; i++)
            {
                sb.AppendLine($"DAY {i}");
                sb.AppendLine(GenerateActivities(request));
                sb.AppendLine("");
            }

            sb.AppendLine("ENJOY YOUR TRIP!");

            return sb.ToString();
        }

        private string GenerateActivities(TripRequest request)
        {
            StringBuilder activities = new StringBuilder();

            if (request.Interests.Contains("Museums"))
                activities.AppendLine("- Visit famous museums");

            if (request.Interests.Contains("Food"))
                activities.AppendLine("- Try local restaurants and street food");

            if (request.Interests.Contains("Nightlife"))
                activities.AppendLine("- Explore nightlife areas");

            if (request.Interests.Contains("Nature"))
                activities.AppendLine("- Visit parks and nature attractions");

            if (request.Interests.Contains("Shopping"))
                activities.AppendLine("- Go shopping in local markets");

            if (request.Interests.Contains("Beaches"))
                activities.AppendLine("- Relax at the beach");

            if (request.Interests.Contains("Cafes"))
                activities.AppendLine("- Discover aesthetic cafes");

            if (request.Interests.Contains("Hidden Gems"))
                activities.AppendLine("- Explore hidden local spots");

            return activities.ToString();
        }
    }
}