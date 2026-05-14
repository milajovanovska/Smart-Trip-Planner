using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TripPlanner.Models;

namespace Trip_Planner.Models
{
    public class DayPlan
    {
        public string DayTitle { get; set; }

        public List<Activity> Activities { get; set; }
    }
}
