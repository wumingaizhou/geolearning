using System;

namespace IDW
{
    /// <summary>
    /// 点的数据结构
    /// </summary>
    class Point
    {
        public string ID;
        public double X;
        public double Y;
        public double H;
        public double dist;
        public void Parse(string line)//读取每一行的坐标
        {
            var buf = line.Split(',');
            ID = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
            H = Convert.ToDouble(buf[3]);
        }
        public Point()//构造函数，先初始化数值
        {
            dist = X = Y = H = 0;
        }
        public Point(string id,double x,double y)//构造函数，用于后面传入待插值点时进行初始化
        {
            ID = id;
            X = x;
            Y = y;
        }
        public override string ToString()
        {

            return $"{ID}   {X:F3}  {Y:F3}  {H:F3}";
        }
    }
}