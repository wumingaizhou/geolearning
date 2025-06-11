using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace VoronoiDiagram
{
    // 定义Voronoi顶点类
    class VoronoiVertex
    {
        public double X;
        public double Y;

        public VoronoiVertex(double x, double y)
        {
            X = x;
            Y = y;
        }
    }

    // 定义Voronoi单元类
    class VoronoiCell
    {
        public Point Site;  // 原始点
        public List<VoronoiVertex> Vertices = new List<VoronoiVertex>();

        public VoronoiCell(Point site)
        {
            Site = site;
        }
    }

    // 定义三角形类
    class Triangle
    {
        public Point A;
        public Point B;
        public Point C;

        public Triangle(Point a, Point b, Point c)
        {
            A = a;
            B = b;
            C = c;
        }

        // 检查点是否在三角形内部
        public bool Contains(Point p)
        {
            return IsPointInTriangle(p, A, B, C);
        }

        // 检查三角形是否包含某个顶点
        public bool HasVertex(Point p)
        {
            return PointEquals(A, p) || PointEquals(B, p) || PointEquals(C, p);
        }

        // 获取三角形的外接圆圆心
        public VoronoiVertex GetCircumcenter()
        {
            double d = 2 * (A.X * (B.Y - C.Y) + B.X * (C.Y - A.Y) + C.X * (A.Y - B.Y));
            if (Math.Abs(d) < 1e-10) return null; // 三点共线

            double ux = ((A.X * A.X + A.Y * A.Y) * (B.Y - C.Y) + 
                        (B.X * B.X + B.Y * B.Y) * (C.Y - A.Y) + 
                        (C.X * C.X + C.Y * C.Y) * (A.Y - B.Y)) / d;
            double uy = ((A.X * A.X + A.Y * A.Y) * (C.X - B.X) + 
                        (B.X * B.X + B.Y * B.Y) * (A.X - C.X) + 
                        (C.X * C.X + C.Y * C.Y) * (B.X - A.X)) / d;

            return new VoronoiVertex(ux, uy);
        }

        // 获取外接圆半径平方
        public double GetCircumradiusSquared()
        {
            var center = GetCircumcenter();
            if (center == null) return double.MaxValue;
            
            double dx = center.X - A.X;
            double dy = center.Y - A.Y;
            return dx * dx + dy * dy;
        }

        // 检查点是否在外接圆内
        public bool IsPointInCircumcircle(Point p)
        {
            var center = GetCircumcenter();
            if (center == null) return false;

            double dx = p.X - center.X;
            double dy = p.Y - center.Y;
            double distSq = dx * dx + dy * dy;
            
            return distSq < GetCircumradiusSquared() - 1e-10;
        }

        private bool IsPointInTriangle(Point p, Point a, Point b, Point c)
        {
            double denom = (b.Y - c.Y) * (a.X - c.X) + (c.X - b.X) * (a.Y - c.Y);
            if (Math.Abs(denom) < 1e-10) return false;

            double alpha = ((b.Y - c.Y) * (p.X - c.X) + (c.X - b.X) * (p.Y - c.Y)) / denom;
            double beta = ((c.Y - a.Y) * (p.X - c.X) + (a.X - c.X) * (p.Y - c.Y)) / denom;
            double gamma = 1 - alpha - beta;

            return alpha >= 0 && beta >= 0 && gamma >= 0;
        }

        private bool PointEquals(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) < 1e-10 && Math.Abs(a.Y - b.Y) < 1e-10;
        }
    }

    // 边类，用于Delaunay三角剖分
    class Edge
    {
        public Point A;
        public Point B;

        public Edge(Point a, Point b)
        {
            A = a;
            B = b;
        }

        public override bool Equals(object obj)
        {
            if (obj is Edge other)
            {
                return (PointEquals(A, other.A) && PointEquals(B, other.B)) ||
                       (PointEquals(A, other.B) && PointEquals(B, other.A));
            }
            return false;
        }

        public override int GetHashCode()
        {
            return A.GetHashCode() ^ B.GetHashCode();
        }

        private bool PointEquals(Point a, Point b)
        {
            return Math.Abs(a.X - b.X) < 1e-10 && Math.Abs(a.Y - b.Y) < 1e-10;
        }
    }

    class Algo
    {
        public List<Triangle> Triangles = new List<Triangle>();
        public List<VoronoiCell> VoronoiCells = new List<VoronoiCell>();

        // 主方法，生成Voronoi图
        public void go(List<Point> points)
        {
            if (points.Count < 3)
            {
                Console.WriteLine("需要至少3个点");
                return;
            }

            // 1. 生成Delaunay三角剖分
            Triangles = GenerateDelaunayTriangulation(points);

            // 2. 生成Voronoi图
            VoronoiCells = GenerateVoronoiDiagram(points);
        }

        // Bowyer-Watson算法生成Delaunay三角剖分
        private List<Triangle> GenerateDelaunayTriangulation(List<Point> points)
        {
            // 创建超级三角形包含所有点
            var bounds = GetBounds(points);
            var superTriangle = CreateSuperTriangle(bounds);
            
            var triangles = new List<Triangle> { superTriangle };

            foreach (var point in points)
            {
                var badTriangles = new List<Triangle>();
                
                // 找到包含当前点的外接圆的三角形
                foreach (var triangle in triangles)
                {
                    if (triangle.IsPointInCircumcircle(point))
                    {
                        badTriangles.Add(triangle);
                    }
                }

                // 找到多边形边界
                var polygon = new List<Edge>();
                foreach (var triangle in badTriangles)
                {
                    var edges = new List<Edge>
                    {
                        new Edge(triangle.A, triangle.B),
                        new Edge(triangle.B, triangle.C),
                        new Edge(triangle.C, triangle.A)
                    };

                    foreach (var edge in edges)
                    {
                        bool shared = false;
                        foreach (var otherTriangle in badTriangles)
                        {
                            if (otherTriangle == triangle) continue;
                            
                            var otherEdges = new List<Edge>
                            {
                                new Edge(otherTriangle.A, otherTriangle.B),
                                new Edge(otherTriangle.B, otherTriangle.C),
                                new Edge(otherTriangle.C, otherTriangle.A)
                            };

                            if (otherEdges.Any(e => e.Equals(edge)))
                            {
                                shared = true;
                                break;
                            }
                        }

                        if (!shared)
                        {
                            polygon.Add(edge);
                        }
                    }
                }

                // 移除坏三角形
                foreach (var triangle in badTriangles)
                {
                    triangles.Remove(triangle);
                }

                // 创建新三角形
                foreach (var edge in polygon)
                {
                    var newTriangle = new Triangle(point, edge.A, edge.B);
                    triangles.Add(newTriangle);
                }
            }

            // 移除包含超级三角形顶点的三角形
            var finalTriangles = new List<Triangle>();
            foreach (var triangle in triangles)
            {
                if (!triangle.HasVertex(superTriangle.A) && 
                    !triangle.HasVertex(superTriangle.B) && 
                    !triangle.HasVertex(superTriangle.C))
                {
                    finalTriangles.Add(triangle);
                }
            }

            return finalTriangles;
        }

        // 生成Voronoi图
        private List<VoronoiCell> GenerateVoronoiDiagram(List<Point> points)
        {
            var cells = new List<VoronoiCell>();

            foreach (var point in points)
            {
                var cell = new VoronoiCell(point);
                
                // 找到包含当前点的所有三角形
                var adjacentTriangles = new List<Triangle>();
                foreach (var triangle in Triangles)
                {
                    if (triangle.HasVertex(point))
                    {
                        adjacentTriangles.Add(triangle);
                    }
                }

                // 获取外接圆圆心作为Voronoi顶点
                foreach (var triangle in adjacentTriangles)
                {
                    var circumcenter = triangle.GetCircumcenter();
                    if (circumcenter != null)
                    {
                        cell.Vertices.Add(circumcenter);
                    }
                }

                // 按角度排序顶点
                if (cell.Vertices.Count > 2)
                {
                    SortVerticesByAngle(cell.Vertices, point);
                }

                cells.Add(cell);
            }

            return cells;
        }

        // 按角度排序顶点
        private void SortVerticesByAngle(List<VoronoiVertex> vertices, Point center)
        {
            vertices.Sort((a, b) =>
            {
                double angleA = Math.Atan2(a.Y - center.Y, a.X - center.X);
                double angleB = Math.Atan2(b.Y - center.Y, b.X - center.X);
                return angleA.CompareTo(angleB);
            });
        }

        // 获取点集边界
        private Rectangle GetBounds(List<Point> points)
        {
            double minX = points.Min(p => p.X);
            double maxX = points.Max(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxY = points.Max(p => p.Y);

            double width = maxX - minX;
            double height = maxY - minY;
            double margin = Math.Max(width, height) * 0.5;

            return new Rectangle(
                (int)(minX - margin), 
                (int)(minY - margin),
                (int)(width + 2 * margin), 
                (int)(height + 2 * margin)
            );
        }

        // 创建超级三角形
        private Triangle CreateSuperTriangle(Rectangle bounds)
        {
            double dx = bounds.Width;
            double dy = bounds.Height;
            double deltaMax = Math.Max(dx, dy) * 2;

            Point p1 = new Point(bounds.X - deltaMax, bounds.Y - deltaMax);
            Point p2 = new Point(bounds.X + deltaMax * 2, bounds.Y - deltaMax);
            Point p3 = new Point(bounds.X, bounds.Y + deltaMax * 2);

            return new Triangle(p1, p2, p3);
        }

        // 绘制Voronoi图
        public void DrawVoronoiDiagram(Graphics g)
        {
            // 设置绘图参数
            Pen voronoiEdgePen = new Pen(Color.Blue, 2);
            Pen delaunayEdgePen = new Pen(Color.LightGray, 1);
            Brush siteBrush = new SolidBrush(Color.Red);
            Brush vertexBrush = new SolidBrush(Color.Green);

            // 清空背景
            g.Clear(Color.White);

            // 绘制Delaunay三角网（可选）
            foreach (var triangle in Triangles)
            {
                var pointA = new System.Drawing.Point((int)triangle.A.X, (int)triangle.A.Y);
                var pointB = new System.Drawing.Point((int)triangle.B.X, (int)triangle.B.Y);
                var pointC = new System.Drawing.Point((int)triangle.C.X, (int)triangle.C.Y);

                g.DrawLine(delaunayEdgePen, pointA, pointB);
                g.DrawLine(delaunayEdgePen, pointB, pointC);
                g.DrawLine(delaunayEdgePen, pointC, pointA);
            }

            // 绘制Voronoi单元
            foreach (var cell in VoronoiCells)
            {
                if (cell.Vertices.Count < 3) continue;

                // 绘制Voronoi多边形
                var points = cell.Vertices.Select(v => 
                    new System.Drawing.Point((int)v.X, (int)v.Y)).ToArray();

                if (points.Length > 2)
                {
                    g.DrawPolygon(voronoiEdgePen, points);
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

            // 绘制原始点
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