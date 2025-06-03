using System;
using System.IO;
using System.Collections.Generic;
namespace TimeTransform
{
    class fileHelper
    {
        public static List<Time> Read(string pathname)
        {
            var timeData = new List<Time>(); 
            var reader = new StreamReader(pathname);
            while(!reader.EndOfStream)
            {
                var time = new Time();
                var line = reader.ReadLine();
                time.Parse(line);
                timeData.Add(time);
            }
            reader.Close();
            return timeData;
        }
    }
}