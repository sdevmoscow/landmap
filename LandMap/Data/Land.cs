using System;
using System.Collections.Generic;

namespace LandMap.Data
{
    public partial class Land
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string InventoryNumber { get; set; }
        public string CadastralNumber { get; set; }
        public int? LandTypeId { get; set; }
        public int? LandRightTypeId { get; set; }
        public string Coordinates { get; set; }

        public LandRightType LandRightType { get; set; }
        public LandType LandType { get; set; }
    }
}
