using System;
using System.Collections.Generic;

namespace DP
{
    /// <summary>
    /// 为什么要特地创建个类来储存points呢？主要是为了和第一次与第二次的代码风格一致。
    /// 本来我们用了dataGridView就没必要创建这个类了，当是既然创建了这个类，就必须要一个东西：索引器
    /// 没有索引器我们不能取得pointEntity里面点的数量，不能通过索引获取里面的某个元素，就不能遍历了
    /// </summary>
    class pointEntity
    {
        public int Count => Data.Count;//点的数量
        public List<Point> Data;
        public pointEntity()//构造函数
        {
            Data = new List<Point>();
        }
        //构建索引器
        public Point this[int i]
        {
            get { return Data[i]; }
            set { Data[i] = value; }
        }

        public void Add(Point pt)
        {
            Data.Add(pt);
        }
    }
}