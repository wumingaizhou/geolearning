using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions; // Regex需要

namespace SpaceResection
{
    class Point
    {
        // 控制点的数据信息
        public int id; // 点号
        public double x, y, X, Y, Z; // 控制点的影像坐标，地面坐标，单位为米
        public double x_new, y_new;// 像点坐标近似值
        public double a11, a12, a13, a14, a15, a16, a21, a22, a23, a24, a25, a26; // 误差矩阵A里的12个参数
        public double tLX, tLY;// L 矩阵的参数。

        public Point(string line)
        {
            //构造函数
            var buf = Regex.Split(line, @"\,+");
            id = Convert.ToInt32(buf[0]);
            x = Convert.ToDouble(buf[4]) / 1000.0;
            y = Convert.ToDouble(buf[5]) / 1000.0;
            X = Convert.ToDouble(buf[1]);
            Y = Convert.ToDouble(buf[2]);
            Z = Convert.ToDouble(buf[3]);
        }


        public Point(int id, double x, double y, double X, double Y, double Z)
        {
            this.id = id;
            this.x = x;
            this.y = y;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
        public Point Clone()
        {
            return new Point(this.id, this.x, this.y, this.X, this.Y, this.Z);
        }
    }
}
