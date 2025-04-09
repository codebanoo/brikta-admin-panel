using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementProfile : Profile
    {
        public AdvertisementProfile()
        {
            CreateMap<Advertisement, AdvertisementVM>();
            CreateMap<AdvertisementVM, Advertisement>();
        }
    }
}
