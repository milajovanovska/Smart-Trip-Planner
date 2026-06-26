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

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }

        Color PRIMARY = Color.FromArgb(113, 144, 199);
        Color PRIMARY_DARK = Color.FromArgb(46, 39, 90);

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
        private Size baseFormSize;
        private Dictionary<Control, Rectangle> originalBounds = new Dictionary<Control, Rectangle>();
        private Dictionary<Control, Font> originalFonts = new Dictionary<Control, Font>();
        private System.Windows.Forms.Timer resizeTimer;
        private void SetButtonIcon(Button btn, IconChar icon, Color iconColor, int iconSize = 25)
        {
            if (btn == null) return;
            _buttonIconMap[btn] = icon;
            btn.Image = icon.ToBitmap(iconColor, iconSize);
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.TextImageRelation = TextImageRelation.ImageBeforeText;
            btn.Padding = new Padding(10, 0, 0, 0);
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

        private void SetLightTheme()
        {
            PRIMARY = Color.FromArgb(113, 144, 199);

            PRIMARY_DARK = Color.FromArgb(46, 39, 90);

            BACKGROUND = Color.FromArgb(180, 200, 240);

            CARD = Color.White;

            TEXT = Color.FromArgb(64, 64, 64);

            SUBTEXT = Color.FromArgb(100, 116, 139);

            BORDER = Color.FromArgb(230, 233, 237);

            ICON_COLOR = TEXT;

            lblDuration.BackColor = Color.White;

            panel2.BackColor = Color.FromArgb(180, 200, 240);

            rbPreferences.BackColor = Color.White;
            rbPreferences.ForeColor = TEXT;
            rbAvoid.ForeColor = TEXT;

        }
        private void SetDarkTheme()
        {
            PRIMARY = Color.FromArgb(46, 39, 90);

            PRIMARY_DARK = Color.FromArgb(113, 144, 199);

            BACKGROUND = Color.FromArgb(29, 25, 61);

            CARD = Color.FromArgb(75, 70, 105);

            TEXT = Color.FromArgb(240, 240, 240);

            SUBTEXT = Color.FromArgb(148, 163, 184);

            BORDER = Color.FromArgb(60, 60, 60);

            ICON_COLOR = TEXT;

            lblDuration.BackColor = Color.FromArgb(29, 25, 61);

            panel2.BackColor = Color.FromArgb(70, 67, 100);

            rbPreferences.BackColor = Color.FromArgb(70, 67, 100);
            rbPreferences.ForeColor = TEXT;
            rbAvoid.ForeColor = TEXT;

        }
        private void ApplyTheme()
        {
            if (isDarkMode)
                SetDarkTheme();
            else
                SetLightTheme();

            foreach (Control c in GetAllControls(this))
            {
                if (c is Button btn)
                {
                    btn.BackColor = PRIMARY;
                    btn.ForeColor = Color.White;
                    btn.FlatStyle = FlatStyle.Flat;
                    btn.UseVisualStyleBackColor = false;
                    UpdateButtonIconColor(btn, Color.White);
                }
            }

            var allToggleButtons = new[] {
                btnContinue, btnBack,
                btnFlorence, btnChicago, btnTokyo, btnVienna, btnIstanbul, btnLisbon,
                btnActive, btnRelaxing, btnAdventure, btnLuxury, btnBackpacking,
                btnMuseum, btnCafes, btnNightLife, btnNature, btnBeaches, btnFood, btnShopping, btnHiddenGems,
                btnRelaxed, btnBalanced, btnPacked,
                btnWalking, btnPublicTransport, btnBike, btnTaxiUber, btnRentACar
             };

            foreach (var btn in allToggleButtons)
            {
                if (btn.BackColor == PRIMARY_DARK)
                {
                    btn.BackColor = PRIMARY;
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = PRIMARY;
                    UpdateButtonIconColor(btn, Color.White);
                }
            }

            lblTitle.ForeColor = TEXT;
            lblStepCount.ForeColor = TEXT;
            lblPercentage.ForeColor = TEXT;
            labelDuration.ForeColor = TEXT;
            lblBudgetValue.ForeColor = TEXT;

            foreach (Panel panel in steps)
            {
                if (panel == null) continue;
                panel.BackColor = Color.Transparent;

                foreach (Control control in panel.Controls)
                {
                    if (control is Label lbl)
                        lbl.ForeColor = TEXT;
                    else if (control is TextBox txt)
                    {
                        txt.BackColor = CARD;
                        txt.ForeColor = TEXT;
                        txt.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else if (control is DateTimePicker picker)
                    {
                        picker.CalendarForeColor = TEXT;
                        picker.CalendarMonthBackground = CARD;
                        picker.CalendarTitleBackColor = PRIMARY;
                        picker.CalendarTitleForeColor = Color.White;
                        picker.BackColor = CARD;
                        picker.ForeColor = TEXT;
                    }
                    else if (control is TrackBar)
                        control.BackColor = BACKGROUND;
                }
            }

            UpdateButtonIconColor(btnContinue, Color.White, 16);
            UpdateButtonIconColor(btnBack, TEXT, 16);

            this.Invalidate();
            this.Refresh();
            this.Invalidate(true);
        }

        private IEnumerable<Control> GetAllControls(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                yield return c;
                foreach (Control child in GetAllControls(c))
                    yield return child;
            }
        }
        private void ConfigureButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn)
                {
                    btn.FlatStyle = FlatStyle.Flat;
                }

                if (c.HasChildren)
                    ConfigureButtons(c);
            }
        }
        private void UpdateButtonIconColor(Button btn, Color newColor, int iconSize = 18)
        {
            if (btn == null || !_buttonIconMap.ContainsKey(btn)) return;
            IconChar icon = _buttonIconMap[btn];
            btn.Image = icon.ToBitmap(newColor, iconSize);
            btnThemeToggle.BackColor = Color.Transparent;
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

            resizeTimer = new System.Windows.Forms.Timer { Interval = 150 };
            resizeTimer.Tick += (s, e) => { resizeTimer.Stop(); DoActualScale(); };

            this.MinimumSize = new Size(797, 499);

            this.Resize += Form1_Resize;

            ApplyButtonIcons();

            ConfigureButtons(this);


            this.DoubleBuffered = true;

            steps = new Panel[] { questionPanel1, questionPanel2, questionPanel3, questionPanel4, questionPanel5, questionPanel6, questionPanel7, questionPanel8 };

            dateTimePickerStart.Value = DateTime.Today;
            dateTimePickerEnd.Value = DateTime.Today.AddDays(7);


            UpdateUI();

            baseFormSize = this.ClientSize;
            CacheOriginalLayout(this);

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

            ApplyTheme();

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
            btnBack.ForeColor = btnBack.Enabled ? TEXT : Color.Gray;
            UpdateButtonIconColor(btnBack, btnBack.Enabled ? TEXT : Color.Gray, 16);

            switch (currentStep)
            {
                case 1:
                    lblTitle.Text = "Pick your destination";
                    txtDestination_TextChanged_1(null, null);
                    btnBack.Visible = false;
                    btnContinue.Visible = true;
                    break;

                case 2:
                    lblTitle.Text = "When are you travelling?";
                    dateTimePickerStart.MinDate = DateTime.Today;
                    dateTimePickerEnd.MinDate = DateTime.Today;
                    CalculateDuration();
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
                    btnContinue.Enabled = (btnActive.BackColor == PRIMARY_DARK ||
                                   btnRelaxing.BackColor == PRIMARY_DARK ||
                                   btnAdventure.BackColor == PRIMARY_DARK ||
                                   btnLuxury.BackColor == PRIMARY_DARK ||
                                   btnBackpacking.BackColor == PRIMARY_DARK);
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
                    btnContinue.Enabled = (btnRelaxed.BackColor == PRIMARY_DARK ||
                                   btnBalanced.BackColor == PRIMARY_DARK ||
                                   btnPacked.BackColor == PRIMARY_DARK);
                    btnBack.Visible = true;
                    btnContinue.Visible = true;
                    break;
                case 7:
                    lblTitle.Text = "How do you want to move?";
                    btnContinue.Enabled = (btnWalking.BackColor == PRIMARY_DARK ||
                                   btnBike.BackColor == PRIMARY_DARK ||
                                   btnPublicTransport.BackColor == PRIMARY_DARK ||
                                   btnTaxiUber.BackColor == PRIMARY_DARK ||
                                   btnRentACar.BackColor == PRIMARY_DARK);
                    btnBack.Visible = true;
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

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            RoundAllButtons(this);
        }

        private void RoundAllButtons(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                if (c is Button btn)
                {
                    ApplyRoundCorners(btn, 20);
                }

                if (c.HasChildren)
                    RoundAllButtons(c);
            }
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

        private void SelectCityButton(Button selected)
        {
            var cityButtons = new[] { btnLisbon, btnVienna, btnIstanbul, btnTokyo, btnFlorence, btnChicago };
            foreach (var btn in cityButtons)
            {
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY;
                btn.FlatAppearance.BorderSize = 0;
                UpdateButtonIconColor(btn, Color.White);
            }
            selected.BackColor = PRIMARY_DARK;
            selected.FlatAppearance.BorderColor = PRIMARY_DARK;
            UpdateButtonIconColor(selected, Color.White);
        }
        private void btnLisbon_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Lisbon, Portugal";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnLisbon);
            btnContinue.Enabled = true;
        }

        private void btnVienna_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Vienna, Austria";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnVienna);
            btnContinue.Enabled = true;
        }

        private void btnIstanbul_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Istanbul, Turkey";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnIstanbul);
            btnContinue.Enabled = true;
        }

        private void btnTokyo_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Tokyo, Japan";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnTokyo);
            btnContinue.Enabled = true;
        }

        private void btnFlorence_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Florence, Italy";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnFlorence);
            btnContinue.Enabled = true;
        }

        private void btnChicago_Click(object sender, EventArgs e)
        {
            txtDestination.Text = "Chicago, Illinois";
            txtDestination.ForeColor = TEXT;
            SelectCityButton(btnChicago);
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
            selectedBtn.BackColor = PRIMARY_DARK;
            selectedBtn.ForeColor = Color.White;
            selectedBtn.FlatAppearance.BorderColor = PRIMARY_DARK;
            UpdateButtonIconColor(selectedBtn, Color.White);
            btnContinue.Enabled = true;
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
            if (btn == null) return;

            int safeRadius = Math.Min(radius, Math.Min(btn.Width, btn.Height) / 2);

            if (safeRadius < 1) safeRadius = 1;

            var oldRegion = btn.Region;
            btn.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, btn.Width, btn.Height, radius, radius));
            oldRegion?.Dispose();
        }


        private void ToggleInterest(Button btn)
        {
            if (btn.BackColor == PRIMARY)
            {
                btn.BackColor = PRIMARY_DARK;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                UpdateButtonIconColor(btn, Color.White);
            }
            else
            {
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                UpdateButtonIconColor(btn, Color.White);
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
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY;
                btn.FlatAppearance.BorderSize = 0;
                UpdateButtonIconColor(btn, Color.White);
                ApplyRoundCorners(btn, 20);
            }

            selectedBtn.BackColor = PRIMARY_DARK;
            selectedBtn.ForeColor = Color.White;
            selectedBtn.FlatAppearance.BorderColor = Color.White;
            selectedBtn.FlatAppearance.BorderSize = 2;
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
            if (btn.BackColor == PRIMARY)
            {
                btn.BackColor = PRIMARY_DARK;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = Color.White;
                btn.FlatAppearance.BorderSize = 2;
                UpdateButtonIconColor(btn, Color.White);
            }
            else
            {
                btn.BackColor = PRIMARY;
                btn.ForeColor = Color.White;
                btn.FlatAppearance.BorderColor = PRIMARY;
                btn.FlatAppearance.BorderSize = 0;
                UpdateButtonIconColor(btn, Color.White);
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
            if (btn.BackColor == PRIMARY_DARK)
            {
                btn.BackColor = PRIMARY;
                btn.FlatAppearance.BorderColor = PRIMARY;
                btn.ForeColor = TEXT_LIGHT;
            }
            else
            {
                btn.BackColor = PRIMARY_DARK;
                btn.FlatAppearance.BorderColor = PRIMARY_DARK;
                btn.ForeColor = TEXT;
            }

            ApplyRoundCorners(btn, 20);
        }

        private async void btnGenerate_Click(object sender, EventArgs e)
        {

            var requestData = CollectFormData();

            LoadingForm loadingForm = new LoadingForm();
            loadingForm.Show();
            loadingForm.BringToFront();
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
            if (btnActive.BackColor == PRIMARY_DARK) return "Active";
            if (btnRelaxing.BackColor == PRIMARY_DARK) return "Relaxing";
            if (btnAdventure.BackColor == PRIMARY_DARK) return "Adventure";
            if (btnLuxury.BackColor == PRIMARY_DARK) return "Luxury";
            if (btnBackpacking.BackColor == PRIMARY_DARK) return "Backpacking";
            return "";
        }

        private List<string> GetSelectedInterests()
        {
            var interests = new List<string>();
            if (btnMuseum.BackColor == PRIMARY_DARK) interests.Add("Museums");
            if (btnFood.BackColor == PRIMARY_DARK) interests.Add("Food");
            if (btnNightLife.BackColor == PRIMARY_DARK) interests.Add("Nightlife");
            if (btnNature.BackColor == PRIMARY_DARK) interests.Add("Nature");
            if (btnShopping.BackColor == PRIMARY_DARK) interests.Add("Shopping");
            if (btnBeaches.BackColor == PRIMARY_DARK) interests.Add("Beaches");
            if (btnCafes.BackColor == PRIMARY_DARK) interests.Add("Cafes");
            if (btnHiddenGems.BackColor == PRIMARY_DARK) interests.Add("Hidden Gems");
            return interests;
        }

        private string GetSelectedPace()
        {
            if (btnRelaxed.BackColor == PRIMARY_DARK) return "Relaxed";
            if (btnBalanced.BackColor == PRIMARY_DARK) return "Balanced";
            if (btnPacked.BackColor == PRIMARY_DARK) return "Packed";
            return "";
        }

        private List<string> GetSelectedTransport()
        {
            var transport = new List<string>();
            if (btnWalking.BackColor == PRIMARY_DARK) transport.Add("Walking");
            if (btnBike.BackColor == PRIMARY_DARK) transport.Add("Bike");
            if (btnPublicTransport.BackColor == PRIMARY_DARK) transport.Add("Public Transport");
            if (btnTaxiUber.BackColor == PRIMARY_DARK) transport.Add("Taxi");
            if (btnRentACar.BackColor == PRIMARY_DARK) transport.Add("Car Rental");
            return transport;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
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
            resizeTimer.Stop();
            resizeTimer.Start();
        }

        private void btnThemeToggle_Click(object sender, EventArgs e)
        {
            isDarkMode = !isDarkMode;

            if (isDarkMode)
            {
                btnThemeToggle.IconChar = IconChar.Sun;
                btnThemeToggle.IconColor = Color.Gold;
            }
            else
            {
                btnThemeToggle.IconChar = IconChar.Moon;
                btnThemeToggle.IconColor = Color.Black;
            }

            ApplyTheme();
        }

        private void rbAvoid_Click(object sender, EventArgs e)
        {
            rbAvoid.Checked = !rbAvoid.Checked;
        }

        private void txtDestination_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnContinue.PerformClick();
            } 
        }

        private void CacheOriginalLayout(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                originalBounds[c] = c.Bounds;
                originalFonts[c] = c.Font;
                if (c.HasChildren)
                    CacheOriginalLayout(c);
            }
        }

        private void ScaleLayout(Control parent, float scaleX, float scaleY)
        {
            foreach (Control c in parent.Controls)
            {
                if (!originalBounds.ContainsKey(c)) continue;

                Rectangle orig = originalBounds[c];
                c.Location = new Point((int)(orig.X * scaleX), (int)(orig.Y * scaleY));
                c.Size = new Size((int)(orig.Width * scaleX), (int)(orig.Height * scaleY));

                float fontScale = Math.Min(scaleX, scaleY);
                Font origFont = originalFonts[c];
                float newSize = origFont.Size * fontScale;
                if (newSize > 4f)
                {
                    Font oldFont = c.Font;
                    c.Font = new Font(origFont.FontFamily, newSize, origFont.Style);
                    if (oldFont != null && oldFont != origFont)
                        oldFont.Dispose();
                }

                if (c is DateTimePicker dtp)
                {
                    var oldRegion = dtp.Region;
                    dtp.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, dtp.Width, dtp.Height, (int)(15 * fontScale), (int)(15 * fontScale)));
                    oldRegion?.Dispose();
                }

                if (c is Panel pnl && pnl.Name == "lblDuration")
                {
                    var oldRegion = pnl.Region;
                    pnl.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, pnl.Width, pnl.Height, (int)(20 * fontScale), (int)(20 * fontScale)));
                    oldRegion?.Dispose();
                }

                if (c.HasChildren)
                    ScaleLayout(c, scaleX, scaleY);
            }
        }

        private void DoActualScale()
        {
            if (baseFormSize.Width == 0 || baseFormSize.Height == 0) return;

            this.SuspendLayout();

            float scaleX = (float)this.ClientSize.Width / baseFormSize.Width;
            float scaleY = (float)this.ClientSize.Height / baseFormSize.Height;

            ScaleLayout(this, scaleX, scaleY);
            RoundAllButtons(this);

            btnContinue.BringToFront();
            btnBack.BringToFront();
            btnThemeToggle.BringToFront();

            this.ResumeLayout(true);

            this.Invalidate(true);
            this.Update();
        }
    }
}