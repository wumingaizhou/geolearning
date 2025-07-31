using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
    public class VoronoiDiagram
    {

        public Dictionary<Point, List<Point>> voronoiCells = new Dictionary<Point, List<Point>>();
        public List<Point> Tubao_Points = new List<Point>(); // 凸包点
        public List<Point> points = new List<Point>(); //原始点
        public List<Triangle> triangles = new List<Triangle>(); //三角网
        public Dictionary<Point, List<Point>> voronoiCellsRemove = new Dictionary<Point, List<Point>>(); //去除边界点的

        public VoronoiDiagram(List<Point> Origin_Points, List<Triangle> Origin_Triangles, List<Point> TubaoPoints)
        {
            Tubao_Points = TubaoPoints;
            points = Origin_Points;
            triangles = Origin_Triangles;

            BuildVoronoiDiagram();

            RemovePoints(); //去除边界点
        }

        public void BuildVoronoiDiagram()
        {
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
        }

        private void RemovePoints()
        {
            // 去除边界点
            const double delta = 1e-5;
            bool IsBoundaryPoint(Point p) =>
                Tubao_Points.Any(tp => Math.Abs(tp.X - p.X) < delta && Math.Abs(tp.Y - p.Y) < delta);

            voronoiCellsRemove = voronoiCells
                .Where(kvp => !IsBoundaryPoint(kvp.Key))
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Select(p => new Point(p.X, p.Y)).ToList()
                );
        }

        // 获得去除边界点后的结果
        public Dictionary<Point, List<Point>> GetResult()
        {
            return voronoiCellsRemove;
        }

        //获得面积
        public string GetResultSquare()
        {
            string result = "泰森多边形面积结果如下：\n";

            // 泰森多边形面积
            foreach (var cell in voronoiCellsRemove)
            {
                var cell_points = cell.Value;

                var cell_points_area = ComputePolygonArea(cell_points);

                var temp_txt = $"点 X 为{cell.Key.X}，Y 为{cell.Key.Y}的生成点的泰森多边形面积是：{cell_points_area} \n";

                result += temp_txt;
            }
            return result;
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

        private List<Point> SortByPolarAngle(List<Point> vertices, Point center)
        {
            return vertices.OrderBy(v => Math.Atan2(v.Y - center.Y, v.X - center.X)).ToList();
        }

        // 判断两个点是否相等
        private bool IsPointEqual(Point p1, Point p2)
        {
            const double tolerance = 1e-10;
            return Math.Abs(p1.X - p2.X) < tolerance && Math.Abs(p1.Y - p2.Y) < tolerance;
        }

    }
}