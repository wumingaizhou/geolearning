using System;

namespace DP
{
    /// <summary>
    /// 点的数据结构
    /// </summary>
    class Point
    {
        public int ID;
        public double X;
        public double Y;

        public void Parse(string line)
        {
            var buf = line.Split(',');
            ID = Convert.ToInt16(buf[0]);
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
        }
    }

}