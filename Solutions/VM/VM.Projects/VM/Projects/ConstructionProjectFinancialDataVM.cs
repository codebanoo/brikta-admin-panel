using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectFinancialDataVM : BaseEntity
    {
        public long ConstructionProjectFinancialDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long? ParentId { get; set; }//ConstructionProjectId, UserId

        public string ParentType { get; set; }//persons, constructionproject

        public int RowIndex { get; set; }

        public int CellIndex { get; set; }

        public string CellData { get; set; }

        public string RecordType { get; set; }//public -- عمومی, private -- اختصاصی, shareholder -- سهامدار
    }
}
