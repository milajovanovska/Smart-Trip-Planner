using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trip_Planner.Models
{
    public class TripRequest
    {
        public string Destination { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Budget { get; set; }

        public string TravelStyle { get; set; }

        public List<string> Interests { get; set; }

        public string Pace { get; set; }

        public List<string> TransportPreferences { get; set; }
       
    }
}
