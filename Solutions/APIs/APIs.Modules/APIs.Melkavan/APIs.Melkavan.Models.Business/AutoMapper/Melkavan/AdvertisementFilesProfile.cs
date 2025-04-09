using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class AdvertisementFilesProfile : Profile
    {
        public AdvertisementFilesProfile()
        {
            CreateMap<AdvertisementFiles, AdvertisementFilesVM>();
            CreateMap<AdvertisementFilesVM, AdvertisementFiles>();
        }
    }
}
