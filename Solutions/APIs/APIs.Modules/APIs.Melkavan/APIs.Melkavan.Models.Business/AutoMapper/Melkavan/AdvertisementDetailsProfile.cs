using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementDetailsProfile : Profile
    {
        public AdvertisementDetailsProfile()
        {
            CreateMap<AdvertisementDetails, AdvertisementDetailsVM>();
            CreateMap<AdvertisementDetailsVM, AdvertisementDetails>();
        }
    }
}
