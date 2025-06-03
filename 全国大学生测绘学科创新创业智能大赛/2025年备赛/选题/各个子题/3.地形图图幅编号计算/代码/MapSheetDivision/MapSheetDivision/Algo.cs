using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapSheetDivision
{
    class Algo
    {
        public List<Point> points = new List<Point>();

        public Algo(ref List<Point> outpoints)
        {
            points = outpoints;
            go();
        }

        public void go()
        {
            //主算法
            foreach(var p in points)
            {
                // 考虑到邻接图计算，封装为函数
                GetMapCode(p);
                CalculateBesideMaps(p); // 计算接图表
            }
        }

        public void GetMapCode(Point p)
        {
            // 1：100万分幅与编号的行号和列号
            p.Row = (int)(p.B / 4.0) + 1;
            p.Column = (int)(p.L / 6.0) + 31;

            //给定点相对于1：100万图幅左下角的经差与纬差
            p.detaL = p.L - (int)(p.L / 6.0) * 6;
            p.detaB = p.B - (int)(p.B / 4.0) * 4;

            var scale = p.scale; //给定点的比例尺
            // 获取1:100万图幅的行号字母
            string rowLetter = GetRowLetter(p.Row);

            if(scale == "1:100万" || scale == "1:1000000")
            {
                // 老式图幅编号规则 (1): 行字母列号，例如 J50
                p.oldMapCode = $"{rowLetter}{p.Column}";
            }
            else if (scale == "1:50万" || scale == "1:500000")
            {
                // 1:100万图幅经差6度，纬差4度。
                // 1:50万图幅由1:100万图幅划分而来，每个1:100万图幅划分为2x2共4个1:50万图幅。
                // 每个1:50万图幅经差3度，纬差2度。
                // 老式图幅编号规则 (2): 1:100万图号 + 1位顺序号(1-4)。顺序号排列：
                //   A(1) B(2)  (北半部，p.detaB >= 2度)
                //   C(3) D(4)  (南半部，p.detaB < 2度)
                // A (左上/西北): detaL < 3, detaB >= 2
                // B (右上/东北): detaL >= 3, detaB >= 2
                // C (左下/西南): detaL < 3, detaB < 2
                // D (右下/东南): detaL >= 3, detaB < 2
                int oldSubCode500k = 0;

                if (p.detaL < 3 && p.detaB >= 2) // A (西北) -> 老顺序号 1
                {
                    oldSubCode500k = 1;
                }
                else if (p.detaL >= 3 && p.detaB >= 2) // B (东北) -> 老顺序号 2
                {
                    oldSubCode500k = 2;
                }
                else if (p.detaL < 3 && p.detaB < 2) // C (西南) -> 老顺序号 3
                { 
                    oldSubCode500k = 3;
                }
                else if (p.detaL >= 3 && p.detaB < 2) // D (东南) -> 老顺序号 4
                {
                    oldSubCode500k = 4;
                }

                // 老式图幅编号：例如 J501
                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode500k}";
                
            }
            else if (scale == "1:25万" || scale == "1:250000")
            {
                // 1:25万图幅由1:100万图幅划分而来，每个1:100万图幅划分为4x4共16个1:25万图幅。
                // 每个1:25万图幅经差1.5度 (90分)，纬差1度 (60分)。
                // 老式图幅编号规则 (3): 1:100万图号 + 2位顺序号(01-16)。顺序号从北往南，从西往东排列。
                // 行索引 r_250k (0-3, 北到南): (int)((4.0 - p.detaB) / 1.0)
                // 列索引 c_250k (0-3, 西到东): (int)(p.detaL / 1.5)
                int r_250k = (int)((4.0 - p.detaB) / 1.0); // 0-3, 0是最北行
                int c_250k = (int)(p.detaL / 1.5);       // 0-3, 0是最西列
                int oldSubCode250k = r_250k * 4 + c_250k + 1;
                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode250k:D2}"; // 2位数字，补零
            }
            else if (scale == "1:10万" || scale == "1:100000")
            {
                // 1:10万图幅由1:100万图幅划分而来，每个1:100万图幅划分为12x12共144个1:10万图幅。
                // 每个1:10万图幅经差0.5度 (30分)，纬差1/3度 (20分)。
                // 老式图幅编号规则 (4): 1:100万图号 + 3位顺序号(001-144)。顺序号从北往南，从西往东排列。
                // 行索引 r_100k (0-11, 北到南): (int)((4.0 - p.detaB) / (1.0/3.0))
                // 列索引 c_100k (0-11, 西到东): (int)(p.detaL / 0.5)
                int r_100k = (int)((4.0 - p.detaB) / (1.0/3.0)); // 0-11, 0是最北行
                int c_100k = (int)(p.detaL / 0.5);             // 0-11, 0是最西列
                int oldSubCode100k = r_100k * 12 + c_100k + 1;
                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode100k:D3}"; // 3位数字，补零
            }
            else if (scale == "1:5万" || scale == "1:50000")
            {
                // 1:5万图幅由1:10万图幅进一步划分，每个1:10万图幅划分为2x2=4个1:5万图幅。
                // 1:10万图幅经差30分(0.5度)，纬差20分(1/3度)。
                // 1:5万图幅经差15分(0.25度)，纬差10分(1/6度)。

                // 老式图幅编号规则 (5): 1:100万图号 + 1:10万顺序号(3位) + 1:5万顺序号(1位，A-D -> 1-4)。
                // 首先计算其所在的1:10万图幅的顺序号 (oldSubCode100k_for_50k)
                int r_100k_for_50k = (int)((4.0 - p.detaB) / (1.0/3.0)); // 0-11, 0是最北行 (用于1:10万图廓)
                int c_100k_for_50k = (int)(p.detaL / 0.5);             // 0-11, 0是最西列 (用于1:10万图廓)
                int oldSubCode100k_for_50k = r_100k_for_50k * 12 + c_100k_for_50k + 1;

                // 然后计算在该1:10万图幅内的相对经纬度，以确定1:5万的顺序号
                // detaL_within_100k: 点相对于其所在1:10万图幅西南角的经差
                // detaB_within_100k: 点相对于其所在1:10万图幅西南角的纬差
                double sw_lon_of_100k_tile = c_100k_for_50k * 0.5; // 1:10万图幅西南角经度 (相对于100万图幅西南角)
                double sw_lat_of_100k_tile = (int)(p.detaB / (1.0/3.0)) * (1.0/3.0); // 1:10万图幅西南角纬度 (相对于100万图幅西南角)
                double detaL_within_100k = p.detaL - sw_lon_of_100k_tile;
                double detaB_within_100k = p.detaB - sw_lat_of_100k_tile;

                // 1:5万顺序号 (1-4)，排列方式为：
                //   A(1) B(2)  (北半部，detaB_within_100k >= 10分)
                //   C(3) D(4)  (南半部，detaB_within_100k < 10分)
                // A (左上/西北): detaL_within_100k < 15分, detaB_within_100k >= 10分
                // B (右上/东北): detaL_within_100k >= 15分, detaB_within_100k >= 10分
                // C (左下/西南): detaL_within_100k < 15分, detaB_within_100k < 10分
                // D (右下/东南): detaL_within_100k >= 15分, detaB_within_100k < 10分
                int oldSubCode50k_suffix = 0;
                if (detaL_within_100k < 0.25 && detaB_within_100k >= (1.0/6.0)) oldSubCode50k_suffix = 1; // A
                else if (detaL_within_100k >= 0.25 && detaB_within_100k >= (1.0/6.0)) oldSubCode50k_suffix = 2; // B
                else if (detaL_within_100k < 0.25 && detaB_within_100k < (1.0/6.0)) oldSubCode50k_suffix = 3; // C
                else if (detaL_within_100k >= 0.25 && detaB_within_100k < (1.0/6.0)) oldSubCode50k_suffix = 4; // D

                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode100k_for_50k:D3}{oldSubCode50k_suffix}";

            }
            else if (scale == "1:2.5万" || scale == "1:25000")
            {
                // 老式图幅编号规则 (6): 1:100万图号 + 1:10万顺序号(3位) + 1:5万顺序号(1位) + 1:2.5万顺序号(1位)
                // 1:2.5万图幅由1:5万图幅进一步划分，每个1:5万图幅划分为2x2=4个1:2.5万图幅。
                // 1:5万图幅经差15分(0.25度)，纬差10分(1/6度)。
                // 1:2.5万图幅经差7.5分(0.125度)，纬差5分(1/12度)。

                // 1. 计算其所在的1:10万图幅的顺序号 (oldSubCode100k)
                int r_100k_for_25k = (int)((4.0 - p.detaB) / (1.0/3.0)); // 0-11, 0是最北行
                int c_100k_for_25k = (int)(p.detaL / 0.5);             // 0-11, 0是最西列
                int oldSubCode100k = r_100k_for_25k * 12 + c_100k_for_25k + 1;

                // 2. 计算点在1:10万图幅内的相对经纬度 (detaL_within_100k, detaB_within_100k)
                //    以确定1:5万的后缀
                double sw_lon_of_100k_tile = c_100k_for_25k * 0.5; // 1:10万图幅西南角经度 (相对于100万图幅西南角)
                double sw_lat_of_100k_tile = (int)(p.detaB / (1.0/3.0)) * (1.0/3.0); // 1:10万图幅西南角纬度 (相对于100万图幅西南角)
                double detaL_within_100k = p.detaL - sw_lon_of_100k_tile;
                double detaB_within_100k = p.detaB - sw_lat_of_100k_tile;

                // 3. 计算1:5万图幅的后缀顺序号 (_50k_suffix_for_25k) (1-4), A/B/C/D -> 1/2/3/4
                int _50k_suffix_for_25k = 0;
                if (detaL_within_100k < 0.25 && detaB_within_100k >= (1.0/6.0)) _50k_suffix_for_25k = 1; // A (NW)
                else if (detaL_within_100k >= 0.25 && detaB_within_100k >= (1.0/6.0)) _50k_suffix_for_25k = 2; // B (NE)
                else if (detaL_within_100k < 0.25 && detaB_within_100k < (1.0/6.0)) _50k_suffix_for_25k = 3; // C (SW)
                else if (detaL_within_100k >= 0.25 && detaB_within_100k < (1.0/6.0)) _50k_suffix_for_25k = 4; // D (SE)

                // 4. 计算点在1:5万图幅内的相对经纬度 (detaL_within_50k, detaB_within_50k)
                //    首先确定当前1:5万图幅的西南角绝对坐标 (相对于1:100万图幅西南角)
                double current_50k_tile_sw_lon_abs = sw_lon_of_100k_tile;
                double current_50k_tile_sw_lat_abs = sw_lat_of_100k_tile;

                // 根据1:5万后缀(_50k_suffix_for_25k)调整1:5万图幅的西南角
                // A(1) B(2) (北半部), C(3) D(4) (南半部)
                // A(1) C(3) (西半部), B(2) D(4) (东半部)
                if (_50k_suffix_for_25k == 1) { /* NW */ current_50k_tile_sw_lat_abs += (1.0/6.0); }
                else if (_50k_suffix_for_25k == 2) { /* NE */ current_50k_tile_sw_lon_abs += 0.25; current_50k_tile_sw_lat_abs += (1.0/6.0); }
                else if (_50k_suffix_for_25k == 3) { /* SW: 坐标基准已是100k图幅的西南角，再叠加此1:5万图幅是100k图幅的西南部分，故不需额外调整*/ }
                else if (_50k_suffix_for_25k == 4) { /* SE */ current_50k_tile_sw_lon_abs += 0.25; }
                
                double detaL_within_50k = p.detaL - current_50k_tile_sw_lon_abs;
                double detaB_within_50k = p.detaB - current_50k_tile_sw_lat_abs;

                // 5. 计算1:2.5万图幅的后缀顺序号 (_25k_suffix) (1-4), A/B/C/D -> 1/2/3/4
                // 1:2.5万图幅经差 0.125度 (7.5分)，纬差 1/12度 (5分)
                int _25k_suffix = 0;
                if (detaL_within_50k < 0.125 && detaB_within_50k >= (1.0/12.0)) _25k_suffix = 1; // A (NW)
                else if (detaL_within_50k >= 0.125 && detaB_within_50k >= (1.0/12.0)) _25k_suffix = 2; // B (NE)
                else if (detaL_within_50k < 0.125 && detaB_within_50k < (1.0/12.0)) _25k_suffix = 3; // C (SW)
                else if (detaL_within_50k >= 0.125 && detaB_within_50k < (1.0/12.0)) _25k_suffix = 4; // D (SE)

                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode100k:D3}{_50k_suffix_for_25k}{_25k_suffix}";
            }
            else if (scale == "1:1万" || scale == "1:10000")
            {
                // 1:1万图幅由1:10万图幅进一步划分，每个1:10万图幅划分为8x8=64个1:1万图幅。
                // 1:10万图幅经差30分(0.5度)，纬差20分(1/3度)。
                // 1:1万图幅经差30/8 = 3.75分(0.0625度)，纬差20/8 = 2.5分(1/24度)。

                // 老式图幅编号规则 (7): 1:100万图号 + 1:10万顺序号(3位) + 1:1万顺序号(2位，01-64)。
                // 首先计算其所在的1:10万图幅的顺序号 (oldSubCode100k_for_10k)
                int r_100k_for_10k = (int)((4.0 - p.detaB) / (1.0/3.0)); // 0-11, 0是最北行 (用于1:10万图廓)
                int c_100k_for_10k = (int)(p.detaL / 0.5);             // 0-11, 0是最西列 (用于1:10万图廓)
                int oldSubCode100k_for_10k = r_100k_for_10k * 12 + c_100k_for_10k + 1;

                // 然后计算在该1:10万图幅内的相对经纬度，以确定1:1万的顺序号
                double sw_lon_of_100k_tile_for_10k = c_100k_for_10k * 0.5;
                double sw_lat_of_100k_tile_for_10k = (int)(p.detaB / (1.0/3.0)) * (1.0/3.0);
                double detaL_within_100k_for_10k = p.detaL - sw_lon_of_100k_tile_for_10k;
                double detaB_within_100k_for_10k = p.detaB - sw_lat_of_100k_tile_for_10k;

                // 1:1万顺序号 (01-64)，从北往南，从西往东排列。
                // 在1:10万图幅内，1:1万图幅的行索引 r_10k_sub (0-7, 北到南)
                // 纬度范围是1/3度。每个1:1万图幅纬差1/24度。
                // r_10k_sub = (int)(((1.0/3.0) - detaB_within_100k_for_10k) / (1.0/24.0));
                // 修正：detaB_within_100k_for_10k 是相对于1:10万图幅西南角的纬差，从南到北增加
                // 1:10万图幅最北边的纬度偏移是 1/3 度。 
                // 所以， ( (1.0/3.0) - detaB_within_100k_for_10k ) 是从北边开始算的距离。
                // 或者，从南往北计算行号，再转换为从北往南的索引。
                // 南到北的行号 (0-7): (int)(detaB_within_100k_for_10k / (1.0/24.0))
                // 北到南的行索引 (0-7): 7 - south_to_north_row_index
                int south_to_north_row_index_10k_sub = (int)(detaB_within_100k_for_10k / (1.0/24.0));
                // 确保在0-7范围内，因为浮点计算可能导致边界问题，例如 detaB_within_100k_for_10k 恰好是 1/3.0 时，会得到8
                south_to_north_row_index_10k_sub = Math.Max(0, Math.Min(7, south_to_north_row_index_10k_sub));
                int r_10k_sub = 7 - south_to_north_row_index_10k_sub; // 0-7, 0是最北行

                // 在1:10万图幅内，1:1万图幅的列索引 c_10k_sub (0-7, 西到东)
                // 经度范围是0.5度。每个1:1万图幅经差0.0625度。
                int c_10k_sub = (int)(detaL_within_100k_for_10k / 0.0625);
                c_10k_sub = Math.Max(0, Math.Min(7, c_10k_sub)); // 0-7, 0是最西列

                int oldSubCode10k_suffix = r_10k_sub * 8 + c_10k_sub + 1;
                p.oldMapCode = $"{rowLetter}{p.Column}{oldSubCode100k_for_10k:D3}{oldSubCode10k_suffix:D2}";

            }

            // 新图幅编号规则:
            // 1:100万: 与老方法相同 (行字母列号，例如 J50)
            // 其他比例尺: 1:100万图号 + 比例尺代码(1位字母) + 行号(3位数字) + 列号(3位数字)
            // 获取1:100万图幅的基本编号 (例如 J50)
            string baseMapCode1M = $"{rowLetter}{p.Column}";
            if (scale == "1:100万" || scale == "1:1000000")
            {
                // 根据规则，1:100万的新图幅编号与老图幅编号相同。
                // p.oldMapCode 已经计算并存储了此编号 (例如 J50)
                p.newMapCode = p.oldMapCode;
            }
            else
            {
                // 使用辅助函数获取比例尺参数
                bool scaleIsSupported = GetScaleDeltasAndLetter(scale, out double detaL_scale_val, out double detaB_scale_val, out char scaleLetterChar);

                if (scaleIsSupported)
                {
                    // 确保 detaB_scale_val 和 detaL_scale_val 不为零以避免除零错误
                    if (detaB_scale_val == 0 || detaL_scale_val == 0)
                    {
                        p.newMapCode = "比例尺参数错误";
                        return; // 或者采取其他错误处理
                    }

                    // 计算新图幅编号的行号和列号
                    // 行号计算公式: (100万图幅纬度跨度 / 当前比例尺纬差) - INT(点在100万图幅内的纬度偏移 / 当前比例尺纬差)
                    //   p.detaB 是点相对于1:100万图幅西南角的纬度偏移 (范围 0 到 4度)
                    //   行号从北向南编号，起始为1
                    int new_row = (int)(Math.Floor(4.0 / detaB_scale_val)) - (int)(Math.Floor(p.detaB / detaB_scale_val));
                    
                    // 列号计算公式: INT(点在100万图幅内的经度偏移 / 当前比例尺经差) + 1
                    //   p.detaL 是点相对于1:100万图幅西南角的经度偏移 (范围 0 到 6度)
                    //   列号从西向东编号，起始为1
                    int new_col = (int)(Math.Floor(p.detaL / detaL_scale_val)) + 1;

                    // 组合新图幅编号: 1:100万图号 + 比例尺代码 + 3位行号 + 3位列号
                    p.newMapCode = $"{baseMapCode1M}{scaleLetterChar}{new_row:D3}{new_col:D3}";
                }
                else
                {
                    // 如果 GetScaleDeltasAndLetter 返回 false，说明比例尺不支持
                    p.newMapCode = "不支持的比例尺";
                    // 确保老图幅编号也反映不支持的状态，如果尚未设置
                    // (通常老图幅编号的计算逻辑会自行处理或有一个默认的else来设置错误信息)
                    if (string.IsNullOrEmpty(p.oldMapCode) || !p.oldMapCode.Contains("不支持"))
                    {
                        // 此处可以考虑是否强制设置 p.oldMapCode，但通常分离的逻辑更好
                    }
                }
            }
        }

        /// <summary>
        /// 根据比例尺字符串获取图幅的经差、纬差和比例尺代码。
        /// </summary>
        /// <param name="scale">比例尺字符串，例如 "1:1000000"。</param>
        /// <param name="detaL_scale_map">输出参数，图幅的经差。</param>
        /// <param name="detaB_scale_map">输出参数，图幅的纬差。</param>
        /// <param name="scaleLetter">输出参数，新图幅编号中的比例尺代码。</param>
        /// <returns>如果比例尺受支持，则返回 true；否则返回 false。</returns>
        private bool GetScaleDeltasAndLetter(string scale, out double detaL_scale_map, out double detaB_scale_map, out char scaleLetter)
        {
            detaL_scale_map = 0;
            detaB_scale_map = 0;
            scaleLetter = ' '; // 默认值

            switch (scale)
            {
                case "1:1000000": case "1:100万":
                    detaL_scale_map = 6.0;
                    detaB_scale_map = 4.0;
                    scaleLetter = 'A'; // 1:100万本身新图幅编号规则特殊，此代码主要为CalculateBesideMaps提供经纬差
                    return true;
                case "1:500000": case "1:50万":
                    detaL_scale_map = 3.0;
                    detaB_scale_map = 2.0;
                    scaleLetter = 'B';
                    return true;
                case "1:250000": case "1:25万":
                    detaL_scale_map = 1.5;
                    detaB_scale_map = 1.0;
                    scaleLetter = 'C';
                    return true;
                case "1:100000": case "1:10万":
                    detaL_scale_map = 0.5;
                    detaB_scale_map = 1.0 / 3.0;
                    scaleLetter = 'D';
                    return true;
                case "1:50000": case "1:5万":
                    detaL_scale_map = 0.25;
                    detaB_scale_map = 1.0 / 6.0;
                    scaleLetter = 'E';
                    return true;
                case "1:25000": case "1:2.5万":
                    detaL_scale_map = 0.125;
                    detaB_scale_map = 1.0 / 12.0;
                    scaleLetter = 'F';
                    return true;
                case "1:10000": case "1:1万":
                    detaL_scale_map = 0.0625; 
                    detaB_scale_map = 1.0 / 24.0; 
                    scaleLetter = 'G';
                    return true;
                // 可根据需要添加更多比例尺，例如 1:5000
                // case "1:5000":
                //     detaL_scale_map = 0.025;        // 经差1分30秒
                //     detaB_scale_map = 1.0 / 60.0;   // 纬差1分
                //     scaleLetter = 'H';
                //     return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 计算并填充给定点p的邻接图幅编号（老式和新式）。
        /// 接图表按九宫格顺序排列：左上、中上、右上、左中、中、右中、左下、中下、右下。
        /// </summary>
        /// <param name="p">中心点对象，包含经纬度、比例尺等信息，其OldBesideMapList和BesideMapList将被填充。</param>
        public void CalculateBesideMaps(Point p)
        {
            // 获取当前比例尺下图幅的经差和纬差
            bool scaleSupported = GetScaleDeltasAndLetter(p.scale, out double detaL_scale_map, out double detaB_scale_map, out char scaleLetter);

            // 初始化接图表数组
            p.OldBesideMapList = new string[9];
            p.BesideMapList = new string[9];

            if (!scaleSupported)
            {
                // 如果比例尺不支持，则填充错误信息
                for (int k = 0; k < 9; k++)
                {
                    p.OldBesideMapList[k] = "不支持的比例尺";
                    p.BesideMapList[k] = "不支持的比例尺";
                }
                return;
            }

            // 遍历九宫格 (3x3)
            // i: 行索引 (0: 上, 1: 中, 2: 下)
            // j: 列索引 (0: 左, 1: 中, 2: 右)
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // 计算邻接图幅中心点的经纬度
                    // 对于行：上方图幅纬度增加 detaB_scale_map, 下方图幅纬度减少 detaB_scale_map
                    // (1-i) 会产生: i=0 -> +1 (上), i=1 -> 0 (中), i=2 -> -1 (下)
                    double neighborB = p.B + (1 - i) * detaB_scale_map;
                    // 对于列：左方图幅经度减少 detaL_scale_map, 右方图幅经度增加 detaL_scale_map
                    // (j-1) 会产生: j=0 -> -1 (左), j=1 -> 0 (中), j=2 -> +1 (右)
                    double neighborL = p.L + (j - 1) * detaL_scale_map;

                    // 创建邻接图幅的临时点对象
                    // ID可以设为0或其他临时值，因为它不影响图幅编号计算
                    Point neighborPoint = new Point(0, neighborL, neighborB, p.scale);

                    // 为这个邻接点计算图幅编号
                    GetMapCode(neighborPoint);

                    // 将计算得到的图幅编号存入中心点p的接图表数组中
                    int index = i * 3 + j; // 九宫格索引，从左到右，从上到下 (0-8)
                    p.OldBesideMapList[index] = neighborPoint.oldMapCode;
                    p.BesideMapList[index] = neighborPoint.newMapCode;
                }
            }
        }

       
        /// <summary>
        /// 根据1:100万图幅的行号获取对应的字母。
        /// 行号从1开始，对应字母A到V。
        /// </summary>
        /// <param name="rowNumber">1:100万图幅的行号 (1-22)</param>
        /// <returns>对应的行字母</returns>
        private string GetRowLetter(int rowNumber)
        {
            // 1:100万图幅行号从1开始，对应 A-V (共22个)
            // ASCII 'A' is 65. rowNumber 1 should be 'A'.
            if (rowNumber >= 1 && rowNumber <= 22)
            {
                return ((char)('A' + rowNumber - 1)).ToString();
            }
            return ""; // 或者抛出异常，根据实际需求处理无效行号
        }

    }
}
