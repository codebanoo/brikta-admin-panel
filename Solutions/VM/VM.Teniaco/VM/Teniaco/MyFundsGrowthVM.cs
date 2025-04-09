using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Teniaco
{
    //رشد سرمایه در داشبورد خارجی
    [NotMapped]
    public class MyFundsGrowthVM
    {
        public long RowNumber { get; set; }

        public string? PriceType { get; set; }

        public DateTime? CreateEnDate { get; set; }

        public double? Price { get; set; }

        public long? ParentId { get; set; }

        public string? MyFundTitle { get; set; }
    }
}
