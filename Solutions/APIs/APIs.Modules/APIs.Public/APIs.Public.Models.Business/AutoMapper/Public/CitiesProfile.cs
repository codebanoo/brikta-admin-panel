using VM.Public;
using APIs.Public.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class CitiesProfile : Profile
    {
        public CitiesProfile()
        {
            CreateMap<Cities, CitiesVM>();
            CreateMap<CitiesVM, Cities>();
        }
    }
}
