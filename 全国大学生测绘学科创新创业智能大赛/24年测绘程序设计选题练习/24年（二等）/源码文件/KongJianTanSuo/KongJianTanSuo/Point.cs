using System;
namespace KongJianTanSuo
{
    class Point
    {
        public string name;
        public double x;
        public double y;
        public double area_code;
        public double a;
        public double b;
        public Point()
        {

        }
        public Point(string line)
        {
            var buf = line.Trim().Split(',');
            name = buf[0];
            x = Convert.ToDouble(buf[1]);
            y = Convert.ToDouble(buf[2]);
            area_code = Convert.ToDouble(buf[3]);
        }
    }
}