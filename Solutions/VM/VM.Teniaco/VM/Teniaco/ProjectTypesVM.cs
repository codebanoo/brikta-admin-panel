using FrameWork;

namespace VM.Teniaco
{
    public class ProjectTypesVM : BaseEntity
    {
        public ProjectTypesVM()
        {
            ProjectsVM = new HashSet<ProjectsVM>();
        }

        public int ProjectTypeId { get; set; }

        public string ProjectTypeTitle { get; set; }

        public virtual ICollection<ProjectsVM> ProjectsVM { get; set; }
    }
}
