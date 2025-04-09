using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class ConstructionSubItemsVM : BaseEntity
    {
        public long ConstructionSubItemId { get; set; }

        public long ConstructionItemId { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ConstructionItemTitle { get; set; }

        public string ConstructionSubItemTitle { get; set; }

        public string ConstructionSubItemDesc { get; set; }

        public string? NewConstructionSubItemDesc
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ConstructionSubItemDesc))
                {
                    if (this.ConstructionSubItemDesc.Length > 50)
                        return this.ConstructionSubItemDesc.Substring(0, 50);
                    else
                        return this.ConstructionSubItemDesc;
                }
                else { return string.Empty; }
            }
        }

        public long? LastItemValue { get; set; }

        //public double? ConstructionSubItemPercentValue { get; set; }

        //public long? ConstructionSubItemValue { get; set; }
    }
}
