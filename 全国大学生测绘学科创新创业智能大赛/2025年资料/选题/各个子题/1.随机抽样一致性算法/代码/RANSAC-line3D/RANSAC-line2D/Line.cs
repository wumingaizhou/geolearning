using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_line3D
{
    class Line
    {
        //直线的数据结构
        public Point Point0 = new Point();
        public double Ux;
        public double Uy;
        public double Uz;

        public int CountPoint = 0; // 内点的数量

        public Line(Point point1, Point point2)
        {
            Point0.X = point1.X;
            Point0.Y = point1.Y;
            Point0.Z = point1.Z;

            Ux = point2.X - point1.X;
            Uy = point2.Y - point1.Y;
            Uz = point2.Z - point1.Z;

            double under = Math.Sqrt(Ux * Ux + Uy * Uy + Uz * Uz);
            Ux = Ux / under;
            Uy = Uy / under;
            Uz = Uz / under;

        }

        public void Calculate(List<Point> points, double deta, int times)
        {
            // deta：阈值，times：迭代次数
            foreach (var p in points)
            {
                double tempx = (p.X - Point0.X) * Ux;
                double tempy = (p.Y - Point0.Y) * Uy;
                double tempz = (p.Z - Point0.Z) * Uz;

                double d = Math.Sqrt(tempx * tempx + tempy * tempy + tempz * tempz);
                if (d <= deta)
                {
                    CountPoint++;
                }
            }
        }
    }
}
