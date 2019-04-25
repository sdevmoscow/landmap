using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LandMap.Models
{
    public partial class Land
    {
        public Land()
        {

        }

        public Land(Data.Land land)
        {
            this.Id = land.Id;
            this.Name = land.Name;
            this.Coordinates = land.Coordinates;
            this.CadastralNumber = land.CadastralNumber;
            this.InventoryNumber = land.InventoryNumber;
            this.LandTypeId = land.LandTypeId;
            this.LandRightTypeId = land.LandRightTypeId;
        }

        [Required]
        [DisplayName("Номер")]
        public int Id { get; set; }
        [Required]
        [DisplayName("Наименование")]
        public string Name { get; set; }
        [Required]
        [DisplayName("Инвентарный номер")]
        public string InventoryNumber { get; set; }
        [Required]
        [DisplayName("Кадастровый номер")]
        public string CadastralNumber { get; set; }
        [Required]
        [DisplayName("Тип участка")]
        public int? LandTypeId { get; set; }
        [DisplayName("Вид прав")]
        [Required]
        public int? LandRightTypeId { get; set; }
        [DisplayName("Координаты")]
        public string Coordinates { get; set; }

        public string Center { get; set; }

        [DisplayName("Тип участка")]
        public string LandTypeName { get; set; }
        [DisplayName("Вид прав")]
        public string LandRightTypeName { get; set; }

    }
}
