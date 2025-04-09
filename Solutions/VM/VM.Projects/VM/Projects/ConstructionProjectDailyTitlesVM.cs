using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectDailyTitlesVM : BaseEntity
    {
        public long ConstructionProjectDailyTitleId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public string CellTitle { get; set; }

        public int CellIndex { get; set; }
    }
}
