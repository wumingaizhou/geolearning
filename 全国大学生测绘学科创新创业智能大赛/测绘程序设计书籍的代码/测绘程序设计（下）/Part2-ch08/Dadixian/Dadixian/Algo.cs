using System;
using System.Collections.Generic;
namespace Dadixian
{
    class Algo
    {
        ///<summary>
        ///主算法
        ///</summary>
        public double Distance;
        public Algo(List<Data> pts,double tuoqiu,double tuoqiu1,double b)
        {
            var distance = Cal(pts,tuoqiu,tuoqiu1,b);
            Distance = distance;
        }
        private double Cal(List<Data> points,double tuoqiu,double tuoqiu1,double tuoqiu_b)
        {
            double[] u = new double[2];
            double[] a = new double[2];
            double[] b = new double[2];
            double l = points[1].L - points[0].L;
            for(int i = 0;i < 2;i++)
            {
                u[i] = Math.Atan(Math.Sqrt(1 - tuoqiu) * Math.Tan(points[i].B)); 
            }
            a[0] = Math.Sin(u[0]) * Math.Sin(u[1]);
            a[1] = Math.Cos(u[0]) * Math.Cos(u[1]);
            b[0] = Math.Cos(u[0]) * Math.Sin(u[1]);
            b[1] = Math.Sin(u[0]) * Math.Cos(u[1]);

            //2.计算起点大地方位角
            double res_nmd, res_A1, res_cigema, res_sinA0;//最终计算结果保存变量
            double start = 0;
            while(true)
            {
                double nmd = l + start;
                nmd = nmd / 180 * Math.PI;
                double p = Math.Cos(u[1]) * Math.Sin(nmd);
                double q = b[0] - b[1] * Math.Cos(nmd);
                double A1 = Math.Atan2(Math.Abs(p), Math.Abs(q));//第一象限角,A1是弧度
                A1 = A1 * 180 / Math.PI;//先弧度转角度;
                if (p > 0)
                {
                    if (q > 0)
                    {

                    }
                    else
                    {
                        A1 = 180 - A1;
                    }
                }
                else
                {
                    if (q > 0)
                    {
                        A1 = 360 - A1;
                    }
                    else
                    {
                        A1 = 180 + A1;
                    }
                }
                if (A1 < 0)
                {
                    A1 = A1 + 360;
                }
                if (A1 > 360)
                {
                    A1 = A1 - 360;
                }
                A1 = A1 / 180 * Math.PI;//再转回来弧度
                double sin1 = p * Math.Sin(A1) + q * Math.Cos(A1);
                double cos1 = a[0] + a[1] * Math.Cos(nmd);
                double atan1 = Math.Atan2(Math.Abs(sin1), Math.Abs(cos1));
                if (cos1 > 0)
                {

                }
                else
                {
                    atan1 = Math.PI - atan1;
                }
                double sinA0 = Math.Cos(u[0]) * Math.Sin(A1);
                double fai1 = Math.Atan(Math.Tan(u[0]) / Math.Cos(A1));
                double temp_a = Math.Pow(tuoqiu, 2) / 2 + Math.Pow(tuoqiu, 4) / 8 + Math.Pow(tuoqiu, 6) / 16 - (Math.Pow(tuoqiu, 4) / 16 + Math.Pow(tuoqiu, 6) / 16) * (1 - sinA0 * sinA0) + 3 * Math.Pow(tuoqiu, 6) / 128 * (1 - sinA0 * sinA0) * (1 - sinA0 * sinA0);
                double temp_b = (Math.Pow(tuoqiu, 4) / 16 + Math.Pow(tuoqiu, 6) / 16) * (1 - sinA0 * sinA0) - Math.Pow(tuoqiu, 6) / 32 * (1 - sinA0 * sinA0) * (1 - sinA0 * sinA0);
                double temp_c = Math.Pow(tuoqiu, 6) / 256 * (1 - sinA0 * sinA0) * (1 - sinA0 * sinA0);
                double last = (temp_a * atan1 + temp_b * Math.Cos(2 * fai1 + atan1) * Math.Sin(atan1) + temp_c * Math.Sin(2 * atan1) * Math.Cos(4 * fai1 + 2 * atan1)) * sinA0;
                if(last == start || Math.Abs(last - start) < 1e-10)
                {
                    res_nmd = nmd;
                    res_A1 = A1;
                    res_cigema = atan1;
                    res_sinA0 = sinA0;
                    break;
                }
                start = last;
            }//迭代

            //3.计算大地线长度
            double faiInStep3 = Math.Atan(Math.Tan(u[0]) / Math.Cos(res_A1));
            double k2 = tuoqiu1 * (1 - res_sinA0 * res_sinA0);
            double A = (1 - k2 / 4 + 7 * k2 * k2 / 64 - 15 * k2 * k2 * k2 / 256) / tuoqiu_b;
            double B = (k2 / 4 - k2 * k2 / 8 + 37 * k2 * k2 * k2 / 512);
            double C = (k2 * k2 / 128 - k2 * k2 * k2 / 128); 
            double Xs = C * Math.Sin(2 * res_cigema) * Math.Cos(4 * faiInStep3 + 2 * res_cigema);
            double S = (res_cigema - B * Math.Sin(res_cigema) * Math.Cos(2 * faiInStep3 + res_cigema) - Xs) / A;
            return S;
        }
    }
}