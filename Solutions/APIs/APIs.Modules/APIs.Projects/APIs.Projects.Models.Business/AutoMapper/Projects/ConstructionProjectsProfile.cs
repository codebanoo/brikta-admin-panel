using APIs.Projects.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;

namespace APIs.Projects.Models.Business.AutoMapper.Projects
{
    public class ConstructionProjectsProfile : Profile
    {
        public ConstructionProjectsProfile()
        {
            CreateMap<ConstructionProjects, ConstructionProjectsVM>();
            CreateMap<ConstructionProjectsVM, ConstructionProjects>();
        }
    }
}
