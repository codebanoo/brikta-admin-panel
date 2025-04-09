using APIs.Public.Models.Entities;
using AutoMapper;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class DistrictFilesProfile : Profile
    {

        public DistrictFilesProfile()
        {
            CreateMap<DistrictFiles, DistrictFilesVM>();
            CreateMap<DistrictFilesVM, DistrictFiles>();
        }
    }
}
