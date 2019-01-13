using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using TM = System.Timers;

using System.Text;
using System.IO;

using AForge.Video;
using AForge.Video.DirectShow;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using OpenCvSharp;

using MMFrame;
using WindowsFormsApp1;

namespace MMFrame
{
    public partial class Form1 : WindowsFormsApp1.WindowsFormsApp1_orig
    {
        public int standard_touch_value = WindowsFormsApp1_orig.standard_touch_value_orig;//キーボード打鍵のしきい値
        public int frontface_check_value = WindowsFormsApp1_orig.frontface_check_value_orig;//キーボード打鍵のしきい値


        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine("start!!!!!!!!");

            this.start_log();

            System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();
            System.Windows.Forms.Timer timer2 = new System.Windows.Forms.Timer();
            System.Windows.Forms.Timer timer3 = new System.Windows.Forms.Timer();
            System.Windows.Forms.Timer timer4 = new System.Windows.Forms.Timer();

            System.Windows.Forms.Timer timer5 = new System.Windows.Forms.Timer();

            System.Windows.Forms.Timer form1_minute_timer = new System.Windows.Forms.Timer();

            loop_timer1(timer1_test);
            loop_timer2(timer2_test);
            loop_timer3(timer3_test);
            loop_timer4(timer4_test);

            loop_timer5(timer5_test);
            loop_minute(minute_test);

        }


        //int videoplayer_stop_flag=0;

