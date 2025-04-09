using FrameWork;
using VM.Console;
using VM.Public;

namespace VM.Teniaco
{
    public class PropertyOwnersVM : BaseEntity
    {
        public int PropertyOwnerId { get; set; }

        //مالک
        public long OwnerId { get; set; }

        public string? OwnerPersonFamily { get; set; }

        public string? OwnerUserFamily { get; set; }
        public long PropertyId { get; set; }

        //دنگ
        public double Share { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        //نوع مالک
        //users
        //persons
        public string OwnerType { get; set; }
        public virtual PersonsVM? OwnerPersons { get; set; }
        public virtual CustomUsersVM? OwnerUsers { get; set; }
    }
}
