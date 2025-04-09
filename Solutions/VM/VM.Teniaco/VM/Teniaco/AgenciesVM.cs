using FrameWork;
using System.ComponentModel.DataAnnotations;
using VM.Console;


namespace VM.Teniaco
{
    public class AgenciesVM : BaseEntity
    {
        public int AgencyId { get; set; }

        public string AgencyName { get; set; }

        public string? Telephone { get; set; }

        [Required(ErrorMessage = "استان را انتخاب کنید")]
        public long StateId { get; set; }

        public string? StateName { get; set; }


        [Required(ErrorMessage = "شهر را انتخاب کنید")]
        public long CityId { get; set; }

        public string? CityName { get; set; }

        public long? ZoneId { get; set; }
        public string? ZoneName { get; set; }

        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public string? Site { get; set; }

        public string? SocialNetworks { get; set; }

        public virtual CustomUsersVM? CustomUsersVM { get; set; }

        public List<AgencyStaffsVM> AgencyStaffsVMList { get; set; }
    }
}
