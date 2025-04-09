using APIs.Projects.Models.Entities;
using AutoMapper;
using VM.Projects;

namespace APIs.Projects.Models.Business.AutoMapper.Projects
{
    public class ConstructionProjectTypesProfile : Profile
    {
        public ConstructionProjectTypesProfile()
        {
            CreateMap<ConstructionProjectTypes, ConstructionProjectTypesVM>();
            CreateMap<ConstructionProjectTypesVM, ConstructionProjectTypes>();
        }
    }
}
