using System;
using System.Collections.Generic;
namespace SpaceFowardIntersection
{
    class Data
    {
        public double[] list1 = new double[9];
        public double[] list2 = new double[9];
        public List<double[]> totalList = new List<double[]>();
        public double u1,u2,v1,v2,w1,w2;//计算辅助坐标系
        public double N1, N2;
        public double X, Y, Z;

        public Data(string[] line)
        {
            for (int i = 0;i <= 8;i++)
            {
                list1[i] = (double.Parse(line[i]));
            }
            for (int i = 9; i <= 17; i++)
            {
                list2[i - 9] = (double.Parse(line[i]));
                if(i == 17)
                {
                    list2[i - 9] = (double.Parse(line[8]));
                }
            }
            totalList.Add(list1);
            totalList.Add(list2);
            Caluvw();//计算辅助坐标系
            Caltouyin();//计算投影系数
            Caldimian();//计算地面坐标
        }
        private void Caluvw()
        {
            
            {
                int i = 1;
                foreach(double[] d in totalList)
                {
                    double a1 = Math.Cos(d[3]) * Math.Cos(d[5]) - Math.Sin(d[3]) * Math.Sin(d[4]) * Math.Sin(d[5]);
                    double a2 = -Math.Cos(d[3]) * Math.Cos(d[5]) - Math.Sin(d[3]) * Math.Sin(d[4]) * Math.Sin(d[5]);
                    double a3 = -Math.Sin(d[3]) * Math.Cos(d[4]);
                    double b1 = Math.Cos(d[4]) * Math.Sin(d[5]);
                    double b2 = Math.Cos(d[4]) * Math.Cos(d[5]);
                    double b3 = -Math.Sin(d[4]);
                    double c1 = Math.Sin(d[3]) * Math.Cos(d[5]) + Math.Cos(d[3]) * Math.Sin(d[4]) * Math.Sin(d[5]);
                    double c2 = -Math.Sin(d[3]) * Math.Cos(d[5]) + Math.Cos(d[3]) * Math.Sin(d[4]) * Math.Sin(d[5]);
                    double c3 = Math.Cos(d[3]) * Math.Cos(d[4]);
                    if(i == 1)
                    {
                        u1 = a1 * d[6] + a2 * d[7] - a3 * d[8];
                        v1 = b1 * d[6] + b2 * d[7] - b3 * d[8];
                        w1 = c1 * d[6] + c2 * d[7] - c3 * d[8];
                    }
                    if(i == 2)
                    {
                        u2 = a1 * d[6] + a2 * d[7] - a3 * d[8];
                        v2 = b1 * d[6] + b2 * d[7] - b3 * d[8];
                        w2 = c1 * d[6] + c2 * d[7] - c3 * d[8];
                    }
                    i++;
                    
                }
                
            }
        }
        private void Caltouyin()
        {
            double Bu, Bv, Bw;
            Bu = totalList[1][0] - totalList[0][0];
            Bv = totalList[1][1] - totalList[0][1];
            Bw = totalList[1][2] - totalList[0][2];
            N1 = (Bu * w2 - Bw * u2) / (u1 * w2 - u2 * w1);
            N2 = (Bu * w1 - Bw * u1) / (u1 * w2 - u2 * w1);
        }
        private void Caldimian()
        {
            X = totalList[0][0] + N1 * u1;
            Y = 0.5 * (totalList[0][1] + N1 * v1 + totalList[1][1] + N2 * v2);
            Z = totalList[0][2] + N1 * w1;
        }
    }
}