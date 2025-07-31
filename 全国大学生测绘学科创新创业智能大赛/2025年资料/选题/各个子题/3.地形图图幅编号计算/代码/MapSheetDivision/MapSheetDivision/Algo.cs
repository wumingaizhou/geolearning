using System;
using System.Collections.Generic;
using System.Linq;

namespace MapSheetDivision
{
    class Algo
    {
        public List<Point> points = new List<Point>();

        public static double DegToDdmmss(double deg)
        {
            int degrees = (int)Math.Floor(Math.Abs(deg));
            double fractional = Math.Abs(deg) - degrees;
            double minutes = fractional * 60;
            double seconds = (minutes - Math.Floor(minutes)) * 60;
            minutes = Math.Floor(minutes);
            double ddmmss = degrees + minutes / 100.0 + seconds / 10000.0;
            return deg >= 0 ? ddmmss : -ddmmss;
        }

        public static double DdmmssToDeg(double ddmmss)
        {
            bool isNegative = ddmmss < 0;
            ddmmss = Math.Abs(ddmmss);

            int degrees = (int)ddmmss;
            double fractional = ddmmss - degrees;
            double minutes = fractional * 100;
            double seconds = (minutes - (int)minutes) * 100;
            minutes = (int)minutes;

            double deg = degrees + minutes / 60.0 + seconds / 3600.0;
            return isNegative ? -deg : deg;
        }

        public Algo(ref List<Point> outpoints)
        {
            points = outpoints;
            go();
        }

        public void go()
        {
            foreach (var p in points)
            {
                GetMapCode(p);
                CalculateBesideMaps(p);
            }
        }

        // 统一的比例尺信息管理
        private static readonly Dictionary<string, ScaleConfig> ScaleConfigs = new Dictionary<string, ScaleConfig>()
        {
            {"1:1000000", new ScaleConfig(1000000, 6.0, 4.0, 'A')},
            {"1:100万", new ScaleConfig(1000000, 6.0, 4.0, 'A')},
            {"1:500000", new ScaleConfig(500000, 3.0, 2.0, 'B')},
            {"1:50万", new ScaleConfig(500000, 3.0, 2.0, 'B')},
            {"1:250000", new ScaleConfig(250000, 1.5, 1.0, 'C')},
            {"1:25万", new ScaleConfig(250000, 1.5, 1.0, 'C')},
            {"1:100000", new ScaleConfig(100000, 0.5, 1.0/3.0, 'D')},
            {"1:10万", new ScaleConfig(100000, 0.5, 1.0/3.0, 'D')},
            {"1:50000", new ScaleConfig(50000, 0.25, 1.0/6.0, 'E')},
            {"1:5万", new ScaleConfig(50000, 0.25, 1.0/6.0, 'E')},
            {"1:25000", new ScaleConfig(25000, 0.125, 1.0/12.0, 'F')},
            {"1:2.5万", new ScaleConfig(25000, 0.125, 1.0/12.0, 'F')},
            {"1:10000", new ScaleConfig(10000, 0.0625, 1.0/24.0, 'G')},
            {"1:1万", new ScaleConfig(10000, 0.0625, 1.0/24.0, 'G')},
            {"1:5000", new ScaleConfig(5000, 112.5/3600.0, 75.0/3600.0, 'H')},
            {"1:5千", new ScaleConfig(5000, 112.5/3600.0, 75.0/3600.0, 'H')},
            {"1:2000", new ScaleConfig(2000, 37.5/3600.0, 25.0/3600.0, 'I')},
            {"1:2千", new ScaleConfig(2000, 37.5/3600.0, 25.0/3600.0, 'I')},
            {"1:1000", new ScaleConfig(1000, 18.75/3600.0, 12.5/3600.0, 'J')},
            {"1:1千", new ScaleConfig(1000, 18.75/3600.0, 12.5/3600.0, 'J')},
            {"1:500", new ScaleConfig(500, 9.375/3600.0, 6.25/3600.0, 'K')},
            {"1:5百", new ScaleConfig(500, 9.375/3600.0, 6.25/3600.0, 'K')}
        };

