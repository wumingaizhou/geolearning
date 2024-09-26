using System;
using System.Collections.Generic;

namespace TimeTransform
{
    class Algo
    {
        public List<Time> times;
        public Algo(List<Time> data)
        {
            times = data;
        }
        public string GetJD() // 获得儒略日
        {
            string res = "---儒略日---\n";
            for(int i = 0;i<times.Count;i++)
            {
                res += Convert.ToString(times[i].jd) + "\n";
            }
            return res;
        }

        public string GetAccday()//获得年积日
        {
            string res = "---年积日---\n";
            for(int i = 0;i<times.Count;i++)
            {
                res += Convert.ToString(times[i].accday) + "\n";
            }
            return res;
        }
        public string GetFish()//打鱼还是晒网
        {
            string res = "";
            for(int i = 0;i<times.Count;i++)
            {
                res += times[i].Fish;
            }
            return res;
        }

    }
}