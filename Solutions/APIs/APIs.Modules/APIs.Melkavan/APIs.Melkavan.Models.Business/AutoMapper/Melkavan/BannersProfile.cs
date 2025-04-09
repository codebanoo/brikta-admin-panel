using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class BannersProfile : Profile
    {
        public BannersProfile()
        {
            CreateMap<Banners, BannersVM>();
            CreateMap<BannersVM, Banners>();
        }
    }
}
