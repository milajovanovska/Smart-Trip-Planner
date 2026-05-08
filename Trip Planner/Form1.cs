using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Trip_Planner
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);

      
        Color PRIMARY = Color.FromArgb(59, 130, 246);
        Color PRIMARY_DARK = Color.FromArgb(37, 99, 235);

        Color BACKGROUND = Color.FromArgb(245, 247, 250);
        Color CARD = Color.White;

        Color BORDER = Color.FromArgb(225, 230, 235);

        Color TEXT = Color.FromArgb(30, 41, 59);
        Color SUBTEXT = Color.FromArgb(100, 116, 139);

        Color SUCCESS = Color.FromArgb(34, 197, 94);
        Color TEXT_LIGHT = Color.White;


        private GraphicsPath GetRoundedRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            return path;
        }


        int currentStep = 1;
        Panel[] steps;

        public Form1()
        {
            InitializeComponent();
            ApplyHoverEffects();

            progressBarLoading.ForeColor = Color.CornflowerBlue;

            this.DoubleBuffered = true;

            steps = new Panel[] { questionPanel1, questionPanel2, questionPanel3, questionPanel4, questionPanel5, questionPanel6, questionPanel7,questionPanel8};

            dateTimePickerStart.Value = DateTime.Today;
            dateTimePickerEnd.Value = DateTime.Today.AddDays(7);


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

            UpdateProgress(currentStep, 8);

            btnBack.Enabled = (currentStep > 1);
            if (btnBack.Enabled)
            {
                btnBack.ForeColor = Color.Black;
            }
            else
            {
                btnBack.ForeColor = Color.Gray;
            }

            switch (currentStep)
            {
                case 1:
                    lblTitle.Text = "Pick your destination";
                    txtDestination_TextChanged(null, null);
                    btnContinue.Enabled = false;
                    btnBack.Visible= false;
                    break;

                case 2:
                    lblTitle.Text = "When are you travelling?";
                    dateTimePickerStart.MinDate = DateTime.Today;
                    dateTimePickerEnd.MinDate = DateTime.Today;
                    CalculateDuration();
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    break;

                case 3:
                    lblTitle.Text = "What's your total budget?";
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    break;

                case 4:
                    lblTitle.Text = "What kind of trip?";
                    ResetStyleButtons();
                    btnContinue.Enabled = false;
                    btnBack.Visible = true;
                    break;
                case 5:
                    lblTitle.Text = "What excites you?";
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    break;
                case 6:
                    lblTitle.Text = "How packed should the days be?";
                    btnContinue.Enabled = false;
                    btnBack.Visible = true;
                    break;
                case 7:
                    lblTitle.Text = "How do you want to move?";
                    btnBack.Visible = true;
                    btnContinue.Enabled = false;
                    break;
                case 8:
                    lblTitle.Text = "A few finishing touches";
                    btnContinue.Visible = false;
                    btnBack.Visible = true;
                    break;
                
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

            int days = (end - start).Days ;

            if (days >= 1)
            {
                labelDuration.Text = $"Duration: {days} {(days == 1 ? "day" : "days")}";
                btnContinue.Enabled = true;
            }
            else
            {
                labelDuration.Text = "End date must be after start date";
                btnContinue.Enabled = false;
            }
        }
        private void btnContinue_Click_1(object sender, EventArgs e)
        {
            if (currentStep < 8)
            {
                currentStep++;
                UpdateUI();
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
            txtBudgetInput.Text = trkBudget_Scroll.Value.ToString() + " €";
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
            bool selected = btn.BackColor == BACKGROUND;

            if (selected)
            {
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                btn.FlatAppearance.BorderSize = 2;
            }
            else
            {
                btn.BackColor = CARD;
                btn.ForeColor = TEXT;
                btn.FlatAppearance.BorderColor = BORDER;
                btn.FlatAppearance.BorderSize = 1;
            }
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
            btnContinue.Enabled = true;
            ToggleTransport(btnWalking);
        }

        private void btnPublicTransport_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = true;
            ToggleTransport(btnPublicTransport);

        }

        private void btnRentACar_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = true;
            ToggleTransport(btnRentACar);
           
        }

        private void btnBike_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = true;
            ToggleTransport(btnBike);
            

        }

        private void btnTaxiUber_Click(object sender, EventArgs e)
        {
            btnContinue.Enabled = true;
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

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Generating your plan...");
            panelLoading.Visible = true;
            panelLoading.BringToFront();
            //fadeValue = 0; 
            panelLoading.BackColor = Color.FromArgb(0, 240, 245, 255);
            timerFade.Start();

        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Color lightColor = Color.White;
            Color darkColor = Color.FromArgb(180, 200, 240);

            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, lightColor, darkColor, 45F))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void panelProgressBarBack_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush brush = new SolidBrush(PRIMARY))
            {
                using (GraphicsPath path = GetRoundedRect(
                    new Rectangle(0, 0, panelProgressBar.Width, panelProgressBar.Height), 10))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }
        private void AddHoverEffect(Button btn, Color hoverBack, Color hoverFore, Color normalBack, Color normalFore)
        {
            btn.MouseEnter += (s, e) => {
                btn.BackColor = hoverBack; btn.ForeColor = hoverFore;
            };
            btn.MouseLeave += (s, e) => {
                btn.BackColor = normalBack; btn.ForeColor = normalFore;
            };
        }

        private void ApplyHoverEffects() {
            var interestButtons = new[] {btnMuseum, btnCafes, btnNightLife, btnNature, btnBeaches, btnFood, btnShopping, btnHiddenGems }; 
            foreach (var btn in interestButtons) { 
                AddHoverEffect(btn, PRIMARY, TEXT_LIGHT, BACKGROUND, TEXT);
            }
            var destinationButtons = new[] { btnLisbon, btnVienna, btnIstanbul, btnTokyo, btnFlorence, btnChicago };
            foreach (var btn in destinationButtons) { 
                AddHoverEffect(btn, PRIMARY, TEXT_LIGHT, CARD, TEXT); 
            } 
        }

    }
}