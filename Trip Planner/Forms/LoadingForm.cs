using System;
using System.Drawing;
using System.Windows.Forms;

namespace Trip_Planner.Forms
{
    public partial class LoadingForm : Form
    {
        int progressValue = 0;
        public LoadingForm()
        {
            InitializeComponent();


            this.AutoScaleMode = AutoScaleMode.None;

            this.StartPosition = FormStartPosition.CenterScreen;

         
            timerLoading.Interval = 50; 
            progressBarLoading.Minimum = 0;
            progressBarLoading.Maximum = 100;
            progressBarLoading.Value = 0;

            progressBarLoading.Style = ProgressBarStyle.Continuous;

            timerLoading.Start();
        }

      
        private void timerLoading_Tick_1(object sender, EventArgs e)
        {
            int step = 5;

            if (progressBarLoading.Value < 100)
            {
                progressBarLoading.Value += step;

                if (progressBarLoading.Value > 100)
                    progressBarLoading.Value = 100;
            }
            else
            {
                timerLoading.Stop();
                this.Close();
            }

        }

        private void LoadingForm_Load_1(object sender, EventArgs e)
        {
            lblGeneratingPlan.Left = (this.ClientSize.Width - lblGeneratingPlan.Width) / 2;
            progressBarLoading.Left = (this.ClientSize.Width - progressBarLoading.Width) / 2;
        }
    }
}