using System;
using MathNet.Numerics.Interpolation;
using MathNet.Numerics;
using System.Collections.Generic;
namespace DuiLiu
{
    class Algo
    {
        #region 年积日的计算
        public static double GetDays(string line)
        {
            int year = Convert.ToInt16(line.Substring(0, 4));
            int month = Convert.ToInt16(line.Substring(4, 2));
            int days = Convert.ToInt16(line.Substring(6,2));
            DateTime dateTime = new DateTime(year, month, days);
            var dayOfYear = dateTime.DayOfYear;
            return dayOfYear;
        }
        #endregion

        #region 湿分量的计算
        public static void GetMw(ref List<Session> sessions)
        {
            double[] x = { 15, 30, 45, 60, 75 };
            double[] Ya = { 0.00058021897, 0.00056794847, 0.00058118019, 0.00059727542, 0.00061641693 };
            double[] Yb = { 0.0014275268, 0.0015138625, 0.0014572752, 0.0015007428, 0.0017599082 };
            double[] Yc = { 0.043472961, 0.046729510, 0.043908931, 0.044626982, 0.054736038 };
            var linerA = Interpolate.Linear(x, Ya);
            var linerB = Interpolate.Linear(x, Yb);
            var linerC = Interpolate.Linear(x, Yc);
            foreach (Session d in sessions)
            {
                double Aw = linerA.Interpolate(d.L);
                double Bw = linerB.Interpolate(d.L);
                double Cw = linerC.Interpolate(d.L);
                if(d.L < 15)
                {
                    Aw = Ya[0];
                    Bw = Yb[0];
                    Cw = Yc[0];
                }
                if(d.L > 75)
                {
                    Aw = Ya[4];
                    Bw = Yb[4];
                    Cw = Yc[4];
                }
                double above = 1.0 / (1.0 + (Aw / (1.0 + (Bw / (1.0 + Cw)))));
                double under = 1.0 / (Math.Sin(d.E / 180.0 * Math.PI) + (Aw / (Math.Sin(d.E / 180.0 * Math.PI) + (Bw / (Math.Sin(d.E / 180.0 * Math.PI) + Cw)))));
                d.Mw = under / above;
            }
        }
        #endregion

        #region 湿分量的计算
        public static void GetMd(ref List<Session> sessions)
        {
            double[] x = { 15, 30, 45, 60, 75 };
            double[] avg = { 0.0012769934, 0.0012683230, 0.0012465397, 0.0012196049, 0.0012045996 };
            double[] bvg = { 0.0029153695, 0.0029152299, 0.0029288445, 0.0029022565, 0.0029024912 };
            double[] cvg = { 0.062610505, 0.062837393, 0.063721774, 0.063824265, 0.064258455 };
            double[] amp = { 0.0, 0.000012709626, 0.000026523662, 0.000034000452, 0.000041202191 };
            double[] bmp = { 0.0, 0.000021414979, 0.000030160779, 0.000072562722, 0.00011723375 };
            double[] cmp = { 0.0, 0.000090128400, 0.000043497037, 0.00084795348, 0.0017037206 };
            var linerAvg = Interpolate.Linear(x, avg);
            var linerBvg = Interpolate.Linear(x, bvg);
            var linerCvg = Interpolate.Linear(x, cvg);
            var linerAmp = Interpolate.Linear(x, amp);
            var linerBmp = Interpolate.Linear(x, bmp);
            var linerCmp = Interpolate.Linear(x, cmp);
            double aht = 2.53 * 1e-5;
            double bht = 5.49 * 1e-3;
            double cht = 1.14 * 1e-3;
            foreach (Session d in sessions)
            {
                double ad = linerAvg.Interpolate(d.L);
                double bd = linerBvg.Interpolate(d.L);
                double cd = linerCvg.Interpolate(d.L);
                double am = linerAmp.Interpolate(d.L);
                double bm = linerBmp.Interpolate(d.L);
                double cm = linerCmp.Interpolate(d.L);
                ad = ad + am * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                bd = bd + bm * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                cd = cd + cm * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));

                if(d.L < 15)
                {
                    ad = linerAvg.Interpolate(15) + linerAvg.Interpolate(15) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                    bd = linerBvg.Interpolate(15) + linerBvg.Interpolate(15) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                    cd = linerCvg.Interpolate(15) + linerCvg.Interpolate(15) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                }
                if (d.L > 75)
                {
                    ad = linerAvg.Interpolate(75) + linerAvg.Interpolate(75) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                    bd = linerBvg.Interpolate(75) + linerBvg.Interpolate(75) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                    cd = linerCvg.Interpolate(75) + linerCvg.Interpolate(75) * Math.Cos(2.0 * Math.PI * (d.days - 28) / (365.25));
                }
                double above1 = 1.0 / (1.0 + (ad / (1.0 + (bd / (1.0 + cd)))));
                double under1 = 1.0 / (Math.Sin(d.E / 180.0 * Math.PI) + (ad / (Math.Sin(d.E / 180.0 * Math.PI) + (bd / (Math.Sin(d.E / 180.0 * Math.PI) + cd)))));
                double above2 = 1.0 / (1.0 + (aht / (1.0 + (bht / (1.0 + cht)))));
                double under2 = 1.0 / (Math.Sin(d.E / 180.0 * Math.PI) + (aht / (Math.Sin(d.E / 180.0 * Math.PI) + (bht / (Math.Sin(d.E / 180.0 * Math.PI) + cht)))));
                double Md = under1 / above1 + (1.0 / Math.Sin(d.E / 180.0 * Math.PI) - under2 / above2) * d.H / 1000.0;
                d.Md = Md;
            }
        }
        #endregion

        #region 延迟改正计算
        public static void GetS(ref List<Session> sessions)
        {
            foreach(Session d in sessions)
            {
                double ZHD = 2.29951 * Math.Pow(Math.E, -0.000116 * d.H);
                double ZWD = 0.1;
                d.S = ZHD * d.Md + ZWD * d.Mw;
            }
        }
        #endregion
    }
}