using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace DianLi2
{
    class Algo
    {
        //主算法
        #region 观测秒数的计算
        public static double CalSeconds(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            double hours = Convert.ToDouble(buf[4]);
            double minutes = Convert.ToDouble(buf[5]);
            double seconds = Convert.ToDouble(buf[6]);
            return (hours * 3600 + minutes * 60 + seconds);
        }
        #endregion

        #region 计算卫星的高度角和方位角
        public static void GetAandE(ref List<Session> sessions,Session sessionP)
        {
            double Bp = 30.0 / 180.0 * Math.PI;
            double Lp = 114.0 / 180.0 * Math.PI;
            foreach (Session d in sessions)
            {
                double detaX = d.X - sessionP.X;
                double detaY = d.Y - sessionP.Y;
                double detaZ = d.Z - sessionP.Z;
                double X = (-Math.Sin(Bp) * Math.Cos(Lp) * detaX - Math.Sin(Bp) * Math.Sin(Lp) * detaY + Math.Cos(Bp) * detaZ);
                double Y = (-Math.Sin(Lp) * detaX + Math.Cos(Lp) * detaY);
                double Z = (Math.Cos(Bp) * Math.Cos(Lp) * detaX + Math.Cos(Bp) * Math.Sin(Lp) * detaY + Math.Sin(Bp) * detaZ);
                double A = Math.Atan(Y / X);
                double E = Math.Atan(Z / (Math.Sqrt(X * X + Y * Y)));
                if(Z < 0)
                {
                    E = E + Math.PI;
                }
                if(X > 0)
                {
                    if(Y > 0)
                    {

                    }
                    else
                    {
                        A += 2.0 * Math.PI;
                    }
                }
                else
                {
                    A += Math.PI;
                }
                d.A = A;
                d.E = E;
            }
        }
        #endregion

        #region 计算地磁纬度
        public static void CalFaim(ref List<Session> sessions)
        {
            foreach(Session d in sessions)
            {
                double Bp = 30.0 / 180.0 * Math.PI;
                double Lp = 114.0 / 180.0 * Math.PI;
                double psi = 0.0137 / (d.E + 0.11) - 0.022;
                double faiIP = Bp + psi * Math.Cos(d.A);
                double namedaIp = Lp + psi * Math.Sin(d.A) / Math.Cos(faiIP);
                d.namedaIP = namedaIp;
                d.faiM = faiIP + 0.064 * Math.Cos(namedaIp - 1.617);
            }
            
        }
        #endregion

        #region 计算电离层延迟量
        public static void CalIone(ref List<Session> sessions,double seconds)
        {
            foreach(Session d in sessions)
            {
                double F = 1.0 + 16.0 * Math.Pow((0.53 - d.E), 3);
                double A1 = 5 * Math.Pow(10,-9);
                double A3 = 50400.0;
                double t = 43200.0 * d.namedaIP + seconds;
                double A2 = 0.1397 * Math.Pow(10, -7) - 0.7451 * Math.Pow(10, -8) * d.faiM;
                A2 += -0.5960 * Math.Pow(10, -7) * Math.Pow(d.faiM, 2);
                A2 += 0.1192 * Math.Pow(10,-6) * Math.Pow(d.faiM, 3);
                double A4 = 0.1270 * Math.Pow(10, 6) - 0.1966 * Math.Pow(10, 6) * d.faiM;
                A4 += 0.6554 * Math.Pow(10, 5) * Math.Pow(d.faiM, 2);
                A4 += 0.2621 * Math.Pow(10, 6) * Math.Pow(d.faiM, 3);
                double temp = 2.0 * Math.PI * (t - A3) / A4;
                double Tion = 0;
                if(Math.Abs(temp) < 1.57)
                {
                    Tion = F * (A1 + A2 * Math.Cos(temp));
                }
                else
                {
                    Tion = F * A1;
                }
                double Dion = Tion * 299792458;
                d.Dion = Dion;
            }
        }
        #endregion
    }
}