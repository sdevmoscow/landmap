using System;
using System.Collections.Generic;

namespace LandMap.Models
{
    public partial class LandType
    {
        public LandType()
        {
            Land = new HashSet<Land>();
        }

        public LandType(Data.LandType landType)
        {
            Land = new HashSet<Land>();
            this.Id = landType.Id;
            this.Name = landType.Name;
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Land> Land { get; set; }
    }
}
