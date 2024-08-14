using System;

namespace CalArea
{
    class Triangle
    {
        public Point A;
        public Point B;
        public Point C;
        public double S;
        public double a;
        public double b;
        public double c;
        public Triangle(Point p1,Point p2,Point p3)
        {
            A = p1;
            B = p2;
            C = p3;
            runS();
        }
        public void runS()
        {
            a = Math.Sqrt((A.X - B.X) * (A.X - B.X) + (A.Y - B.Y) * (A.Y - B.Y));
            b = Math.Sqrt((B.X - C.X) * (B.X - C.X) + (B.Y - C.Y) * (B.Y - C.Y));
            c = Math.Sqrt((A.X - C.X) * (A.X - C.X) + (A.Y - C.Y) * (A.Y - C.Y));
            double p = (a + b + c) / 2;
            S = Math.Sqrt(p * (p - a) * (p - b) * (p - c));
        }
    }
}