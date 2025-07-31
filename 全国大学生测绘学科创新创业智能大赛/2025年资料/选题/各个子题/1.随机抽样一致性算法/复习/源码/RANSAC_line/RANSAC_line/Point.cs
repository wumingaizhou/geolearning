using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RANSAC_line
{
    class Point
    {
        // 点的数据结构，XYZ的，我把line_2D和line_3D都在这里写了。
        public string ID;
        public double X;
        public double Y;
        public double Z;

        public Point(string Input)
        {
            var split = Regex.Split(Input, @",");
            ID = split[0];
            X = Convert.ToDouble(split[1]);
            Y = Convert.ToDouble(split[2]);
            Z = Convert.ToDouble(split[3]);
        }
        public Point(string ID, double X, double Y, double Z)
        {
            this.ID = ID;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }


        public Point Clone()
        {
            return new Point(ID, X, Y, Z);
        }
    }
}
