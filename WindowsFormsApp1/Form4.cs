﻿using System;
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
    public partial class Form4 : WindowsFormsApp1.WindowsFormsApp1_orig
    {
        public Form4()
        {
            InitializeComponent();

            WindowsFormsApp1_orig.timer_start_flag = 0;//timer_stop
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.log(this, sender, e);

            Form5 newform = new Form5();
            newform.Show();
            this.Close();
            //Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.log(this, sender, e);

            this.Close();
                  
            //WindowsFormsApp1_orig.standard_touch_value_orig--;
            //WindowsFormsApp1_orig.frontface_check_value_orig--;

            //WindowsFormsApp1_orig.work_limit_time = 7000;

            WindowsFormsApp1_orig.timer_start_flag = 1;//timer_restart
            WindowsFormsApp1_orig.finish_time_flag = 1;//if =1 timer_start  else timer_stop
    }
    }
}
