namespace Trip_Planner.Forms
{
    partial class LoadingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelLoading = new System.Windows.Forms.Panel();
            this.lblGeneratingPlan = new System.Windows.Forms.Label();
            this.progressBarLoading = new System.Windows.Forms.ProgressBar();
            this.timerLoading = new System.Windows.Forms.Timer(this.components);
            this.panelLoading.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelLoading
            // 
            this.panelLoading.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(240)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.panelLoading.Controls.Add(this.lblGeneratingPlan);
            this.panelLoading.Controls.Add(this.progressBarLoading);
            this.panelLoading.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLoading.Location = new System.Drawing.Point(0, 0);
            this.panelLoading.Name = "panelLoading";
            this.panelLoading.Size = new System.Drawing.Size(1033, 541);
            this.panelLoading.TabIndex = 32;
            // 
            // lblGeneratingPlan
            // 
            this.lblGeneratingPlan.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblGeneratingPlan.Font = new System.Drawing.Font("Modern No. 20", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGeneratingPlan.Location = new System.Drawing.Point(203, 138);
            this.lblGeneratingPlan.Name = "lblGeneratingPlan";
            this.lblGeneratingPlan.Size = new System.Drawing.Size(576, 73);
            this.lblGeneratingPlan.TabIndex = 2;
            this.lblGeneratingPlan.Text = "Generating your plan...";
            this.lblGeneratingPlan.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBarLoading
            // 
            this.progressBarLoading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.progressBarLoading.Location = new System.Drawing.Point(114, 254);
            this.progressBarLoading.MarqueeAnimationSpeed = 30;
            this.progressBarLoading.Name = "progressBarLoading";
            this.progressBarLoading.Size = new System.Drawing.Size(750, 43);
            this.progressBarLoading.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBarLoading.TabIndex = 1;
            // 
            // timerLoading
            // 
            this.timerLoading.Enabled = true;
            this.timerLoading.Interval = 50;
            this.timerLoading.Tick += new System.EventHandler(this.timerLoading_Tick_1);
            // 
            // LoadingForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1033, 541);
            this.Controls.Add(this.panelLoading);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoadingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoadingForm";
            this.Load += new System.EventHandler(this.LoadingForm_Load_1);
            this.panelLoading.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelLoading;
        private System.Windows.Forms.Label lblGeneratingPlan;
        private System.Windows.Forms.ProgressBar progressBarLoading;
        private System.Windows.Forms.Timer timerLoading;
    }
}