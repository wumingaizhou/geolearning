using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_area
{
    class Algo
    {
        public int min_samples;
        public double threshold;
        public int max_iterations;
        private Random random = new Random(); // 推荐：在类级别创建一次 Random 实例

        public Algo(int samples, double Threshold, int iterations)
        {
            // 最小样本数，距离阈值，最大迭代次数
            min_samples = samples;
            threshold = Threshold;
            max_iterations = iterations;
        }

        public Area go(List<Point> points)
        {
            var flag = 0; //迭代次数
            List<Area> areas = new List<Area>(); //直线列表
            while (true)
            {
                flag++;

                // 检查剩余点是否足够抽取三个
                if (points.Count < 3)
                {
                    break;
                }

                // Randomly generate the index for the first point
                int index1 = random.Next(points.Count);

                // Randomly generate the index for the second point, ensuring it's different from the first
                int index2;
                do
                {
                    index2 = random.Next(points.Count);
                } while (index2 == index1);

                // Randomly generate the index for the third point, ensuring it's different from both the first and second
                int index3;
                do
                {
                    index3 = random.Next(points.Count);
                } while (index3 == index1 || index3 == index2); // Key change here

                // Get the selected three points
                Point point1 = points[index1];
                Point point2 = points[index2];
                Point point3 = points[index3];

                Area area = new Area(point1, point2, point3); //生成线
                area.Calculate(points, threshold, max_iterations);
                areas.Add(area);

                if (area.CountPoint == points.Count || flag >= max_iterations)
                {
                    break;
                }
            }
            // OrderByDescending 按 PointCount 从大到小排序
            // FirstOrDefault 取排序后的第一个元素（即 PointCount 最大的）
            Area areaWithMaxPoints = areas.OrderByDescending(area => area.CountPoint)
                                         .FirstOrDefault();
            return areaWithMaxPoints;
        }
    }
}
