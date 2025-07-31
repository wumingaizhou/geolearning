using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_line2D
{
    class Algo
    {
        public int min_samples;
        public double threshold;
        public int max_iterations;
        private Random random = new Random(); // 推荐：在类级别创建一次 Random 实例

        public Algo(int samples,double Threshold,int iterations)
        {
            // 最小样本数，距离阈值，最大迭代次数
            min_samples = samples;
            threshold = Threshold;
            max_iterations = iterations;
        }

        public Line go(List<Point> points)
        {
            var flag = 0; //迭代次数
            List<Line> lines = new List<Line>(); //直线列表
            while (true)
            {
                flag++;

                // 检查剩余点是否足够抽取两个
                if (points.Count < 2)
                {
                    Console.WriteLine("Not enough unique points left to draw two. Resetting or stopping.");
                    break;
                }

                // 随机生成第一个点的索引
                int index1 = random.Next(points.Count);

                // 随机生成第二个点的索引，确保与第一个不同
                int index2;
                do
                {
                    index2 = random.Next(points.Count);
                } while (index2 == index1); // 确保在当前循环中，两个点是不同的

                // 获取选取的两个点
                Point point1 = points[index1];
                Point point2 = points[index2];

                Line line = new Line(point1, point2); //生成线
                line.Calculate(points, threshold, max_iterations);
                lines.Add(line);

                if(line.CountPoint == points.Count || flag >= max_iterations)
                {
                    break;
                }
            }
            // OrderByDescending 按 PointCount 从大到小排序
            // FirstOrDefault 取排序后的第一个元素（即 PointCount 最大的）
            Line lineWithMaxPoints = lines.OrderByDescending(line => line.CountPoint)
                                         .FirstOrDefault();
            return lineWithMaxPoints;
        }
    }
}
