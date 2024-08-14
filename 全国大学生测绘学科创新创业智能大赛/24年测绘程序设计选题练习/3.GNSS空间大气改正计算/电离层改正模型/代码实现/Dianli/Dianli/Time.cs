using System;
using System.Text.RegularExpressions;
namespace Dianli
{
    /// <summary>
    /// 时间的保存、转换
    /// </summary>
    class Time
    {
        public double secondOfDay;

        public Time(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            //计算一天中的秒数
            secondOfDay = Convert.ToDouble(buf[4]) * 3600 + Convert.ToDouble(buf[5]) * 60 + Convert.ToDouble(buf[6]);
        }
    }
}