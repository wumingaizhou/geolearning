using System;
using System.Collections.Generic;
namespace lono
{
    class Algo
    {
        public List<satellite> satellites;

        public Algo(List<satellite> satellites)
        {
            this.satellites = satellites;
            Run();
        }
        public string Run()
        {
            string res = "SV    E(度)    A(度)    L1(m)    L2(m)\n";
            foreach (satellite d in satellites)
            {
                res += $"{d.name}   {d.E:F3}   {d.A:F3}   {d.Tion:F3}    {d.Dion:F3}\n";
            }
            return res;
        }
    }
}