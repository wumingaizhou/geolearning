using System;
using System.Collections.Generic;
using System.Linq;
namespace ZhongHeng2
{
    class Algo
    {
        #region 坐标方位角计算
        public static double CalAngle(Point A,Point B)
        {
            double angle = 0;
            angle = Math.Atan((B.Y - A.Y) / (B.X - A.X));
            if((B.Y - A.Y) > 0)
            {
                if((B.X - A.X) == 0)
                {
                    angle = Math.PI / 2.0;
                }
                if((B.X - A.X) > 0)
                {

                }
                else
                {
                    angle = Math.PI + angle;
                }
            }
            else
            {
                if ((B.X - A.X) == 0)
                {
                    angle = Math.PI / 2.0 * 3.0;
                }
                if ((B.X - A.X) > 0)
                {
                    angle += 2 * Math.PI;
                }
                else
                {
                    angle = Math.PI + angle;
                }
            }
            return angle;
        }
        public static double Calddmmssss(double angle)
        {
            double temp = angle / Math.PI * 180.0 * 3600;
            double dd = (int)(angle / Math.PI * 180.0);
            double mm = (int)((temp - dd * 3600) / 60.0);
            double ssss = temp - dd * 3600 - mm * 60;
            double ddmmss = dd + mm / 100.0 + ssss / 10000.0;
            return ddmmss;
        }
        #endregion

        #region 内插高程
        public static double NeiCha(Point point,List<Point> points)
        {
            var DistanceList = new List<Point>();
            DistanceList = CalDistanceList(point, points);
            var temp_paixu = DistanceList.OrderBy(p => p.distance).ToList();
            double H = 0;
            double above = 0;
            double under = 0;
            for(int i = 0;i < 5;i++)
            {
                above += temp_paixu[i].Z / temp_paixu[i].distance;
                under += 1.0 / temp_paixu[i].distance;
            }
            H = above / under;
            return H;
        }
        public static List<Point> CalDistanceList(Point point,List<Point> points)
        {
            var res = new List<Point>();
            res = points;
            for(int i = 0;i < points.Count;i++)
            {
                points[i].distance = CalDistance(point, points[i]);
            }
            return res;
        }
        public static double CalDistance(Point A,Point B)
        {
            double distance = Math.Sqrt((A.Y - B.Y) * (A.Y - B.Y) + (A.X - B.X) * (A.X - B.X));
            return distance;
        }
        #endregion

        #region 断面面积计算
        public static double CalS(Point A,Point B, double H0)
        {
            double S = (A.Z + B.Z - 2 * H0) / 2.0 * CalDistance(A, B);
            return S;
        }
        #endregion

        #region 内插点的坐标
        public static double NeiChaZhongDuanMian(List<Point> importantPoints,List<Point> points,double H0)
        {
            List<Point> result = new List<Point>(importantPoints);
            double LK0K1 = CalDistance(importantPoints[0], importantPoints[1]);
            double L = (CalDistance(importantPoints[0], importantPoints[1]) + CalDistance(importantPoints[1], importantPoints[2]));
            for (int i = 0;i < (int)(L / 10.0);i++)
            {
                if(((i + 1) * 10.0 - LK0K1) < 0)
                {
                    var temp_point = new Point();
                    temp_point.X = importantPoints[0].X + (i + 1) * 10 * Math.Cos(CalAngle(importantPoints[0], importantPoints[1]));
                    temp_point.Y = importantPoints[0].Y + (i + 1) * 10 * Math.Sin(CalAngle(importantPoints[0], importantPoints[1]));
                    temp_point.Z = NeiCha(temp_point, points);
                    temp_point.name = $"Z{i + 1}";
                    result.Insert((i + 1), temp_point);
                }
                else
                {
                    var temp_point = new Point();
                    temp_point.X = importantPoints[1].X + ((i + 1) * 10 - LK0K1) * Math.Cos(CalAngle(importantPoints[1], importantPoints[2]));
                    temp_point.Y = importantPoints[1].Y + ((i + 1) * 10 - LK0K1) * Math.Sin(CalAngle(importantPoints[1], importantPoints[2]));
                    temp_point.Z = NeiCha(temp_point, points);
                    temp_point.name = $"Z{i + 1}";
                    result.Insert((i + 1 + 1), temp_point);//注意
                }
            }
            double Stotal = 0;
            for(int i = 0;i < result.Count - 1;i++)
            {
                Stotal += CalS(result[i], result[i + 1], H0);
            }
            return Stotal;
        }
        #endregion

        #region 第三部的算法
        public static void CalStep3(List<Point> importantPoints,List<Point> points,double H0)
        {
            Point M0 = new Point();
            M0.X = (importantPoints[0].X + importantPoints[1].X) / 2.0;
            M0.Y = (importantPoints[0].Y + importantPoints[1].Y) / 2.0;
            Point M1 = new Point();
            M1.X = (importantPoints[1].X + importantPoints[2].X) / 2.0;
            M1.Y = (importantPoints[1].Y + importantPoints[2].Y) / 2.0;
            List<Point> PointsM0 = new List<Point>();
            List<Point> PointsM1 = new List<Point>();
            double angleM0 = CalAngle(importantPoints[0], importantPoints[1]) + Math.PI / 2.0;
            double angleM1 = CalAngle(importantPoints[1], importantPoints[2]) + Math.PI / 2.0;
            for(int i = -5;i <= 5;i++)
            {
                var point0 = new Point();
                point0.X = M0.X + i * 5 * Math.Cos(angleM0);
                point0.Y = M0.Y + i * 5 * Math.Sin(angleM0);
                point0.Z = NeiCha(point0, points);
                point0.name = $"NA{i}";
                if(i == 0)
                {
                    PointsM0.Add(M0);
                }
                else
                {
                    PointsM0.Add(point0);
                }
                var point1 = new Point();
                point1.X = M1.X + i * 5 * Math.Cos(angleM1);
                point1.Y = M1.Y + i * 5 * Math.Sin(angleM1);
                point1.Z = NeiCha(point1, points);
                point1.name = $"NB{i}";
                if (i == 0)
                {
                    PointsM1.Add(M1);
                }
                else
                {
                    PointsM1.Add(point1);
                }
            }
            double StotalM0 = 0;
            double StotalM1 = 0;
            for (int i = 0; i < PointsM0.Count - 1; i++)
            {
                StotalM0 += CalS(PointsM0[i], PointsM0[i + 1], H0);
            }
            for (int j = 0; j < PointsM1.Count - 1; j++)
            {
                StotalM1 += CalS(PointsM1[j], PointsM1[j + 1], H0);
            }
        }
        #endregion
    }
}