        // 比例尺配置结构
        private struct ScaleConfig
        {
            public int Denominator { get; }
            public double DeltaL { get; }
            public double DeltaB { get; }
            public char Letter { get; }

            public ScaleConfig(int denominator, double deltaL, double deltaB, char letter)
            {
                Denominator = denominator;
                DeltaL = deltaL;
                DeltaB = deltaB;
                Letter = letter;
            }
        }

        // 统一的比例尺信息获取方法
        private bool GetScaleConfig(string scale, out ScaleConfig config)
        {
            return ScaleConfigs.TryGetValue(scale, out config);
        }

        public void GetMapCode(Point p)
        {
            // 1：100万分幅与编号的行号和列号
            p.Row = (int)(p.B / 4.0) + 1;
            p.Column = (int)(p.L / 6.0) + 31;

            // 给定点相对于1：100万图幅左下角的经差与纬差
            p.detaL = p.L - (int)(p.L / 6.0) * 6;
            p.detaB = p.B - (int)(p.B / 4.0) * 4;

            string rowLetter = GetRowLetter(p.Row);
            string baseMapCode1M = $"{rowLetter}{p.Column}";

            if (!GetScaleConfig(p.scale, out ScaleConfig scaleConfig))
            {
                p.oldMapCode = "不支持的比例尺";
                p.newMapCode = "不支持的比例尺";
                return;
            }

            // 计算旧式图幅编号
            p.oldMapCode = CalculateOldMapCode(p, rowLetter, scaleConfig);

            // 计算新式图幅编号
            if (scaleConfig.Denominator == 1000000)
            {
                p.newMapCode = p.oldMapCode;
            }
            else
            {
                p.newMapCode = CalculateNewMapCode(p, baseMapCode1M, scaleConfig);
            }
        }

        // 计算旧图幅编号
        private string CalculateOldMapCode(Point p, string rowLetter, ScaleConfig config)
        {
            switch (config.Denominator)
            {
                case 1000000:
                    return $"{rowLetter}{p.Column}";

                case 500000:
                    return CalculateOldMapCode500k(p, rowLetter);

                case 250000:
                    return CalculateOldMapCode250k(p, rowLetter);

                case 100000:
                    return CalculateOldMapCode100k(p, rowLetter);

                case 50000:
                    return CalculateOldMapCode50k(p, rowLetter);

                case 25000:
                    return CalculateOldMapCode25k(p, rowLetter);

                case 10000:
                    return CalculateOldMapCode10k(p, rowLetter);

                case 5000:
                    return CalculateOldMapCode5k(p, rowLetter);

                default:
                    return "不支持的比例尺";
            }
        }

        // 各比例尺的旧编号计算方法
        private string CalculateOldMapCode500k(Point p, string rowLetter)
        {
            int oldSubCode500k = 0;
            if (p.detaL < 3 && p.detaB >= 2) oldSubCode500k = 1;
            else if (p.detaL >= 3 && p.detaB >= 2) oldSubCode500k = 2;
            else if (p.detaL < 3 && p.detaB < 2) oldSubCode500k = 3;
            else if (p.detaL >= 3 && p.detaB < 2) oldSubCode500k = 4;

            return $"{rowLetter}{p.Column}{oldSubCode500k}";
        }

        private string CalculateOldMapCode250k(Point p, string rowLetter)
        {
            int r_250k = (int)((4.0 - p.detaB) / 1.0);
            int c_250k = (int)(p.detaL / 1.5);
            int oldSubCode250k = r_250k * 4 + c_250k + 1;
            return $"{rowLetter}{p.Column}{oldSubCode250k:D2}";
        }

        private string CalculateOldMapCode100k(Point p, string rowLetter)
        {
            int r_100k = (int)((4.0 - p.detaB) / (1.0 / 3.0));
            int c_100k = (int)(p.detaL / 0.5);
            int oldSubCode100k = r_100k * 12 + c_100k + 1;
            return $"{rowLetter}{p.Column}{oldSubCode100k:D3}";
        }

