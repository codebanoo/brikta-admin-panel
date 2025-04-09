using FrameWork;

namespace VM.Projects
{
    public class ConstructionProjectRepresentativesVM : BaseEntity
    {

        public long ConstructionProjectRepresentativeId { get; set; }

        //نماینده
        public long RepresentativeId { get; set; }


        public string? RepresentativeFamiy { get; set; }

        public string? RepresentativeName { get; set; }


        //سهامدار
        public long OwnerUserId { get; set; }

        //پروژه
        public long ConstructionProjectId { get; set; }

        public virtual ConstructionProjectsVM ConstructionProjectsVM { get; set; }

        public string? LastProgessUpdateDate { get; set; }

    }
}
