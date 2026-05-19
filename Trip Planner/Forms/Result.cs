using System;
using System.Windows.Forms;

namespace Trip_Planner.Forms
{
    public partial class Result : Form
    {
        public string CityIntro { get; private set; }
        public string TripPlan { get; private set; }

        public Result(string resultText)
        {
            InitializeComponent();

            string[] parts = resultText.Split(new string[] { "|||" }, StringSplitOptions.None);

            string cityIntro = parts.Length > 1 ? parts[0].Trim() : "";
            string tripPlan = parts.Length > 1 ? parts[1].Trim() : resultText;

            txtResult.Text = cityIntro + "\r\n\r\n" + tripPlan;
        }
    }
}