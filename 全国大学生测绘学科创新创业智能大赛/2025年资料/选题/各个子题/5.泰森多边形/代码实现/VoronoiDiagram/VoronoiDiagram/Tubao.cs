using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
    class Tubao
    {
        //凸包实现算法
        public List<Point> TubaoPoints = new List<Point>();
        //凸包面积

        public Tubao(List<Point> points)
        {
            List<Point> sortPoints = new List<Point>();
            sortPoints = points.Distinct().OrderBy(p => p.X).ThenBy(p => p.Y).ToList();

            // 下凸包
            var lower = new List<Point>();
            foreach (var point in sortPoints)
            {
                while (lower.Count >= 2 && CrossProduct(lower[lower.Count - 2], lower[lower.Count - 1], point) <= 0)
                {
                    //此时右转了
                    lower.RemoveAt(lower.Count - 1);
                }
                lower.Add(point);
            }

            // 上凸包
            var upper = new List<Point>();
            for (int i = sortPoints.Count - 1; i >= 0; i--)
            {
                var tempPoint = sortPoints[i];
                while (upper.Count >= 2 && CrossProduct(upper[upper.Count - 2], upper[upper.Count - 1], tempPoint) <= 0)
                {
                    upper.RemoveAt(upper.Count - 1);
                }
                upper.Add(tempPoint);
            }

            // 合并
            lower.RemoveAt(lower.Count - 1);
            upper.RemoveAt(upper.Count - 1);
            lower.AddRange(upper);

            TubaoPoints = lower;
        }

        //叉积运算
        private double CrossProduct(Point o, Point a, Point b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        //获取凸包
        public List<Point> GetTuBaoPoints()
        {
            return TubaoPoints;
        }
        //获取面积
        public double GetTuBaoSquare()
        {
            return ComputePolygonArea(TubaoPoints);
        }

        // 计算多边形面积
        public static double ComputePolygonArea(List<Point> polygon)
        {
            int n = polygon.Count;
            if (n < 3) return 0;

            double area = 0;

            for (int i = 0; i < n; i++)
            {
                Point current = polygon[i];
                Point next = polygon[(i + 1) % n];
                area += current.X * next.Y - next.X * current.Y;
            }

            return Math.Abs(area) / 2.0;
        }
    }
}
