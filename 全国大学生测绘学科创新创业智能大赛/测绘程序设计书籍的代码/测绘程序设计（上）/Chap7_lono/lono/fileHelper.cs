using System;
using System.Collections.Generic;

namespace lono
{
    class fileHelper
    {
        
        public static satellite Read(string line,Time time)
        {
            satellite Satellite = new satellite(time,line);
            return Satellite;
        }
    }
}