using FrameWork;
using VM.Console;
using VM.Public;

namespace VM.Melkavan
{
    public class AdvertisementOwnersVM : BaseEntity
    {
        public int AdvertisementOwnerId { get; set; }

        //مالک
        public long OwnerId { get; set; }

        public long AdvertisementId { get; set; }

        //دنگ
        public double Share { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        public string? OwnerUserFamily { get; set; }

        //نوع مالک
        //users
        //persons
        public string OwnerType { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }

        public virtual UsersProfileVM? UsersProfileVM { get; set; }
    }
}
