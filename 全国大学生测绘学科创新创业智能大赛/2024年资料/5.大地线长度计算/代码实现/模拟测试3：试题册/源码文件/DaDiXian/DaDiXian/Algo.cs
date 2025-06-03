using System;
using System.Collections.Generic;
namespace DaDiXian
{
    class Algo
    {
        #region 辅助计算
        public static void CalStep12(ref List<Session> sessions,double e2)
        {
            foreach(Session d in sessions)
            {
                d.u1 = Math.Atan(Math.Sqrt(1.0 - e2) * Math.Tan(d.B1));
                d.u2 = Math.Atan(Math.Sqrt(1.0 - e2) * Math.Tan(d.B2));
                d.l = d.L2 - d.L1;
                d.a1 = Math.Sin(d.u1) * Math.Sin(d.u2);
                d.a2 = Math.Cos(d.u1) * Math.Cos(d.u2);
                d.b1 = Math.Cos(d.u1) * Math.Sin(d.u2);
                d.b2 = Math.Sin(d.u1) * Math.Cos(d.u2);
            }
        }
        #endregion

        #region 计算起点大地方位角
        public static void CalStep2(ref List<Session> sessions,double e2)
        {
            foreach(Session d in sessions)
            {
                double start = 0;
                while(true)
                {
                    double nameda = d.l + start;
                    double p = Math.Cos(d.u2) * Math.Sin(nameda);
                    double q = d.b1 - d.b2 * Math.Cos(nameda);
                    double A1 = Math.Atan(p / q);
                    if(p > 0)
                    {
                        if(q > 0)
                        {
                            A1 = Math.Abs(A1);
                        }
                        else
                        {
                            A1 = Math.PI - Math.Abs(A1);
                        }
                    }
                    else
                    {
                        if(q > 0)
                        {
                            A1 = 2.0 * Math.PI - Math.Abs(A1);
                        }
                        else
                        {
                            A1 = Math.PI + Math.Abs(A1);
                        }
                    }
                    if(A1 < 0)
                    {
                        A1 += 2.0 * Math.PI;
                    }
                    if(A1 > 2.0 * Math.PI)
                    {
                        A1 -= 2.0 * Math.PI;
                    }
                    double sinfai = p * Math.Sin(A1) + q * Math.Cos(A1);
                    double cosfai = d.a1 + d.a2 * Math.Cos(nameda);
                    double fai = Math.Atan(sinfai / cosfai);
                    if(cosfai > 0)
                    {
                        fai = Math.Abs(fai);
                    }
                    else
                    {
                        fai = Math.PI - Math.Abs(fai);
                    }
                    double sinA0 = Math.Cos(d.u1) * Math.Sin(A1);
                    double fai1 = Math.Atan(Math.Tan(d.u1) / Math.Cos(A1));
                    double cosA02 = 1.0 - sinA0 * sinA0;
                    double alpha = e2 / 2.0 + e2 * e2 / 8.0 + e2 * e2 * e2 / 16.0;
                    alpha = alpha - (e2 * e2 / 16.0 + e2 * e2 * e2 / 16.0) * cosA02 + (3.0 * e2 * e2 * e2) / 128.0 * cosA02 * cosA02;
                    double beta = (e2 * e2 / 16.0 + e2 * e2 * e2 / 16.0) * cosA02 - (e2 * e2 * e2) / 32.0 * cosA02 * cosA02;
                    double gama = e2 * e2 * e2 / 256.0 * cosA02 * cosA02; ;
                    double end = (alpha * fai + beta * Math.Cos(2.0 * fai1 + fai) * Math.Sin(fai) + gama * Math.Sin(2.0 * fai) * Math.Cos(4.0 * fai1 + 2.0 * fai)) * sinA0;
                    if(Math.Abs(end - start) < 1.0 * Math.Pow(10, -10))
                    {
                        d.nameda = nameda;
                        d.A1 = A1;
                        d.fai = fai;
                        d.SinA0 = sinA0;
                        d.alpha = alpha;
                        d.beta = beta;
                        d.gama = gama;
                        d.fai1 = fai1;
                        break;
                    }
                    else
                    {
                        start = end;
                        continue;
                    }
                }
            }
        }
        #endregion

        #region 计算大地线长度
        public static void CalLength(ref List<Session> sessions,double ep2,double b)
        {
            foreach(Session d in sessions)
            {
                double fai1 = Math.Atan(Math.Tan(d.u1) / Math.Cos(d.A1));
                double cosA02 = 1.0 - d.SinA0 * d.SinA0;
                double k2 = ep2 * cosA02;
                double A = 1.0 - k2 / 4.0 + 7.0 * k2 * k2 / 64.0 - 15.0 * k2 * k2 * k2 / 256.0;
                A = A / b;
                double B = k2 / 4.0 - k2 * k2 / 8.0 + 37.0 * k2 * k2 * k2 / 512.0;
                double C = k2 * k2 / 128.0 - k2 * k2 * k2 / 128.0;
                double Xs = C * Math.Sin(2.0 * d.fai) * Math.Cos(4.0 * fai1 + 2.0 * d.fai);
                double S = (d.fai - B * Math.Sin(d.fai) * Math.Cos(2.0 * fai1 + d.fai) - Xs) / A;

                d.A = A;
                d.B = B;
                d.C = C;
                d.fai1 = fai1;
                d.S = S;
            }
        }
        #endregion
    }
}