using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAllAdvertisementListPVM : BPVM
    {
        public int? AdvertisementTypeId { get; set; }
        public int? PropertyTypeId { get; set; }

        public string PropertyCodeName { get; set; }

        public int? TypeOfUseId { get; set; }

        public int? DocumentTypeId { get; set; }

        //public int? DocumentOwnershipTypeId { get; set; }

        //public int? DocumentRootTypeId { get; set; }

        public string? NeighborhoodName { get; set; }

        public string Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        //public long? IntermediaryPersonId { get; set; }

        public long? ConsultantUserId { get; set; }
        public long? OwnerId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        public long? ZoneId { get; set; }

        public long? DistrictId { get; set; }

        //public string Intermediary { get; set; }//نام/شماره تماس واسطه

        //public bool? IsPrivate { get; set; }
    }
}
