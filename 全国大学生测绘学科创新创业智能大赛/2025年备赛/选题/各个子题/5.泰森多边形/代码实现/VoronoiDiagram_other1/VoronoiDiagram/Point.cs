using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace VoronoiDiagram
{
    public class Point
    {
        public double X, Y;
        //public double ID; 最好不要写ID

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
        public Point(string line)
        {
            var regx = Regex.Split(line, @"\,");
            X = Convert.ToDouble(regx[1]);
            Y = Convert.ToDouble(regx[2]);
        }

        /// <summary>
        /// 计算两点之间的距离
        /// </summary>
        public double GetDistance(Point other)
        {
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2));
        }

        /// <summary>
        /// 判断两点是否相等
        /// </summary>
        public bool Equals(Point other)
        {
            const double epsilon = 1e-10;
            return Math.Abs(X - other.X) < epsilon && Math.Abs(Y - other.Y) < epsilon;
        }
    }
}
