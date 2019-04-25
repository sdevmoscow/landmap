using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LandMap.Models
{
    public class LandModel
    {
        public IEnumerable<LandMap.Models.Land> Lands { get; set; }
        public LandMap.Models.Land Selected { get; set; }
    }
}
