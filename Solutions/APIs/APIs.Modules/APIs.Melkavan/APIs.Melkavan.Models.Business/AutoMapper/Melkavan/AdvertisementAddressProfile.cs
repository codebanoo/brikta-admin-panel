using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementAddressProfile : Profile
    {
        public AdvertisementAddressProfile()
        {
            CreateMap<AdvertisementAddress, AdvertisementAddressVM>();
            CreateMap<AdvertisementAddressVM, AdvertisementAddress>();
        }
    }
}
