using FrameWork;

namespace VM.Teniaco
{
    public class InvestorsRequestsVM : BaseEntity
    {
        public int InvestorRequestId { get; set; }

        public int InvestorId { get; set; }

        public int ProjectId { get; set; }

        public int? ProposedProjectId { get; set; }

        public virtual InvestorsVM InvestorsVM { get; set; }

        //public virtual ProjectsVM ProjectsVM { get; set; }

        public virtual ProposedProjectsVM ProposedProjectsVM { get; set; }
    }
}
