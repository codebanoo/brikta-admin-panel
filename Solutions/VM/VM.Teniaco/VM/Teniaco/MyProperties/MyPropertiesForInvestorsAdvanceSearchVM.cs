using FrameWork;
using System.ComponentModel.DataAnnotations.Schema;

namespace VM.Teniaco
{
    [NotMapped]
    public class MyPropertiesForInvestorsAdvanceSearchVM
    {
        public string? RecordType { get; set; }
        public long PropertyId { get; set; }
        public string? PropertyTypeTilte { get; set; }
        public int? PropertyTypeId { get; set; }
        public string? TypeOfUseTitle { get; set; }
        public string? DocumentTypeTitle { get; set; }
        public string? DocumentOwnershipTypeTitle { get; set; }
        public string? DocumentRootTypeTitle { get; set; }
        public string? PropertyCodeName { get; set; }
        public string? Area { get; set; }
        public string? PropertyDescriptions { get; set; }
        public long? OwnerId { get; set; }
        public string? StateName { get; set; }
        public long? StateId { get; set; }
        public string? CityName { get; set; }
        public long? CityId { get; set; }
        public string? ZoneName { get; set; }
        public long? ZoneId { get; set; }
        public string? DistrictName { get; set; }
        public long? DistrictId { get; set; }
        public string? Address { get; set; }
        public string? Foundation { get; set; }
        public long? UserIdCreator { get; set; }
        public DateTime? CreateEnDate { get; set; }
        public int? RebuiltInYearFa { get; set; }
        public DateTime? EditEnDate { get; set; }
        public string RegisterOrEditDate
        {
            get
            {
                if (EditEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(EditEnDate.Value);
                }
                else if (CreateEnDate.HasValue)
                {
                    return PersianDate.PersianDateFrom(CreateEnDate.Value);
                }
                else
                    return "تاریخ ندارد";
            }
        }
        public bool? Participable { get; set; }
        public bool? Exchangeable { get; set; }
        [NotMapped]
        public List<PropertyOwnersVM>? PropertyOwnersVM { get; set; }
        [NotMapped]
        public long? LastPrice { get; set; }
        [NotMapped]
        public List<PropertyFilesVM>? Photos { get; set; }
        public string? BuildingLifeTitle { get; set; }

        public double? SharePercent { get; set; }
        public long? TotalPrice { get; set; }
        public long? PricePerMeter { get; set; }
        // فیلد های لوکیشن
        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }
    }
}
