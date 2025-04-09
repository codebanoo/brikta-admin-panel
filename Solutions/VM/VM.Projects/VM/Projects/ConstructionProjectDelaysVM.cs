using FrameWork;

namespace VM.Projects
{
    public class ConstructionProjectDelaysVM : BaseEntity
    {
        public long ConstructionProjectsDelayId { get; set; }
        public long ConstructionProjectId { get; set; }

        public string ConstructionProjectsDelayDates { get; set; }
        public int ConstructionProjectBillDelayId { get; set; }

        public string? ConstructionProjectBillDelayTitle { get; set; }
        public string? ConstructionProjectTitle { get; set; }
    }
}
