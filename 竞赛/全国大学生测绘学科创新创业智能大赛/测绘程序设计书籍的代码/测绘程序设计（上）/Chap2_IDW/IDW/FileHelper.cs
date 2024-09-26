using System;
using System.IO;//streamreader的空间

namespace IDW
{
    class FileHelper
    {
        public static DataEntity Read(string pathname)
        {
            DataEntity data = new DataEntity();
            try
            {
                var reader = new StreamReader(pathname);
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if(line.Length > 0)
                    {
                        var pt = new Point();
                        pt.Parse(line);
                        data.Add(pt);
                        
                    }
                }

                reader.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return data;
        }
    }
}