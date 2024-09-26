using System;
using System.Collections.Generic;
namespace ShortPath
{
    /// <summary>
    /// 包含了点的集合，边的集合的集合
    /// </summary>
    class SessionList
    {
        public List<Point> pts;
        public List<Session> dataList;
        public SessionList(List<Session> sessions)
        {
            dataList = sessions;
            GetPoints(sessions);
        }
        private void GetPoints(List<Session> sessions)//通过遍历session里面的点信息，去重，得到不重复的所有点
        {
            pts = new List<Point>();
            foreach(var d in sessions)
            {
                Point pt = new Point(d.start);
                if(!pts.Contains(pt))//Contains方法比较值类型的时候是直接比较值，而比较对象的时候是比较内存地址，即使两个pt的name一样，但是内存地址不一样，会返回false，所以我们要重写equals方法
                {
                    pts.Add(pt);
                }
                pt = new Point(d.end);
                if(!pts.Contains(pt))
                {
                    pts.Add(pt);
                }

            }
        }
        public override string ToString()
        {
            string res = "起始点    终点     距离\n";
            foreach(Session d in dataList)
            {
                res += d.ToString();
            }
            return res;
        }
    }
}