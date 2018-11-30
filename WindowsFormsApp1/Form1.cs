using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

using TM = System.Timers;

using System.Text;
using System.IO;

using AForge.Video.DirectShow;

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;



namespace MMFrame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine("start!!!!!!!!");

            /*var cascadeFace = Accord.Vision.Detection.Cascades.FaceHaarCascade.FromXml(@"\haarcascade_frontalface_default.xml");
            // Haar-Like特徴量による物体検出を行うクラスの生成
            var detectorFace = new Accord.Vision.Detection.HaarObjectDetector(cascadeFace);

            // 読み込んだ画像から顔の位置を検出（顔の位置はRectangle[]で返される）
            var image = new Bitmap(@"C:Lenna.jpg");
            var faces = detectorFace.ProcessFrame(image);

            // 画像に検出された顔の位置を書き込みPictureBoxに表示
            var markerFaces = new Accord.Imaging.Filters.RectanglesMarker(faces, Color.Yellow);
            pictureBox1.Image = markerFaces.Apply(image);*/


        }

        //up is added(videocapture)
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
        }
        //down is added(videocapture)



        private void button1_Click(object sender, EventArgs e)
        {
            // 現在のフレームをビットマップに保存
            var bmp = videoSourcePlayer1.GetCurrentVideoFrame();
            bmp.Save("a.bmp");

        }






        //int key_count_num = 0;
        int  before_interval_num;
        int  sum_interval_num;
        int  keep_interval_num;//differnce (sum-before_sum)
        


        private static void WriteCSV(int inputnum) {
            try {
                var append=false;
                using (var sw = new System.IO.StreamWriter(@"test.csv", append))//C:/users/koda/source/repos/windowsformapp1/bin/x64/debug
                {
                    /*for (int i = 0; i < inputnum.Length; ++i)
                    {
                        // 
                        sw.WriteLine("{0}, {1}, {2},", x[i], y[i], z[i]);
                    }*/
                   //Debug.WriteLine("debug:"+inputnum+"\r\n");
                    //Debug.WriteLine("debug:" + "test" + "\r\n");
                    sw.WriteLine("{0},",inputnum);
                    sw.Close();
                }


            }
            catch (System.Exception e)
            {
                // if file can't open
                System.Console.WriteLine(e.Message);
            }

        }

        void hookKeyboardTest(ref MMFrame.Windows.GlobalHook.KeyboardHook.StateKeyboard s)
        {
            textBox1.Text = s.Stroke + " : " + s.Key + ":" + s.key_count_num + "\r\n" + textBox1.Text;
            Debug.WriteLine(s.Stroke + " : " + s.Key +":"+s.key_count_num+"\r\n" + textBox1.Text);
            sum_interval_num = s.key_count_num;
        }

        int get_num(ref MMFrame.Windows.GlobalHook.KeyboardHook.StateKeyboard s)
        {
            return s.key_count_num;
        }

        
        void timer1_test(object sender, EventArgs e)
        {
            keep_interval_num = sum_interval_num - before_interval_num;
            before_interval_num = sum_interval_num;

            textBox3.Text = keep_interval_num + "\r\n" + textBox3.Text;
            Debug.WriteLine(keep_interval_num + "\r\n"+textBox3.Text);

            WriteCSV(keep_interval_num);

            
        }

        /*void timer2_test(object sender, EventArgs e)
        {
            
            Debug.WriteLine("timer limited" + "\r\n");
            MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
            MMFrame.Windows.GlobalHook.KeyboardHook.Pause();
            limited_timer.Stop();
            timer1.Stop();
            return;

        }*/


        private void button2_Click(object sender, EventArgs e)
        {
            if (MMFrame.Windows.GlobalHook.KeyboardHook.IsHooking)
            {
                MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
                return;
            }

            /*var limited_timer = new Timer();
            limited_timer.Interval = 15000;
            limited_timer.Enabled = true;
            limited_timer.Tick += new EventHandler(this.timer2_test);
            //timer1.Tick += new EventHandler(hookKeyboardTest_interval);
            limited_timer.Start();*/

            var timer1 = new Timer();
            timer1.Interval = 5000;
            timer1.Enabled = true;
            timer1.Tick += new EventHandler(this.timer1_test);
            //timer1.Tick += new EventHandler(hookKeyboardTest_interval);
            timer1.Start();


            MMFrame.Windows.GlobalHook.KeyboardHook.AddEvent(hookKeyboardTest);

            //timer.Elapsed += new TM.ElapsedEventHandler(timer1_test);

            MMFrame.Windows.GlobalHook.KeyboardHook.Start();
        }



        /*void hookKeyboardStop(ref MMFrame.Windows.GlobalHook.KeyboardHook.StateKeyboard s)
        {
            MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
            return;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MMFrame.Windows.GlobalHook.KeyboardHook.AddEvent(hookKeyboardStop);

        }*/
        private void button3_Click(object sender, EventArgs e)
        {
            MMFrame.Windows.GlobalHook.KeyboardHook.Stop();
            MMFrame.Windows.GlobalHook.KeyboardHook.Pause();
            timer1.Stop();
            return;

        }

        /*private void Form1_Load(object sender, EventArgs e)
        {

        }*/

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

       


        /* private void button3_Click(object sender, EventArgs e)
         {
             List<MMFrame.Windows.Simulation.InputSimulator.Input> inputs = new List<MMFrame.Windows.Simulation.InputSimulator.Input>();
             List<MMFrame.Windows.Simulation.InputSimulator.MouseStroke> flags = new List<MMFrame.Windows.Simulation.InputSimulator.MouseStroke>();

             flags.Add(MMFrame.Windows.Simulation.InputSimulator.MouseStroke.LEFT_DOWN);
             flags.Add(MMFrame.Windows.Simulation.InputSimulator.MouseStroke.LEFT_UP);
             flags.Add(MMFrame.Windows.Simulation.InputSimulator.MouseStroke.MOVE);

             MMFrame.Windows.Simulation.InputSimulator.AddMouseInput(ref inputs, flags, 0, false, 0, 50);
             MMFrame.Windows.Simulation.InputSimulator.AddKeyboardInput(ref inputs, "！！");

             MMFrame.Windows.Simulation.InputSimulator.SendInput(inputs);
         }*/
    }
}