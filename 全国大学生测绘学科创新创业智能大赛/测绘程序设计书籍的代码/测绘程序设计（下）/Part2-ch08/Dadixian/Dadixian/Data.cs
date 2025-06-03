using System;
using System.Collections.Generic;
namespace Dadixian
{
    class Data
    {
        public double B;
        public double L;

        public Data(string[] line)
        {
            B = double.Parse(line[0]);
            L = double.Parse(line[1]);
        }
    }
}