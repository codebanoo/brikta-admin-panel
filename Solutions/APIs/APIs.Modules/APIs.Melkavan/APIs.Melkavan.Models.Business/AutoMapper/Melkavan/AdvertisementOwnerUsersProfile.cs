using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementOwnersProfile : Profile
    {
        public AdvertisementOwnersProfile()
        {
            CreateMap<AdvertisementOwners, AdvertisementOwnersVM>();
            CreateMap<AdvertisementOwnersVM, AdvertisementOwners>();
        }
    }
}
