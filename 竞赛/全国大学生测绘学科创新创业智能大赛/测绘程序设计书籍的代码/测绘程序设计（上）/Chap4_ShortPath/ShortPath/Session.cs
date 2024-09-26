using System;

namespace ShortPath
{
    /// <summary>
    /// 边数据结构，包含起始点，终点，边长
    /// </summary>
    class Session
    {
        public string start;
        public string end;
        public double length;
        public void Parse(string line)
        {
            var buf = line.Split(',');
            start = buf[0];
            end = buf[1];
            length = Convert.ToDouble(buf[2]);
        }
        public override string ToString()
        {
            return $"{start}     {end}     {length:F3}\n";
        }
    }
}