using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class RowsDataVM : BaseEntity
    {
        public int RowIndex { get; set; }
        public int CellIndex { get; set; }
        public string CellData { get; set; }
        public long? ParentId { get; set; }
    }
}
