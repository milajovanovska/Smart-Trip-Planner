using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Trip_Planner.Models;
using Trip_Planner.Services;
namespace Trip_Planner.Services
{
    public class TripGeneratorService
    {
        public async Task<string> GenerateTripAsync(TripRequest request)
        {
            string serpKey = ConfigurationManager.AppSettings["SerpApiKey"];
            string groqKey = ConfigurationManager.AppSettings["GroqApiKey"];

            var serpService = new SerpApiService(serpKey);
            var allActivities = new List<Activity>();

            foreach (string interest in request.Interests)
            {
                var found = await serpService.SearchPlacesAsync(request.Destination, interest);
                allActivities.AddRange(found);
            }

            allActivities = allActivities
                .GroupBy(a => a.Name)
                .Select(g => g.First())
                .OrderByDescending(a => a.Rating)
                .Take(30)
                .ToList();

            string placesText = BuildPlacesText(allActivities);
            string prompt = BuildPrompt(request, placesText);

            return await CallGroqAsync(groqKey, prompt);
        }

        private async Task<string> CallGroqAsync(string apiKey, string prompt)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", apiKey);

                var body = new
                {
                    model = "llama-3.3-70b-versatile",
                    messages = new[]
                    {
                        new { role = "user", content = prompt }
                    },
                    max_tokens = 4096
                };

                string json = JsonConvert.SerializeObject(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(
                    "https://api.groq.com/openai/v1/chat/completions", content);

                string responseJson = await response.Content.ReadAsStringAsync();

                dynamic parsed = JsonConvert.DeserializeObject(responseJson);
                return parsed.choices[0].message.content.ToString();
            }
        }

        private string BuildPlacesText(List<Activity> activities)
        {
            var sb = new StringBuilder();
            foreach (var a in activities)
            {
                sb.AppendLine($"- {a.Name} | Category: {a.Category} | Rating: {a.Rating} | Address: {a.Address}");
                if (!string.IsNullOrEmpty(a.Description))
                    sb.AppendLine($"  Description: {a.Description}");
            }
            return sb.ToString();
        }

        private string BuildPrompt(TripRequest request, string placesText)
        {
            int totalDays = Math.Max(1, (request.EndDate - request.StartDate).Days);

            return $@"You are an expert travel planner. Create a detailed day-by-day trip plan.

TRIP DETAILS:
- Destination: {request.Destination}
- Duration: {totalDays} days ({request.StartDate:dd MMM yyyy} to {request.EndDate:dd MMM yyyy})
- Budget: {request.Budget} EUR total
- Travel Style: {request.TravelStyle}
- Pace: {request.Pace}
- Interests: {string.Join(", ", request.Interests)}
- Transport: {string.Join(", ", request.TransportPreferences)}

REAL PLACES FOUND IN {request.Destination.ToUpper()} (live data):
{placesText}

INSTRUCTIONS:
1. Plan exactly {totalDays} days with morning, afternoon and evening slots
2. Group nearby places together so the route makes geographic sense
3. For each activity write: time, place name, duration, cost in EUR
4. End each day with that day total cost
5. End the whole plan with TOTAL COST and note if it fits the {request.Budget} EUR budget
6. Only use places from the list above, do not invent places
7. Use Day 1, Day 2 headers";
        }
    }
}