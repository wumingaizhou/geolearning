using System;
namespace ZhongHeng
{
    class Point
    {
        public string name;
        public double X;
        public double Y;
        public double Z;
        public double distance;
        public Point()
        {

        }
        public Point(string line)
        {
            var buf = line.Trim().Split(',');
            name = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
            Z = Convert.ToDouble(buf[3]);        }
    }
}