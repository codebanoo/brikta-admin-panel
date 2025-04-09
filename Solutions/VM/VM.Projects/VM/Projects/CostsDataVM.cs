using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    //لیست هزینه اختصاصی و درصد از هزینه عمومی
    //لیست پرداختی ماهیانه سرمایه گذار
    public class CostsDataVM
    {
        public long RowNumber { get; set; }

        public string Date { get; set; }

        public int? CellIndex { get; set; }

        public double? Amount { get; set; }
    }
}
