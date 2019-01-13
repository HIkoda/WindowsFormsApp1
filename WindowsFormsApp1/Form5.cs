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

            System.Windows.Forms.Timer breaktime_timer = new System.Windows.Forms.Timer();

            //breaktime_timer1(breaktime_test);
            //breaktime_timer.Enabled = true;
            /*StartTime = DateTime.Now;
            TimeLimit = new TimeSpan(0,0,30);*/
            //int limitsecond = 30;
            
            label2.Text =limitminute.ToString() + "分で";
            /*button1.ForeColor = Color.Gray;
            button1.BackColor = Color.LightGreen;*/

        }
        //DateTime StartTime;
        //TimeSpan TimeLimit;

        /*int limitsecond = 5;
        int second = 0;*/

        int limitminute = 10;
        int minute = 0;
        int second = 0;

        private void Form5_Load(object sender, EventArgs e)
        {
            breaktime_timer.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.log(this, sender, e);

            WindowsFormsApp1_orig.timer_start_flag = 1;//timer_restart
            WindowsFormsApp1_orig.finish_time_flag = 0;

            this.Close();
        }

       /* private void button2_Click(object sender, EventArgs e)
        {

            this.log(this, sender, e);

            WindowsFormsApp1_orig.timer_start_flag = 1;//timer_restart

            this.Close();
        }*/
        private void button2_Click_1(object sender, EventArgs e)
        {
            this.log(this, sender, e);

            //WindowsFormsApp1_orig.timer_start_flag = 1;//timer_restart
            //Applicaton.Exit();
            Environment.Exit(0);
            this.Close();
        }

        /*public void breaktime_timer1(EventHandler eventHandler)//keyboard
        {
            breaktime_timer.Tick += new EventHandler(eventHandler);
            breaktime_timer.Interval = 10000;
            //timer1.Enabled = false;

        }

        void breaktime_test(object sender, EventArgs e)
        {

            //WindowsFormsApp1_orig.timer_start_flag = 0;
            Form4 newform = new Form4();
            newform.Show();

            //return;
        }*/

        private void breaktime_timer_Tick(object sender, EventArgs e)
        {
            //TimeSpan tm = DateTime.Now - StartTime;
            //minute++;

            if (minute == limitminute)
            {
                button1.Enabled = true;
                label2.Text = (limitminute - minute).ToString() + "分で";
            }
            else if (minute > limitminute)
            {
                breaktime_timer.Enabled = false;
                //button1.BackColor = Color.Black;
                button1.Enabled = true;
            }
            else
            {
                label2.Text = (limitminute - minute).ToString() + "分で";
            }
        }

        private void minute_timer_Tick(object sender, EventArgs e)
        {
            second++;
            if (second == 60)
            {
                minute++;
                second = 0;
            }
        }

        
    }
}
