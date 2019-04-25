using System;
using System.Collections.Generic;

namespace LandMap.Data
{
    public partial class LandType
    {
        public LandType()
        {
            Land = new HashSet<Land>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Land> Land { get; set; }
    }
}
