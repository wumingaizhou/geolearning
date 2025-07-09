using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoronoiDiagram
{
    public class Delaunay
    {
        // 三角网生成
        List<Triangle> triangles = new List<Triangle>();
        private Point superA, superB, superC; // 保存超级三角形的三个顶点

        public Delaunay(List<Point> points)
        {
            Triangle superTriangle = CreateSuperTriangle(points);
            // 保存超级三角形的顶点
            superA = superTriangle.A;
            superB = superTriangle.B;
            superC = superTriangle.C;
            triangles.Add(superTriangle);

            // 逐点插入
            foreach (var newPoint in points)
            {
                InsertPoint(newPoint, triangles);
            }

            // 最后删除包含超级三角形顶点的所有三角形
            RemoveSuperTriangle();
        }

        // 获取最终的三角网
        public List<Triangle> GetTriangles()
        {
            return triangles;
        }

        // 生成巨大的三角形
        private Triangle CreateSuperTriangle(List<Point> points)
        {
            // 找到边界
            double minX = points.Min(p => p.X);
            double minY = points.Min(p => p.Y);
            double maxX = points.Max(p => p.X);
            double maxY = points.Max(p => p.Y);

            // 创建一个比边界大的三角形
            double dx = maxX - minX;
            double dy = maxY - minY;
            double deltaMax = Math.Max(dx, dy) * 2;

            Point p1 = new Point(minX - deltaMax, minY - deltaMax);
            Point p2 = new Point(maxX + deltaMax, minY - deltaMax);
            Point p3 = new Point((minX + maxX) / 2, maxY + deltaMax);

            return new Triangle(p1, p2, p3);
        }

        //逐点插入
        private void InsertPoint(Point newPoint, List<Triangle> triangles)
        {
            // 1.找到所有"坏"三角形
            var badTriangles = FindBadTriangles(newPoint, triangles);

            // 2.找到空洞边界
            var boundaryEdges = FindPolygonBoundary(badTriangles);

            // 3. 删除坏三角形
            foreach (var badTriangle in badTriangles)
            {
                triangles.Remove(badTriangle);
            }

            // 4. 用新点连接边界，创建新三角形
            ConnectNewPoint(newPoint, boundaryEdges, triangles);
        }

        // 查找坏三角形，当插入新点时，triangles 里的某些三角形的外接圆里包含了新插入的点，我们需要把这些三角形删除
        private List<Triangle> FindBadTriangles(Point newPoint, List<Triangle> triangles)
        {
            var badTriangles = new List<Triangle>();

            foreach (var triangle in triangles)
            {
                if (IsPointInCircumcircle(newPoint, triangle))
                {
                    badTriangles.Add(triangle);
                }
            }

            return badTriangles;
        }
        // 检查点是否在三角形的外接圆内
        private bool IsPointInCircumcircle(Point point, Triangle triangle)
        {
            // 计算外接圆圆心
            Point circumcenter = triangle.GetCircumenter();

            // 计算外接圆半径
            double radius = Distance(triangle.A, circumcenter);

            //计算新点到圆心的距离
            double distanceToNewPoint = Distance(circumcenter, point);

            //如果距离小于半径，点在圆内
            return distanceToNewPoint < radius;
        }
        private double Distance(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }

        // 查找空洞
        private List<Edge> FindPolygonBoundary(List<Triangle> badtriangles)
        {
            var allEdges = new List<Edge>();

            // 收集所有坏三角形的边
            foreach (var triangle in badtriangles)
            {
                allEdges.Add(new Edge(triangle.A, triangle.B));
                allEdges.Add(new Edge(triangle.B, triangle.C));
                allEdges.Add(new Edge(triangle.C, triangle.A));
            }

            // 边界边的特点：只出现一次
            // 内部边会出现两次
            var boundaryEdges = new List<Edge>();

            foreach (var edge in allEdges)
            {
                int count = 0;
                foreach (var otherEdge in allEdges)
                {
                    if (edge.EdgeEquals(otherEdge))
                    {
                        count++;
                    }
                }

                if (count == 1) //只出现一次的就是边界
                {
                    boundaryEdges.Add(edge);
                }
            }
            return boundaryEdges;
        }

        // 把新点和空洞边界的每条边连起来，形成新的三角形
        private void ConnectNewPoint(Point newPoint, List<Edge> boundaryEdges, List<Triangle> triangles)
        {
            foreach (var edge in boundaryEdges)
            {
                // 新点 + 边界边 = 新三角形
                var newTriangle = new Triangle(newPoint, edge.P1, edge.P2);
                triangles.Add(newTriangle);
            }
        }

        // 删除包含超级三角形顶点的所有三角形
        private void RemoveSuperTriangle()
        {
            // 创建需要删除的三角形列表
            var trianglesToRemove = new List<Triangle>();

            foreach (var triangle in triangles)
            {
                // 检查三角形是否包含超级三角形的任何顶点
                if (ContainsSuperTriangleVertex(triangle))
                {
                    trianglesToRemove.Add(triangle);
                }
            }

            // 删除这些三角形
            foreach (var triangle in trianglesToRemove)
            {
                triangles.Remove(triangle);
            }
        }

        // 检查三角形是否包含超级三角形的顶点
        private bool ContainsSuperTriangleVertex(Triangle triangle)
        {
            // 检查三角形的每个顶点是否与超级三角形的顶点重合
            return IsPointEqual(triangle.A, superA) ||
                   IsPointEqual(triangle.A, superB) ||
                   IsPointEqual(triangle.A, superC) ||
                   IsPointEqual(triangle.B, superA) ||
                   IsPointEqual(triangle.B, superB) ||
                   IsPointEqual(triangle.B, superC) ||
                   IsPointEqual(triangle.C, superA) ||
                   IsPointEqual(triangle.C, superB) ||
                   IsPointEqual(triangle.C, superC);
        }

        // 判断两个点是否相等（考虑浮点数精度）
        private bool IsPointEqual(Point p1, Point p2)
        {
            const double tolerance = 1e-10;
            return Math.Abs(p1.X - p2.X) < tolerance && Math.Abs(p1.Y - p2.Y) < tolerance;
        }
    }
}