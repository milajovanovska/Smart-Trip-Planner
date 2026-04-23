using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trip_Planner
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);


        int currentStep = 1;
        Panel[] steps;
        
        public Form1()
        {
            InitializeComponent();
            steps = new Panel[] { questionPanel1 , questionPanel2, questionPanel3 };

            UpdateUI();

            this.Shown += (s, e) => {
                btnContinue.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnContinue.Width, btnContinue.Height, 30, 30));
            };
            this.Shown += (s, e) => {
                btnBack.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 30, 30));
            };
            this.Shown += (s, e) => {
                dateTimePickerStart.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, dateTimePickerStart.Width, dateTimePickerStart.Height, 15, 15));
               dateTimePickerEnd.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, dateTimePickerEnd.Width, dateTimePickerEnd.Height, 15, 15));
                lblDuration.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, lblDuration.Width, lblDuration.Height, 20, 20));
            };

        }
        private void UpdateUI()
        {
            for (int i = 0; i < steps.Length; i++)
            {
                if (steps[i] != null)
                    steps[i].Visible = (i == currentStep - 1);
            }

            UpdateProgress(currentStep, 9);

            btnBack.Enabled = (currentStep > 1);

            switch (currentStep)
            {
                case 1:
                    lblTitle.Text = "Pick your destination";
                    txtDestination_TextChanged(null, null);
                    break;

                case 2:
                    lblTitle.Text = "When are you travelling?";
                    dateTimePickerStart.MinDate = DateTime.Today;
                    dateTimePickerEnd.MinDate = DateTime.Today;
                    CalculateDuration();
                    break;

                case 3:
                    lblTitle.Text = "What's your total budget?";
                    btnContinue.Enabled = true;
                    break;

                case 4: lblTitle.Text = "Travel style?"; break;
                case 5: lblTitle.Text = "Interests?"; break;
                case 6: lblTitle.Text = "Pace of travel?"; break;
                case 7: lblTitle.Text = "Transport preference?"; break;
                case 8: lblTitle.Text = "Almost there!"; break;
                case 9: lblTitle.Text = "Review your plan"; break;
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void btnBack_MouseEnter(object sender, EventArgs e)
        {
            btnBack.BackColor = Color.Orange;
            btnBack.ForeColor = Color.White;
        }

        private void btnBack_MouseLeave(object sender, EventArgs e)
        {
            btnBack.BackColor = Color.Transparent; 
            btnBack.ForeColor = Color.Gray;
        }
        private void UpdateProgress(int step, int totalSteps)
        {
            lblStepCount.Text = $"Step {step} of {totalSteps}";

            int percentage = (int)(((double)step / totalSteps) * 100);
            lblPercentage.Text = $"{percentage}%";

            int fullWidth = panelProgressBarBack.Width;
            int newWidth = (int)((double)step / totalSteps * fullWidth);

            panelProgressBar.Width = newWidth;
        }
        private void txtDestination_Enter(object sender, EventArgs e)
        {
            if (txtDestination.Text == "e. g. Rome, Sevilla, Japan")
            {
                txtDestination.Text = "";
                txtDestination.ForeColor = Color.Black; 
            }
        }

        private void txtDestination_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                txtDestination.Text = "e. g. Rome, Sevilla, Japan";
                txtDestination.ForeColor = Color.Gray;
            }
        }
        private void txtDestination_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void btnContinue_Click(object sender, EventArgs e)
        {
          
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            
        }

        private void lblPercentage_Click(object sender, EventArgs e)
        {

        }

        private void btnLisbon_Click_2(object sender, EventArgs e)
        {
           
        }

       
        private void CalculateDuration()
        {
            DateTime start = dateTimePickerStart.Value.Date;
            DateTime end = dateTimePickerEnd.Value.Date;

            TimeSpan difference = end - start;
            int days = difference.Days + 1;

            if (days >= 1)
            {
                lblDuration.Text = $"Duration: {days} days";
                btnContinue.Enabled = true;
            }
            else
            {
                lblDuration.Text = "End date is before start date";
                btnContinue.Enabled = false;
            }
        }

        private void dateTimePickerStart_ValueChanged(object sender, EventArgs e)
        {
        }

        private void dateTimePickerEnd_ValueChanged(object sender, EventArgs e)
        {
        }
        private void dateTimePickerEnd_ValueChanged_1(object sender, EventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void trkBudget_Scroll_Scroll(object sender, EventArgs e)
        {
           
        }

        private void txtBudgetTotal_TextChanged(object sender, EventArgs e)
        {
            
        }

       

        private void btnContinue_Click_1(object sender, EventArgs e)
        {
            if (currentStep < 9)
            {
                currentStep++;
                UpdateUI();
            }
            else
            {
                MessageBox.Show("Generating your plan...");
            }
        }
        private void btnBack_Click_1(object sender, EventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                UpdateUI();
            }
        }

        private void btnLisbon_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Lisbon, Portugal";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void btnVienna_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Vienna, Austria";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void btnIstanbul_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Istanbul, Turkey";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void btnTokyo_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Tokyo, Japan";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void btnFlorence_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Florence, Italy";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void btnChicago_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Chicago, Illionis";
            txtDestination.ForeColor = Color.Black;
            btnContinue.Enabled = true;
        }

        private void txtDestination_TextChanged_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtDestination.Text) && txtDestination.Text != "e. g. Rome, Sevilla, Japan")
            {
                btnContinue.Enabled = true;
            }
            else
            {
                btnContinue.Enabled = false;
            }
        }

        private void dateTimePickerStart_ValueChanged_1(object sender, EventArgs e)
        {
            CalculateDuration();

        }

        private void dateTimePickerEnd_ValueChanged_2(object sender, EventArgs e)
        {
            CalculateDuration();

        }

        private void lblDuration_Paint(object sender, PaintEventArgs e)
        {

        }

        private void trkBudget_Scroll_Scroll_1(object sender, EventArgs e)
        {
            lblBudgetValue.Text = trkBudget_Scroll.Value.ToString("N0") + " €";
            txtBudgetInput.Text = trkBudget_Scroll.Value.ToString();
        }

        private void txtBudgetInput_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtBudgetInput.Text, out int val))
            {
                if (val >= trkBudget_Scroll.Minimum && val <= trkBudget_Scroll.Maximum)
                {
                    trkBudget_Scroll.Value = val;
                    lblBudgetValue.Text = val.ToString("N0") + " €";
                }
            }
        }
    }
}
