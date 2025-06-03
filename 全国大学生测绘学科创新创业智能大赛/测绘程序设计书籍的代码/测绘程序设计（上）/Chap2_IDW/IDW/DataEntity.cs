using System;
using System.Collections.Generic;

namespace IDW
{
    /// <summary>
    /// 所有点的集合
    /// </summary>
    class DataEntity
    {
        public List<Point> Data;//和这个构造函数搭配的，不能在构造函数里声明，作用域问题，不然Data一出构造函数就销毁
        public int Count => Data.Count;
        public DataEntity()//构造函数，创建泛型
        {
            Data = new List<Point>();
        }

        public void Add(Point pt)
        {
            Data.Add(pt);
        }

        public override string ToString()
        {
            string res = "测站：  X（m）   Y（m）   H（m）\n";
            foreach (var d in Data)
            {
                res += d.ToString() + "\n";
            }
            return res;
        }
        public Point this[int i]//索引器，用于遍历
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }
    }
}