using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RANSAC_line
{
    class Algo
    {
        public List<Point> points_2D = new List<Point>();
        public List<Point> points_3D = new List<Point>();
        public List<Line2D> line2Ds = new List<Line2D>();
        public List<Line3D> line3Ds = new List<Line3D>();

        public Algo(List<Point> origin_points)
        {
            points_2D = origin_points.Select(p => p.Clone()).ToList();
            points_3D = origin_points.Select(p => p.Clone()).ToList();

            //先得到线的集合。
            Go();
        }

        private void Go()
        {
            if (UserInput.min_samples == 2)
            {
                int times = UserInput.max_times;

                // 用于存储已选择的点对，确保不重复
                var selectedPairs = new HashSet<Tuple<int, int>>();
                var random = new Random();

                // 计算最大可能的点对数量
                int maxPossiblePairs = points_2D.Count * (points_2D.Count - 1) / 2;

                // 如果请求的次数超过可能的组合数，则限制为最大可能数
                times = Math.Min(times, maxPossiblePairs);

                int successfulSelections = 0;
                int attempts = 0;
                int maxAttempts = times * 10; // 防止无限循环

                while (successfulSelections < times && attempts < maxAttempts)
                {
                    attempts++;

                    // 随机选择两个不同的点
                    int point1Index = random.Next(points_2D.Count);
                    int point2Index;
                    do
                    {
                        point2Index = random.Next(points_2D.Count);
                    } while (point2Index == point1Index);

                    // 创建标准化的点对（较小的索引在前，确保(a,b)和(b,a)被视为相同）
                    Tuple<int, int> normalizedPair;

                    if (point1Index < point2Index)
                    {
                        normalizedPair = Tuple.Create(point1Index, point2Index);
                    }
                    else
                    {
                        normalizedPair = Tuple.Create(point2Index, point1Index);
                    }
          
                    // 检查这个点对是否已经被选择过
                    if (selectedPairs.Add(normalizedPair))
                    {
                        // 成功添加新的点对
                        Point point1 = points_2D[point1Index];
                        Point point2 = points_2D[point2Index];
                        if(Math.Abs(point1.X - point2.X) < 1e-5 && Math.Abs(point1.Y - point2.Y) < 1e-5)
                        {
                            successfulSelections++;
                            continue;
                        }

                        // 处理选中的点对
                        var line_2D = new Line2D(point1, point2);
                        line2Ds.Add(line_2D);

                        successfulSelections++;
                    }
                }

                // 如果无法找到足够的唯一点对，可以添加警告或日志
                if (successfulSelections < times)
                {
                    Console.WriteLine($"警告: 只能找到 {successfulSelections} 个唯一点对，少于请求的 {times} 个");
                }
                // 一致性评估
                ProcessLine2D();
            }
            else if (UserInput.min_samples == 3)
            {
                int times = UserInput.max_times;

                // 用于存储已选择的点对，确保不重复
                var selectedPairs = new HashSet<Tuple<int, int, int>>();
                var random = new Random();

                // 计算最大可能的点对数量
                int maxPossiblePairs = points_3D.Count * (points_3D.Count - 1) / 2;

                // 如果请求的次数超过可能的组合数，则限制为最大可能数
                times = Math.Min(times, maxPossiblePairs);

                int successfulSelections = 0;
                int attempts = 0;
                int maxAttempts = times * 10; // 防止无限循环

                while (successfulSelections < times && attempts < maxAttempts)
                {
                    attempts++;

                    int point1Index = random.Next(points_3D.Count);
                    int point2Index;
                    int point3Index;
                    do
                    {
                        point2Index = random.Next(points_3D.Count);
                        point3Index = random.Next(points_3D.Count);
                    } while (point2Index == point1Index && point2Index == point3Index);

                    // 对三个索引进行排序，确保顺序固定
                    var sortedIndices = new[] { point1Index, point2Index, point3Index };
                    Array.Sort(sortedIndices);

                    // 创建标准化的三元组（从小到大排序）
                    var normalizedPair = Tuple.Create(sortedIndices[0], sortedIndices[1], sortedIndices[2]);

                    // 检查这个点对是否已经被选择过
                    if (selectedPairs.Add(normalizedPair))
                    {
                        // 成功添加新的点对
                        Point point1 = points_3D[point1Index];
                        Point point2 = points_3D[point2Index];
                        Point point3 = points_3D[point3Index];
                        if (Math.Abs(point1.X - point2.X) < 1e-5 && Math.Abs(point1.Y - point2.Y) < 1e-5 && Math.Abs(point1.Z - point2.Z) < 1e-5 && Math.Abs(point1.X - point3.X) < 1e-5 && Math.Abs(point1.Y - point3.Y) < 1e-5 && Math.Abs(point1.Z - point3.Z) < 1e-5)
                        {
                            successfulSelections++;
                            continue;
                        }

                        // 处理选中的点对
                        var line_3D = new Line3D(point1, point2, point3);
                        line3Ds.Add(line_3D);

                        successfulSelections++;
                    }
                }

                // 如果无法找到足够的唯一点对，可以添加警告或日志
                if (successfulSelections < times)
                {
                    Console.WriteLine($"警告: 只能找到 {successfulSelections} 个唯一点对，少于请求的 {times} 个");
                }
                // 一致性评估
                ProcessLine3D();
            }
        }

        private void ProcessLine2D()
        {
            foreach(var line in line2Ds)
            {
                foreach(var p in points_2D)
                {
                    double d = Math.Abs(line.A * p.X + line.B * p.Y + line.C);
                    d /= Math.Sqrt(line.A * line.A + line.B * line.B);
                    if (d < UserInput.threshold) line.Inner_count++;
                }
            }
        }

        private void ProcessLine3D()
        {
            foreach (var line in line3Ds)
            {
                foreach (var p in points_3D)
                {
                    double px = p.X - line.P0x;
                    double py = p.Y - line.P0y;
                    double pz = p.Z - line.P0z;

                    double x = px * line.ux;
                    double y = py * line.uy;
                    double z = pz * line.uz;

                    double d = Math.Sqrt(x * x + y * y + z * z);
                    if (d < UserInput.threshold) line.Inner_count++;
                }
            }
        }

        public string GetResult()
        {
            string result = "结果：\n";

            if(UserInput.min_samples == 2)
            {
                var maxline = line2Ds.OrderByDescending(line => line.Inner_count).FirstOrDefault();

                result += $"点 {maxline.point1.ID} 和 点 {maxline.point2.ID} 组成的线的内点数为：{maxline.Inner_count}\n";
            }
            else if (UserInput.min_samples == 3)
            {
                var maxline = line3Ds.OrderByDescending(line => line.Inner_count).FirstOrDefault();

                result += $"点 {maxline.point1.ID}, 点 {maxline.point2.ID} 和 点 {maxline.point3.ID} 组成的线的内点数为：{maxline.Inner_count}\n";
            }

            return result;
        }

    }




    //// 如果你想要更高效的解决方案（适用于大量点的情况），可以使用预生成所有可能组合的方法：
    //class AlgoEfficient
    //{
    //    public List<Point> points_2D = new List<Point>();

    //    public AlgoEfficient(List<Point> origin_points)
    //    {
    //        points_2D = origin_points.Select(p => p.Clone()).ToList();
    //        GoEfficient();
    //    }

    //    private void GoEfficient()
    //    {
    //        if (UserInput.min_samples == 2)
    //        {
    //            int times = UserInput.max_times;

    //            // 生成所有可能的点对组合
    //            var allPairs = new List<(int, int)>();
    //            for (int i = 0; i < points_2D.Count; i++)
    //            {
    //                for (int j = i + 1; j < points_2D.Count; j++)
    //                {
    //                    allPairs.Add((i, j));
    //                }
    //            }

    //            // 如果请求的次数超过可能的组合数，则限制为最大可能数
    //            times = Math.Min(times, allPairs.Count);

    //            // 随机打乱所有点对，然后取前times个
    //            var random = new Random();
    //            allPairs = allPairs.OrderBy(x => random.Next()).ToList();

    //            // 选择前times个点对
    //            for (int i = 0; i < times; i++)
    //            {
    //                var (point1Index, point2Index) = allPairs[i];
    //                Point point1 = points_2D[point1Index];
    //                Point point2 = points_2D[point2Index];

    //                // 在这里处理选中的点对
    //                ProcessPointPair(point1, point2);
    //            }
    //        }
    //    }

    //    private void ProcessPointPair(Point point1, Point point2)
    //    {
    //        // 处理点对的逻辑
    //        Console.WriteLine($"选中点对: ({point1.X}, {point1.Y}) - ({point2.X}, {point2.Y})");
    //    }
    //}
}
