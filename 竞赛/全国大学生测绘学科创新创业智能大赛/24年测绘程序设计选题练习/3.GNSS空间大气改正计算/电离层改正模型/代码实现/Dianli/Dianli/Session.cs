using System;
using System.Text.RegularExpressions;
namespace DianLi
{
    class Session
    {
        public string name;
        public double X;
        public double Y;
        public double Z;
        public double A;
        public double E;
        public double FaiM;
        public double namedaIp;
        public double Tion;
        public double Dion;
        public Session()
        {

        }
        public Session(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            name = buf[0];
            X = Convert.ToDouble(buf[1]) * 1000.0;
            Y = Convert.ToDouble(buf[2]) * 1000.0;
            Z = Convert.ToDouble(buf[3]) * 1000.0;
        }
        public Session(double X,double Y,double Z)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}