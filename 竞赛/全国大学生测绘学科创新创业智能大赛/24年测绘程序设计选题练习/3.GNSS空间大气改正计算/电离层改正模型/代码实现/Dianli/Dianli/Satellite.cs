using System;
using System.Text.RegularExpressions;
namespace Dianli
{
    /// <summary>
    /// 一个卫星的数据，计算高度角方位角，以及地磁纬度，延迟量
    /// </summary>
    class Satellite
    {
        public string name;
        public double X,Y,Z;
        public double A;//方位角
        public double E;//高度角
        public double L_dici;//地磁纬度
        public double IPP;//计算结果，电离层延迟时间
        public double IPP_s;//电离层延迟距离
        private double seconds;//秒时间
        private double[] CezhanXYZ = { -2225669.7744, 4998936.1598, 3265908.9678 };//测站地心坐标，单位为米
        private double B = 30;//测站经度
        private double L = 114;//测站纬度，单位为度

        public Satellite(string line,Time time)
        {
            var buf = Regex.Split(line, @"\s+");
            name = buf[0];
            X = Convert.ToDouble(buf[1]) * 1000;
            Y = Convert.ToDouble(buf[2]) * 1000;
            Z = Convert.ToDouble(buf[3]) * 1000;
            seconds = time.secondOfDay;
            CalAE();
            Algo();
        }

        private void CalAE()
        {
            //计算高度角方位角
            //1.计算卫星的测站站心坐标
            B = B / 180.0 * Math.PI;
            L = L / 180.0 * Math.PI;
            double[] right_matrix = { X - CezhanXYZ[0], Y - CezhanXYZ[1], Z - CezhanXYZ[2] };
            double X0 = -Math.Sin(B) * Math.Cos(L) * right_matrix[0] + (-Math.Sin(B) * Math.Sin(L) * right_matrix[1]) + Math.Cos(B) * right_matrix[2];
            double Y0 = -Math.Sin(L) * right_matrix[0] + Math.Cos(L) * right_matrix[1] + 0;
            double Z0 = Math.Cos(B) * Math.Cos(L) * right_matrix[0] + Math.Cos(B) * Math.Sin(L) * right_matrix[1] + Math.Sin(B) * right_matrix[2];
            //2.计算A，E
            E = Math.Atan(Z0 / Math.Sqrt(X0 * X0 + Y0 * Y0));
            if(Z0 <= 0)//书上没写这个判断，卫星高度角好像有的说法是可以为负号的，不过我找的资料里是0到90度，问题不大
            {
                E = E + Math.PI;
            }
            A = Math.Atan(Y0 / X0);
            if(X0 > 0)
            {
                if(Y0 < 0)
                {
                    A = A + 2 * Math.PI;
                }
            }
            if(X0 < 0)
            {
                A = A + Math.PI;
            }
        }

        private void Algo()
        {
            //第一步，计算地磁纬度L_dici
            double zhangjiao = 0.0137 / (E + 0.11) - 0.022;
            double faiIPP = L + zhangjiao * Math.Cos(A);
            if(faiIPP > 0.416)
            {
                faiIPP = 0.416;
            }
            if(faiIPP < -0.416)
            {
                faiIPP = -0.416;
            }
            double nmedaIPP = B + zhangjiao * Math.Sin(A) / Math.Cos(faiIPP);
            L_dici = faiIPP + 0.064 * Math.Cos(nmedaIPP - 1.617);
            //第二步，计算电离层延迟
            double t = 43200 * nmedaIPP + seconds;
            if(t > 86400)
            {
                t -= 86400;
            }
            if(t < 0)
            {
                t += 86400;
            }
            double[] alist = { 0.1397 * 1e-7, -0.7451 * 1e-8, -0.5960 * 1e-7, 0.1192 * 1e-6 };
            double[] blist = { 0.1270 * 1e6, -0.1966 * 1e6, 0.6554 * 1e5, 0.2621 * 1e6 };
            double AIPP = 0;
            double PIPP = 0;
            for(int i = 0;i <= 3;i++)
            {
                AIPP += alist[i] * Math.Pow(L_dici, i);
                PIPP += blist[i] * Math.Pow(L_dici, i);
            }
            if(AIPP < 0)
            {
                AIPP = 0;
            }
            if(PIPP < 72000)
            {
                PIPP = 72000;
            }
            double XIPP = 2 * Math.PI * (t - 50400) / PIPP;
            double F = 1.0 + 16.0 * Math.Pow((0.53 - E), 3);
            if(Math.Abs(XIPP) <= 1.57)
            {
                IPP = F * (5e-9 + AIPP * Math.Cos(XIPP));
            }
            else
            {
                IPP = F * 5e-9;
            }
            IPP_s = IPP * 299792458;
        }
    }
}