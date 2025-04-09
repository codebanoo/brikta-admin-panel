using FrameWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Projects
{
    public class ConstructionProjectDailyDataForOuterDashbordVM : BaseEntity
    {
        public DateTime RecordDate { get; set; }
        public string PersianRecordDate { get; set; }
        public string DescriptionOfOperation { get; set; }
        public string? Progress { get; set; }
        public string? OperatorApprove { get; set; }
        public string? WorkshopSupervisorSignatureApprove { get; set; }
        public string? ProjectControlAndRegistrationApprove { get; set; }
    }
}
