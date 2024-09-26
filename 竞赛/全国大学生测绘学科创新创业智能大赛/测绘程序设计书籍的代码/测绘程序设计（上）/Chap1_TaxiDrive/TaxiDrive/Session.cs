using System;
using System.Collections.Generic;
using TaxiDrive;

namespace TaxiDrive
{
    class Session
    {
        public int Sn;
        public double StartMjd, EndMjd;
        public double Length;
        public double Velocity, Azimuth;

        public Session(Epoch start, Epoch end)
        {
            Sn = 0;
            StartMjd = start.Mjd;
            EndMjd = end.Mjd;
            GetLength(start, end);//求距离
            GetVelocity();//求速度
            GetAzimuth(start, end);//求方位角
        }
        private void GetAzimuth(Epoch start, Epoch end)
        {
            double eps = 1e-5;
            double dx = end.x - start.x;
            double dy = end.y - start.y;
            if (Math.Abs(dx) < eps)
            {
                if (Math.Abs(dy) < eps)
                {
                    Azimuth = 0;
                }
                else if (dy > 0)
                {
                    Azimuth = 0.5 * Math.PI;
                }
                else
                {
                    Azimuth = 1.5 * Math.PI;
                }
            }
            else
            {
                Azimuth = Math.Atan2(dy, dx);
                if (dx < 0)
                {
                    Azimuth += Math.PI;

                }
            }
            if (Azimuth < 0)
            {
                Azimuth += 2 * Math.PI;
            }
            if (Azimuth > 2 * Math.PI)
            {
                Azimuth -= 2 * Math.PI;
            }
            Azimuth *= 180 / Math.PI;
        }

        private void GetVelocity()
        {
            double dt = (EndMjd - StartMjd) * 24;
            Velocity = Length / dt;
        }

        private void GetLength(Epoch start,Epoch end)
        {
            double dx = end.x - start.x;
            double dy = end.y - start.y;
            Length = Math.Sqrt(dx * dx + dy * dy) / 1000.0;
        }
        public override string ToString()
        {
            string line = $"{Sn:00}, {StartMjd:f5}-{EndMjd:f5}, ";
            line += $"{Velocity:f3}, {Azimuth:f3}";
            return line;
        }
    }
}

