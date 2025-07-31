using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_line
{
    class Line2D
    {
        // 2D 直线
        public Point point1;
        public Point point2;
        // 直线参数
        public double A;
        public double B;
        public double C;

        public int Inner_count = 0; //内点数量

        public Line2D(Point point1, Point point2)
        {
            this.point1 = point1;
            this.point2 = point2;

            Cal();
        }
        private void Cal()
        {
            // 计算直线参数
            //首先判断特殊情况
            double deta = 1e-5;
            if(Math.Abs(point1.X - point2.X) < deta)
            {
                // X1=X2
                A = 1;
                B = 0;
                C = -point1.X;
            }
            else if (Math.Abs(point1.Y - point2.Y) < deta)
            {
                // Y1 = Y2
                A = 0;
                B = 1;
                C = -point1.Y;
            }
            else
            {
                // 一般情况
                double m = (point2.Y - point1.Y) / (point2.X - point1.X);
                double b = point1.Y - m * point1.X;
                A = m / Math.Sqrt(m * m + 1);
                B = -1 / Math.Sqrt(m * m + 1);
                C = b / Math.Sqrt(m * m + 1);
            }
        }
    }

    class Line3D
    {
        // 3D 直线
        public Point point1;
        public Point point2;
        public Point point3;
        // 直线参数
        public double ux;
        public double uy;
        public double uz;
        public double P0x;
        public double P0y;
        public double P0z;

        public int Inner_count = 0; //内点数量

        public Line3D(Point point1, Point point2, Point point3)
        {
            this.point1 = point1;
            this.point2 = point2;
            this.point3 = point3;

            Cal();
        }
        private void Cal()
        {
            // 计算直线参数
            double under = Math.Sqrt((point2.X - point1.X) * (point2.X - point1.X) + (point2.Y - point1.Y) * (point2.Y - point1.Y) + (point2.Z - point1.Z) * (point2.Z - point1.Z));
            ux = (point2.X - point1.X) / under;
            uy = (point2.Y - point1.Y) / under;
            uz = (point2.Z - point1.Z) / under;
            P0x = point1.X;
            P0y = point1.Y;
            P0z = point1.Z;
        }
    }
}
