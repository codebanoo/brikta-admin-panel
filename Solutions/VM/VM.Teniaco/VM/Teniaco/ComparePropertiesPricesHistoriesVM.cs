using FrameWork;

namespace VM.Teniaco
{
    public class ComparePropertiesPricesHistoriesVM : BaseEntity
    {
        public long PropertyId { get; set; }
        public long? OfferPrice { get; set; }
        public int? OfferPriceType { get; set; }
        public long? CalculatedOfferPrice { get; set; }
        public int? PriceTypeRegister { get; set; }
    }
}
