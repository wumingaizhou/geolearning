using System;
using System.Collections.Generic;
using System.Linq;
namespace ZhongHeng
{
    class Algo
    {

        #region 坐标方位角的计算
        public static double GetAngle(Point A,Point B)
        {
            double angle = 0;
            angle = Math.Atan((B.Y - A.Y) / (B.X - A.X));
            if((B.Y - A.Y) > 0)
            {
                if((B.X - A.X) > 0)
                {

                }
                if((B.X - A.X) < 0)
                {
                    angle += Math.PI;
                }
                if((B.X - A.X) == 0)
                {
                    angle = Math.PI / 2.0;
                }
            }
            else
            {
                if ((B.X - A.X) > 0)
                {
                    angle += 2.0 * Math.PI;
                }
                if ((B.X - A.X) < 0)
                {
                    angle += Math.PI;
                }
                if ((B.X - A.X) == 0)
                {
                    angle = Math.PI * 1.5;
                }
            }
            return angle;
        }
        #endregion

        #region 内插高程
        public static void NeiCha(ref Point pointP,List<Point> points)
        {
            var tempList = new List<Point>(points);//不要污染最原始的数据，所以用一个新的泛型
            GetDistanceList(pointP, ref tempList);
            tempList = tempList.OrderBy(p => p.distance).ToList();//排序
            double above = 0;
            double under = 0;
            for(int i = 0;i < 5;i++)
            {
                above += tempList[i].Z / tempList[i].distance;
                under += 1.0 / tempList[i].distance;
            }
            pointP.Z = above / under;
        }
        public static void GetDistanceList(Point pointP,ref List<Point> points)
        {
            foreach(Point d in points)
            {
                d.distance = GetDistance(pointP, d);
            }
        }
        public static double GetDistance(Point pointA,Point pointB)
        {
            return Math.Sqrt((pointA.Y - pointB.Y) * (pointA.Y - pointB.Y) + (pointA.X - pointB.X) * (pointA.X - pointB.X));
        }
        #endregion

        #region 断面面积计算
        public static double GetS(Point A,Point B,double H0)
        {
            return (A.Z + B.Z - 2.0 * H0) / 2.0 * GetDistance(A, B);
        }
        #endregion

        #region 内插K0到K1,K1到K2
        public static void GetK0K1(List<Point> points,List<Point> importantPoints,ref Output output)
        {
            double distanceK0K1 = GetDistance(importantPoints[0], importantPoints[1]);
            double distanceK1K2 = GetDistance(importantPoints[1], importantPoints[2]);
            double distanceK0K2 = distanceK0K1 + distanceK1K2;
            double a01 = GetAngle(importantPoints[0], importantPoints[1]);
            double a12 = GetAngle(importantPoints[1], importantPoints[2]);
            List<Point> K0K1Points = new List<Point>();
            K0K1Points.Add(importantPoints[0]);
            K0K1Points.Add(importantPoints[1]);
            List<Point> K1K2Points = new List<Point>();
            K1K2Points.Add(importantPoints[1]);
            K1K2Points.Add(importantPoints[2]);
            for (int i = 1;i <= (int)(distanceK0K2 / 10.0);i++)
            {
                if(i * 10.0 < distanceK0K1)
                {
                    var tempPoint = new Point();
                    tempPoint.name = $"Z{i}";
                    tempPoint.X = importantPoints[0].X + i * 10.0 * Math.Cos(a01);
                    tempPoint.Y = importantPoints[0].Y + i * 10.0 * Math.Sin(a01);
                    NeiCha(ref tempPoint, points);
                    K0K1Points.Insert(i, tempPoint);
                }
                else
                {
                    var tempPoint = new Point();
                    tempPoint.name = $"Y{i - (int)(distanceK0K1 / 10.0)}";
                    tempPoint.X = importantPoints[1].X + (i * 10.0 - distanceK0K1) * Math.Cos(a12);
                    tempPoint.Y = importantPoints[1].Y + (i * 10.0 - distanceK0K1) * Math.Sin(a12);
                    NeiCha(ref tempPoint, points);
                    K1K2Points.Insert(i - (int)(distanceK0K1 / 10.0), tempPoint);
                }
            }
            output.listK0K1 = K0K1Points;
            output.listK1K2 = K1K2Points;
        }
        #endregion

        #region 计算纵断面的面积
        public static void GetZongDuanS(ref Output output,double H0)
        {
            double S1 = 0;
            double S2 = 0;
            double Stotal = 0;
            for(int i = 0;i < output.listK0K1.Count - 1;i++)
            {
                S1 += GetS(output.listK0K1[i], output.listK0K1[i + 1], H0);
            }
            for (int i = 0; i < output.listK1K2.Count - 1; i++)
            {
                S2 += GetS(output.listK1K2[i], output.listK1K2[i + 1], H0);
            }
            Stotal = S1 + S2;
            output.S1 = S1;
            output.S2 = S2;
            output.Stotal = Stotal;
        }
        #endregion

        #region 内插横断面
        public static void GetHengDuanMian(List<Point> importantPoints,List<Point> points,ref Output output)
        {
            double a01 = GetAngle(importantPoints[0], importantPoints[1]) + Math.PI * 0.5;
            double a12 = GetAngle(importantPoints[1], importantPoints[2]) + Math.PI * 0.5;
            Point M0 = new Point();
            M0.X = (importantPoints[0].X + importantPoints[1].X) / 2.0;
            M0.Y = (importantPoints[0].Y + importantPoints[1].Y) / 2.0;
            NeiCha(ref M0, points);
            Point M1 = new Point();
            M1.X = (importantPoints[1].X + importantPoints[2].X) / 2.0;
            M1.Y = (importantPoints[1].Y + importantPoints[2].Y) / 2.0;
            NeiCha(ref M1, points);
            List<Point> listHeng1 = new List<Point>();
            List<Point> listHeng2 = new List<Point>();
            for(int i = -5;i <= 5;i++)
            {
                if(i == 0)
                {
                    listHeng1.Add(M0);
                    listHeng2.Add(M1);
                    continue;
                }
                Point temp1 = new Point();
                temp1.name = $"Q{i + 6}";
                temp1.X = M0.X + i * 5.0 * Math.Cos(a01);
                temp1.Y = M0.Y + i * 5.0 * Math.Sin(a01);
                NeiCha(ref temp1, points);
                listHeng1.Add(temp1);
                Point temp2 = new Point();
                temp2.name = $"W{i + 6}";
                temp2.X = M1.X + i * 5.0 * Math.Cos(a12);
                temp2.Y = M1.Y + i * 5.0 * Math.Sin(a12);
                NeiCha(ref temp2, points);
                listHeng2.Add(temp2);
            }
            output.listHeng1 = listHeng1;
            output.listHeng2 = listHeng2;
        }
        #endregion

        #region 计算横断面的面积
        public static void GetHengDuanS(Output output,double H0)
        {
            double srow1 = 0;
            double srow2 = 0;
            for (int i = 0;i < output.listHeng1.Count - 1;i++)
            {
                srow1 += GetS(output.listHeng1[i], output.listHeng1[i + 1],H0);
                srow2 += GetS(output.listHeng2[i], output.listHeng2[i + 1],H0);
            }
            output.Srow1 = srow1;
            output.Srow2 = srow2;
        }
        #endregion
    }
}