using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandMap
{
    public class Helper
    {
        public static string DoubleToJson((double x, double y) coords)
        {
            return $"[{coords.x.ToString().Replace(',','.')},{coords.y.ToString().Replace(',', '.')}]";
        }
    }
}