        private void Form1_Load(object sender, EventArgs e)
        {

            // ビデオキャプチャデバイスを選択するダイアログの生成
            var form = new VideoCaptureDeviceForm();
            // 選択ダイアログを開く
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // 選択されたデバイスをVideoSourcePlayerのソースに設定
                videoSourcePlayer1.VideoSource = form.VideoDevice;


                // ビデオキャプチャのスタート
                videoSourcePlayer1.Start();
            }

        }



        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {

            // 閉じるときの処理
            if (videoSourcePlayer1.VideoSource != null && videoSourcePlayer1.VideoSource.IsRunning)
            {
                videoSourcePlayer1.VideoSource.SignalToStop();
                videoSourcePlayer1.VideoSource = null;
            }
            MMFrame.Windows.GlobalHook.KeyboardHook.Stop();

            timer1.Enabled = false;
            timer2.Enabled = false;
            timer3.Enabled = false;
            timer4.Enabled = false;
        }
        //down is added(videocapture)


        //int key_count_num = 0;
        int before_interval_num;
        int sum_interval_num;
        int keep_interval_num;//differnce (sum-before_sum)

        int send_keyboard_csv_flag;
        int send_frontface_csv_flag;




        public static int WriteCSV(int inputnum, int csv_flag, int judge_which_csv)
        {

            try
            {

                var append = true;
                System.Console.WriteLine("append(sengen) is {0}", append);

                if (csv_flag == 1)
                {
                    append = true;
                }
                else
                {
                    append = false;
                }

                System.Console.WriteLine("append(ifbefore) is {0}", append);

                if (judge_which_csv == 0)//keyboard
                {

                    using (var sw = new System.IO.StreamWriter(@"keyboard.csv", append))//C:/users/koda/source/repos/windowsformapp1/bin/x64/debug
                    {

                        csv_flag = 1;
                        sw.WriteLine("{0},", inputnum);
                        System.Console.WriteLine("inputnum is {0}", inputnum);
                        System.Console.WriteLine("append(inusing) is {0}", append);
                        
                        sw.Close();
                    }
                }
                if (judge_which_csv == 1)//frontface
                {
                    using (var sw = new System.IO.StreamWriter(@"frontface.csv", append))//C:/users/koda/source/repos/windowsformapp1/bin/x64/debug
                    {
                        sw.WriteLine("{0},", inputnum);
                        csv_flag = 1;

                        sw.Close();
                    }
                }

            }
            catch (System.Exception e)
            {
                // if file can't open
                System.Console.WriteLine(e.Message);
            }
            return csv_flag;
        }

        private static int KeyboardReadCSV(int begin_rownum, int end_rownum)
        {
            int row_num = 1;
            int value_sum = 0;
            int average = 0;

            int check_string;

            try
            {
                //open csvfile
                using (var sr = new System.IO.StreamReader(@"keyboard.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        string[] values = line.Split(',');

                        //出力
                        if (begin_rownum <= row_num && end_rownum >= row_num)
                        {
                            foreach (var value in values)
                            {

                                System.Console.WriteLine("reserve_num is " + "{0}", value);
                                System.Console.WriteLine("----------------------------------");
                                
                                if (int.TryParse(value, out check_string) != false)
                                {
                                    value_sum += int.Parse(value);
                                }

                            }

                            System.Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                           
                        }

                        row_num++;
                    }

                    switch (end_rownum)
                    {
                        case 1: average = value_sum; break;
                        case 2: average = value_sum / 2; break;
                        case 3: average = value_sum / 3; break;
                        case 4: average = value_sum / 4; break;
                        default: average = value_sum / 5; break;
                    }

                }
            }
            catch (System.Exception e)
            {
                // failure file open
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("average is " + "{0}", average);
            return average;

        }


        private static int FrontfaceReadCSV(int end_facerownum)
        {
            System.Console.WriteLine("frontfacenumber is " + "{0}", end_facerownum);
            int row_num = 1;
            int frontface_count = 0;

            int check_string;

            try
            {
                //open csvfile
                using (var sr = new System.IO.StreamReader(@"frontface.csv"))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();
                        string[] values = line.Split(',');

                        //出力
                        if (end_facerownum == row_num)
                        {
                            foreach (var value in values)
                            {

                                System.Console.WriteLine("frontfacenumber is " + "{0}", value);
                                System.Console.WriteLine("----------------------------------");
                                
                                if (int.TryParse(value, out check_string) != false)
                                {
                                    frontface_count += int.Parse(value);
                                }

                            }

                            System.Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            
                        }

                        row_num++;
                    }
                }
            }
            catch (System.Exception e)
            {
                // failure file open
                System.Console.WriteLine(e.Message);
            }
            System.Console.WriteLine("frontface_count is " + "{0}", frontface_count);
            return frontface_count;

        }







        void hookKeyboardTest(ref MMFrame.Windows.GlobalHook.KeyboardHook.StateKeyboard s)
        {
            sum_interval_num = s.key_count_num;
        }

        int get_num(ref MMFrame.Windows.GlobalHook.KeyboardHook.StateKeyboard s)
        {
            return s.key_count_num;
        }


        int frametest;

        private void timer1_test(object sender, EventArgs e)/*keyboard hook*/
        {
            if (form1_count_second % 60 == 0)
            {
                decide_keyboard_csv = 0;
                keep_interval_num = sum_interval_num - before_interval_num;
                before_interval_num = sum_interval_num;

                System.Console.WriteLine("keyboard  is " + "{0}", keep_interval_num);//キーボード取れてる

                send_keyboard_csv_flag = WriteCSV(keep_interval_num, send_keyboard_csv_flag, decide_keyboard_csv);

                send_frontface_csv_flag = WriteCSV(frontface_check_num, send_frontface_csv_flag, decide_frontface_csv);
                frontface_check_num = 0;
            }
        }

        int decide_keyboard_csv;
        int decide_frontface_csv;
        int frontface_check_num;

        void timer2_test(object sender, EventArgs e)/*face recognize (1frame)*/
        {
            decide_frontface_csv = 1;

            var bmp = videoSourcePlayer1.GetCurrentVideoFrame();
            bmp.Save("a.bmp");

            CvColor[] colors = new CvColor[]{
                new CvColor(0,0,255),
                new CvColor(0,128,255),
                new CvColor(0,255,255),
                new CvColor(0,255,0),
                new CvColor(255,128,0),
                new CvColor(255,255,0),
                new CvColor(255,0,0),
                new CvColor(255,0,255),
            };

            const double Scale = 1.14;
            const double ScaleFactor = 1.0850;
            const int MinNeighbors = 2;

            //using (IplImage img = new IplImage(@"C:\Yalta.jpg", LoadMode.Color))
            using (IplImage img = new IplImage(@"a.bmp", LoadMode.Color))
            //using (IplImage img = new IplImage(@"C:\Lenna.jpg", LoadMode.Color))
            using (IplImage smallImg = new IplImage(new CvSize(Cv.Round(img.Width / Scale), Cv.Round(img.Height / Scale)), BitDepth.U8, 1))
            {
                // 顔検出用の画像の生成
                using (IplImage gray = new IplImage(img.Size, BitDepth.U8, 1))
                {
                    Cv.CvtColor(img, gray, ColorConversion.BgrToGray);
                    Cv.Resize(gray, smallImg, Interpolation.Linear);
                    Cv.EqualizeHist(smallImg, smallImg);
                }

                //using (CvHaarClassifierCascade cascade = Cv.Load<CvHaarClassifierCascade>(Const.XmlHaarcascade))  // どっちでも可
                using (CvHaarClassifierCascade cascade = CvHaarClassifierCascade.FromFile("haarcascade_frontalface_default.xml"))    // 
                using (CvMemStorage storage = new CvMemStorage())
                {
                    storage.Clear();

                    // 顔の検出
                    Stopwatch watch = Stopwatch.StartNew();
                    CvSeq<CvAvgComp> faces = Cv.HaarDetectObjects(smallImg, cascade, storage, ScaleFactor, MinNeighbors, 0, new CvSize(100, 100), new CvSize(1000, 1000)); //new CvSize(30, 30)
                    watch.Stop();
                    //Console.WriteLine("detection time = {0}ms\n", watch.ElapsedMilliseconds);

                    // 検出した箇所にまるをつける
                    for (int i = 0; i < faces.Total; i++)
                    {
                        CvRect r = faces[i].Value.Rect;
                        CvPoint center = new CvPoint
                        {
                            X = Cv.Round((r.X + r.Width * 0.5) * Scale),
                            Y = Cv.Round((r.Y + r.Height * 0.5) * Scale)
                        };
                        int radius = Cv.Round((r.Width + r.Height) * 0.25 * Scale);
                        img.Circle(center, radius, colors[i % 8], 3, LineType.AntiAlias, 0);

                        if (radius >= 50)
                        {
                            frontface_check_num++;
                            break;
                        }

                        System.Console.WriteLine("radius = {0}!!!!!!!!!!!!\n", radius);
                    }
                }
                // CvWindow.ShowImages(img);
            }

            frametest++;

        }

        int send_begin_rownum = 1;
        int send_end_rownum = 1;
        int send_end_facerownum = 0;
        

        void timer3_test(object sender, EventArgs e)/*judge keyboard&face_recognize*/
        {
            if (form1_count_second % 300 == 0)
            { 
                int average_keyboard_touch = 0;
                int frontface_check = 0;

                if (send_keyboard_csv_flag == 1)
                {
                    average_keyboard_touch = KeyboardReadCSV(send_begin_rownum, send_end_rownum);
                }
                if (send_frontface_csv_flag == 1)
                {
                frontface_check = FrontfaceReadCSV(send_end_facerownum);
                }

                if (send_end_rownum >= 5)
                {
                    send_begin_rownum += 1;
                }

                send_end_rownum += 1;
                send_end_facerownum += 1;

                // standard_touch_value = 1;//キーボード打鍵のしきい値
                // frontface_check_value = 1;//キーボード打鍵のしきい値

                if (send_keyboard_csv_flag == 1 && average_keyboard_touch < standard_touch_value)//キーボード打鍵はここの値を変える
                {
                
                    Form2 newform = new Form2();
                    newform.StartPosition = FormStartPosition.Manual;
                    newform.Show();
                

                }
                else if (send_frontface_csv_flag == 1 && frontface_check < frontface_check_value)//顔の向きはここの値を変える
                {
                    Form3 newform = new Form3();
                    newform.StartPosition = FormStartPosition.Manual;
                    newform.Show();

                }
            //frontface_check_num = 0;
            }
        }

        public void loop_timer1(EventHandler eventHandler)//keyboard
        {
            timer1.Tick += new EventHandler(eventHandler);
            timer1.Interval = 1000;
            //timer1.Enabled = false;

        }
        public void loop_timer2(EventHandler eventHandler)//
        {
            timer2.Tick += new EventHandler(eventHandler);
            timer2.Interval = 100;//(==100)
            //timer1.Enabled = false;

        }
        public void loop_timer3(EventHandler eventHandler)
        {
            timer3.Tick += new EventHandler(eventHandler);
            timer3.Interval = 1000;
            //timer3.Enabled = false;

        }
        public void loop_timer4(EventHandler eventHandler)
        {
            timer4.Tick += new EventHandler(eventHandler);
            timer4.Interval = 1000;
            //timer3.Enabled = false;

        }
        public void loop_timer5(EventHandler eventHandler)
        {
            timer5.Tick += new EventHandler(eventHandler);
            timer5.Interval = 50;
            //timer3.Enabled = false;

        }
        public void loop_minute(EventHandler eventHandler)
        {
            timer5.Tick += new EventHandler(eventHandler);
            timer5.Interval = 1000;//(==1000)
            //timer3.Enabled = false;

        }

        void timer4_test(object sender, EventArgs e)
        {

            if (form1_count_second == 3600 && finish_time_flag==0)
            {
                WindowsFormsApp1_orig.timer_start_flag = 0;
                Form4 newform = new Form4();
                newform.Show();
            }
            else if(form1_count_second%600 == 0 && finish_time_flag == 1)
            {
                WindowsFormsApp1_orig.timer_start_flag = 0;
                Form4 newform = new Form4();
                newform.Show();
            }
            //return;
        }

        void timer5_test(object sender, EventArgs e)
        {
            if (WindowsFormsApp1_orig.timer_start_flag == 0) {
                if (MMFrame.Windows.GlobalHook.KeyboardHook.IsHooking)
                {
                    MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
                    return;
                }

                timer1.Enabled = false;
                timer2.Enabled = false;
                timer3.Enabled = false;
                timer4.Enabled = false;
                form1_minute_timer.Enabled = false;
            }
            else
            {
                
                if (MMFrame.Windows.GlobalHook.KeyboardHook.IsPaused)
                {
                    MMFrame.Windows.GlobalHook.KeyboardHook.AddEvent(hookKeyboardTest);
                    MMFrame.Windows.GlobalHook.KeyboardHook.Start();
                    return;
                }

                timer1.Enabled = true;
                timer2.Enabled = true;
                timer3.Enabled = true;
                timer4.Enabled = true;
                form1_minute_timer.Enabled = true;
            }

            
        }

        int form1_count_second=0;
        //int form1_count_minute;

        void minute_test(object sender, EventArgs e)
        {
            form1_count_second++;
            /*if (form1_count_second == 60)
            {
                form1_count_minute++;
                form1_count_second = 0;
            }*/
        }


            private void button2_Click_1(object sender, EventArgs e)
        {
            this.log(this, sender, e);

            if (MMFrame.Windows.GlobalHook.KeyboardHook.IsHooking)
            {
                MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
                return;
            }

            timer1.Enabled = true;

            timer2.Enabled = true;

            timer3.Enabled = true;

            timer4.Enabled = true;

            timer5.Enabled = true;

            form1_minute_timer.Enabled = true;

            MMFrame.Windows.GlobalHook.KeyboardHook.AddEvent(hookKeyboardTest);

            MMFrame.Windows.GlobalHook.KeyboardHook.Start();
            this.Hide();

            Form6 newform = new Form6();
            newform.StartPosition = FormStartPosition.Manual;
            newform.Show();
        }

    }
}















