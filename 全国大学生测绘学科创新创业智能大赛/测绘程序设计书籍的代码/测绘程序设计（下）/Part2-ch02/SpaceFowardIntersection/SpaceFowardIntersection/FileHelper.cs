using System;
using System.Text.RegularExpressions;

namespace SpaceFowardIntersection
{
    class FileHelper
    {
        public static Data Read(string line)
        {
            var buf = Regex.Split(line, @"/s+");
            var data = new Data(buf);
            return data;
        }
    }
}