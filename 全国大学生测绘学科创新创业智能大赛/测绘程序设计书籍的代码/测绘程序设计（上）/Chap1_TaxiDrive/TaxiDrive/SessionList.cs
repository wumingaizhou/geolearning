using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiDrive;

namespace TaxiDrive
{
    class SessionList
    {
        public List<Session> Data = new List<Session>();
        public double TotalLength, DirctLength;//累积距离和首尾直线距离
        public SessionList(List<Epoch> epoches)//构造函数，创建类时首先进入这个函数
        {
            for (int i = 0; i < epoches.Count - 1; i++)
            {
                Session s = new Session(epoches[i], epoches[i + 1]);
                s.Sn = i;
                Data.Add(s);
            }
            GetTotalLength();//求累积距离
            GetDirctLength(epoches);//求首尾直线距离
        }

        private void GetDirctLength(List<Epoch> epoches)
        {
            int n = epoches.Count;
            Session s = new Session(epoches[0], epoches[n - 1]);
            DirctLength = s.Length;
        }

        private void GetTotalLength()
        {
            TotalLength = 0;
            foreach (var d in Data)
            {
                TotalLength += d.Length;
            }
        }

        public override string ToString()
        {
            string line = "------------速度和方位角计算结果----------\r\n";
            foreach (var d in Data)
            {
                line += d.ToString() + "\r\n";
            }
            line += "------------距离计算结果-----------------\r\n";
            line += $"累积距离：{TotalLength:f3} (km)\r\n";
            line += $"首尾直线距离： {DirctLength:f3} (km)";

            return line;
        }
    }
}