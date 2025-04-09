using FrameWork;
using System.Collections.Generic;

namespace VM.Projects
{
    public class FinancialDetailsDataVM : BaseEntity
    {
        public List<RowsDataVM> RowsDataVM { get; set; }

        public List<HeaderMonthsVM> HeaderMonthsVM { get; set; }

        public List<RowsDataVM>? TotalPublicCost { get; set; }
    }
}
