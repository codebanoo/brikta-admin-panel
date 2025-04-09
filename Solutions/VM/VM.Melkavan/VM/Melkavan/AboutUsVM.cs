using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Teniaco;


namespace VM.Melkavan
{
    public class AboutUsVM : BaseEntity
    {

        public int? countOfAgencies { get; set; }

        public int? countOfAgencyStaffs{ get; set; }

        public int? countOfAdvertisements { get; set; }

        public int? countOfOwners { get; set; }

        
        
    }
}
