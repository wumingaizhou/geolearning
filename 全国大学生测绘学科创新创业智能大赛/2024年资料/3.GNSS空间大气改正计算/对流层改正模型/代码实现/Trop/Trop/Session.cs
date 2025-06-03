using System;
namespace Trop
{
    class Session
    {
        public string name;
        public double days;
        public double B;
        public double L;
        public double H;
        public double E;

        public double Mw;
        public double Md;
        public double S;

        public Session()
        {

        }
        public Session(string line)
        {
            var buf = line.Trim().Split(',');
            name = buf[0];
            days = Algo.GetDays(buf[1]);
            B = Convert.ToDouble(buf[2]);
            L = Convert.ToDouble(buf[3]);
            H = Convert.ToDouble(buf[4]);
            E = Convert.ToDouble(buf[5]);
        }
    }
}