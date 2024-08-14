using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;//打开窗体需要using这个

namespace TaxiDrive
{
    class Program
    {
        [STAThread] //设置单线程，这个不知道为什么，反正不加这句不能读入文件
        static void Main(string[] args)
        {   
            //下面两个不知道是什么作用
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new Form1());//启动窗口程序
        }
    }
}
