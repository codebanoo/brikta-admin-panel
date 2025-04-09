using APIs.Automation.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Automation;

namespace APIs.Automation.Models.Business.AutoMapper.Automation
{
    public class MyCompaniesProfile : Profile
    {
        public MyCompaniesProfile()
        {
            CreateMap<MyCompanies, MyCompaniesVM>();
            CreateMap<MyCompaniesVM, MyCompanies>();
        }
    }
}
