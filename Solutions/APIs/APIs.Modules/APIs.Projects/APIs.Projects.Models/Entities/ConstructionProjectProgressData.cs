using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectProgressData : BaseEntity
    {
        [Key]
        public long ConstructionProjectProgressDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long? ConstructionProjectId { get; set; }//ConstructionProjectId

        public string CellData { get; set; }

        public int CellIndex { get; set; }

        public int RowIndex { get; set; }
    }
}
