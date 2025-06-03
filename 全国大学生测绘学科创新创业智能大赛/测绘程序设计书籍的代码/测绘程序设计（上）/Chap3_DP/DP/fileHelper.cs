using System;
using System.IO;
using System.Windows.Forms;

namespace DP
{
    /// <summary>
    /// 文件读取程序
    /// </summary>
    class fileHelper
    {
        public static pointEntity Read(string filename)
        {
            pointEntity oripoints = new pointEntity();//oripoints是原始所有点
            try
            {
                var reader = new StreamReader(filename);
                while(!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if(line.Length != 0)
                    {
                        var pt = new Point();
                        pt.Parse(line);//整理每一行数据
                        oripoints.Add(pt);//添加到oripoints里面
                    }
                }
                reader.Close();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
            return oripoints;

        }

    }

}