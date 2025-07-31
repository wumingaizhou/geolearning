using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_line2D
{
    class Line
    {
        //直线的数据结构，两点构成一条直线，Ax + By + C = 0
        public double LineA;
        public double LineB;
        public double LineC;
        public int CountPoint = 0; // 内点的数量

        public Line (Point point1,Point point2)
        {
            double deta = 1e-5; //接近于0
            if(Math.Abs(point1.X - point2.X) < deta)
            {
                //直线为垂直线
                LineA = 1;
                LineB = 0;
                LineC = -point1.X;
            }
            else if(Math.Abs(point1.Y - point2.Y) < deta)
            {
                // 水平线
                LineA = 0;
                LineB = 1;
                LineC = -point1.Y;
            }
            else
            {
                // 一般情况
                double m = (point2.Y - point1.Y)/(point2.X - point1.X);
                double b = point1.Y - m * point1.X;

                double under = Math.Sqrt(m * m + 1);
                LineA = m / under;
                LineB = -1 / under;
                LineC = b / under;

            }
        }

        public void Calculate(List<Point> points,double deta,int times)
        {
            // deta：阈值，times：迭代次数
            foreach(var p in points)
            {
                double d = Math.Abs(LineA * p.X + LineB * p.Y + LineC);
                if(d <= deta)
                {
                    CountPoint++;
                }
            }
        }
    }
}
