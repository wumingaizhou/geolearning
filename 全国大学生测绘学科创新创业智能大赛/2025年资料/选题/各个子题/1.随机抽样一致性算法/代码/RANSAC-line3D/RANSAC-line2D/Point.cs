using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RANSAC_line3D
{
    class Point
    {
        public String ID;
        public double X;
        public double Y;
        public double Z;

        public Point(string line)
        {
            var split = Regex.Split(line, @"\,+");
            ID = split[0];
            X = Convert.ToDouble(split[1]);
            Y = Convert.ToDouble(split[2]);
            Z = Convert.ToDouble(split[3]);
        }
        public Point()
        {
            ID = "temp";
            X = 0;
            Y = 0;
            Z = 0;
        }
    }
}
