using System;
namespace WuDian
{
    class Point
    {
        /// <summary>
        /// 点的数据结构
        /// </summary>
        
        public string name;
        public double X;
        public double Y;

        public double Sin;
        public double Cos;

        public double r;
        public double E0;
        public double E1;
        public double E2;
        public double E3;
        public double F0;
        public double F1;
        public double F2;
        public double F3;

        public Point()
        {

        }
        public Point(string line)
        {
            var buf = line.Trim().Split(',');
            name = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
        }
    }
}