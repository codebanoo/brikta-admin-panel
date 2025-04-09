using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class ConstructionItemPricesHistoriesVM : BaseEntity
    {
        public long ConstructionItemPricesHistoryId { get; set; }

        public string ParentType { get; set; }//ConstructionItems(item) or ConstructionSubItems(subItem)

        public long ParentId { get; set; }

        public double? ItemPercentValue { get; set; }

        public long? ItemValue { get; set; }
    }
}
