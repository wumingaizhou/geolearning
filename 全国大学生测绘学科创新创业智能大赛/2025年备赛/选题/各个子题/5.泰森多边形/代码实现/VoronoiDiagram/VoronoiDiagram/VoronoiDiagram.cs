using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
    public class VoronoiDiagram
    {
        public Dictionary<Point, List<Point>> BuildVoronoiDiagram(
            List<Point> points, List<Triangle> triangles)
        {
            var voronoiCells = new Dictionary<Point, List<Point>>();

            // 为每个原始点创建其泰森多边形
            foreach (var point in points)
            {
                // 找到包含该点的所有三角形
                var relatedTriangles = triangles.Where(t =>
                    IsPointEqual(t.A, point) || IsPointEqual(t.B, point) || IsPointEqual(t.C, point)).ToList();

                if (relatedTriangles.Count == 0)
                    continue;

                // 获取这些三角形的外接圆心
                var circumcenters = relatedTriangles.Select(t => t.GetCircumenter()).ToList();

                // 按极角排序
                var sortedVertices = SortByPolarAngle(circumcenters, point);

                voronoiCells[point] = sortedVertices;
            }

            return voronoiCells;
        }

        private List<Point> SortByPolarAngle(List<Point> vertices, Point center)
        {
            return vertices.OrderBy(v => Math.Atan2(v.Y - center.Y, v.X - center.X)).ToList();
        }

        // 判断两个点是否相等（考虑浮点数精度）
        private bool IsPointEqual(Point p1, Point p2)
        {
            const double tolerance = 1e-10;
            return Math.Abs(p1.X - p2.X) < tolerance && Math.Abs(p1.Y - p2.Y) < tolerance;
        }

    }
}