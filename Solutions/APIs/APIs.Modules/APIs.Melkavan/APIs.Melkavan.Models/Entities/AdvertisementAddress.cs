using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace APIs.Melkavan.Models.Entities
{
    public partial class AdvertisementAddress : BaseEntity
    {
        [Key]
        [ForeignKey("Advertisement")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long AdvertisementId { get; set; }

        public long? StateId { get; set; }
        public long? TempStateId { get; set; }

        public long? CityId { get; set; }
        public long? TempCityId { get; set; }

        //منطقه
        public long? ZoneId { get; set; }
        public long? TempZoneId { get; set; }

        //ناحیه
        public long? DistrictId { get; set; }
        //public long? TempDistrictId { get; set; }

        //public string? VillageName { get; set; }

        public string? TownName { get; set; }


        [StringLength(500)]
        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public virtual Advertisement Advertisement { get; set; }
    }
}
