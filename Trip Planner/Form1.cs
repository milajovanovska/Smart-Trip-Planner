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

        Color PRIMARY = Color.FromArgb(140, 190, 230);
        Color PRIMARY_DARK = Color.FromArgb(100, 150, 200);
        Color BACKGROUND = Color.FromArgb(210, 230, 250);
        Color BORDER = Color.FromArgb(180, 210, 240);
        Color TEXT = Color.FromArgb(70, 100, 140);
        Color TEXT_LIGHT = Color.White;

        int currentStep = 1;
        Panel[] steps;

        public Form1()
        {
            InitializeComponent();
            steps = new Panel[] { questionPanel1, questionPanel2, questionPanel3, questionPanel4, questionPanel5, questionPanel6, questionPanel7};

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
            this.Shown += (s, e) => {
                var allStyleButtons = new[] {
        btnMuseum, btnCafes, btnNightLife, btnNature, btnBeaches, btnFood, btnShopping, btnHiddenGems,
        btnRelaxed, btnBalanced, btnPacked, btnWalking,  btnPublicTransport, btnBike, btnTaxiUber, btnRentACar
    };

                foreach (var btn in allStyleButtons)
                {
                    if (btn != null)
                        ApplyRoundCorners(btn, 20);
                }
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

                case 4:
                    lblTitle.Text = "What kind of trip?";
                    ResetStyleButtons();
                    btnContinue.Enabled = false;
                    break;
                case 5:
                    lblTitle.Text = "What excites you?";
                    btnContinue.Enabled = true;
                    break;
                case 6:
                    lblTitle.Text = "How packed should the days be?";
                    break;
                case 7:
                    lblTitle.Text = "How do you want to move?";
                    btnContinue.Enabled = true;
                    break;
                case 8:
                    lblTitle.Text = "A few finishing touches";
                    btnContinue.Enabled = true;
                    break;
                case 9: lblTitle.Text = "Review your plan"; break;
            }
        }

        private void txtDestination_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnBack_MouseEnter(object sender, EventArgs e)
        {
            btnBack.BackColor = PRIMARY;
            btnBack.ForeColor = TEXT_LIGHT;
        }

        private void btnBack_MouseLeave(object sender, EventArgs e)
        {
            btnBack.BackColor = Color.Transparent;
            btnBack.ForeColor = TEXT;
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
                txtDestination.ForeColor = TEXT;
            }
        }

        private void txtDestination_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDestination.Text))
            {
                txtDestination.Text = "e. g. Rome, Sevilla, Japan";
                txtDestination.ForeColor = TEXT;
            }
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
            txtDestination.ForeColor = TEXT;
            btnContinue.Enabled = true;
        }

        private void btnVienna_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Vienna, Austria";
            txtDestination.ForeColor = TEXT;
            btnContinue.Enabled = true;
        }

        private void btnIstanbul_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Istanbul, Turkey";
            txtDestination.ForeColor = TEXT;
            btnContinue.Enabled = true;
        }

        private void btnTokyo_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Tokyo, Japan";
            txtDestination.ForeColor = TEXT;
            btnContinue.Enabled = true;
        }

        private void btnFlorence_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Florence, Italy";
            txtDestination.ForeColor = TEXT;
            btnContinue.Enabled = true;
        }

        private void btnChicago_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Chicago, Illionis";
            txtDestination.ForeColor = TEXT;
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

        private void SelectStyle(Button selectedBtn)
        {
            ResetStyleButtons();
            selectedBtn.FlatAppearance.BorderColor = PRIMARY_DARK;
            selectedBtn.FlatAppearance.BorderSize = 2;
            selectedBtn.BackColor = PRIMARY;
            btnContinue.Enabled = true;
        }

        private void ResetStyleButtons()
        {
            var styleButtons = new[] { btnActive, btnRelaxing, btnAdventure, btnLuxury, btnBackpacking };
            foreach (var btn in styleButtons)
            {
                if (btn != null)
                {
                    btn.FlatAppearance.BorderColor = BORDER;
                    btn.FlatAppearance.BorderSize = 1;
                    btn.BackColor = BACKGROUND;
                }
            }
        }

        private void btnActive_Click(object sender, EventArgs e)
        {
            SelectStyle(btnActive);
        }

        private void btnRelaxing_Click(object sender, EventArgs e)
        {
            SelectStyle(btnRelaxing);
        }

        private void btnAdventure_Click(object sender, EventArgs e)
        {
            SelectStyle(btnAdventure);
        }

        private void btnLuxury_Click(object sender, EventArgs e)
        {
            SelectStyle(btnLuxury);
        }

        private void btnBackpacking_Click(object sender, EventArgs e)
        {
            SelectStyle(btnBackpacking);
        }

        private void ApplyRoundCorners(Button btn, int radius)
        {
            btn.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));
        }

        private void ToggleInterest(Button btn)
        {
            if (btn.BackColor == BACKGROUND)
            {
                btn.BackColor = PRIMARY;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                btn.ForeColor = TEXT_LIGHT;

                btn.Size = new Size(btn.Width + 10, btn.Height + 10);
                btn.Location = new Point(btn.Location.X - 5, btn.Location.Y - 5);
            }
            else
            {
                btn.BackColor = BACKGROUND;
                btn.FlatAppearance.BorderColor = BACKGROUND;
                btn.ForeColor = TEXT;

                btn.Size = new Size(btn.Width - 10, btn.Height - 10);
                btn.Location = new Point(btn.Location.X + 5, btn.Location.Y + 5);
            }

            ApplyRoundCorners(btn, 20);
        }

        private void btnMuseum_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnMuseum);
        }

        private void btnBeaches_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnBeaches);
        }

        private void btnCafes_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnCafes);
        }

        private void btnFood_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnFood);
        }

        private void btnNightLife_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnNightLife);
        }

        private void btnShopping_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnShopping);
        }

        private void btnNature_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnNature);
        }

        private void btnHiddenGems_Click(object sender, EventArgs e)
        {
            ToggleInterest(btnHiddenGems);
        }
        private void SelectPace(Button selectedBtn)
        {
            var paceButtons = new[] { btnRelaxed, btnBalanced, btnPacked };
            foreach (var btn in paceButtons)
            {
                if (btn.BackColor == PRIMARY)
                {
                    btn.Size = new Size(btn.Width - 10, btn.Height - 10);
                    btn.Location = new Point(btn.Location.X + 5, btn.Location.Y + 5);
                }

                btn.BackColor = BACKGROUND;
                btn.FlatAppearance.BorderColor = BORDER;
                btn.ForeColor = TEXT;
                ApplyRoundCorners(btn, 20);
            }

            selectedBtn.BackColor = PRIMARY;
            selectedBtn.FlatAppearance.BorderColor = PRIMARY_DARK;
            selectedBtn.ForeColor = TEXT_LIGHT;

            selectedBtn.Size = new Size(selectedBtn.Width + 10, selectedBtn.Height + 10);
            selectedBtn.Location = new Point(selectedBtn.Location.X - 5, selectedBtn.Location.Y - 5);

            ApplyRoundCorners(selectedBtn, 20);
            btnContinue.Enabled = true;
        }

        private void btnRelaxed_Click(object sender, EventArgs e) { 
            SelectPace(btnRelaxed); 
        }
        private void btnBalanced_Click(object sender, EventArgs e) { SelectPace(btnBalanced); }
        private void btnPacked_Click(object sender, EventArgs e) { SelectPace(btnPacked); }
        private void ToggleTransport(Button btn)
        {
            if (btn.BackColor != PRIMARY)
            {
                btn.BackColor = PRIMARY;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                btn.ForeColor = TEXT_LIGHT;

                btn.Size = new Size(btn.Width + 6, btn.Height + 6);
                btn.Location = new Point(btn.Location.X - 3, btn.Location.Y - 3);
            }
            else
            {
                btn.BackColor = BACKGROUND;
                btn.FlatAppearance.BorderColor = BORDER;
                btn.ForeColor = TEXT;

                btn.Size = new Size(btn.Width - 6, btn.Height - 6);
                btn.Location = new Point(btn.Location.X + 3, btn.Location.Y + 3);
            }
            ApplyRoundCorners(btn, 20);
        }

        private void btnWalking_Click(object sender, EventArgs e)
        {
            ToggleTransport(btnWalking);
        }

        private void btnPublicTransport_Click(object sender, EventArgs e)
        {
            ToggleTransport(btnPublicTransport);

        }

        private void btnRentACar_Click(object sender, EventArgs e)
        {
            ToggleTransport(btnRentACar);

        }

        private void btnBike_Click(object sender, EventArgs e)
        {
            ToggleTransport(btnBike);

        }

        private void btnTaxiUber_Click(object sender, EventArgs e)
        {
            ToggleTransport(btnTaxiUber);

        }
        private void ToggleQuality(Button btn, string title, string description)
        {
            if (btn.BackColor != PRIMARY)
            {
                btn.BackColor = PRIMARY;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                btn.ForeColor = TEXT_LIGHT;
            }
            else
            {
                btn.BackColor = BACKGROUND;
                btn.FlatAppearance.BorderColor = BORDER;
                btn.ForeColor = TEXT;
            }

            ApplyRoundCorners(btn, 20);
        }

   

       
    }
}