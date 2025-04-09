using FrameWork;
using VM.Console;
using VM.Public;

namespace VM.Projects
{
    public class ConstructionProjectOwnerUsersVM : BaseEntity
    {
        public long ConstructionProjectOwnerUserId { get; set; }

        //مالک
        public long OwnerUserId { get; set; }

        public string? OwnerUserFamiy { get; set; }

        public string? UserName { get; set; }

        public long ConstructionProjectId { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }
    }
}
