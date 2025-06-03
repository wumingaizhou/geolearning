using System;
using System.Collections.Generic;

namespace DP
{
    class Algo
    {
        public pointEntity pts;
        public int calMax;//阈值
        public int count;
        public Point temp;
        public Algo(pointEntity oripts,int setMax)
        {
            pts = oripts;
            calMax = setMax;
            count = oripts.Count;
        }
        public Stack<Point> calRun()
        {
            //DP算法
            Point P0 = pts[0];//初始起点
            Point P1 = pts[count - 1];//初始终点
            Stack<Point> proPoints = new Stack<Point>();//待处理的堆栈
            Stack<Point> rePoints = new Stack<Point>();//结果堆栈
            proPoints.Push(P1);
            rePoints.Push(P0);
            while(proPoints.Count != 0)//当待处理的堆栈里面没有点时，执行以下
            {
                double dMax = 0;//两点之间距离最大的点
                if (P0.ID != P1.ID - 1)//当两点之间没有点的时候执行
                {
                    for(int i = P0.ID;i < P1.ID - 1;i++)//求距离最远的点
                    {
                        double d = Distance(pts[i],P0, P1);
                        if(d > dMax)
                        {
                            temp = pts[i];
                            dMax = d;
                        }
                    }
                    if (dMax > calMax)//如果大于阈值
                    {
                        P1 = temp;
                        proPoints.Push(P1);
                    }
                    else//如果小于阈值
                    {
                        P0 = P1;
                        rePoints.Push(P0);
                        proPoints.Pop();
                        if(proPoints.Count != 0)
                        {
                            P1 = proPoints.Peek();
                        }
                    }
                }
                else//当两点之间没有点的时候执行
                {
                    P0 = P1;
                    rePoints.Push(P0);
                    proPoints.Pop();
                    if(proPoints.Count != 0)
                    {
                        P1 = proPoints.Peek();
                    }
                }

            }
            while(rePoints.Count != 0)//当proPoints里面没有点时，算法执行完毕，置换顺序
            {
                proPoints.Push(rePoints.Pop());
            }
            return proPoints;
        }
        public double Distance(Point pi, Point p0,Point p1)//海伦公式求高度
        {
            double L01 = Math.Sqrt((p0.X - p1.X) * (p0.X - p1.X) + (p0.Y - p1.Y) * (p0.Y - p1.Y));
            double L0i = Math.Sqrt((p0.X - pi.X) * (p0.X - pi.X) + (p0.Y - pi.Y) * (p0.Y - pi.Y));
            double Li1 = Math.Sqrt((pi.X - p1.X) * (pi.X - p1.X) + (pi.Y - p1.Y) * (pi.Y - p1.Y));
            double P = (L01 + L0i + Li1) / 2;
            double s = Math.Sqrt(P * (P - L0i) * (P - Li1) * (P - L01));
            double d = 2 * (s / L01);
            return d;
        }

    }

}