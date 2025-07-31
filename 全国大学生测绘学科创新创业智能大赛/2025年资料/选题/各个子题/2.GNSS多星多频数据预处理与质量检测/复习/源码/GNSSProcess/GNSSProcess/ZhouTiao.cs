using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GNSSProcess
{
    class ZhouTiao
    {
        // 周跳检测
        public List<Satellite> ZhouTiao_satellites = new List<Satellite>();

        public ZhouTiao(List<Satellite> satellites)
        {
            ZhouTiao_satellites = satellites;

            Cal_zhoutiao();
        }
        private void Cal_zhoutiao()
        {
            foreach(var satellite in ZhouTiao_satellites)
            {
                var obss = satellite.satellite_observations;

                obss[0].IS_Zhoutiao = false;
                obss[0].Nwl_avg = obss[0].Nwl;
                obss[0].fangcha_2 = 0;

                for(int i = 1; i < obss.Count; i++)
                {
                    obss[i].Nwl_avg = obss[i - 1].Nwl_avg + 1.0 / i * (obss[i].Nwl - obss[i - 1].Nwl_avg);
                    obss[i].fangcha_2 = obss[i - 1].fangcha_2 + 1.0 / i * (Math.Pow((obss[i].Nwl - obss[i - 1].Nwl_avg), 2) - obss[i - 1].fangcha_2);
                }
                for (int i = 2; i < obss.Count; i++)
                {
                    bool flag = Math.Abs(obss[i].Nwl - obss[i - 1].Nwl_avg) >= 4.0 * Math.Sqrt(obss[i - 1].fangcha_2);
                    bool flag2 = false;
                    bool flag3 = false;  // flag3 是用于判断异常值的
                    if(i < obss.Count - 1)
                    {
                        flag2 = Math.Abs(obss[i + 1].Nwl - obss[i].Nwl) <= 1.0;
                        flag3 = Math.Abs(obss[i + 1].Nwl - obss[i].Nwl) >= 1.0;  
                    }
                    obss[i].IS_Zhoutiao = flag && flag2;
                    obss[i].ERROR = flag && flag3;
                }
            }
        }

        public string GetResult()
        {
            string result = "周跳探测结果：\n";

            foreach(var satellite in ZhouTiao_satellites)
            {
                string sat = $"-------卫星编号：{ satellite.satellite_name} 的结果如下-------\n";
                foreach(var obs in satellite.satellite_observations)
                {
                    sat += $"历元名：{obs.obs_name}，";
                    if (obs.IS_Zhoutiao)
                    {
                        sat += "发生周跳\n";
                        continue;
                    }
                    else if (obs.ERROR)
                    {
                        sat += "是异常值\n";
                        continue;
                    }
                    else
                    {
                        sat += "正常\n";
                        continue;
                    }
                }
                result += sat;
            }

            return result;
        }
    }
}
