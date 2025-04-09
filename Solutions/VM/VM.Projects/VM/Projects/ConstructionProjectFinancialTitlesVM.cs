using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectFinancialTitlesVM : BaseEntity
    {
        public long ConstructionProjectFinancialTitleId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public string CellTitle { get; set; }

        public int CellIndex { get; set; }

        public string RecordType { get; set; }//public -- عمومی, private -- اختصاصی, shareholder -- سهامدار
    }
}
