using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
namespace DianLi
{
    class Algo
    {
        #region 计算一天里的秒数
        public static double GetSeconds(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            double hours = Convert.ToDouble(buf[4]);
            double minutes = Convert.ToDouble(buf[5]);
            double seconds = Convert.ToDouble(buf[6]);
            return hours * 3600.0 + minutes * 60.0 + seconds;
        }
        #endregion

        #region 卫星的高度角和方位角
        public static void GetAandE(ref List<Session> sessions,Session sessionP)
        {
            double Bp = 30.0 / 180.0 * Math.PI;
            double Lp = 114.0 / 180.0 * Math.PI;
            foreach(Session d in sessions)
            {
                double detaX = d.X - sessionP.X;
                double detaY = d.Y - sessionP.Y;
                double detaZ = d.Z - sessionP.Z;
                double X = -Math.Sin(Bp) * Math.Cos(Lp) * detaX + (-Math.Sin(Bp) * Math.Sin(Lp) * detaY) + Math.Cos(Bp) * detaZ;
                double Y = -Math.Sin(Lp) * detaX + Math.Cos(Lp) * detaY;
                double Z = Math.Cos(Bp) * Math.Cos(Lp) * detaX + Math.Cos(Bp) * Math.Sin(Lp) * detaY + Math.Sin(Bp) * detaZ;
                d.A = Math.Atan(Y / X);
                d.E = Math.Atan(Z / Math.Sqrt(X * X + Y * Y));
            }
        }
        #endregion

        #region 计算穿刺点的地磁纬度
        public static void GetFaiM(ref List<Session> sessions)
        {
            double Bp = 30.0 / 180.0 * Math.PI;
            double Lp = 114.0 / 180.0 * Math.PI;
            foreach (Session d in sessions)
            {
                double psi = 0.0137 / (d.E + 0.11) - 0.022;
                double faiIp = Bp + psi * Math.Cos(d.A);
                double namedaIp = Lp + psi * Math.Sin(d.A) / Math.Cos(faiIp);
                d.FaiM = faiIp + 0.064 * Math.Cos(namedaIp - 1.617);
                d.namedaIp = namedaIp;
            }
        }
        #endregion

        #region 计算电离层延迟量
        public static void GetYanCi(ref List<Session> sessions,double seconds)
        {
            double a0 = 0.1397 * 1e-7;
            double a1 = -0.7451 * 1e-8;
            double a2 = -0.596 * 1e-7;
            double a3 = 0.1192 * 1e-6;
            double b0 = 0.1270 * 1e6;
            double b1 = -0.1966 * 1e6;
            double b2 = 0.6554 * 1e5;
            double b3 = 0.2621 * 1e6;
            foreach(Session d in sessions)
            {
                double A1 = 5 * 1e-9;
                double A2 = a0 + a1 * d.FaiM + a2 * d.FaiM * d.FaiM + a3 * d.FaiM * d.FaiM * d.FaiM;
                double A4 = b0 + b1 * d.FaiM + b2 * d.FaiM * d.FaiM + b3 * d.FaiM * d.FaiM * d.FaiM;
                double A3 = 50400;
                double t = 43200 * d.namedaIp + seconds;
                double F = 1.0 + 16.0 * Math.Pow(0.53 - d.E, 3);
                double temp = 2.0 * Math.PI * (t - A3) / A4;
                if(Math.Abs(temp) < 1.57)
                {
                    d.Tion = F * (A1 + A2 * Math.Cos(temp));
                }
                else
                {
                    d.Tion = F * A1;
                }
                d.Dion = d.Tion * 299792458;
            }
        }
        #endregion
    }
}