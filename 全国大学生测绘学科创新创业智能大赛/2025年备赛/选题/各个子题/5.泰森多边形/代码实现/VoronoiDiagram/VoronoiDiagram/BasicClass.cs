using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{

    public class Point
    {
        public double X;
        public double Y;

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // 三角形
    public class Triangle
    {
        public Point A;
        public Point B;
        public Point C;

        public Triangle(Point point1, Point point2, Point point3)
        {
            A = new Point(point1.X, point1.Y);
            B = new Point(point2.X, point2.Y);
            C = new Point(point3.X, point3.Y);
        }

        // 计算外接圆圆心
        public Point GetCircumenter()
        {
            double ax = A.X, ay = A.Y;
            double bx = B.X, by = B.Y;
            double cx = C.X, cy = C.Y;

            double d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));

            // 检查是否为退化三角形
            if (Math.Abs(d) < 1e-10)
            {
                // 如果三点共线，返回中点
                return new Point((ax + bx + cx) / 3, (ay + by + cy) / 3);
            }

            double ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            double uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

            return new Point(ux, uy);
        }

        // 计算面积
        public double GerArea()
        {
            return 0;
        }
    }

    // 边结构
    public class Edge
    {
        public Point P1;
        public Point P2;

        public Edge(Point point1, Point point2)
        {
            P1 = new Point(point1.X, point1.Y);
            P2 = new Point(point2.X, point2.Y);
        }

        public bool EdgeEquals(Edge otherEdge)
        {
            var flag = P1.X == otherEdge.P1.X && P1.Y == otherEdge.P1.Y && P2.X == otherEdge.P2.X && P2.Y == otherEdge.P2.Y;
            var flagReverse = P1.X == otherEdge.P2.X && P1.Y == otherEdge.P2.Y && P2.X == otherEdge.P1.X && P2.Y == otherEdge.P1.Y;
            if(flag || flagReverse)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

}
