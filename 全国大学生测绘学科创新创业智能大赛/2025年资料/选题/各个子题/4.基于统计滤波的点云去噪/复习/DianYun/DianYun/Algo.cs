using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DianYun
{
    class Algo
    {
        public List<Point> points = new List<Point>();
        public Dictionary<Tuple<int, int, int>, List<Point>> grid = new Dictionary<Tuple<int, int, int>, List<Point>>();
        public double minx;
        public double miny;
        public double minz;
        public double maxx;
        public double maxy;
        public double maxz;

        public Algo(List<Point> Points)
        {
            points = Points;
            grid.Clear();
            CalBoundBox(); //计算点云边界
            CalDiavg(); //计算每个点的平均邻近距离
            RemovePoint(); //标记噪声点
        }
        public void CalBoundBox()
        {
            minx = points.OrderBy(p => p.X).FirstOrDefault().X;
            miny = points.OrderBy(p => p.Y).FirstOrDefault().Y;
            minz = points.OrderBy(p => p.Z).FirstOrDefault().Z;
            maxx = points.OrderByDescending(p => p.X).FirstOrDefault().X;
            maxy = points.OrderByDescending(p => p.Y).FirstOrDefault().Y;
            maxz = points.OrderByDescending(p => p.Z).FirstOrDefault().Z;

            int num_cells_x = (int)Math.Ceiling((maxx - minx) / 2.0);
            int num_cells_y = (int)Math.Ceiling((maxy - miny) / 2.0);
            int num_cells_z = (int)Math.Ceiling((maxz - minz) / 2.0);

            foreach(var p in points)
            {
                int ix = (int)Math.Floor((p.X - minx) / 2.0);
                int iy = (int)Math.Floor((p.Y - miny) / 2.0);
                int iz = (int)Math.Floor((p.Z - minz) / 2.0);

                var key = Tuple.Create(ix, iy, iz);

                if (!grid.ContainsKey(key))
                {
                    grid[key] = new List<Point>();
                }
                grid[key].Add(p);
            }
        }

        public void CalDiavg()
        {
            foreach (var p in points)
            {
                int ix = (int)Math.Floor((p.X - minx) / 2.0);
                int iy = (int)Math.Floor((p.Y - miny) / 2.0);
                int iz = (int)Math.Floor((p.Z - minz) / 2.0);

                List<Point> temppoints = new List<Point>();
                List<Point> goodPoints = new List<Point>(); //最终从temppoints里筛选出来的候选点
                for(int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        for (int k = -1; k <= 1; k++)
                        {
                            var key = Tuple.Create(ix + i, iy + j, iz + k);
                            if (grid.ContainsKey(key))
                            {
                                temppoints.AddRange(grid[key]);
                            }
                        }
                    }
                }
                //求与p的距离
                foreach(var tempp in temppoints)
                {
                    tempp.GetDistance(p);
                }
                //去除自己
                temppoints = temppoints.Where(tempp => !(tempp.di < 1e-5)).ToList();

                // 即使没有10个，take也会自动返回所有元素
                goodPoints = temppoints.OrderBy(temp => temp.di).Take(10).ToList();

                if(goodPoints.Count == 0)
                {
                    //极端情况下，这个点及其稀疏
                    p.di_avg = 999999;
                    continue;
                }

                p.di_avg = goodPoints.Average(goodp => goodp.di);
            }
        }
        public void RemovePoint()
        {
            double d_mean = points.Average(p => p.di_avg);
            double d_std_dev = points.Sum(p => (p.di_avg - d_mean) * (p.di_avg - d_mean));
            d_std_dev = Math.Sqrt(d_std_dev / points.Count);

            foreach(var p in points)
            {
                if(p.di_avg > d_mean + 2 * d_std_dev)
                {
                    p.IS_bad = true;
                }
            }
        }

        public string GetResult()
        {
            string result = "噪声点结果：\n";

            foreach(var p in points)
            {
                if (p.IS_bad)
                {
                    result += $"点X为{p.X}，Y为{p.Y}的点是噪声点\n";
                }
            }
            
            return result;
        }
    }
}
