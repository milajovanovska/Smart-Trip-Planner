using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TripPlanner.Models;
using System.Collections.Generic;

namespace TripPlanner.Services
{
    public class SerpApiService
    {
        private readonly string _apiKey;

        public SerpApiService(string apiKey)
        {
            _apiKey = apiKey;
        }

        public async Task<string> SearchPlacesAsync(string query)
        {
            using (HttpClient client = new HttpClient())
            {
                string url =
                    $"https://serpapi.com/search.json?engine=google_maps&q={query}&api_key={_apiKey}";

                var response =
                    await client.GetAsync(url);

                return await response.Content.ReadAsStringAsync();
            }
        }
        public List<Activity> ParseActivities(string json)
        {
            List<Activity> activities =
                new List<Activity>();

            var parsed =
                JsonConvert.DeserializeObject<SerpApiResponse>(json);

            if (parsed?.local_results == null)
            {
                return activities;
            }

            foreach (var item in parsed.local_results)
            {
                Activity activity = new Activity
                {
                    Name = item.title,
                    Rating = item.rating,
                    Address = item.address,
                    Description = item.description,
                    Category = item.type,
                    EstimatedCost = 25
                };

                activities.Add(activity);
            }

            return activities;
        }
    }
}