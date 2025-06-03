using System;
using System.Text.RegularExpressions;
namespace lono
{
    class Time
    {
        public int year;
        public int month;
        public int day;
        public int hour;
        public int minute;
        public double second;
        public double second_of_day;
        
        public Time(string line)
        {
            var buf = Regex.Split(line, @"\s+");//正则表达式的内容；
            year = Convert.ToInt16(buf[1]);
            month = Convert.ToInt16(buf[2]);
            day = Convert.ToInt16(buf[3]);
            hour = Convert.ToInt16(buf[4]);
            minute = Convert.ToInt16(buf[5]);
            second = Convert.ToDouble(buf[6]);
            CalSecond();
        }
        public void CalSecond()//计算一天中的秒数;
        {
            second_of_day = hour * 60 * 60 + minute * 60 + second;
        }
    }
}