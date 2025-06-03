using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LIDAR
{
    class Algo
    {
        public List<Point> AlgoPoints = new List<Point>();
        public double CellSize;
        public int N_neighbors;
        public double K_std;


        public double min_x;
        public double max_x;
        public double min_y;
        public double max_y;
        public double min_z;
        public double max_z;

        public Dictionary<Tuple<int, int, int>, List<Point>> grid; //格网结构


        public Algo(List<Point> points,double cellSize,int n_neighbors,double k_std)
        {
            // 初始化参数，包括格网尺寸，临近点数量，标准差倍数
            AlgoPoints = points;
            CellSize = cellSize;
            N_neighbors = n_neighbors;
            K_std = k_std;
        }

        public List<Point> Go()
        {
            Setp1_CalCell();
            Setp2_CalNeighbors();
            var result = Setp3_RemovePoint();
            return result;
        }

        public void Setp1_CalCell()
        {
            //步骤一：初始化格网，分配点
            //1.1 确定点云边界
            min_x = AlgoPoints.OrderBy(p => p.X).First().X;
            max_x = AlgoPoints.OrderByDescending(p => p.X).First().X;
            min_y = AlgoPoints.OrderBy(p => p.Y).First().Y;
            max_y = AlgoPoints.OrderByDescending(p => p.Y).First().Y;
            min_z = AlgoPoints.OrderBy(p => p.Z).First().Z;
            max_z = AlgoPoints.OrderByDescending(p => p.Z).First().Z;
            //1.2 确定格网维度
            var num_cell_x = Math.Ceiling((max_x - min_x) / CellSize);
            var num_cell_y = Math.Ceiling((max_y - min_y) / CellSize);
            var num_cell_z = Math.Ceiling((max_z - min_z) / CellSize);
            //1.3 创建格网结构
            grid = new Dictionary<Tuple<int, int, int>, List<Point>>();
            //1.4 分配点
            foreach(var p in AlgoPoints)
            {
                var ix = (int)Math.Floor((p.X - min_x) / CellSize);
                var iy = (int)Math.Floor((p.Y - min_y) / CellSize);
                var iz = (int)Math.Floor((p.Z - min_z) / CellSize);

                var key = Tuple.Create(ix, iy, iz);
                if(!grid.ContainsKey(key))
                {
                    //如果没有，需要新建
                    grid[key] = new List<Point>();
                }
                grid[key].Add(new Point(p.X,p.Y,p.Z)); // 最好是新new一个point，避免污染原数据
            }
        }

        public void Setp2_CalNeighbors()
        {
            //步骤二：计算每个点的平均邻近距离
            foreach(var p in AlgoPoints)
            {
                var ix = (int)Math.Floor((p.X - min_x) / CellSize);
                var iy = (int)Math.Floor((p.Y - min_y) / CellSize);
                var iz = (int)Math.Floor((p.Z - min_z) / CellSize);

                var Point_candidate = new List<Point>(); // 候选点，3*3*3=27个格网。
                for(int i = ix - 1;i <= ix + 1; i++)
                {
                    for(int j = iy - 1;j <= iy + 1; j++)
                    {
                        for (int k = iz - 1; k <= iz + 1; k++)
                        {
                            var key = Tuple.Create(i, j, k);
                            if (!grid.ContainsKey(key))
                            {
                                //在格网最外围的区域，是没有27个格子的，如果没有，则跳过
                                continue;
                            }
                            var tuplePoint = grid[key];

                            foreach(var point in tuplePoint)
                            {
                                var deta = 1e-5; //用于判断点是不是其本身
                                if (p.X - point.X < deta && p.Y - point.Y < deta && p.Z - point.Z < deta)
                                {
                                    // 如果是其本身
                                    continue;
                                }
                                Point_candidate.Add(new Point(point.X, point.Y, point.Z));
                            }
                        }
                    }
                }

                //接下来要筛选出最近的N_neighbors个数的点
                foreach(var candidatePoint in Point_candidate)
                {
                    candidatePoint.length = CalLength(p, candidatePoint);
                }
                var Point_neighobrs = Point_candidate.OrderBy(point => point.length).Take(N_neighbors).ToList();

                //接下来就是计算pi的平均邻近距离di
                var sum = Point_neighobrs.Sum(point => point.length);
                p.di = sum / Point_neighobrs.Count;
            }
        }

        public List<Point> Setp3_RemovePoint()
        {
            //步骤三：计算统计量；去除噪声点
            //首先计算统计量
            double d_mean = AlgoPoints
                .Where(p => p.di > 0)
                .Average(p => p.di);
            // 计算标准差
            double d_std_dev = Math.Sqrt(
                AlgoPoints
                    .Where(p => p.di > 0)
                    .Select(p => Math.Pow(p.di - d_mean, 2))  // 计算每个元素与平均值的差的平方
                    .Average()                                // 计算平方差的平均值（即方差）
            );

            //然后去除噪声点
            var resultPoints = AlgoPoints.Where(p => p.di <= d_mean + K_std * d_std_dev).ToList();
            return resultPoints;
        }
        public double CalLength(Point point1,Point point2)
        {
            //计算两点的距离
            double a = (point1.X - point2.X) * (point1.X - point2.X);
            double b = (point1.Y - point2.Y) * (point1.Y - point2.Y);
            double c = (point1.Z - point2.Z) * (point1.Z - point2.Z);

            return Math.Sqrt(a * a + b * b + c * c);
        }
    }
}
