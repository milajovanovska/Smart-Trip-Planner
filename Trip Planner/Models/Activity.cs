namespace TripPlanner.Models
{
    public class Activity
    {
        public string Name { get; set; }

        public double Rating { get; set; }

        public string Address { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public decimal EstimatedCost { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }
    }
}