        private string CalculateOldMapCode50k(Point p, string rowLetter)
        {
            Map100kContext context = Get100kContext(p);

            int oldSubCode50k_suffix = GetQuadrantSuffix(context.DetaL_within_100k, context.DetaB_within_100k, 0.25, 1.0 / 6.0);
            return $"{rowLetter}{p.Column}{context.OldSubCode100k:D3}{oldSubCode50k_suffix}";
        }

        private string CalculateOldMapCode25k(Point p, string rowLetter)
        {
            Map100kContext context = Get100kContext(p);

            int _50k_suffix = GetQuadrantSuffix(context.DetaL_within_100k, context.DetaB_within_100k, 0.25, 1.0 / 6.0);
            Map50kTileContext tileContext = Get50kTileContext(p, _50k_suffix);

            double detaL_within_50k = p.detaL - tileContext.SW_Lon;
            double detaB_within_50k = p.detaB - tileContext.SW_Lat;

            int _25k_suffix = GetQuadrantSuffix(detaL_within_50k, detaB_within_50k, 0.125, 1.0 / 12.0);
            return $"{rowLetter}{p.Column}{context.OldSubCode100k:D3}{_50k_suffix}{_25k_suffix}";
        }

        private string CalculateOldMapCode10k(Point p, string rowLetter)
        {
            Map100kContext context = Get100kContext(p);

            int south_to_north_row_index_10k_sub = (int)(context.DetaB_within_100k / (1.0 / 24.0));
            south_to_north_row_index_10k_sub = Math.Max(0, Math.Min(7, south_to_north_row_index_10k_sub));
            int r_10k_sub = 7 - south_to_north_row_index_10k_sub;

            int c_10k_sub = (int)(context.DetaL_within_100k / 0.0625);
            c_10k_sub = Math.Max(0, Math.Min(7, c_10k_sub));

            int oldSubCode10k_suffix = r_10k_sub * 8 + c_10k_sub + 1;
            return $"{rowLetter}{p.Column}{context.OldSubCode100k:D3}{oldSubCode10k_suffix:D2}";
        }

        private string CalculateOldMapCode5k(Point p, string rowLetter)
        {
            Map100kContext context = Get100kContext(p);

            int r_5k_sub = (int)(context.DetaB_within_100k / (75.0 / 3600.0));
            int c_5k_sub = (int)(context.DetaL_within_100k / (112.5 / 3600.0));
            r_5k_sub = 15 - r_5k_sub;

            int oldSubCode5k_suffix = r_5k_sub * 16 + c_5k_sub + 1;
            return $"{rowLetter}{p.Column}{context.OldSubCode100k:D3}{oldSubCode5k_suffix:D3}";
        }

        // 计算新图幅编号
        private string CalculateNewMapCode(Point p, string baseMapCode1M, ScaleConfig config)
        {
            if (config.DeltaB == 0 || config.DeltaL == 0)
            {
                return "比例尺参数错误";
            }

            int new_row = (int)(4 / config.DeltaB) - (int)((p.B % 4) / config.DeltaB);
            int new_col = (int)((p.L % 6) / config.DeltaL) + 1;

            return $"{baseMapCode1M}{config.Letter}{new_row:D3}{new_col:D3}";
        }

        // 100万图幅上下文信息结构
        private struct Map100kContext
        {
            public int OldSubCode100k;
            public double DetaL_within_100k;
            public double DetaB_within_100k;

            public Map100kContext(int oldSubCode100k, double detaL_within_100k, double detaB_within_100k)
            {
                OldSubCode100k = oldSubCode100k;
                DetaL_within_100k = detaL_within_100k;
                DetaB_within_100k = detaB_within_100k;
            }
        }

        // 5万图幅上下文信息结构
        private struct Map50kTileContext
        {
            public double SW_Lon;
            public double SW_Lat;

