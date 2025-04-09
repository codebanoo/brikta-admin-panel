using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectProgressDataVM : BaseEntity
    {
        public long ConstructionProjectProgressDataId { get; set; }

        public long ExcelSheetConfigId { get; set; }

        public long? ConstructionProjectId { get; set; }//ConstructionProjectId

        public string CellData { get; set; }

        public int CellIndex { get; set; }

        public int RowIndex { get; set; }
    }
}
