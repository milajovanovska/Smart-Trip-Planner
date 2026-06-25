using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Trip_Planner.Forms
{
    public partial class Result : Form
    {
        public string CityIntro { get; private set; }
        public string TripPlan { get; private set; }

        public Result(string resultText)
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;

            string[] parts = resultText.Split(new string[] { "|||" }, StringSplitOptions.None);

            this.DoubleBuffered = true;

            string cityIntro = parts.Length > 1 ? parts[0].Trim() : "";
            string tripPlan = parts.Length > 1 ? parts[1].Trim() : resultText;

            tripPlan = tripPlan.Replace("━━━━━━━━━━━━━━━━━━━━", "");
            tripPlan = tripPlan.Replace("━", "");

            for (int i = 1; i <= 31; i++)
            {
                tripPlan = tripPlan.Replace(
                    $"Day {i} Summary:",
                    $"\r\n\r\nDay {i} Summary:");
            }

            txtResult.Text =
      "\r\n" +
      cityIntro +
      "\r\n\r\n\r\n\r\n" +
      tripPlan;

            txtResult.SelectionStart = 0;
            txtResult.SelectionLength = 0;

            FormatDays();

            ApplyFormatting();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush brush =
                new LinearGradientBrush(
                    this.ClientRectangle,
                    Color.White,
                    Color.FromArgb(180, 200, 240),
                    45F))
            {
                e.Graphics.FillRectangle(
                    brush,
                    this.ClientRectangle);
            }

            base.OnPaint(e);
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
        }

        private void FormatDays()
        {
            foreach (string line in txtResult.Lines)
            {
                if (line.StartsWith("DAY "))
                {
                    int start =
                        txtResult.Text.IndexOf(line);

                    txtResult.Select(start, line.Length);

                    txtResult.SelectionAlignment =
                        HorizontalAlignment.Center;

                    txtResult.SelectionFont =
                        new Font(
                            "Segoe UI",
                            14,
                            FontStyle.Bold);

                    txtResult.SelectionColor =
                        Color.FromArgb(0, 110, 170);
                }
            }
        }

        private void ApplyFormatting()
        {
            FormatIntro();
            FormatDays();
            FormatTimes();
            FormatSummaries();
            FormatTotals();

            txtResult.Select(0, 0);
        }

        private void FormatIntro()
        {
            int dayIndex = txtResult.Text.IndexOf("DAY 1");

            if (dayIndex == -1)
                return;

            txtResult.Select(0, dayIndex);

            txtResult.SelectionAlignment =
                HorizontalAlignment.Center;

            txtResult.SelectionFont =
                new Font("Segoe UI", 12, FontStyle.Regular);

            txtResult.SelectionColor =
                Color.FromArgb(70, 70, 70);

            txtResult.DeselectAll();
        }

        private void FormatTimes()
        {
            string text = txtResult.Text;

            for (int i = 0; i < text.Length - 5; i++)
            {
                bool isTime =
                    char.IsDigit(text[i]) &&
                    char.IsDigit(text[i + 1]) &&
                    text[i + 2] == ':' &&
                    char.IsDigit(text[i + 3]) &&
                    char.IsDigit(text[i + 4]);

                if (isTime)
                {
                    txtResult.Select(i, 5);

                    txtResult.SelectionFont =
                        new Font(
                            "Segoe UI",
                            10,
                            FontStyle.Bold);
                }
            }
        }

        private void FormatSummaries()
        {
            foreach (string line in txtResult.Lines)
            {
                if (line.Contains("Summary:"))
                {
                    int start = txtResult.Text.IndexOf(line);

                    if (start < 0)
                        continue;

                    int separator =
                        line.IndexOf("Summary:") + "Summary:".Length;

                    txtResult.Select(start, separator);

                    txtResult.SelectionFont =
                        new Font(
                            "Segoe UI",
                            10,
                            FontStyle.Bold);

                    txtResult.SelectionColor =
                        Color.Teal;

                    txtResult.Select(
                        start + separator,
                        line.Length - separator);

                    txtResult.SelectionFont =
                        new Font(
                            "Segoe UI",
                            10,
                            FontStyle.Regular);

                    txtResult.SelectionColor =
                        Color.DimGray;
                }
            }
        }

        private void FormatTotals()
        {
            foreach (string line in txtResult.Lines)
            {
                if (line.Contains("Day") &&
                    line.Contains("Total Cost"))
                {
                    int start =
                        txtResult.Text.IndexOf(line);

                    txtResult.Select(start, line.Length);

                    txtResult.SelectionFont =
                        new Font(
                            "Segoe UI",
                            12,
                            FontStyle.Bold);

                    txtResult.SelectionColor =
                       Color.FromArgb(0, 110, 170);
                }
            }

            int finalTotal =
                txtResult.Text.LastIndexOf("TOTAL");

            if (finalTotal >= 0)
            {
                txtResult.Select(
                    finalTotal,
                    txtResult.Text.Length - finalTotal);

                txtResult.SelectionFont =
                    new Font(
                        "Segoe UI",
                        14,
                        FontStyle.Bold);

                txtResult.SelectionColor =
                    Color.DarkBlue;
            }
        }

        private void saveBtn_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveDialog = new SaveFileDialog())
            {
                saveDialog.Title = "Save Trip Plan";
                saveDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                saveDialog.FileName = "MyTripPlan";
                saveDialog.DefaultExt = "txt";

                if (saveDialog.ShowDialog() == DialogResult.OK)
                {
                    System.IO.File.WriteAllText(saveDialog.FileName, txtResult.Text);
                    MessageBox.Show("Plan saved!", "Saved",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void newBtn_Click(object sender, EventArgs e)
        {
            QuestionnaireForm newForm = new QuestionnaireForm();
            newForm.Show();
            this.Close();
        }
    }
}