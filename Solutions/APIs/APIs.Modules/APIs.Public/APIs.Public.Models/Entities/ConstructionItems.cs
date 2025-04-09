using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class ConstructionItems : BaseEntity
    {
        [Key]
        public long ConstructionItemId { get; set; }

        public long? ConstructionItemParentId { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ConstructionItemTitle { get; set; }

        public string ConstructionItemDesc { get; set; }

        //public double? ConstructionItemPercentValue { get; set; }

        //public long? ConstructionItemValue { get; set; }
    }
}
