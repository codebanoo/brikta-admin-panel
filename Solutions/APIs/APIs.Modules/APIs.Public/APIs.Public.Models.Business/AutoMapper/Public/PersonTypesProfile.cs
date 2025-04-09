using APIs.Public.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class PersonTypesProfile : Profile
    {
        public PersonTypesProfile()
        {
            CreateMap<PersonTypes, PersonTypesVM>();
            CreateMap<PersonTypesVM, PersonTypes>();
        }
    }
}
