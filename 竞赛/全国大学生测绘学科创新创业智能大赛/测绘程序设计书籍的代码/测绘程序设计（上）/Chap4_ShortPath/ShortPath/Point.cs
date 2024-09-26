using System;

namespace ShortPath
{
    /// <summary>
    /// 点的集合,包含点的名字和权重
    /// </summary>
    class Point
    {
        public string name { get; set; }
        public double weight { get; set; }
        public Point(string Name)
        {
            name = Name;
            weight = 100000;//距离无穷远
        }
        public override bool Equals(object obj)//去重操作的重点
        {
            if (obj.GetType() != typeof(Point))
                return true;

            Point v = obj as Point;
            return v != null && this.name == v.name;
        }
    }
}
