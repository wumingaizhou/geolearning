using System;
using System.IO;
using System.Collections.Generic;
namespace CalArea
{
    class fileHelper
    {
        public static List<Point> Read(string pathname)
        {
            var reader = new StreamReader(pathname);
            reader.ReadLine();
            var pts = new List<Point>();
            while(!reader.EndOfStream)
            {
                Point pt = new Point();
                var line = reader.ReadLine();
                pt.Parse(line);
                pts.Add(pt);
            }
            reader.Close();
            return pts;
        }
    }

}