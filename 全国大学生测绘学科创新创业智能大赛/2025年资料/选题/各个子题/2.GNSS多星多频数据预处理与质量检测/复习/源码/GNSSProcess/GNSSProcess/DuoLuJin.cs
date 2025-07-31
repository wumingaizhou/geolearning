using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class DuoLuJin
    {
        public List<Satellite> DuoLuJin_Satellite = new List<Satellite>();
        List<Observation> liner_obs = new List<Observation>();

        public DuoLuJin(List<Satellite> satellites)
        {
            DuoLuJin_Satellite = satellites;

            CalDuoLuJin();
        }
        private void CalDuoLuJin()
        {
            foreach (var satellite in DuoLuJin_Satellite)
            {
                liner_obs.Clear();
                foreach (var obs in satellite.satellite_observations)
                {
                    //if (obs.ERROR)
                    //{
                    //    continue;
                    //}
                    if (!obs.IS_Zhoutiao)
                    {
                        liner_obs.Add(obs);
                    }
                    else
                    {
                        // 发生了周跳
                        Cal_cmcest();
                        liner_obs.Clear();
                        liner_obs.Add(obs);
                        obs.CMC_est = 0;
                    }

                }
                if (satellite.satellite_observations[satellite.satellite_observations.Count - 1].IS_Zhoutiao) continue;
                Cal_cmcest();
            }
        }

        private void Cal_cmcest()
        {
            var mean = liner_obs.Average(obs => obs.CMCIF - 2 * obs.I);

            foreach(var obs in liner_obs)
            {
                obs.CMC_est = obs.CMCIF - 2.0 * obs.I - mean;
            }
        }

        public string GetResult()
        {
            string result = "多路径误差结果：\n";

            foreach(var satllite in DuoLuJin_Satellite)
            {
                string sat = $"-------卫星编号：{ satllite.satellite_name} 的结果如下-------\n";
                foreach (var obs in satllite.satellite_observations)
                {
                    sat += $"历元名为 {obs.obs_name} 的多路径误差为：{obs.CMC_est}\n";
                }
                result += sat;
            }

            return result;
        }
    }
}
