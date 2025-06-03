using System;
namespace DaDiXian
{
    class Session
    {
        //每一条大地线
        public string startName;
        public string endName;
        public double B1;
        public double L1;
        public double B2;
        public double L2;

        public double l;//经差
        public double u1;
        public double u2;
        public double a1;
        public double a2;
        public double b1;
        public double b2;

        public double nameda;
        public double A1;
        public double fai;
        public double SinA0;
        public double alpha;
        public double beta;
        public double gama;
        public double fai1;

        public double A;
        public double B;
        public double C;
        public double S;

        public Session(string line)
        {
            var buf = line.Trim().Split(',');
            startName = buf[0];
            B1 = ddmmsssToAngle(Convert.ToDouble(buf[1]));
            L1 = ddmmsssToAngle(Convert.ToDouble(buf[2]));
            endName = buf[3];
            B2 = ddmmsssToAngle(Convert.ToDouble(buf[4]));
            L2 = ddmmsssToAngle(Convert.ToDouble(buf[5]));
        }
        public static double ddmmsssToAngle(double ddmmsss)
        {
            var dd = (int)ddmmsss;
            var mm = (int)((ddmmsss - dd) * 100);
            var sss = ((ddmmsss - dd) * 100 - mm) * 100;
            double angle = dd + mm / 60.0 + sss / 3600.0;
            angle = angle * Math.PI / 180.0;
            return angle;
        }
    }
}