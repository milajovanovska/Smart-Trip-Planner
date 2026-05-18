using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Trip_Planner.Models;

namespace Trip_Planner.Services
{
    public class SerpApiService
    {
        private readonly string _apiKey;

        public SerpApiService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<List<Activity>> SearchPlacesAsync(string city, string category)
        {
            using (HttpClient client = new HttpClient())
            {
                string query = System.Uri.EscapeDataString($"{category} in {city}");
                string url = $"https://serpapi.com/search.json?engine=google_maps&q={query}&api_key={_apiKey}";

                var response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();

                return ParseActivities(json, category);
            }
        }

        private List<Activity> ParseActivities(string json, string category)
        {
            var activities = new List<Activity>();
            var parsed = JsonConvert.DeserializeObject<SerpApiResponse>(json);

            if (parsed?.local_results == null) return activities;

            foreach (var item in parsed.local_results)
            {
                activities.Add(new Activity
                {
                    Name = item.title,
                    Rating = item.rating,
                    Address = item.address,
                    Description = item.description,
                    Category = category,
                    EstimatedCost = 20,
                    Latitude = item.gps?.latitude ?? 0,
                    Longitude = item.gps?.longitude ?? 0
                });
            }

            return activities;
        }
    }
}