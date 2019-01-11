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
using WindowsFormsApp1;

namespace WindowsFormsApp1
{
    public partial class WindowsFormsApp1_orig : Form       
    {
        public static int standard_touch_value_orig = 1;//キーボード打鍵のしきい値
        public static int frontface_check_value_orig = 1;//キーボード打鍵のしきい値

        public static int limit_time = 15000;

        public static int timer_start_flag = 1;//if =1 timer_start  else timer_stop

        // public static int globalhook_start_flag = 0;//if =1 globalhook_restart  else 

            //log関係1/11追加
        static log logger;
        static int already_log_started = 0;


        public WindowsFormsApp1_orig()
        {
            InitializeComponent();
        }

        protected void log(object cur_form, object cur_button, EventArgs e)
        {
            // ログを生成してログ取得インスタンスに渡す
            long cur_tick = (DateTime.Now).Ticks - logger.get_tick();
            logger.write(cur_form.ToString() + ", " + cur_button.ToString() + ", " + cur_tick.ToString());
            return;
        }

        protected void start_log()
        {
            // ログを取るインスタンスを作る。１度以上呼ばれない。
            if (0 == already_log_started++)
            {
                logger = new log(Application.StartupPath);
            }
        }


    }
}
