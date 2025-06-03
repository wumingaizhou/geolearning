using System;
using System.Text.RegularExpressions;
namespace Dadixian
{
    class FileHelper
    {
        public static Data Read(string line)
        {
            var buf = Regex.Split(line, @"\s+");
            Data data = new Data(buf);
            return data;
        }
    }
}