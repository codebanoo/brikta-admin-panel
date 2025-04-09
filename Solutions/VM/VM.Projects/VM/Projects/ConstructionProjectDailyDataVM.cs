using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectDailyDataVM : BaseEntity
    {
        public long ConstructionProjectDailyDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long ConstructionProjectId { get; set; }

        public int RowIndex { get; set; }

        public int CellIndex { get; set; }

        public string CellData { get; set; }
    }
}
