using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    public class ZhouTiao
    {
        // 周跳探测
        public List<Satellite> Satellite_observations_zhoutiao = new List<Satellite>();

        public ZhouTiao(List<Satellite> satellites)
        {
            Satellite_observations_zhoutiao = satellites;
            Method_MW();
        }

        // MW组合检测法
        private void Method_MW()
        {
            // 定义探测阈值
            const double THRESHOLD_FACTOR = 3.0; // 3倍标准差
            const double CHECK_VALUE = 1.0;      // Nwl后续历元差异检查值
            const int MIN_SAMPLES = 5;           // 最少样本数，少于此数不进行检测

            foreach (var satellite in Satellite_observations_zhoutiao)
            {
                var observations = satellite.satellite_observations;

                if (observations.Count < MIN_SAMPLES)
                {
                    // 样本太少，全部标记为正常
                    foreach (var obs in observations)
                    {
                        obs.IS_zhoutiao = false;
                    }
                    continue;
                }

                // 第一个历元标记为正常
                observations[0].IS_zhoutiao = false;

                double sumNwl = observations[0].Nwl;
                double sumNwlSq = observations[0].Nwl * observations[0].Nwl;

                // 从第二个历元开始进行探测
                for (int i = 1; i < observations.Count; i++)
                {
                    int sampleCount = i; // 到上一个历元为止的样本数量（从0到i-1）

                    // 只有当样本数量足够时才进行检测
                    if (sampleCount >= MIN_SAMPLES)
                    {
                        // 1. 计算到上一个历元(i-1)为止的均值和标准差
                        double mean = sumNwl / sampleCount;
                        double variance = (sumNwlSq / sampleCount) - (mean * mean);

                        // 防止方差为负数（数值精度问题）
                        variance = Math.Max(variance, 0);
                        double stdDev = Math.Sqrt(variance);

                        // 2. 检查当前历元 Nwl[i] 与历史均值的偏差
                        bool isJump = stdDev > 1e-10 && Math.Abs(observations[i].Nwl - mean) >= THRESHOLD_FACTOR * stdDev;

                        // 3. 检查发生跳变后的历元是否稳定在一个新的值上（防止粗差误判）
                        if (isJump)
                        {
                            // 如果不是最后一个历元，检查与下一个历元的一致性
                            if (i < observations.Count - 1)
                            {
                                if (Math.Abs(observations[i].Nwl - observations[i + 1].Nwl) <= CHECK_VALUE)
                                {
                                    observations[i].IS_zhoutiao = true;
                                    // 发生周跳，从当前历元开始重新累积统计量
                                    sumNwl = observations[i].Nwl;
                                    sumNwlSq = observations[i].Nwl * observations[i].Nwl;
                                }
                                else
                                {
                                    // 可能是粗差，不标记为周跳
                                    observations[i].IS_zhoutiao = false;
                                    // 将当前值加入统计，继续迭代
                                    sumNwl += observations[i].Nwl;
                                    sumNwlSq += observations[i].Nwl * observations[i].Nwl;
                                }
                            }
                            else
                            {
                                // 最后一个历元，无法检查后续稳定性，但偏差很大就标记为周跳
                                observations[i].IS_zhoutiao = true;
                            }
                        }
                        else
                        {
                            observations[i].IS_zhoutiao = false;
                            // 如果没有周跳，将当前值加入统计，用于下一次迭代
                            sumNwl += observations[i].Nwl;
                            sumNwlSq += observations[i].Nwl * observations[i].Nwl;
                        }
                    }
                    else
                    {
                        // 样本数量不足，标记为正常，但仍加入统计
                        observations[i].IS_zhoutiao = false;
                        sumNwl += observations[i].Nwl;
                        sumNwlSq += observations[i].Nwl * observations[i].Nwl;
                    }
                }
            }
        }

        // 打印结果
        public string GetZhoutiaoResult()
        {
            string result = "周跳探测结果：\n";

            foreach (var satellite in Satellite_observations_zhoutiao)
            {
                string text = $"---卫星名：{satellite.satellite_name}---\n";
                int epochIndex = 0;
                foreach (var obs in satellite.satellite_observations)
                {
                    if (obs.IS_zhoutiao)
                    {
                        text += $"历元 {obs.obs_name}，发生周跳！\n";
                    }
                    else
                    {
                        text += $"历元 {obs.obs_name}，正常。\n";
                    }
                    epochIndex++;
                }
                result += text;
            }

            return result;
        }
    }
}