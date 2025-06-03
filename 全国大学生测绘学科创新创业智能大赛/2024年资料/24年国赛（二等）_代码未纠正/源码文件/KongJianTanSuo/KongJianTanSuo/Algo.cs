using System;
using System.Collections.Generic;
namespace KongJianTanSuo
{
    class Algo
    {
        #region 根据区号得到对应的分析区的所有点
        public static void GetArea(List<Point> points,double area_code,ref Area area)
        {
            List<Point> pointsCode = new List<Point>();
            foreach(Point d in points)
            {
                if(d.area_code == area_code)
                {
                    pointsCode.Add(d);
                }
            }
            area.points = pointsCode;
        }
        #endregion

        #region 计算XY的平均值
        public static void GetXavgYavg(ref Area area)
        {
            foreach(Point d in area.points)
            {
                area.Xavg += d.x;
                area.Yavg += d.y;
            }
            area.Xavg /= area.points.Count;
            area.Yavg /= area.points.Count;
        }
        #endregion

        #region 计算椭圆参数
        public static void GetTuoYuan(ref Area area)
        {
            foreach(Point d in area.points)
            {
                d.a = d.x - area.Xavg;
                d.b = d.y - area.Yavg;
            }
            foreach(Point d in area.points)
            {
                area.ai2 += d.a * d.a;
                area.bi2 += d.b * d.b;
                area.aibi += d.a * d.b;
            }
            double A = area.ai2 - area.bi2;
            double B = Math.Sqrt(Math.Pow(area.ai2 - area.bi2, 2) + 4.0 * Math.Pow(area.aibi, 2));
            double C = 2.0 * area.aibi;
            area.A = A;
            area.B = B;
            area.C = C;
            area.Angle = Math.Atan((A + B) / C);
            double SDEXabove = 0;
            double SDEYabove = 0;
            foreach(Point d in area.points)
            {
                SDEXabove += Math.Pow((d.a * Math.Cos(area.Angle) + d.b * Math.Sin(area.Angle)),2);
                SDEYabove += Math.Pow((d.a * Math.Sin(area.Angle) - d.b * Math.Cos(area.Angle)),2);
            }
            area.SDEX = Math.Sqrt(2.0 * (SDEXabove / area.points.Count));
            area.SDEY = Math.Sqrt(2.0 * (SDEYabove / area.points.Count));
        }
        #endregion

        #region 构建权重矩阵
        public static double[,] GetMatrix(ref Area area1, ref Area area2, ref Area area3,ref  Area area4, ref Area area5, ref Area area6, ref Area area7)
        {
            GetXavgYavg(ref area1);
            GetXavgYavg(ref area2);
            GetXavgYavg(ref area3);
            GetXavgYavg(ref area4);
            GetXavgYavg(ref area5);
            GetXavgYavg(ref area6);
            GetXavgYavg(ref area7);
            Area[] areas = { area1, area2, area3, area4, area5, area6, area7 };
            double[,] Matrix = new double[7, 7];
            for(int i = 0;i < 7;i++)
            {
                for(int j = 0;j < 7;j++)
                {
                    if(i == j)
                    {
                        Matrix[i, j] = 0;
                        continue;
                    }
                    Matrix[i, j] = 1000.0 / (GetDistance(areas[i], areas[j]));
                }
            }
            return Matrix;
        }
        public static double GetDistance(Area area1,Area area2)
        {
            double temp = Math.Pow(area1.Xavg - area2.Xavg, 2) + Math.Pow(area1.Yavg - area2.Yavg, 2);
            return Math.Sqrt(temp);
        }
        #endregion

        #region 计算全局莫兰指数
        public static void GetMolan(ref Area areaTotal,ref Area area1, ref Area area2,ref Area area3, ref Area area4,ref  Area area5, ref Area area6, ref Area area7,ref double[,]Martrix)
        {
            //全局莫兰
            Area[] areas = { area1, area2, area3, area4, area5, area6, area7 };
            double Xavg = areaTotal.points.Count / 7.0;
            double S0 = 0;
            double Iabove = 0;
            double Iunder = 0;
            for(int i = 0;i < 7;i++)
            {
                Iunder += (areas[i].points.Count - Xavg) * (areas[i].points.Count - Xavg);
                for(int j = 0;j < 7;j++)
                {
                    S0 += Martrix[i, j];
                    Iabove += Martrix[i, j] * (areas[i].points.Count - Xavg) * (areas[j].points.Count - Xavg);
                }
            }
            double Molan = 7.0 / S0 * (Iabove / Iunder);
            //局部莫兰
            double[] JuBuMolan = new double[7];
            for (int i = 0; i < 7; i++)
            {
                double Si2 = 0;
                double right = 0;
                for (int j = 0; j < 7;j++)
                {
                    if(j != i)
                    {
                        Si2 += Math.Pow(areas[j].points.Count - Xavg, 2);
                        right += Martrix[i, j] * (areas[j].points.Count - Xavg);
                    }
                }
                Si2 /= (areas[i].points.Count - 1.0);
                JuBuMolan[i] = (areas[i].points.Count - Xavg) / Si2 * right;
            }
            areaTotal.JuBuMolan = JuBuMolan;
        }
        #endregion

        #region 计算Z得分
        public static void GetZ(ref Area areaTotal)
        {
            double u = 0;
            foreach(double d in areaTotal.JuBuMolan)
            {
                u += d;
            }
            u /= 7.0;
            double fai = 0;
            foreach(double d in areaTotal.JuBuMolan)
            {
                fai += Math.Pow(d - u, 2); 
            }
            fai = Math.Sqrt(fai / 6.0);
            double[] Z = new double[7];
            for(int i = 0;i < 7;i++)
            {
                Z[i] = (areaTotal.JuBuMolan[i] - u) / fai;
            }
            ;
        }
        #endregion
    }
}