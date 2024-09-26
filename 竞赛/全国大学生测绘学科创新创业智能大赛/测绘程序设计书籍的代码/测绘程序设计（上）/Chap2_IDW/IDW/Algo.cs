using System;

namespace IDW
{
    /// <summary>
    /// 主算法的实现
    /// </summary>
    class Algo
    {
        public int N;//最近的N个点参与计算
        DataEntity Data;
        public double QH;//插值结果的高度
        public Algo(DataEntity data,int n)//构造函数，传入点的数据和N
        {
            Data = data;
            N = n;
        }
        public string idw(Point Q)
        {
            string res = $"{Q.ID}     {Q.X:F3}      {Q.Y:F3}";
            for (int i = 0; i < Data.Count;i++)
            {
                Data[i].dist = GetDistance(Q, Data[i]);
            }
            Sort();
            double QH = GetH(Data);
            res += $"       {QH:F3}       ";
            for(int j = 0;j < N;j++)
            {
                res += $"{Data[j].ID} ";
            }
            res += "\n";
            return res;
            
        }
        private double GetDistance(Point Q,Point pt)//算距离
        {
            double dx = Q.X - pt.X;
            double dy = Q.Y - pt.Y;
            double ds = Math.Sqrt(dx * dx + dy * dy);
            return ds;
        }
        public double GetH(DataEntity data)//计算插值点的高程
        {
            double over = 0;
            double under = 0;
            for(int i = 0;i < N;i++)
            {
                over += data[i].H / data[i].dist;
                under += 1 / data[i].dist;
            }
            double res = over / under;
            return res;
        }
        private void Sort()//排序
        {
            for(int i = 0;i < Data.Count;i++)
            {
                for(int j = i;j < Data.Count;j++)
                {
                    if(Data[i].dist > Data[j].dist)
                    {
                        Point pt = new Point();
                        pt = Data[i];
                        Data[i] = Data[j];
                        Data[j] = pt;
                    }
                }
            }
        }
    }
}