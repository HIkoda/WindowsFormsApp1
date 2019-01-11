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
    public partial class Form5 : WindowsFormsApp1.WindowsFormsApp1_orig
    {
        public Form5()
        {
            InitializeComponent();
            WindowsFormsApp1_orig.timer_start_flag = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Form1 newform = new Form1();
            //Hide();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new Form1());

            //WindowsFormsApp1_orig.globalhook_start_flag = 1;

            this.log(this, sender, e);

            WindowsFormsApp1_orig.timer_start_flag = 1;//timer_restart

            //MMFrame.Windows.GlobalHook.KeyboardHook.Start();

            this.Close();
        }
    }
}
