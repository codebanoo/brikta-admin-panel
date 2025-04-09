using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementCallersProfile : Profile
    {
        public AdvertisementCallersProfile()
        {
            CreateMap<AdvertisementCallers, AdvertisementCallersVM>();
            CreateMap<AdvertisementCallersVM, AdvertisementCallers>();
        }
    }
}
