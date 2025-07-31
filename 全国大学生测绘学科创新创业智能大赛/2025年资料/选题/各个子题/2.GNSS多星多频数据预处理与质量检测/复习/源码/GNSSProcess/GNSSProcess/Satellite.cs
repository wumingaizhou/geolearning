using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class Satellite
    {
        // 卫星
        public List<Observation> satellite_observations = new List<Observation>();
        public string satellite_name;
        public double NamedaWL;
        public double f1;
        public double f2;
        public double nameda1;
        public double nameda2;

        public Satellite(List<Observation> observations, string name)
        {
            satellite_observations = observations.Select(obs => obs.Clone()).ToList();
            satellite_name = name;

            GetNwl(); // 求每个历元的宽巷模糊度
        }

        private void GetNwl()
        {
            if (satellite_name.StartsWith("G")){
                f1 = 1575.42 * 1e6;
                f2 = 1227.6 * 1e6;
                nameda1 = 0.190;
                nameda2 = 0.244;
                NamedaWL = 299792458.0 / (1575.42 * 1e6 - 1227.6 * 1e6);
            }
            foreach(var obs in satellite_observations)
            {
                obs.Nwl = obs.obs_L1 - obs.obs_L2 - (f1 * obs.obs_P1 + f2 * obs.obs_P2) / (NamedaWL * (f1 + f2));
                double cmc1 = obs.obs_P1 - obs.obs_L1 * nameda1;
                double cmc2 = obs.obs_P2 - obs.obs_L2 * nameda2;
                obs.CMCIF = (f1 * f1 * cmc1 - f2 * f2 * cmc2) / (f1 * f1 - f2 * f2);
                obs.I = (obs.obs_L1 * nameda1 - obs.obs_L2 * nameda2) / (1.0 - (f1 * f1) / (f2 * f2));

                obs.Pif = (f1 * f1 * obs.obs_P1 - f2 * f2 * obs.obs_P2) / (f1 * f1 - f2 * f2);
                obs.FAIif = (f1 * f1 * obs.obs_L1 * nameda1 - f2 * f2 * obs.obs_L2 * nameda2) / (f1 * f1 - f2 * f2);
            }
        }
    }
}
