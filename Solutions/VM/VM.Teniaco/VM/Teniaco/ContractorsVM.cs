using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace VM.Teniaco
{
    public class ContractorsVM : BaseEntity
    {
        public int ContractorId { get; set; }
        public string ContractorName { get; set; }
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
        public string? Site { get; set; }

        public string? SocialNetworks { get; set; }

        public int? GuildCategoryId { get; set; }
    }
}
