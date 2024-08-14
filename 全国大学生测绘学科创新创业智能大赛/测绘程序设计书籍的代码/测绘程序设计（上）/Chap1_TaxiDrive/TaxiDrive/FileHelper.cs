using System;
using System.Collections.Generic;//List范型
using System.IO;

namespace TaxiDrive
{
    /// <summary>
    /// 
    /// 1.读文件
    /// 2.保存计算结果
    /// 
    /// </summary>
    
    class FileHelper
    {
        public static List<Epoch> Read(string Id,string pathname)
        {
            var data = new List<Epoch>();
            try
            {
                var reader = new StreamReader(pathname);//读文件
                reader.ReadLine();//读第一行，第一行没用
                while(!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    if(line.Length > 0)
                    {
                        var ep = new Epoch();
                        ep.Parse(line);
                        if (Id.Equals(ep.Id))
                        {
                            data.Add(ep);
                        }
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
        public static void Write(SessionList data, string filename)//保存文件
        {
            var writer = new StreamWriter(filename);
            writer.Write(data.ToString());
            writer.Close();
        }
    }
}