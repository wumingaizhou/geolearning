using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DianYun
{
    class Point
    {
        public double X;
        public double Y;
        public double Z;
        public double di;
        public double di_avg = 0;
        public bool IS_bad = false;

        public Point(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point Clone()
        {
            return new Point(X, Y, Z);
        }

        public Point(string line)
        {
            var split = Regex.Split(line, @",");
            X = Convert.ToDouble(split[0]);
            Y = Convert.ToDouble(split[1]);
            Z = Convert.ToDouble(split[2]);
        }

        public void GetDistance(Point point1)
        {
            di = Math.Sqrt(Math.Pow(X - point1.X, 2) + Math.Pow(Y - point1.Y, 2) + Math.Pow(Z - point1.Z, 2));
        }
    }
}
