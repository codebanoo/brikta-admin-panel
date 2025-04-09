using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectFinancialData : BaseEntity
    {
        [Key]
        public long ConstructionProjectFinancialDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long? ParentId { get; set; }//ConstructionProjectId, UserId

        public string ParentType { get; set; }//persons, constructionproject

        public string CellData { get; set; }

        public int CellIndex { get; set; }

        public int RowIndex { get; set; }

        public string RecordType { get; set; }//public -- عمومی, private -- اختصاصی, shareholder -- سهامدار
    }
}
