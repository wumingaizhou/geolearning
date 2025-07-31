using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace GNSSProcess
{
    public class Satellite
    {
        public double f1; // L1的频率
        public double f2; // L2的频率
        public double nameda1; // L1的波长
        public double nameda2; // L2的波长

        public string satellite_name;
        public List<Observation> satellite_observations;

        public Satellite(string name, List<Observation> observations)
        {
            satellite_name = name;
            satellite_observations = new List<Observation>(observations);

            CalMWandCMC(); //周跳探测和多路径计算里，需要的一个变量
        }
        private void CalMWandCMC()
        {
            //假设，到时候给了不同的卫星，那么频率会不同，我的数据里没有这种情况，都是G
            if (satellite_name.StartsWith("G")){
                f1 = 1575.42 * 1e6; // L1的频率
                f2 = 1227.6 * 1e6; // L2的频率
                nameda1 = 0.190;
                nameda2 = 0.244;
            }
            //if (satellite_name.StartsWith("C"))
            //{
            //    f1 = 1561.098 * 1e6; // L1的频率
            //    f2 = 1207.140 * 1e6; // L2的频率
            //}
            foreach(var obs in satellite_observations)
            {
                // 我们数据里的载波是以 周 为单位
                obs.namedaWL = 299792458 / (f1 - f2);
                obs.Nwl = obs.obs_fai1 - obs.obs_fai2 - (f1 * obs.obs_p1 + f2 * obs.obs_p2) / (obs.namedaWL * (f1 + f2));

                double CMC1 = obs.obs_p1 - obs.obs_fai1 * nameda1;
                double CMC2 = obs.obs_p2 - obs.obs_fai2 * nameda2;
                obs.CMC = (f1 * f1 * CMC1 - f2 * f2 * CMC2) / (f1 * f1 - f2 * f2);
                obs.I = (obs.obs_fai1 * nameda1 - obs.obs_fai2 * nameda2) / (1.0 - (f1 * f1) / (f2 * f2));

                obs.Pif = (f1 * f1 * obs.obs_p1 - f2 * f2 * obs.obs_p2) / (f1 * f1 - f2 * f2);
                obs.FAIif = (f1 * f1 * obs.obs_fai1 * nameda1 - f2 * f2 * obs.obs_fai2 * nameda2) / (f1 * f1 - f2 * f2);
            }
        }
    }

    public class Observation
    {
        // 测站信息
        public string obs_name;
        public double obs_p1;
        public double obs_p2;
        public double obs_fai1; // 单位为周
        public double obs_fai2;

        public double Nwl;
        public double Nwl_avgi;// 看不懂这些变量没关系，刚好三个部分，分别对应周跳，多路径，平滑需要用到的辅助变量
        public double fangcha_i2;
        public double namedaWL;
        public bool IS_zhoutiao = false; // 是否发生了周跳，True 为发生了

        public double CMC; // 多路径估算需要用到，用的是双频
        public double I; // 电离层误差，由于是双频，可以计算
        public double cmc_est; // 最终估算出来的伪距多路径误差

        public double Pif;
        public double FAIif;
        public double Psmooth; // 伪距平滑

        public Observation(string line)
        {
            var regex = Regex.Split(line, @",");
            obs_name = regex[0];
            obs_fai1 = Convert.ToDouble(regex[1]);
            obs_fai2 = Convert.ToDouble(regex[2]);
            obs_p1 = Convert.ToDouble(regex[3]);
            obs_p2 = Convert.ToDouble(regex[4]);
        }
    }
}
