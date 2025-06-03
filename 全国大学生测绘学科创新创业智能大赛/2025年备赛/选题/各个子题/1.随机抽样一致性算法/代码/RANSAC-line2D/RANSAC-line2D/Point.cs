using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RANSAC_line2D
{
    class Point
    {
        public String ID;
        public double X;
        public double Y;

        public Point(string line)
        {
            var split = Regex.Split(line, @"\,+");
            ID = split[0];
            X = Convert.ToDouble(split[1]);
            Y = Convert.ToDouble(split[2]);
        }
    }
}
