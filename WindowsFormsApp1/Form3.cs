using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form3 : WindowsFormsApp1.WindowsFormsApp1_orig
    {
        public Form3()
        {
            InitializeComponent();
            int left = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
            int top = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            DesktopBounds = new Rectangle(left, top, this.Width, this.Height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.log(this, sender, e);
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.log(this, sender, e);
            this.Close();
        }
    }
}
