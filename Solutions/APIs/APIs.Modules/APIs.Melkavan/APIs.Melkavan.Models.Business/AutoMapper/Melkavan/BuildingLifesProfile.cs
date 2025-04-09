using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class BuildingLifesProfile : Profile
    {
        public BuildingLifesProfile()
        {
            CreateMap<BuildingLifes, BuildingLifesVM>();
            CreateMap<BuildingLifesVM, BuildingLifes>();
        }
    }
}
