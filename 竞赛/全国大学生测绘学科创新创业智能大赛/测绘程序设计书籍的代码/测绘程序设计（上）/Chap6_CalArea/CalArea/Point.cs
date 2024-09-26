using System;

namespace CalArea
{
    /// <summary>
    /// 顶点的数据结构
    /// </summary>
    class Point
    {
        public string PointName;
        public double X;
        public double Y;
        public void Parse(string line)
        {
            var buf = line.Split(',');
            PointName = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
        }
    }
}