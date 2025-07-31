using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GNSSProcess
{
    class Observation
    {
        //历元
        public string obs_name;
        public double obs_L1;
        public double obs_L2;
        public double obs_P1;
        public double obs_P2;

        public double Nwl; // 宽项模糊度
        public double Nwl_avg;
        public double fangcha_2; // 方差
        public bool IS_Zhoutiao;
        public bool ERROR; //异常值

        public double CMCIF;
        public double I;
        public double CMC_est;

        public double Pif;
        public double FAIif;
        public double Psmooth;

        public Observation(string line)
        {
            var split = Regex.Split(line, @",");
            obs_name = split[0];
            obs_L1 = Convert.ToDouble(split[1]);
            obs_L2 = Convert.ToDouble(split[2]);
            obs_P1 = Convert.ToDouble(split[3]);
            obs_P2 = Convert.ToDouble(split[4]);
        }

        public Observation(string name, double L1, double L2, double P1, double P2)
        {
            obs_name = name;
            obs_L1 = L1;
            obs_L2 = L2;
            obs_P1 = P1;
            obs_P2 = P2;
        }

        public Observation Clone()
        {
            return new Observation(obs_name, obs_L1, obs_L2, obs_P1, obs_P2);
        }
    }
}