            public Map50kTileContext(double sw_lon, double sw_lat)
            {
                SW_Lon = sw_lon;
                SW_Lat = sw_lat;
            }
        }

        // 辅助方法：获取100万图幅内的上下文信息
        private Map100kContext Get100kContext(Point p)
        {
            int r_100k = (int)((4.0 - p.detaB) / (1.0 / 3.0));
            int c_100k = (int)(p.detaL / 0.5);
            int oldSubCode100k = r_100k * 12 + c_100k + 1;

            double sw_lon_of_100k_tile = c_100k * 0.5;
            double sw_lat_of_100k_tile = (int)(p.detaB / (1.0 / 3.0)) * (1.0 / 3.0);
            double detaL_within_100k = p.detaL - sw_lon_of_100k_tile;
            double detaB_within_100k = p.detaB - sw_lat_of_100k_tile;

            return new Map100kContext(oldSubCode100k, detaL_within_100k, detaB_within_100k);
        }

        // 辅助方法：获取5万图幅的上下文信息
        private Map50kTileContext Get50kTileContext(Point p, int _50k_suffix)
        {
            Map100kContext context = Get100kContext(p);
            int c_100k = (int)(p.detaL / 0.5);
            double sw_lon_of_100k_tile = c_100k * 0.5;
            double sw_lat_of_100k_tile = (int)(p.detaB / (1.0 / 3.0)) * (1.0 / 3.0);

            double current_50k_tile_sw_lon_abs = sw_lon_of_100k_tile;
            double current_50k_tile_sw_lat_abs = sw_lat_of_100k_tile;

            switch (_50k_suffix)
            {
                case 1: current_50k_tile_sw_lat_abs += (1.0 / 6.0); break;
                case 2: current_50k_tile_sw_lon_abs += 0.25; current_50k_tile_sw_lat_abs += (1.0 / 6.0); break;
                case 3: break;
                case 4: current_50k_tile_sw_lon_abs += 0.25; break;
            }

            return new Map50kTileContext(current_50k_tile_sw_lon_abs, current_50k_tile_sw_lat_abs);
        }

        // 辅助方法：获取象限后缀
        private int GetQuadrantSuffix(double deltaL, double deltaB, double lonThreshold, double latThreshold)
        {
            if (deltaL < lonThreshold && deltaB >= latThreshold) return 1;
            else if (deltaL >= lonThreshold && deltaB >= latThreshold) return 2;
            else if (deltaL < lonThreshold && deltaB < latThreshold) return 3;
            else return 4;
        }

        // 计算并填充给定点p的邻接图幅编号
        public void CalculateBesideMaps(Point p)
        {
            if (!GetScaleConfig(p.scale, out ScaleConfig config))
            {
                p.OldBesideMapList = new string[9];
                p.BesideMapList = new string[9];
                for (int k = 0; k < 9; k++)
                {
                    p.OldBesideMapList[k] = "不支持的比例尺";
                    p.BesideMapList[k] = "不支持的比例尺";
                }
                return;
            }

            p.OldBesideMapList = new string[9];
            p.BesideMapList = new string[9];

            // 遍历九宫格 (3x3)
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    // 计算邻接图幅中心点的经纬度
                    double neighborB = p.B + (1 - i) * config.DeltaB;
                    double neighborL = p.L + (j - 1) * config.DeltaL;

                    // 创建邻接图幅的临时点对象
                    Point neighborPoint = new Point(0, neighborL, neighborB, p.scale);

                    // 为这个邻接点计算图幅编号
                    GetMapCode(neighborPoint);

                    // 将计算得到的图幅编号存入中心点p的接图表数组中
                    int index = i * 3 + j;
                    p.OldBesideMapList[index] = neighborPoint.oldMapCode;
                    p.BesideMapList[index] = neighborPoint.newMapCode;
                }
            }
        }

        private string GetRowLetter(int rowNumber)
        {
            if (rowNumber >= 1 && rowNumber <= 22)
            {
                return ((char)('A' + rowNumber - 1)).ToString();
            }
            return "";
        }

    }
}