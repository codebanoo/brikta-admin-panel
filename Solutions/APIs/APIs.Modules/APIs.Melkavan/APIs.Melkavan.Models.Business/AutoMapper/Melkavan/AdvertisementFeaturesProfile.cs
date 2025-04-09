using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementFeaturesProfile : Profile
    {
        public AdvertisementFeaturesProfile()
        {
            CreateMap<AdvertisementFeatures, AdvertisementFeaturesVM>();
            CreateMap<AdvertisementFeaturesVM, AdvertisementFeatures>();
        }
    }
}
