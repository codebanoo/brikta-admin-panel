using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Projects.Models.Entities
{
    public class ConstructionProjectDailyData : BaseEntity
    {
        [Key]
        public long ConstructionProjectDailyDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long ConstructionProjectId { get; set; }

        //public string CellTitle { get; set; }

        public int CellIndex { get; set; }

        public string CellData { get; set; }

        public int RowIndex { get; set; }
    }
}
