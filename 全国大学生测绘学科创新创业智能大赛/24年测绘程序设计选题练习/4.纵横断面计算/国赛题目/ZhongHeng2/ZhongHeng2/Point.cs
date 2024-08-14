using System;
namespace ZhongHeng2
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
            name = "";
            X = 0;
            Y = 0;
            Z = 0;
            distance = 0;
        }

        public Point(string line)
        {
            var buf = line.Trim().Split(',');
            name = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
            Z = Convert.ToDouble(buf[3]);
        }
    }
}