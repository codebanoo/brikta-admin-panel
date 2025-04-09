using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class ConstructionSubItems : BaseEntity
    {
        [Key]
        public long ConstructionSubItemId { get; set; }

        public long ConstructionItemId { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ConstructionSubItemTitle { get; set; }

        public string ConstructionSubItemDesc { get; set; }

        //public double? ConstructionSubItemPercentValue { get; set; }

        //public long? ConstructionSubItemValue { get; set; }
    }
}
