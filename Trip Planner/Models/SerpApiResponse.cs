using System.Collections.Generic;
using Trip_Planner.Models; 

namespace Trip_Planner.Models
{
    public class SerpApiResponse
    {
        public List<LocalResult> local_results { get; set; }
    }

    public class LocalResult
    {
        public string title { get; set; }

        public double rating { get; set; }

        public string address { get; set; }

        public string description { get; set; }

        public string type { get; set; }
    }
}