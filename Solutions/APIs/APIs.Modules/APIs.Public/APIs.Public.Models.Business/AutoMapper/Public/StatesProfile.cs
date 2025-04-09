using VM.Public;
using APIs.Public.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class StatesProfile : Profile
    {
        public StatesProfile()
        {
            CreateMap<States, StatesVM>();
            CreateMap<StatesVM, States>();
        }
    }
}
