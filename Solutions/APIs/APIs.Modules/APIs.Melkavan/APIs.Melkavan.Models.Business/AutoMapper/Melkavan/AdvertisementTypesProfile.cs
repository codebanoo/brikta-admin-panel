using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementTypesProfile : Profile
    {
        public AdvertisementTypesProfile()
        {
            CreateMap<AdvertisementTypes, AdvertisementTypesVM>();
            CreateMap<AdvertisementTypesVM, AdvertisementTypes>();
        }
    }
}
