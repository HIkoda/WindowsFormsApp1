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



namespace MMFrame
{
    public partial class Form1 : Form
    {
        //12/3追加分
        //static readonly string InputMoviePath = @"D:\tmp\sample.mp4";
        //static readonly TimeSpan Interval = TimeSpan.FromSeconds(5);

        /*private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;
        private VideoCapabilities[] videoCapabilities;

        private Bitmap workImage;*/


        public Form1()
        {
            InitializeComponent();
            Debug.WriteLine("start!!!!!!!!");

            /*
            var cascadeFace = Accord.Vision.Detection.Cascades.FaceHaarCascade.FromXml(@"\haarcascade_frontalface_default.xml");
            // Haar-Like特徴量による物体検出を行うクラスの生成
            var detectorFace = new Accord.Vision.Detection.HaarObjectDetector(cascadeFace);

            // 読み込んだ画像から顔の位置を検出（顔の位置はRectangle[]で返される）
            var image = new Bitmap(@"C:Lenna.jpg");
            var faces = detectorFace.ProcessFrame(image);

            // 画像に検出された顔の位置を書き込みPictureBoxに表示
            var markerFaces = new Accord.Imaging.Filters.RectanglesMarker(faces, Color.Yellow);
            pictureBox1.Image = markerFaces.Apply(image);
            */


            {
                //12/3追加分               
                // パイプを使って指定時間の画像を一枚抽出する
                //var image = new ImageExtractor().ExtractImageByPipe(InputMoviePath, TimeSpan.FromSeconds(10));
                //image.Save(@"D:\tmp\pipe.jpg");
            }


        }
        /*
        public class ImageExtractor
        {
            static readonly string FfmpegPath = @"C:\Lib\ffmpeg-20170824-f0f4888-win64-static\bin\ffmpeg.exe";
            static readonly string FfprobePath = @"C:\Lib\ffmpeg-20170824-f0f4888-win64-static\bin\ffprobe.exe";

            /// <summary>動画ファイルからパイプを用いて画像を抽出する</summary>
            public Image ExtractImageByPipe(string inputMoviePath, TimeSpan extractTime)
            {
                var arguments = $"-ss {extractTime.TotalSeconds} -i \"{inputMoviePath}\" -vframes 1 -f image2 pipe:1";

                using (var process = new Process())

                {
                    process.StartInfo = new ProcessStartInfo
                    {
                        FileName = FfmpegPath,
                        Arguments = arguments,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                    };
                    process.Start();
                    var image = Image.FromStream(process.StandardOutput.BaseStream);
                    process.WaitForExit();
                    return image;
                }

            }
        }

            */

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

        private void button4_Click(object sender, EventArgs e)
        {
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
            using (IplImage img = new IplImage(@"C:\Lenna.jpg", LoadMode.Color))
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
                    CvSeq<CvAvgComp> faces = Cv.HaarDetectObjects(smallImg, cascade, storage, ScaleFactor, MinNeighbors, 0, new CvSize(30, 30), new CvSize(1000, 1000)); //new CvSize(30, 30)
                    watch.Stop();
                    Console.WriteLine("detection time = {0}ms\n", watch.ElapsedMilliseconds);

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
                    }
                }
                CvWindow.ShowImages(img);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            
            // 現在のフレームをビットマップに保存
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
                    CvSeq<CvAvgComp> faces = Cv.HaarDetectObjects(smallImg, cascade, storage, ScaleFactor, MinNeighbors, 0, new CvSize(30, 30), new CvSize(1000, 1000)); //new CvSize(30, 30)
                    watch.Stop();
                    Console.WriteLine("detection time = {0}ms\n", watch.ElapsedMilliseconds);

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
                    }
                }
                CvWindow.ShowImages(img);
            }

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