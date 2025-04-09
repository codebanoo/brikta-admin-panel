using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Public.Models.Entities
{
    public partial class ConstructionItemPricesHistories : BaseEntity
    {
        [Key]
        public long ConstructionItemPricesHistoryId { get; set; }

        public string ParentType { get; set; }//ConstructionItems(item) or ConstructionSubItems(subItem)

        public long ParentId { get; set; }

        public double? ItemPercentValue { get; set; }

        public long? ItemValue { get; set; }
    }
}
