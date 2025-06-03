using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
namespace VoronoiDiagram
{
    class Point
    {
        public double X, Y;
        //public double ID; 最好不要写ID

        public Point(double x,double y)
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
    }
}
