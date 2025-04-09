using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAllAdvertisementFeatureValuesPVM : BPVM
    {
        public int? AdvertisementId { get; set; }

        public int PropertyTypeId { get; set; }

        public int AdvertisementTypeId { get; set; }
    }
}
