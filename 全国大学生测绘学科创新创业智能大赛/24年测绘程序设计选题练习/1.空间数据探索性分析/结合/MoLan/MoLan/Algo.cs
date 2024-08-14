using System;
using System.Collections.Generic;
namespace MoLan
{
    class Algo
    {
        #region 计算椭圆参数
        public static void GetTuoYuan(ref List<Point> points,ref Output output)
        {
            double Xavg = 0;
            double Yavg = 0;
            foreach (Point d in points)
            {
                Xavg += d.X;
                Yavg += d.Y;
            }
            Xavg /= points.Count;
            Yavg /= points.Count;
            foreach (Point d in points)
            {
                d.a = d.X - Xavg;
                d.b = d.Y - Yavg;
            }
            double cita = 0;
            double ai2 = 0;
            double bi2 = 0;
            double aibi = 0;
            foreach (Point d in points)
            {
                ai2 += d.a * d.a;
                bi2 += d.b * d.b;
                aibi += d.a * d.b;
            }
            cita = Math.Atan((ai2 - bi2 + (Math.Pow((ai2 - bi2) * (ai2 - bi2) + 4.0 * aibi * aibi, 0.5))) / (2.0 * aibi));
            double SDExAbove = 0;
            double SDEyAbove = 0;
            foreach (Point d in points)
            {
                SDExAbove += Math.Pow(d.a * Math.Cos(cita) + d.b * Math.Sin(cita), 2);
                SDEyAbove += Math.Pow(d.a * Math.Sin(cita) - d.b * Math.Cos(cita), 2);
            }
            double SDEx = Math.Sqrt(2.0) * Math.Sqrt(SDExAbove / points.Count);
            double SDEy = Math.Sqrt(2.0) * Math.Sqrt(SDEyAbove / points.Count);
            output.cita = cita;
            output.SDEx = SDEx;
            output.SDEy = SDEy;
        }
        #endregion

        #region 构建权重矩阵
        public static double[,] GetMatrix(ref List<Point> points)
        {
            double[,] Matrix = new double[points.Count, points.Count];
            for(int i = 0;i < points.Count;i++)
            {
                double rowSum = 0;
                //double colSum = 0;
                for(int j = 0;j < points.Count;j++)
                {
                    Matrix[i, j] = GetWeight(points[i], points[j]);
                    rowSum += Matrix[i, j];
                    //Matrix[j, i] = GetWeight(points[j], points[i]);
                    //colSum += Matrix[j, i];
                }
                for(int k = 0;k < points.Count;k++)
                {
                    Matrix[i, k] /= rowSum;//行标准化
                    //Matrix[k, i] /= colSum;//列标准化
                }
            }
            return Matrix;
        }
        private static double GetWeight(Point pointA,Point pointB)
        {
            double temp = Math.Sqrt((pointA.X - pointB.X) * (pointA.X - pointB.X) + (pointA.Y - pointB.Y) * (pointA.Y - pointB.Y));
            return temp;
        }
        #endregion

        #region 计算莫兰指数
        public static void GetMolan(ref List<Point> points,ref double[,] Matrix,ref Output output)
        {
            //全局莫兰指数
            double interestAvg = 0;
            foreach(Point d in points)
            {
                interestAvg += d.interest;
            }
            interestAvg /= points.Count;
            double above1 = 0;
            double under1 = 0;
            double totalWeight = 0;
            for(int i = 0;i < points.Count;i++)
            {
                under1 += (points[i].interest - interestAvg) * (points[i].interest - interestAvg);
                for (int j = 0;j < points.Count;j++)
                {
                    above1 += Matrix[i, j] * (points[i].interest - interestAvg) * (points[j].interest - interestAvg);
                    totalWeight += Matrix[i, j];
                }
            }
            double GlobalMoaln = points.Count / totalWeight * (above1 / under1);
            //局部莫兰
            double[] JuBuMolan = new double[points.Count];
            for(int i = 0;i < points.Count;i++)
            {
                double right = 0;
                double m2 = 0;
                for(int j = 0;j < points.Count;j++)
                {
                    right += Matrix[i, j] * (points[j].interest - interestAvg);
                    m2 += (points[j].interest - interestAvg) * (points[j].interest - interestAvg);
                }
                m2 /= points.Count;
                JuBuMolan[i] = (points[i].interest - interestAvg) / m2 * right;
            }
            output.GlobalMolan = GlobalMoaln;
            output.JuBuMolan = JuBuMolan;
        }
        #endregion

        #region 计算Z得分
        public static void GetZ(ref List<Point> points,ref Output output)
        {
            double interestAvg = 0;
            foreach (Point d in points)
            {
                interestAvg += d.interest;
            }
            interestAvg /= points.Count;
            double[] zArr = new double[points.Count];
            for(int i = 0;i < points.Count;i++)
            {
                double under = 0;
                foreach (Point d in points)
                {
                    under += (d.interest - interestAvg) * (d.interest - interestAvg);
                }
                under /= points.Count;
                zArr[i] = (points[i].interest - interestAvg) / Math.Sqrt(under);
            }
            output.zArr = zArr;
        }
        #endregion
    }
}