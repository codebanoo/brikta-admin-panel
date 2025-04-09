using FrameWork;
using VM.Console;

namespace VM.Teniaco
{
    public class AgencyStaffsVM : BaseEntity
    {
        public int AgencyStaffsId { get; set; }

        public int AgencyId { get; set; }

        public long? UserId { get; set; }
        public int PositionId { get; set; }
        public string? SocialNetworks { get; set; }

        public long ParentId { get; set; }

        public virtual AgenciesVM? AgenciesVM { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }

        public virtual PositionsVM? PositionsVM { get; set; }


        #region comments

        //public long PersonId { get; set; }
        //public virtual PersonsVM? PersonsVM { get; set; }


        #endregion

    }
}
