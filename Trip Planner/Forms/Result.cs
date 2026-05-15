using System;
using System.Windows.Forms;

namespace Trip_Planner.Forms
{
    public partial class Result : Form
    {
        public Result(string resultText)
        {
            InitializeComponent();

            txtResult.Text = resultText;
        }
    }
}