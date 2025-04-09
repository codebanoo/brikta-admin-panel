using APIs.Public.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class PersonsProfile : Profile
    {
        public PersonsProfile()
        {
            CreateMap<Persons, PersonsVM>();
            CreateMap<PersonsVM, Persons>();
        }
    }
}
