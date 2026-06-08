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

namespace Trip_Planner.Services
{
    public class TripGeneratorService
    {
        public async Task<string> GenerateTripAsync(TripRequest request)
        {
            string serpKey = ConfigurationManager.AppSettings["SerpApiKey"];
            string groqKey = ConfigurationManager.AppSettings["GroqApiKey"];

            string cityIntro = await GenerateCityIntroAsync(groqKey, request.Destination, request.StartDate);

            var serpService = new SerpApiService(serpKey);
            var allActivities = new List<Activity>();


            var baseCategories = new List<string>
{
    "tourist attractions",
    "landmarks",
    "churches cathedrals",
    "parks gardens",
    "museums",
    "restaurants local food",
    "cafes coffee"
};

            var allCategories = new List<string>(baseCategories);
            foreach (string interest in request.Interests)
            {
                if (!allCategories.Contains(interest))
                    allCategories.Add(interest);
            }

            foreach (string category in allCategories)
            {
                var found = await serpService.SearchPlacesAsync(request.Destination, category);
                allActivities.AddRange(found);
            }

            allActivities = allActivities
                .GroupBy(a => a.Name)
                .Select(g => g.First())
                .OrderByDescending(a => a.Rating)
                .Take(30)
                .ToList();

            allActivities = SortByProximity(allActivities);

            string placesText = BuildPlacesText(allActivities);
            string prompt = BuildPrompt(request, placesText);

            string tripPlan = await CallGroqAsync(groqKey, prompt);
            return cityIntro + "|||" + tripPlan;
        }
        private async Task<string> GenerateCityIntroAsync(string apiKey, string destination, DateTime startDate)
        {
            string month = startDate.ToString("MMMM");

            string prompt = $@"Write a short introduction for a trip to {destination}.

Return exactly 3 parts separated by newlines:
1. One sentence describing what makes {destination} special as a travel destination
2. One sentence about what a visitor will experience there
3. One sentence about the weather in {month} in {destination} and what to pack, starting with a weather emoji

Example format:
Tokyo, Japan's bustling capital, offers a captivating blend of ultramodern skyscrapers and historic temples.
It's a city that seamlessly merges tradition with innovation, providing endless opportunities for exploration.
☀️ May in Tokyo typically offers pleasant spring weather around 15-23°C - pack light layers and an umbrella.

Write only these 3 sentences, nothing else, no labels, no extra text.";

            return await CallGroqAsync(apiKey, prompt);
        }
        private List<Activity> SortByProximity(List<Activity> activities)
        {
            if (activities.Count == 0) return activities;

            var sorted = new List<Activity>();
            var remaining = new List<Activity>(activities);

            var current = remaining[0];
            sorted.Add(current);
            remaining.RemoveAt(0);

            while (remaining.Count > 0)
            {
                var next = remaining
                    .OrderBy(a => Math.Pow(a.Latitude - current.Latitude, 2) +
                                  Math.Pow(a.Longitude - current.Longitude, 2))
                    .First();

                sorted.Add(next);
                remaining.Remove(next);
                current = next;
            }

            return sorted;
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
            int numInterests = Math.Max(2, request.Interests.Count);
            string transport = request.TransportPreferences.Count > 0
                ? string.Join(", ", request.TransportPreferences)
                : "walking";

            return $@"You are an expert travel planner. Create a detailed, descriptive day-by-day trip plan written like a story guide.

TRIP DETAILS:
- Destination: {request.Destination}
- Duration: {totalDays} days ({request.StartDate:dd MMM yyyy} to {request.EndDate:dd MMM yyyy})
- Budget: {request.Budget} EUR total
- Travel Style: {request.TravelStyle}
- Pace: {request.Pace}
- Interests: {string.Join(", ", request.Interests)}
- Transport available: {transport}

REAL PLACES FOUND IN {request.Destination.ToUpper()} (live data, sorted by proximity):
{placesText}

INSTRUCTIONS:
1. Plan exactly {totalDays} days
2. Each day must have exactly {numInterests} sightseeing stops (matching the number of interests selected), plus breakfast, lunch and dinner
3. Write each activity descriptively - not just the name, but what the traveler will experience, see, smell, taste or feel there. Example: instead of 'Visit Schönbrunn Palace' write 'Stroll through the grand halls of Schönbrunn Palace, marvel at the baroque architecture and enjoy the panoramic view of Vienna from the hilltop'
4. Always include breakfast, lunch and dinner with a specific place from the list:
   - 08:00 - Breakfast at [cafe name] - describe the atmosphere, mention trying a local specialty (~1 hour, ~10 EUR)
   - 13:00 - Lunch at [restaurant name] - describe what to try there (~1 hour, ~15 EUR)
   - 19:00 - Dinner at [restaurant name] - describe the evening atmosphere (~1.5 hours, ~25 EUR)
5. For museums and galleries:
   - If Museums was selected as an interest: include full guided tour entry inside
   - If Museums was NOT selected: write 'Admire [museum name] from outside, take in the impressive facade and enjoy the surrounding square' - no entry needed
6. Apply the same logic for other attractions: if the category was selected, go inside/participate. If not selected, enjoy from outside or walk past
7. Transport between places: if the next stop is more than 1km away AND the traveler has selected a transport option, write a transition line like: 'The next stop is about 2.3 km away - hop on a [selected transport] to get there comfortably'
8. If walking distance (under 1km), just say 'A short walk brings you to...'
9. Keep nearby places on the same day to minimize unnecessary travel
10. For each activity write: time, place name, vivid description, duration, cost in EUR
11. End each day with a summary line and that day total cost
12. End the whole plan with TOTAL TRIP COST and a note on whether it fits the {request.Budget} EUR budget
13. Only use places from the list above, never invent places
14. Format each day EXACTLY like this:

━━━━━━━━━━━━━━━━━━━━
DAY 1: Creative Title
━━━━━━━━━━━━━━━━━━━━

08:00 - Activity

09:00 - Activity

Day 1 Summary: ...

Day 1 Total Cost: ...

15. Use the exact same format for every day.

16. Do NOT use markdown headings (#, ##, ###).

17. Leave a blank line between activities for readability.

18. Cover as many places from the list as possible spread across all days.";
        }
    }
}