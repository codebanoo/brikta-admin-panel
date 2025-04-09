using APIs.Public.Models.Entities;
using AutoMapper;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class ZoneFilesProfile : Profile
    {

        public ZoneFilesProfile()
        {
            CreateMap<ZoneFiles, ZoneFilesVM>();
            CreateMap<ZoneFilesVM, ZoneFiles>();
        }
    }
}
