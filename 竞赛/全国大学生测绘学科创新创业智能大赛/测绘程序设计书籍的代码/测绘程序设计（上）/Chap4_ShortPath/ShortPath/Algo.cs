using System;

namespace ShortPath
{
    class Algo
    {
        public SessionList data;
        public Algo(SessionList sessionlist)
        {
            data = sessionlist;
        }
        public void Run()
        {
            data.pts[0].weight = 0;//起始点权重为0，这里是武大
            for(int i = 0;i < data.pts.Count - 1;i++)
            {
                for(int j = i + 1;j < data.pts.Count;j++)
                {
                    foreach(Session d in data.dataList)
                    {
                        if(d.start == data.pts[i].name && d.end == data.pts[j].name)//如果有边
                        {
                            double weight = data.pts[i].weight + d.length;
                            if(weight < data.pts[j].weight)
                            {
                                data.pts[j].weight = weight;
                            }
                        }
                    }
                }
            }
        }
        public override string ToString()
        {
            string res = "";
            for(int i = 0;i < data.pts.Count;i++)
            {
                res += $"{data.pts[0].name}  {data.pts[i].name}  {data.pts[i].weight}\n";
            }
            return res;
        }
    }
}