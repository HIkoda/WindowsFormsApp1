using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WindowsFormsApp1
{
    class log
    {
        private static string filename;
        private static StreamWriter sw;
        private long starttick;

        public log(string startup_path)
        {
            // ファイル名を生成するついでに、このインスタンスが初期化された時間を格納
            starttick = (DateTime.Now).Ticks;
            filename = startup_path + "\\log" + starttick.ToString() + ".txt";

            // ファイルを作って1行目を書く
            sw = File.CreateText(filename);
            sw.WriteLine("Application, Window Text, Action Object, Object Text, Time(1 = 100 nanosec)");
            sw.Flush();
        }

        public void write(string arg)
        {
            // 引数の文字列を何も考えずにログに放り込む
            sw.WriteLine(arg);
            sw.Flush();
        }

        public long get_tick()
        {
            // インスタンスが初期化された時間を返す
            return starttick;
        }
    }
}
