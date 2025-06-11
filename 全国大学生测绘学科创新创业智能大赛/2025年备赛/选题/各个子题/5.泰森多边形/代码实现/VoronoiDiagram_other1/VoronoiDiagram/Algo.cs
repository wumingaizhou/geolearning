using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VoronoiDiagram
{
    /// <summary>
    /// 定义点类
    /// </summary>


    /// <summary>
    /// 定义三角形类
    /// </summary>
    public class Triangle
    {
        public Point P0 { get; set; }
        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public Triangle(Point p0, Point p1, Point p2)
        {
            P0 = p0;
            P1 = p1;
            P2 = p2;
        }

        /// <summary>
        /// 判断三角形是否包含指定点
        /// </summary>
        public bool HasVertex(Point p)
        {
            return P0.Equals(p) || P1.Equals(p) || P2.Equals(p);
        }

        /// <summary>
        /// 判断三角形是否包含指定边
        /// </summary>
        public bool HasEdge(Point p1, Point p2)
        {
            return (P0.Equals(p1) && P1.Equals(p2)) || (P0.Equals(p1) && P2.Equals(p2)) ||
                   (P1.Equals(p1) && P0.Equals(p2)) || (P1.Equals(p1) && P2.Equals(p2)) ||
                   (P2.Equals(p1) && P0.Equals(p2)) || (P2.Equals(p1) && P1.Equals(p2));
        }

        /// <summary>
        /// 获取不包含指定点的其他顶点
        /// </summary>
        public List<Point> Exclude(Point p)
        {
            var result = new List<Point>();
            if (!P0.Equals(p)) result.Add(P0);
            if (!P1.Equals(p)) result.Add(P1);
            if (!P2.Equals(p)) result.Add(P2);
            return result;
        }

        /// <summary>
        /// 获取不包含指定两点的第三个顶点
        /// </summary>
        public List<Point> Exclude(Point p1, Point p2)
        {
            var result = new List<Point>();
            if (!P0.Equals(p1) && !P0.Equals(p2)) result.Add(P0);
            if (!P1.Equals(p1) && !P1.Equals(p2)) result.Add(P1);
            if (!P2.Equals(p1) && !P2.Equals(p2)) result.Add(P2);
            return result;
        }

        /// <summary>
        /// 创建包围给定矩形的超级三角形
        /// </summary>
        public static Triangle CreateSuperTriangle(List<Point> points)
        {
            double minX = points.Min(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxX = points.Max(p => p.X);
            double maxY = points.Max(p => p.Y);

            double dx = maxX - minX;
            double dy = maxY - minY;
            double deltaMax = Math.Max(dx, dy);
            double midX = (minX + maxX) / 2;
            double midY = (minY + maxY) / 2;

            Point p0 = new Point(midX - 20 * deltaMax, midY - deltaMax);
            Point p1 = new Point(midX, midY + 20 * deltaMax);
            Point p2 = new Point(midX + 20 * deltaMax, midY - deltaMax);

            return new Triangle(p0, p1, p2);
        }
    }

    /// <summary>
    /// 定义圆类
    /// </summary>
    public class Circle
    {
        public Point Center { get; set; }
        public double Radius { get; set; }

        public Circle(Point center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// 计算三角形的外接圆
        /// </summary>
        public static Circle Circumcircle(Triangle triangle)
        {
            Point p1 = triangle.P0;
            Point p2 = triangle.P1;
            Point p3 = triangle.P2;

            double ax = p1.X; double ay = p1.Y;
            double bx = p2.X; double by = p2.Y;
            double cx = p3.X; double cy = p3.Y;

            double d = 2 * (ax * (by - cy) + bx * (cy - ay) + cx * (ay - by));
            if (Math.Abs(d) < 1e-10) return null; // 三点共线

            double ux = ((ax * ax + ay * ay) * (by - cy) + (bx * bx + by * by) * (cy - ay) + (cx * cx + cy * cy) * (ay - by)) / d;
            double uy = ((ax * ax + ay * ay) * (cx - bx) + (bx * bx + by * by) * (ax - cx) + (cx * cx + cy * cy) * (bx - ax)) / d;

            Point center = new Point(ux, uy);
            double radius = center.GetDistance(p1);

            return new Circle(center, radius);
        }

        /// <summary>
        /// 测试点是否在圆内
        /// </summary>
        public string Test(Point p)
        {
            double distance = Center.GetDistance(p);
            if (distance < Radius - 1e-10) return "in";
            if (distance > Radius + 1e-10) return "out";
            return "on";
        }
    }

    /// <summary>
    /// 边类，用于Bowyer-Watson算法
    /// </summary>
    public class Edge
    {
        public Point P1 { get; set; }
        public Point P2 { get; set; }

        public Edge(Point p1, Point p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public bool Equals(Edge other)
        {
            return (P1.Equals(other.P1) && P2.Equals(other.P2)) ||
                   (P1.Equals(other.P2) && P2.Equals(other.P1));
        }
    }

    /// <summary>
    /// Voronoi单元类
    /// </summary>
    public class VoronoiCell
    {
        public Point Site { get; set; }
        public List<Point> Vertices { get; set; }

        public VoronoiCell(Point site)
        {
            Site = site;
            Vertices = new List<Point>();
        }
    }

    /// <summary>
    /// 主算法类
    /// </summary>
    public class Algo
    {
        public List<Triangle> Triangles { get; set; }
        public List<VoronoiCell> VoronoiCells { get; set; }

        public Algo()
        {
            Triangles = new List<Triangle>();
            VoronoiCells = new List<VoronoiCell>();
        }

        /// <summary>
        /// 主要计算方法，替代原来的go方法
        /// </summary>
        public void Calculate(List<Point> points)
        {
            // 清空之前的结果
            Triangles.Clear();
            VoronoiCells.Clear();

            if (points.Count < 3) return;

            // 执行Delaunay三角剖分
            DelaunayBowyer(points);

            // 生成Voronoi图
            GenerateVoronoi(points);
        }

        /// <summary>
        /// 使用Bowyer-Watson算法进行Delaunay三角剖分
        /// </summary>
        private void DelaunayBowyer(List<Point> points)
        {
            // 按X坐标排序点
            var sortedPoints = points.OrderBy(p => p.X).ToList();

            // 创建超级三角形
            Triangle superTriangle = Triangle.CreateSuperTriangle(sortedPoints);
            var triangles = new List<Triangle> { superTriangle };

            // 逐个添加点
            foreach (var point in sortedPoints)
            {
                var badTriangles = new List<Triangle>();
                var polygon = new List<Edge>();

                // 找到包含当前点的三角形（需要删除的三角形）
                foreach (var triangle in triangles.ToList())
                {
                    var circle = Circle.Circumcircle(triangle);
                    if (circle != null && circle.Test(point) != "out")
                    {
                        badTriangles.Add(triangle);
                        // 添加三角形的边到多边形
                        polygon.Add(new Edge(triangle.P0, triangle.P1));
                        polygon.Add(new Edge(triangle.P1, triangle.P2));
                        polygon.Add(new Edge(triangle.P2, triangle.P0));
                        triangles.Remove(triangle);
                    }
                }

                // 移除重复的边（内部边）
                var uniqueEdges = new List<Edge>();
                foreach (var edge in polygon)
                {
                    bool isDuplicate = false;
                    for (int i = uniqueEdges.Count - 1; i >= 0; i--)
                    {
                        if (edge.Equals(uniqueEdges[i]))
                        {
                            uniqueEdges.RemoveAt(i);
                            isDuplicate = true;
                            break;
                        }
                    }
                    if (!isDuplicate)
                    {
                        uniqueEdges.Add(edge);
                    }
                }

                // 用当前点和唯一边创建新三角形
                foreach (var edge in uniqueEdges)
                {
                    if (!point.Equals(edge.P1) && !point.Equals(edge.P2) && !edge.P1.Equals(edge.P2))
                    {
                        triangles.Add(new Triangle(point, edge.P1, edge.P2));
                    }
                }
            }

            // 移除包含超级三角形顶点的三角形
            for (int i = triangles.Count - 1; i >= 0; i--)
            {
                var triangle = triangles[i];
                if (triangle.HasVertex(superTriangle.P0) ||
                    triangle.HasVertex(superTriangle.P1) ||
                    triangle.HasVertex(superTriangle.P2))
                {
                    triangles.RemoveAt(i);
                }
            }

            Triangles = triangles;
        }

        /// <summary>
        /// 生成Voronoi图
        /// </summary>
        private void GenerateVoronoi(List<Point> points)
        {
            var voronoiCells = new List<VoronoiCell>();

            foreach (var vertex in points)
            {
                var cell = new VoronoiCell(vertex);

                // 找到包含当前顶点的所有三角形
                var containingTriangles = new List<Triangle>();
                foreach (var triangle in Triangles)
                {
                    if (triangle.HasVertex(vertex))
                    {
                        containingTriangles.Add(triangle);
                    }
                }

                if (containingTriangles.Count == 0) continue;

                // 按顺序排列三角形
                var sortedTriangles = new List<Triangle>();
                var remaining = new List<Triangle>(containingTriangles);

                if (remaining.Count > 0)
                {
                    sortedTriangles.Add(remaining[0]);
                    remaining.RemoveAt(0);

                    var currentEdge = sortedTriangles[0].Exclude(vertex)[0];

                    while (remaining.Count > 0)
                    {
                        bool found = false;
                        for (int i = 0; i < remaining.Count; i++)
                        {
                            var triangle = remaining[i];
                            if (triangle.HasEdge(vertex, currentEdge))
                            {
                                var excludedPoints = triangle.Exclude(vertex, currentEdge);
                                if (excludedPoints.Count > 0)
                                {
                                    currentEdge = excludedPoints[0];
                                }
                                sortedTriangles.Add(triangle);
                                remaining.RemoveAt(i);
                                found = true;
                                break;
                            }
                        }
                        if (!found) break;
                    }
                }

                // 如果所有三角形都已排序，计算Voronoi顶点
                if (remaining.Count == 0)
                {
                    foreach (var triangle in sortedTriangles)
                    {
                        var circle = Circle.Circumcircle(triangle);
                        if (circle != null)
                        {
                            cell.Vertices.Add(circle.Center);
                        }
                    }
                }

                voronoiCells.Add(cell);
            }

            VoronoiCells = voronoiCells;
        }

        /// <summary>
        /// 绘制Voronoi图
        /// </summary>
        public void DrawVoronoiDiagram(Graphics g)
        {
            // 设置绘图参数
            Pen voronoiEdgePen = new Pen(Color.Blue, 1);      // Voronoi边的画笔
            Pen delaunayEdgePen = new Pen(Color.LightGray, 1); // Delaunay边的画笔
            Brush siteBrush = new SolidBrush(Color.Red);       // 原始点的画刷
            Brush vertexBrush = new SolidBrush(Color.Green);   // Voronoi顶点的画刷

            // 用白色填充背景
            g.Clear(Color.White);

            // 绘制Delaunay三角网（可选，作为背景）
            foreach (var triangle in Triangles)
            {
                System.Drawing.Point pointA = new System.Drawing.Point((int)triangle.P0.X, (int)triangle.P0.Y);
                System.Drawing.Point pointB = new System.Drawing.Point((int)triangle.P1.X, (int)triangle.P1.Y);
                System.Drawing.Point pointC = new System.Drawing.Point((int)triangle.P2.X, (int)triangle.P2.Y);

                g.DrawLine(delaunayEdgePen, pointA, pointB);
                g.DrawLine(delaunayEdgePen, pointB, pointC);
                g.DrawLine(delaunayEdgePen, pointC, pointA);
            }

            // 绘制Voronoi单元
            foreach (var cell in VoronoiCells)
            {
                if (cell.Vertices.Count < 2) continue;

                // 绘制Voronoi单元的边
                for (int i = 0; i < cell.Vertices.Count; i++)
                {
                    var current = cell.Vertices[i];
                    var next = cell.Vertices[(i + 1) % cell.Vertices.Count];

                    g.DrawLine(voronoiEdgePen,
                        (float)current.X, (float)current.Y,
                        (float)next.X, (float)next.Y);
                }

                // 绘制Voronoi顶点
                foreach (var vertex in cell.Vertices)
                {
                    g.FillEllipse(vertexBrush,
                        (float)vertex.X - 2,
                        (float)vertex.Y - 2,
                        4, 4);
                }
            }

            // 绘制原始点（生成点）
            foreach (var cell in VoronoiCells)
            {
                g.FillEllipse(siteBrush,
                    (float)(cell.Site.X - 3),
                    (float)(cell.Site.Y - 3),
                    6, 6);
            }
        }
    }
}