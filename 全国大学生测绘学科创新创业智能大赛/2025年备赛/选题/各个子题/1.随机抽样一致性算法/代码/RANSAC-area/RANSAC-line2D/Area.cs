using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_area
{
    class Area
    {
        //面的数据结构
        public Point Point0 = new Point();
        public double areaA;
        public double areaB;
        public double areaC;
        public double areaD;

        public int CountPoint = 0; // 内点的数量

        public Area(Point point1, Point point2,Point point3)
        {
            // 首先判断三点是否共面
            // 定义浮点数比较的容差值。这是一个很小的正数，用于处理浮点数计算误差。
            const double epsilon = 1e-9; // 例如 0.000000001

            // --- 1. 处理点重合的情况：如果任意两点重合，则三点共线 ---
            bool p1EqualsP2 = Math.Abs(point1.X - point2.X) < epsilon &&
                              Math.Abs(point1.Y - point2.Y) < epsilon &&
                              Math.Abs(point1.Z - point2.Z) < epsilon;

            bool p2EqualsP3 = Math.Abs(point2.X - point3.X) < epsilon &&
                              Math.Abs(point2.Y - point3.Y) < epsilon &&
                              Math.Abs(point2.Z - point3.Z) < epsilon;

            bool p1EqualsP3 = Math.Abs(point1.X - point3.X) < epsilon &&
                              Math.Abs(point1.Y - point3.Y) < epsilon &&
                              Math.Abs(point1.Z - point3.Z) < epsilon;

            if (p1EqualsP2 || p2EqualsP3 || p1EqualsP3)
            {
                Console.WriteLine("警告：传入的三个点中有重合点，它们共线，无法构成一个非零面积的平面区域。");
                return; // 结束构造函数执行
            }

            // --- 2. 计算两个向量 ---
            // 向量 V1：从 point1 到 point2
            double v1X = point2.X - point1.X;
            double v1Y = point2.Y - point1.Y;
            double v1Z = point2.Z - point1.Z;

            // 向量 V2：从 point1 到 point3
            double v2X = point3.X - point1.X;
            double v2Y = point3.Y - point1.Y;
            double v2Z = point3.Z - point1.Z;

            // --- 3. 计算这两个向量的叉积（Cross Product）---
            // 叉积结果向量的 X 分量
            double crossX = v1Y * v2Z - v1Z * v2Y;
            // 叉积结果向量的 Y 分量
            double crossY = v1Z * v2X - v1X * v2Z;
            // 叉积结果向量的 Z 分量
            double crossZ = v1X * v2Y - v1Y * v2X;

            // --- 4. 判断叉积向量的模长平方是否接近零 ---
            // 如果叉积的模长（Magnitude）接近零，则表示两个向量是平行的，从而三点共线。
            // 计算模长的平方，避免耗时的 Math.Sqrt，效率更高。
            double crossProductMagnitudeSquared = crossX * crossX + crossY * crossY + crossZ * crossZ;

            if (crossProductMagnitudeSquared < epsilon * epsilon) // 注意这里用 epsilon * epsilon
            {
                Console.WriteLine("警告：传入的三个点在 3D 空间中共线，无法构成一个非零面积的平面区域。");
                return;
            }

            // 不共面，上面的一些变量需要用到
            double under = Math.Sqrt(crossX * crossX + crossY * crossY + crossZ * crossZ); // 模
            areaA = crossX / under;
            areaB = crossY / under;
            areaC = crossZ / under;
            //计算D
            areaD = -(areaA * point1.X + areaB * point1.Y + areaC * point1.Z);
        }

        public void Calculate(List<Point> points, double deta, int times)
        {
            // deta：阈值，times：迭代次数
            foreach (var p in points)
            {
                double d = Math.Abs(areaA * p.X + areaB * p.Y + areaC * p.Z + areaD);
                if (d <= deta)
                {
                    CountPoint++;
                }
            }
        }
    }
}
