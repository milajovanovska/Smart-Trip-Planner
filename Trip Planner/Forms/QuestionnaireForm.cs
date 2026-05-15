using FontAwesome.Sharp;
using Microsoft.IdentityModel.Protocols;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Windows.Forms;
using Trip_Planner.Forms;
using Trip_Planner.Models;
using Trip_Planner.Services;


namespace Trip_Planner
{
    public partial class QuestionnaireForm : Form
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

        Color ICON_COLOR = Color.FromArgb(30, 41, 59);

        bool isDarkMode = false;

        private readonly Dictionary<Button, IconChar> _buttonIconMap = new Dictionary<Button, IconChar>();

        private void SetButtonIcon(Button btn, IconChar icon, Color iconColor, int iconSize = 18)
        {
            if (btn == null) return;
            _buttonIconMap[btn] = icon;
            btn.Image = icon.ToBitmap(iconColor, iconSize);
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextAlign = ContentAlignment.MiddleRight;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.Padding = new Padding(10, 0, 10, 0);
        }
        private void ApplyButtonIcons()
        {
            SetButtonIcon(btnLisbon, IconChar.MapMarkerAlt, ICON_COLOR);
            SetButtonIcon(btnVienna, IconChar.MapMarkerAlt, ICON_COLOR);
            SetButtonIcon(btnIstanbul, IconChar.MapMarkerAlt, ICON_COLOR);
            SetButtonIcon(btnTokyo, IconChar.MapMarkerAlt, ICON_COLOR);
            SetButtonIcon(btnFlorence, IconChar.MapMarkerAlt, ICON_COLOR);
            SetButtonIcon(btnChicago, IconChar.MapMarkerAlt, ICON_COLOR);


            SetButtonIcon(btnMuseum, IconChar.Building, ICON_COLOR);
            SetButtonIcon(btnCafes, IconChar.MugHot, ICON_COLOR);
            SetButtonIcon(btnNightLife, IconChar.GlassMartini, ICON_COLOR);
            SetButtonIcon(btnNature, IconChar.Tree, ICON_COLOR);
            SetButtonIcon(btnBeaches, IconChar.UmbrellaBeach, ICON_COLOR);
            SetButtonIcon(btnFood, IconChar.Utensils, ICON_COLOR);
            SetButtonIcon(btnShopping, IconChar.ShoppingBag, ICON_COLOR);
            SetButtonIcon(btnHiddenGems, IconChar.Gem, ICON_COLOR);


            SetButtonIcon(btnActive, IconChar.Running, ICON_COLOR);
            SetButtonIcon(btnRelaxing, IconChar.Smile, ICON_COLOR);
            SetButtonIcon(btnAdventure, IconChar.Mountain, ICON_COLOR);
            SetButtonIcon(btnLuxury, IconChar.Crown, ICON_COLOR);
            SetButtonIcon(btnBackpacking, IconChar.Hiking, ICON_COLOR);


            SetButtonIcon(btnRelaxed, IconChar.Moon, ICON_COLOR);
            SetButtonIcon(btnBalanced, IconChar.BalanceScale, ICON_COLOR);
            SetButtonIcon(btnPacked, IconChar.Bolt, ICON_COLOR);


            SetButtonIcon(btnWalking, IconChar.Walking, ICON_COLOR);
            SetButtonIcon(btnPublicTransport, IconChar.Bus, ICON_COLOR);
            SetButtonIcon(btnBike, IconChar.Bicycle, ICON_COLOR);
            SetButtonIcon(btnTaxiUber, IconChar.Car, ICON_COLOR);
            SetButtonIcon(btnRentACar, IconChar.Key, ICON_COLOR);


            SetButtonIcon(btnContinue, IconChar.ArrowRight, Color.White, 16);
            SetButtonIcon(btnBack, IconChar.ArrowLeft, ICON_COLOR, 16);
        }
        private void ApplyTheme()
        {
            /*  if (isDarkMode)
              {
                  BACKGROUND = Color.FromArgb(30, 30, 30);
                  CARD = Color.FromArgb(45, 45, 45);
                  TEXT = Color.FromArgb(240, 240, 240);
                  BORDER = Color.FromArgb(60, 60, 60);
                  PRIMARY_DARK = Color.FromArgb(80, 120, 200);
              }
              else
              {
                  BACKGROUND = Color.FromArgb(245, 247, 250);
                  CARD = Color.White;
                  TEXT = Color.FromArgb(64, 64, 64);
                  BORDER = Color.FromArgb(230, 233, 237);
                  PRIMARY_DARK = Color.FromArgb(70, 130, 180);
              }

              this.BackColor = BACKGROUND;
              lblTitle.ForeColor = TEXT;
              lblStepCount.ForeColor = TEXT;
              lblPercentage.ForeColor = TEXT;

              foreach (var panel in steps)
              {
                  if (panel == null) continue;

                  panel.BackColor = Color.Transparent;
                  foreach (Control child in panel.Controls)
                  {
                      if (child is Label)
                      {
                          child.ForeColor = TEXT;
                      }
                      else if (child is TextBox txt)
                      {
                          txt.ForeColor = Color.FromArgb(30, 41, 59);
                      }
                      else if (child is Button btn)
                      {
                          if (btn.BackColor != PRIMARY)
                          {
                              btn.ForeColor = Color.FromArgb(30, 41, 59);
                          }
                      }
                  }
              }

              this.Invalidate();
            */
        }
        private void UpdateButtonIconColor(Button btn, Color newColor, int iconSize = 18)
        {
            if (btn == null || !_buttonIconMap.ContainsKey(btn)) return;
            IconChar icon = _buttonIconMap[btn];
            btn.Image = icon.ToBitmap(newColor, iconSize);
        }

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

