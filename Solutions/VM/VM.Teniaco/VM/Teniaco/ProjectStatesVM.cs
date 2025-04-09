using FrameWork;

namespace VM.Teniaco
{
    public class ProjectStatesVM : BaseEntity
    {
        public ProjectStatesVM()
        {
            //ProjectsVM = new HashSet<ProjectsVM>();
        }

        public int ProjectStateId { get; set; }

        public string ProjectStateTitle { get; set; }

        public string ProjectStateColor { get; set; }

        //public virtual ICollection<ProjectsVM> ProjectsVM { get; set; }
    }
}
