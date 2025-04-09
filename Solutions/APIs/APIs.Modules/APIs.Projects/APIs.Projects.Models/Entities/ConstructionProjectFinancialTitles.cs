using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectFinancialTitles : BaseEntity
    {
        [Key]
        public long ConstructionProjectFinancialTitleId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public string CellTitle { get; set; }

        public int CellIndex { get; set; }

        public string RecordType { get; set; }//public -- عمومی, private -- اختصاصی, shareholder -- سهامدار
    }
}
