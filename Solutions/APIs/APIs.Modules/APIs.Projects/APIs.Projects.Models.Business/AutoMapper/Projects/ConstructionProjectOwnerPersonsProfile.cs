using APIs.Projects.Models.Entities;
using AutoMapper;
using VM.Projects;

namespace APIs.Projects.Models.Business.AutoMapper.Projects
{
    public class ConstructionProjectOwnerPersonsProfile : Profile
    {
        public ConstructionProjectOwnerPersonsProfile()
        {

            CreateMap<ConstructionProjectOwnerUsers, ConstructionProjectOwnerUsersVM>();
            CreateMap<ConstructionProjectOwnerUsersVM, ConstructionProjectOwnerUsers>();
        }
    }
}
