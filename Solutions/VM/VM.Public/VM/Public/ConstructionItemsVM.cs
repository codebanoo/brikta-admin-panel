using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Public
{
    public class ConstructionItemsVM : BaseEntity
    {
        public long ConstructionItemId { get; set; }

        public long? ConstructionItemParentId { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public string ConstructionItemTitle { get; set; }

        public string ConstructionItemDesc { get; set; }

        public string? NewConstructionItemDesc
        {
            get
            {
                if (!string.IsNullOrEmpty(this.ConstructionItemDesc))
                {
                    if (this.ConstructionItemDesc.Length > 50)
                        return this.ConstructionItemDesc.Substring(0, 50);
                    else
                        return this.ConstructionItemDesc;
                }
                else { return string.Empty; }
            }
        }

        public long? LastItemValue { get; set; }

        public DateTime? LastCreateEnDateItemValue { get; set; }

        public string PersianRegisterDate
        { 
            get 
            {
                if (LastCreateEnDateItemValue.HasValue)
                {
                    return PersianDate.PersianDateFrom(LastCreateEnDateItemValue.Value);
                }
                else
                    //return "تاریخ ندارد";
                    return "";
            }
        }

        //public double? ConstructionItemPercentValue { get; set; }

        //public long? ConstructionItemValue { get; set; }
    }
}
