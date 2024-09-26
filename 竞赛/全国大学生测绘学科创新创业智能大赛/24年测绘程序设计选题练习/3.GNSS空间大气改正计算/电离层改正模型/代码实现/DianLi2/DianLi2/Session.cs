using System;
namespace DianLi2
{
    class Session
    {
        //每个卫星的数据结构
        public string name;
        public double X;
        public double Y;
        public double Z;

        public double A;
        public double E;

        public double namedaIP;
        public double faiM;
        public double Dion;

        public Session()
        {

        }
        public Session(string name,double X,double Y,double Z)
        {
            this.name = name;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
        }
    }
}