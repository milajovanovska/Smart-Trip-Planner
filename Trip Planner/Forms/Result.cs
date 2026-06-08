using System;
using System.Drawing;
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

            this.BackColor = Color.FromArgb(240, 240, 240);

            string cityIntro = parts.Length > 1 ? parts[0].Trim() : "";
            string tripPlan = parts.Length > 1 ? parts[1].Trim() : resultText;

            for (int i = 1; i <= 31; i++)
            {
                tripPlan = tripPlan.Replace(
                    $"## Day {i}",
                    $"\r\n\r\n🌍 DAY {i}\r\n────────────────────────────\r\n");
            }

            txtResult.Text = cityIntro + "\r\n\r\n" + tripPlan;

            txtResult.SelectionStart = 0;
            txtResult.SelectionLength = 0;

            HighlightDays();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }
        private void HighlightDays()
        {
            for (int i = 1; i <= 20; i++)
            {
                string search = $"🌍 DAY {i}";
                int start = 0;

                while ((start = txtResult.Text.IndexOf(search, start)) != -1)
                {
                    txtResult.Select(start, search.Length);

                    txtResult.SelectionFont =
                        new Font("Segoe UI", 14, FontStyle.Bold);

                    start += search.Length;
                }
            }

            txtResult.Select(0, 0);
        }
        
    }
}