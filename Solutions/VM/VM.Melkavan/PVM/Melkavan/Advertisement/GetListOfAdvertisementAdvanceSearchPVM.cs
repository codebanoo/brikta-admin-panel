using System.Collections.Generic;
using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetListOfAdvertisementAdvanceSearchPVM : BPVM
    {

        public string? PropertyCodeName { get; set; }
        public string? AdvertisementTitle { get; set; }

        public bool? OnlyFavorite { get; set; }
        // Properties for Advanced Filters
        public bool IsFiltered { get; set; } = false;
        public int? AdvertisementTypeId { get; set; }
        public int? PropertyTypeId { get; set; }
        public string? TypeOfUseId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public long? ZoneId { get; set; }
        public string? TownName { get; set; }
        public string? FromArea { get; set; }
        public string? ToArea { get; set; }
        public int? FromFoundation { get; set; }
        public int? ToFoundation { get; set; }
        public long? FromPrice { get; set; }
        public long? ToPrice { get; set; }
        public string? BuildingLifeId { get; set; }
        public int? FromRebuiltInYear { get; set; }
        public int? ToRebuiltInYear { get; set; }
        public int? DocumentTypeId { get; set; }
        public int? DocumentRootTypeId { get; set; }
        public int? DocumentOwnershipTypeId { get; set; }
        public long? DepositFromPrice { get; set; }
        public long? DepositToPrice { get; set; }
        public long? RentFromPrice { get; set; }
        public long? RentToPrice { get; set; }
        public int? MaritalStatusId { get; set; }
        public string? Convertable { get; set; }
        public Dictionary<string, string>? Features { get; set; }
    }
}
