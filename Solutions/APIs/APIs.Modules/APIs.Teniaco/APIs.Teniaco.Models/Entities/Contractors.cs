using FrameWork;
using System.ComponentModel.DataAnnotations;

namespace APIs.Teniaco.Models.Entities
{
    public partial class Contractors : BaseEntity
    {
        [Key]
        public int ContractorId { get; set; }

        public string ContractorName { get; set; }

        public string Telephone { get; set; }

        public long StateId { get; set; }

        public long CityId { get; set; }

        public long? ZoneId { get; set; }

        public string? Address { get; set; }

        public string? Site { get; set; }

        public string? SocialNetworks { get; set; }
        public int? GuildCategoryId { get; set; }

    }
}
