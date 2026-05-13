using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trip_Planner.Models
{
    public class Activity
    {
        public string Name { get; set; }

        public string Time { get; set; }

        public string Description { get; set; }

        public decimal EstimatedCost { get; set; }
    }
}
