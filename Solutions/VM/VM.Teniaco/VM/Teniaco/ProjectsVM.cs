using FrameWork;

namespace VM.Teniaco
{
    public class ProjectsVM : BaseEntity
    {
        public ProjectsVM()
        {
            InvestorsRequestsVM = new HashSet<InvestorsRequestsVM>();
            Projects1VM = new HashSet<ProjectsVM>();
            ProposedProjectsVM = new HashSet<ProposedProjectsVM>();
        }

        public int ProjectId { get; set; }

        public int? ParentProjectId { get; set; }

        public long PropertyId { get; set; }

        public int ProjectTypeId { get; set; }

        public string ProjectTitle { get; set; }

        public int? ProjectStateId { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<InvestorsRequestsVM> InvestorsRequestsVM { get; set; }

        public virtual ICollection<ProjectsVM> Projects1VM { get; set; }

        public virtual ProjectsVM Projects2VM { get; set; }

        public virtual ProjectStatesVM ProjectStatesVM { get; set; }

        public virtual ProjectTypesVM ProjectTypesVM { get; set; }

        public virtual PropertiesVM PropertiesVM { get; set; }

        public virtual ICollection<ProposedProjectsVM> ProposedProjectsVM { get; set; }
    }
}
