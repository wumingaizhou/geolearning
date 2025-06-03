using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace LIDAR
{
    class Point
    {
        public double X;
        public double Y;
        public double Z;

        public double length; // algo 里的步骤二需要用到

        public double di = 0; // 点pi的平均邻近距离，步骤二需要用到

        public Point(string line)
        {
            // 读取数据时，传入了txt每行文本
            var regx = Regex.Split(line, @"\s+");
            X = Convert.ToDouble(regx[0]);
            Y = Convert.ToDouble(regx[1]);
            Z = Convert.ToDouble(regx[2]);
        }

        public Point(double x ,double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
