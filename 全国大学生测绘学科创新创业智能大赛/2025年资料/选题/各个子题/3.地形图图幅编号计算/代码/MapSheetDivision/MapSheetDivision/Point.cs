using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSheetDivision
{
    class Point
    {
        public double ID;     //ID
        public double L;     //经度
        public double B;     //纬度
        public string scale; // 比例尺

        public int Row;//行号
        public int Column; //列号
        public double detaL; //经差
        public double detaB; //纬差

        public String oldMapCode; //老图幅编号
        public String newMapCode; //新图幅编号

        public String[] OldBesideMapList; // 老式图幅编号的接图表数据
        public String[] BesideMapList; //接图表数据，从左到右，从上到下，123456789.

        public Point(double newID, double newL, double newB, string newScale)
        {
            ID = newID;
            L = newL;
            B = newB;
            scale = newScale;
        }
    }
}
