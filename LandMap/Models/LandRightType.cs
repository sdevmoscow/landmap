using System;
using System.Collections.Generic;

namespace LandMap.Models
{
    public partial class LandRightType
    {
        public LandRightType()
        {
            Land = new HashSet<Land>();
        }

        public LandRightType(Data.LandRightType landRightType)
        {
            Land = new HashSet<Land>();
            this.Id = landRightType.Id;
            this.Name = landRightType.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Land> Land { get; set; }
    }
}
