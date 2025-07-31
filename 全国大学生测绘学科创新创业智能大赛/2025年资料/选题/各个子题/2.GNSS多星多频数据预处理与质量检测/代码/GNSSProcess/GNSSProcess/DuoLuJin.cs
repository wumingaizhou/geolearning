using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class DuoLuJin
    {
        // 伪距多路径估算
        public List<Satellite> DuoLuJin_satellites = new List<Satellite>();
        private List<Observation> liner_obs = new List<Observation>();

        public DuoLuJin(List<Satellite> satellites)
        {
            DuoLuJin_satellites = satellites;
            Cal();
        }

        private void Cal()
        {
            // 主要是需要注意，连续的历元
            foreach (var satellite in DuoLuJin_satellites)
            {
                liner_obs.Clear(); // 每个卫星开始时清空

                for (int i = 0; i < satellite.satellite_observations.Count; i++)
                {
                    var obs = satellite.satellite_observations[i];

                    if (!obs.IS_zhoutiao)
                    {
                        // 正常观测值，加入连续序列
                        liner_obs.Add(obs);
                    }
                    else
                    {
                        // 遇到周跳，先处理之前累积的连续观测值
                        if (liner_obs.Count > 0)
                        {
                            Calcmc_est();
                        }

                        // 发生周跳的点无法计算多路径误差
                        obs.cmc_est = 0;

                        // 清空并准备开始新的连续序列
                        liner_obs.Clear();

                        // 注意：周跳点本身不加入新序列，因为它的载波相位不连续
                        // 如果需要从周跳点开始新序列，可以在这里添加：
                        // liner_obs.Add(obs);
                    }
                }

                // 处理最后一段连续的观测值
                if (liner_obs.Count > 0)
                {
                    Calcmc_est();
                }

                liner_obs.Clear(); // 处理完当前卫星后清空
            }
        }

        private void Calcmc_est()
        {
            // 检查列表是否为空
            if (liner_obs.Count == 0)
                return;

            // 如果只有一个观测值，多路径误差设为0
            if (liner_obs.Count == 1)
            {
                liner_obs[0].cmc_est = 0;
                return;
            }

            // 计算平均值
            double average = liner_obs.Average(p => p.CMC - 2 * p.I);

            // 计算每个观测值的多路径误差
            foreach (var obs in liner_obs)
            {
                obs.cmc_est = (obs.CMC - 2 * obs.I) - average;
            }
        }

        public string GetDuoLuJinResult()
        {
            string result = "多路径误差计算结果：\n";

            foreach (var satellite in DuoLuJin_satellites)
            {
                var text = $"---卫星名： {satellite.satellite_name}---\n";
                foreach (var obs in satellite.satellite_observations)
                {
                    if (obs.IS_zhoutiao)
                    {
                        text += $"历元 {obs.obs_name} 发生周跳，多路径误差：{obs.cmc_est:F4}\n";
                    }
                    else
                    {
                        text += $"历元 {obs.obs_name} 的伪距多路径误差：{obs.cmc_est:F4}\n";
                    }
                }
                result += text;
            }

            return result;
        }
    }
}