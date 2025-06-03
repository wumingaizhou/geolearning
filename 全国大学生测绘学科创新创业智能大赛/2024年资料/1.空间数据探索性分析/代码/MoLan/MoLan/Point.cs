using System;
using System.Text.RegularExpressions;
namespace MoLan
{
    class Point
    {
        /// <summary>
        /// 点的数据结构
        /// </summary>
        public string name;
        public double X;
        public double Y;
        public double interest;//属性值
        public double a;//计算椭圆参数用的
        public double b;//计算椭圆参数用的
        public Point()
        {

        }
        public Point(string line)
        {
            var buf = Regex.Split(line, @"\,+");
            name = buf[0];
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
            interest = Convert.ToDouble(buf[3]);
        }
    }
}