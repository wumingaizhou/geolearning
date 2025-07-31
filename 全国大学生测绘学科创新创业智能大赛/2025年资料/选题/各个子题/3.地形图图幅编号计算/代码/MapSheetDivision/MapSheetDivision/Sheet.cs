using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MapSheetDivision
{
    public class Sheet
    {
        // 经差和纬差
        public class LatLonDiff
        {
            public double LatDiff { get; set; }
            public double LonDiff { get; set; }

            public LatLonDiff(double latDiff, double lonDiff)
            {
                LatDiff = latDiff;
                LonDiff = lonDiff;
            }
        }

        const double base_sheet = -180.0;

        // 图廓点结构
        public struct CornerPoints
        {
            // 1: 西南, 2: 东南, 3: 东北, 4: 西北
            public double Point1Lon;
            public double Point1Lat;
            public double Point2Lon;
            public double Point2Lat;
            public double Point3Lon;
            public double Point3Lat;
            public double Point4Lon;
            public double Point4Lat;
        }

        // 比例尺
        private static readonly Dictionary<char, int> ScaleCodeToDenominator = new Dictionary<char, int>
        {
            {'B', 500000}, {'C', 250000}, {'D', 100000}, {'E', 50000},
            {'F', 25000}, {'G', 10000}, {'H', 5000}, {'I', 2000},
            {'J', 1000}, {'K', 500}
        };

        // 比例尺分母到经差纬差
        private static readonly Dictionary<int, LatLonDiff> ScaleToDiff =
            new Dictionary<int, LatLonDiff>
            {
                {500000, new LatLonDiff(2.0, 3.0)},
                {250000, new LatLonDiff(1.0, 1.5)},
                {100000, new LatLonDiff(20.0/60.0, 30.0/60.0)},
                {50000, new LatLonDiff(10.0/60.0, 15.0/60.0)},
                {25000, new LatLonDiff(5.0/60.0, 7.5/60.0)},
                {10000, new LatLonDiff(2.0/60.0, 3.0/60.0)},
                {5000, new LatLonDiff(1.0/60.0, 1.5/60.0)},
                {2000, new LatLonDiff(0.4/60.0, 0.6/60.0)},
                {1000, new LatLonDiff(0.2/60.0, 0.3/60.0)},
                {500, new LatLonDiff(0.1/60.0, 0.15/60.0)}
            };

        // 根据新图幅编号计算图廓点经纬度
        public CornerPoints CalculateCornerPoints(string sheetCode)
        {
            CornerPoints points = new CornerPoints();

            if (string.IsNullOrEmpty(sheetCode) || sheetCode.Length < 3)
            {
                return points;
            }

            try
            {
                // 1. 1:100万图幅行列号
                char millionRowChar = sheetCode[0];
                if (millionRowChar < 'A' || millionRowChar > 'V')
                {
                    return points;
                }
                int millionRowIndex = millionRowChar - 'A' + 1;

                string millionColStr = "";
                int scaleCodeIndex = 3;
                if (sheetCode.Length >= 4 && char.IsDigit(sheetCode[1]) && char.IsDigit(sheetCode[2]))
                {
                    millionColStr = sheetCode.Substring(1, 2);
                    scaleCodeIndex = 3;
                }
                else if (sheetCode.Length >= 5 && char.IsDigit(sheetCode[1]) && char.IsDigit(sheetCode[2]) && char.IsDigit(sheetCode[3]))
                {
                    millionColStr = sheetCode.Substring(1, 2);
                    scaleCodeIndex = 3;
                }
                else
                {
                    return points;
                }

                if (!int.TryParse(millionColStr, out int millionColIndex) || millionColIndex < 1 || millionColIndex > 60)
                {
                    return points;
                }

                // 2. 解析比例尺代码
                if (scaleCodeIndex >= sheetCode.Length)
                {
                    return points;
                }
                char scaleCodeChar = sheetCode[scaleCodeIndex];
                if (!ScaleCodeToDenominator.TryGetValue(scaleCodeChar, out int scaleDenominator))
                {
                    return points;
                }


                // 3. 解析本比例尺行列号
                int rowDigits = (scaleDenominator == 1000 || scaleDenominator == 500) ? 4 : 3;
                int colDigits = rowDigits;
                int remainingLength = sheetCode.Length - scaleCodeIndex - 1;

                if (remainingLength != rowDigits + colDigits)
                {
                    return points;
                }

                string rowStr = sheetCode.Substring(scaleCodeIndex + 1, rowDigits);
                string colStr = sheetCode.Substring(scaleCodeIndex + 1 + rowDigits, colDigits);

                if (!int.TryParse(rowStr, out int rowIndex) || !int.TryParse(colStr, out int colIndex))
                {
                    return points;
                }

                if (rowIndex < 1 || colIndex < 1)
                {
                    return points;
                }

                // 4. 计算1:100万图幅西南图廓点经纬度
                double millionSWLat = (millionRowIndex - 1) * 4.0;
                double millionSWLon = base_sheet + (millionColIndex - 1) * 6.0;

                // 5. 获取比例尺经差纬差
                if (!ScaleToDiff.TryGetValue(scaleDenominator, out LatLonDiff diff))
                {
                    return points;
                }
                double latDiff = diff.LatDiff;
                double lonDiff = diff.LonDiff;

                // 6. 计算本比例尺图幅西南图廓点经纬度 
                double swLat = millionSWLat + (rowIndex - 1) * latDiff;
                double swLon = millionSWLon + (colIndex - 1) * lonDiff;

                // 7. 计算其他图廓点
                double seLat = swLat;
                double seLon = swLon + lonDiff;
                double neLat = swLat + latDiff;
                double neLon = swLon + lonDiff;
                double nwLat = swLat + latDiff;
                double nwLon = swLon;

                // 8. 填充结果
                points.Point1Lon = swLon;
                points.Point1Lat = swLat;
                points.Point2Lon = seLon;
                points.Point2Lat = seLat;
                points.Point3Lon = neLon;
                points.Point3Lat = neLat;
                points.Point4Lon = nwLon;
                points.Point4Lat = nwLat;

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return points;
        }

        // 辅助方法：将度分秒(dd.mmss)格式字符串转换为度(double)
        public static double DmsToDecimal(string dmsStr)
        {
            if (string.IsNullOrWhiteSpace(dmsStr)) return 0.0;
            string[] parts = dmsStr.Split('.');
            if (parts.Length != 2) return double.Parse(dmsStr);

            int degrees = int.Parse(parts[0]);
            string decimalMinutes = parts[1].PadRight(4, '0');

            int minutes = int.Parse(decimalMinutes.Substring(0, 2));
            double seconds = double.Parse(decimalMinutes.Substring(2));

            double decimalDegrees = Math.Abs(degrees) + minutes / 60.0 + seconds / 3600.0;
            return degrees < 0 ? -decimalDegrees : decimalDegrees;
        }

        // 辅助方法：将度(double)转换为度分秒(dd.mmss)格式字符串
        public static string DecimalToDms(double decimalDegrees, int precision = 5)
        {
            int sign = Math.Sign(decimalDegrees);
            double absDecimalDegrees = Math.Abs(decimalDegrees);

            int degrees = (int)absDecimalDegrees;
            double fractionalMinutes = (absDecimalDegrees - degrees) * 60.0;
            int minutes = (int)fractionalMinutes;
            double seconds = (fractionalMinutes - minutes) * 60.0;

            string secondsStr = seconds.ToString("00.00000").Replace(".", "");
            if (secondsStr.Length > 5)
            {
                secondsStr = (Math.Round(seconds, 3)).ToString("00.000").Replace(".", "");
                if (secondsStr.Length > 5)
                {
                    if (seconds >= 60.0)
                    {
                        seconds = 0;
                        minutes += 1;
                        if (minutes >= 60)
                        {
                            minutes = 0;
                            degrees += 1;
                        }
                        secondsStr = "00000";
                    }
                    else
                    {
                        secondsStr = secondsStr.Substring(0, 5);
                    }
                }
            }
            else if (secondsStr.Length < 5)
            {
                secondsStr = secondsStr.PadRight(5, '0');
            }
            string dmsStr = $"{sign * degrees}.{minutes:D2}{secondsStr}";
            return dmsStr;
        }

    }
}