        public QuestionnaireForm()
        {
            InitializeComponent();

            ApplyButtonIcons();

            this.DoubleBuffered = true;

            steps = new Panel[] { questionPanel1, questionPanel2, questionPanel3, questionPanel4, questionPanel5, questionPanel6, questionPanel7, questionPanel8 };

            dateTimePickerStart.Value = DateTime.Today;
            dateTimePickerEnd.Value = DateTime.Today.AddDays(7);


            UpdateUI();

            this.Shown += (s, e) =>
            {
                btnContinue.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnContinue.Width, btnContinue.Height, 30, 30));
            };
            this.Shown += (s, e) =>
            {
                btnBack.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, btnBack.Width, btnBack.Height, 30, 30));
            };
            this.Shown += (s, e) =>
            {
                dateTimePickerStart.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, dateTimePickerStart.Width, dateTimePickerStart.Height, 15, 15));
                dateTimePickerEnd.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, dateTimePickerEnd.Width, dateTimePickerEnd.Height, 15, 15));
                lblDuration.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, lblDuration.Width, lblDuration.Height, 20, 20));
            };
            this.Shown += (s, e) =>
            {
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
                    btnBack.Visible = false;
                    btnContinue.Visible = true;
                    break;

                case 2:
                    lblTitle.Text = "When are you travelling?";
                    dateTimePickerStart.MinDate = DateTime.Today;
                    dateTimePickerEnd.MinDate = DateTime.Today;
                    CalculateDuration();
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;

                case 3:
                    lblTitle.Text = "What's your total budget?";
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;

                case 4:
                    lblTitle.Text = "What kind of trip?";
                    ResetStyleButtons();
                    btnContinue.Enabled = false;
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;
                case 5:
                    lblTitle.Text = "What excites you?";
                    btnContinue.Enabled = true;
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;
                case 6:
                    lblTitle.Text = "How packed should the days be?";
                    btnContinue.Enabled = false;
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;
                case 7:
                    lblTitle.Text = "How do you want to move?";
                    btnBack.Visible = true;
                    btnContinue.Enabled = false;
                    btnContinue.Visible = true;
                    break;
                case 8:
                    lblTitle.Text = "A few finishing touches";
                    btnContinue.Visible = false;
                    btnBack.Visible = true;
                    rbAvoid.Checked = false;
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

            int days = (end - start).Days;

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
            selectedBtn.BackColor = PRIMARY;
            selectedBtn.ForeColor = Color.White;
            UpdateButtonIconColor(selectedBtn, Color.White);
            btnContinue.Enabled = true;
        }

        private void ResetStyleButtons()
        {
            var styleButtons = new[] { btnActive, btnRelaxing, btnAdventure, btnLuxury, btnBackpacking };
            foreach (var btn in styleButtons)
            {
                btn.BackColor = BACKGROUND;
                btn.ForeColor = TEXT;
                UpdateButtonIconColor(btn, TEXT);
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
            if (btn.BackColor != PRIMARY)
            {
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                UpdateButtonIconColor(btn, Color.White);
            }
            else
            {
                btn.BackColor = CARD;
                btn.ForeColor = TEXT;
                UpdateButtonIconColor(btn, TEXT);
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

        private void btnRelaxed_Click(object sender, EventArgs e)
        {
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

        private async void btnGenerate_Click(object sender, EventArgs e)
        {

            var requestData = CollectFormData();

            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            Application.DoEvents();

            this.Enabled = false;

            TripGeneratorService service = new TripGeneratorService();

            string generatedResult = await service.GenerateTripAsync(requestData);

            loadingForm.Close();

            this.Enabled = true;

            Result resultForm = new Result(generatedResult);

            resultForm.Show();

            this.Hide();
        }
        private TripRequest CollectFormData()
        {
            return new TripRequest
            {
                Destination = txtDestination.Text,

                StartDate = dateTimePickerStart.Value,

                EndDate = dateTimePickerEnd.Value,

                Budget = trkBudget_Scroll.Value,

                TravelStyle = GetSelectedTravelStyle(),

                Interests = GetSelectedInterests(),

                Pace = GetSelectedPace(),

                TransportPreferences = GetSelectedTransport()
            };
        }
        private string GetSelectedTravelStyle()
        {
            if (btnActive.BackColor == PRIMARY)
                return "Active";

            if (btnRelaxing.BackColor == PRIMARY)
                return "Relaxing";

            if (btnAdventure.BackColor == PRIMARY)
                return "Adventure";

            if (btnLuxury.BackColor == PRIMARY)
                return "Luxury";

            if (btnBackpacking.BackColor == PRIMARY)
                return "Backpacking";

            return "";
        }
        private List<string> GetSelectedInterests()
        {
            List<string> interests = new List<string>();

            if (btnMuseum.BackColor == PRIMARY)
                interests.Add("Museums");

            if (btnFood.BackColor == PRIMARY)
                interests.Add("Food");

            if (btnNightLife.BackColor == PRIMARY)
                interests.Add("Nightlife");

            if (btnNature.BackColor == PRIMARY)
                interests.Add("Nature");

            if (btnShopping.BackColor == PRIMARY)
                interests.Add("Shopping");

            if (btnBeaches.BackColor == PRIMARY)
                interests.Add("Beaches");

            if (btnCafes.BackColor == PRIMARY)
                interests.Add("Cafes");

            if (btnHiddenGems.BackColor == PRIMARY)
                interests.Add("Hidden Gems");

            return interests;
        }
        private string GetSelectedPace()
        {
            if (btnRelaxed.BackColor == PRIMARY)
                return "Relaxed";

            if (btnBalanced.BackColor == PRIMARY)
                return "Balanced";

            if (btnPacked.BackColor == PRIMARY)
                return "Packed";

            return "";
        }
        private List<string> GetSelectedTransport()
        {
            List<string> transport = new List<string>();

            if (btnWalking.BackColor == PRIMARY)
                transport.Add("Walking");

            if (btnBike.BackColor == PRIMARY)
                transport.Add("Bike");

            if (btnPublicTransport.BackColor == PRIMARY)
                transport.Add("Public Transport");

            if (btnTaxiUber.BackColor == PRIMARY)
                transport.Add("Taxi");

            if (btnRentACar.BackColor == PRIMARY)
                transport.Add("Car Rental");

            return transport;
        }


        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Color lightColor = Color.White;
            //Color darkColor = Color.FromArgb(180, 200, 240);

            //using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, lightColor, darkColor, 45F))
            //{
            //    e.Graphics.FillRectangle(brush, this.ClientRectangle);
            //}

            using (LinearGradientBrush brush = new LinearGradientBrush(this.ClientRectangle, CARD, BACKGROUND, 45F))
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


        private void Form1_Resize(object sender, EventArgs e)
        {
            //panelLoading.Left = (this.ClientSize.Width - panelLoading.Width) / 2;
            //panelLoading.Top = (this.ClientSize.Height - panelLoading.Height) / 2;
        }

        private void btnThemeToggle_Click(object sender, EventArgs e)
        {
            /*    isDarkMode = !isDarkMode;
                if (isDarkMode)
                {
                    btnThemeToggle.IconChar = IconChar.Sun;
                    btnThemeToggle.IconColor = Color.Gold;
                }
                else
                {
                    btnThemeToggle.IconChar = IconChar.Moon;
                    btnThemeToggle.IconColor = TEXT;
                }
             ApplyTheme();  
            */
        }

        private void rbAvoid_Click(object sender, EventArgs e)
        {
            rbAvoid.Checked = !rbAvoid.Checked;
        }

        private async void btnTestAI_Click(object sender, EventArgs e)
        {
            try
            {
                btnTestAI.Enabled = false;

                string apiKey =
     ConfigurationManager.AppSettings["SerpApiKey"];

                SerpApiService service =
                    new SerpApiService(apiKey);

                string json =
                    await service.SearchPlacesAsync(
                        "best museums in Vienna");

                List<Activity> activities =
                    service.ParseActivities(json);

                txtResponse.Clear();

                foreach (var activity in activities)
                {
                    txtResponse.AppendText(
                        $"{activity.Name}\r\n");

                    txtResponse.AppendText(
                        $"Rating: {activity.Rating}\r\n");

                    txtResponse.AppendText(
                        $"Address: {activity.Address}\r\n");

                    txtResponse.AppendText(
                        $"Description: {activity.Description}\r\n");

                    txtResponse.AppendText(
                        $"--------------------------\r\n\r\n");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                btnTestAI.Enabled = true;
            }
        }
    }
}