using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class Hatch
    {
        public List<Satellite> Hatch_satellites = new List<Satellite>();

        public Hatch(List<Satellite> satellites)
        {
            Hatch_satellites = satellites;

            CalHatch();
        }
        private void CalHatch()
        {
            foreach(var satellite in Hatch_satellites)
            {
                var obss = satellite.satellite_observations;
                obss[0].Psmooth = obss[0].Pif;
                int N = 1;
                for (int i = 1; i < obss.Count; i++)
                {
                    if (obss[i].IS_Zhoutiao)
                    {
                        obss[i].Psmooth = obss[i].Pif;
                        N = 1;
                        continue;
                    }
                    N++;
                    double n = Math.Min(N, 100);
                    double a = 1.0 / n;
                    obss[i].Psmooth = (1.0 - a) * obss[i - 1].Psmooth + a * obss[i].Pif + (1.0 - a) * (obss[i].FAIif - obss[i - 1].FAIif);
                }
            }
        }

        public string GetResult()
        {
            string result = "相位平滑伪距结果：\n";

            foreach (var satllite in Hatch_satellites)
            {
                string sat = $"-------卫星编号：{ satllite.satellite_name} 的结果如下-------\n";
                foreach (var obs in satllite.satellite_observations)
                {
                    sat += $"历元名为 {obs.obs_name} 的相位平滑伪距为：{obs.Psmooth}\n";
                }
                result += sat;
            }

            return result;
        }
    }
}
