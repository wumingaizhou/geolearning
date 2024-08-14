using System;

namespace Trop
{
    class FileHelper
    {
        public static Satellite Read(string line)
        {
            try
            {
                var satellite = new Satellite(line);
                return satellite;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}