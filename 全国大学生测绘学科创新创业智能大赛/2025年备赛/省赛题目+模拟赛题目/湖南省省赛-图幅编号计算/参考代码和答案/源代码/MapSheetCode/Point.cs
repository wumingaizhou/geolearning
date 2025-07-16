using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MapSheetCode
{
    public class Point
    {
        //点的数据结构
        public double ID;
        public double B; //纬度
        public double L; //经度
        public int Row; //行号，不是最终结果的行号
        public int Col; // 列号
        public string letter_100K; //行号字母,100比例尺
        public string scale; //比例尺
        public string newMapCode; // 最终的图幅编号

        public Point(string line)
        {
            var Split = Regex.Split(line, @",");
            ID = Convert.ToDouble(Split[0]);
            L = Convert.ToDouble(Split[1]);
            B = Convert.ToDouble(Split[2]);
        }
    }
}
