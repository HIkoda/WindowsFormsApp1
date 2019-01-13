using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using MMFrame;

namespace WindowsFormsApp1
{
    public partial class Form6 : WindowsFormsApp1.WindowsFormsApp1_orig
    {
        public Form6()
        {
            InitializeComponent();
            int left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            DesktopBounds = new Rectangle(left, top, this.Width, this.Height);

            WindowsFormsApp1_orig.warning_form_leftlocation = left;
            WindowsFormsApp1_orig.warning_form_toplocation = top;
        }

        /*private void button1_Click(object sender, EventArgs e)
        {
            WindowsFormsApp1_orig.timer_start_flag = 0;

            this.log(this, sender, e);

            Form5 newform = new Form5();
            newform.Show();
            this.Close();
        }*/

        private void button1_Click_1(object sender, EventArgs e)
        {
            WindowsFormsApp1_orig.timer_start_flag = 0;

            this.log(this, sender, e);

            Form5 newform = new Form5();
            newform.Show();
            this.Close();
        }
    }
}
