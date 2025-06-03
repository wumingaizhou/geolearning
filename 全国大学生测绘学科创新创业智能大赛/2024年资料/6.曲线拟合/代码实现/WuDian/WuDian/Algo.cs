using System;
using System.Collections.Generic;
namespace WuDian
{
    class Algo
    {
        /// <summary>
        /// 主算法
        /// </summary>

        #region 补充点
        public static List<Point> AddPoints(List<Point> points,int flag)
        {
            if(flag == 1)//闭合
            {
                List<Point> resPoints = new List<Point>(points);
                resPoints.Add(points[0]);
                resPoints.Add(points[1]);
                resPoints.Insert(0, points[points.Count - 1]);
                resPoints.Insert(0, points[points.Count - 2]);
                return resPoints;
            }
            else//不闭合
            {
                List<Point> resPoints = new List<Point>(points);
                Point pointA = new Point();
                pointA.X = resPoints[2].X - resPoints[1].X - (resPoints[1].X - resPoints[0].X);
                pointA.Y = resPoints[2].Y - resPoints[1].Y - (resPoints[1].Y - resPoints[0].Y);
                resPoints.Insert(0, pointA);
                Point pointB = new Point();
                pointB.X = resPoints[2].X - resPoints[1].X - (resPoints[1].X - resPoints[0].X);
                pointB.Y = resPoints[2].Y - resPoints[1].Y - (resPoints[1].Y - resPoints[0].Y);
                resPoints.Insert(0, pointB);

                Point pointC = new Point();
                pointC.X = resPoints[resPoints.Count - 3].X - resPoints[resPoints.Count - 2].X - (resPoints[resPoints.Count - 2].X - resPoints[resPoints.Count - 1].X);
                pointC.Y = resPoints[resPoints.Count - 3].Y - resPoints[resPoints.Count - 2].Y - (resPoints[resPoints.Count - 2].Y - resPoints[resPoints.Count - 1].Y);
                resPoints.Add(pointC);
                Point pointD = new Point();
                pointD.X = resPoints[resPoints.Count - 3].X - resPoints[resPoints.Count - 2].X - (resPoints[resPoints.Count - 2].X - resPoints[resPoints.Count - 1].X);
                pointD.Y = resPoints[resPoints.Count - 3].Y - resPoints[resPoints.Count - 2].Y - (resPoints[resPoints.Count - 2].Y - resPoints[resPoints.Count - 1].Y);
                resPoints.Add(pointD);
                return resPoints;
            }
        }
        #endregion

        #region 计算梯度
        public static void GetTidu(ref List<Point> points, List<Point> addPoints)
        {
            for(int i = 2;i <= points.Count;i++)
            {
                double a1 = addPoints[i - 1].X - addPoints[i - 2].X;
                double a2 = addPoints[i].X - addPoints[i - 1].X;
                double a3 = addPoints[i + 1].X - addPoints[i].X;
                double a4 = addPoints[i + 2].X - addPoints[i + 1].X;

                double b1 = addPoints[i - 1].Y - addPoints[i - 2].Y;
                double b2 = addPoints[i].Y - addPoints[i - 1].Y;
                double b3 = addPoints[i + 1].Y - addPoints[i].Y;
                double b4 = addPoints[i + 2].Y - addPoints[i + 1].Y;

                double w2 = Math.Abs(a3 * b4 - a4 * b3);
                double w3 = Math.Abs(a1 * b2 - a2 * b1);
                double a0 = w2 * a2 + w3 * a3;
                double b0 = w2 * b2 + w3 * b3;
                points[i - 2].Cos = a0 / (Math.Sqrt(a0 * a0 + b0 * b0));
                points[i - 2].Sin = b0 / (Math.Sqrt(a0 * a0 + b0 * b0));
            }
        }
        #endregion

        #region 计算曲线参数
        public static void GetCanshu(ref List<Point> points)
        {
            for(int i = 0;i < points.Count - 1;i++)
            {
                double r = Math.Sqrt(Math.Pow((points[i + 1].X - points[i].X), 2) + Math.Pow((points[i + 1].Y - points[i].Y), 2));
                points[i].r = r;
                points[i].E0 = points[i].X;
                points[i].E1 = r * points[i].Cos;
                points[i].E2 = 3.0 * (points[i + 1].X - points[i].X) - r * (points[i + 1].Cos + 2.0 * points[i].Cos);
                points[i].E3 = -2.0 * (points[i + 1].X - points[i].X) + r * (points[i + 1].Cos + points[i].Cos);
                points[i].F0 = points[i].Y;
                points[i].F1 = r * points[i].Sin;
                points[i].F2 = 3.0 * (points[i + 1].Y - points[i].Y) - r * (points[i + 1].Sin + 2.0 * points[i].Sin);
                points[i].F3 = -2.0 * (points[i + 1].Y - points[i].Y) + r * (points[i + 1].Sin + points[i].Sin);
            }
        }
        #endregion
    }
}