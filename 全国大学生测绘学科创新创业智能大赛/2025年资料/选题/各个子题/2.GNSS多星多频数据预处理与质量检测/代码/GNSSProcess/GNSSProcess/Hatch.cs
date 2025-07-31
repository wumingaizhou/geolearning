using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class Hatch
    {
        // 伪距平滑 (Hatch滤波)
        public List<Satellite> Hatch_satellites = new List<Satellite>();
        private const int n = 100; // 平滑窗口长度，通常设为100秒

        public Hatch(List<Satellite> satellites)
        {
            Hatch_satellites = satellites;
            Cal();
        }

        public void Cal()
        {
            foreach (var satellite in Hatch_satellites)
            {
                var observations = satellite.satellite_observations;

                if (observations.Count == 0) continue;

                // 第一个历元直接使用伪距
                observations[0].Psmooth = observations[0].Pif;

                for (int i = 1; i < observations.Count; i++)
                {
                    var obs_k = observations[i];
                    var obs_k_1 = observations[i - 1];

                    // 检查当前历元是否发生周跳
                    if (obs_k.IS_zhoutiao)
                    {
                        // 发生周跳，重新初始化平滑过程
                        obs_k.Psmooth = obs_k.Pif;
                    }
                    else
                    {
                        // 正常情况，使用Hatch滤波公式
                        // 计算当前的平滑窗口长度（不超过最大窗口长度n）
                        int currentN = Math.Min(n, GetContinuousLength(observations, i));

                        // Hatch滤波公式
                        double alpha = 1.0 / currentN;
                        obs_k.Psmooth = (1 - alpha) * obs_k_1.Psmooth +
                                       alpha * obs_k.Pif +
                                       (1 - alpha) * (obs_k.FAIif - obs_k_1.FAIif);
                    }
                }
            }
        }

        /// <summary>
        /// 计算从当前历元向前的连续观测长度（没有周跳）
        /// </summary>
        /// <param name="observations">观测数据列表</param>
        /// <param name="currentIndex">当前历元索引</param>
        /// <returns>连续观测长度</returns>
        private int GetContinuousLength(List<Observation> observations, int currentIndex)
        {
            int length = 1; // 包括当前历元

            // 向前查找连续的观测值（没有周跳）
            for (int i = currentIndex - 1; i >= 0; i--)
            {
                if (observations[i + 1].IS_zhoutiao) // 如果下一个历元发生周跳，停止计数
                    break;
                length++;
                if (length >= n) // 达到最大窗口长度
                    break;
            }

            return length;
        }

        public string GetHatchResult()
        {
            string result = "相位平滑伪距结果：\n";

            foreach (var satellite in Hatch_satellites)
            {
                var text = $"---卫星名：{satellite.satellite_name}---\n";
                foreach (var obs in satellite.satellite_observations)
                {
                    if (obs.IS_zhoutiao)
                    {
                        text += $"历元 {obs.obs_name} (周跳重新初始化) 平滑后的伪距：{obs.Psmooth:F4}\n";
                    }
                    else
                    {
                        text += $"历元 {obs.obs_name} 平滑后的伪距：{obs.Psmooth:F4}\n";
                    }
                }
                result += text;
            }

            return result;
        }
    }
}