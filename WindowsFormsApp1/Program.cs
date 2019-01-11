using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MMFrame;

using System.Text;
//using OpenCvSharp;




namespace WindowsFormsApp1
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //WindowsFormsApp1_orig.timer_start_flag = 1;
            Application.Run(new Form1());
           //Application.Run(new Form_setup());



            /*using (var img = new IplImage(@"C:\Lenna.jpg")) //while shown windows form, not shown image
            {
                Cv.SetImageROI(img, new CvRect(20, 20, 18, 20));
                Cv.Not(img, img);
                Cv.ResetImageROI(img);
                using (new CvWindow(img))
                {
                    Cv.WaitKey();
                }
            }*/
        }
    }
}

/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCvSharp;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var img = new IplImage(@"C:\Lenna.jpg"))
            {
                Cv.SetImageROI(img, new CvRect(20, 20, 18, 20));
                Cv.Not(img, img);
                Cv.ResetImageROI(img);
                using (new CvWindow(img))
                {
                    Cv.WaitKey();
                }
            }

        }
    }
}*/