using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementPricesHistoriesProfile : Profile
    {
        public AdvertisementPricesHistoriesProfile()
        {
            CreateMap<AdvertisementPricesHistories, AdvertisementPricesHistoriesVM>();
            CreateMap<AdvertisementPricesHistoriesVM, AdvertisementPricesHistories>();
        }
    }
}
