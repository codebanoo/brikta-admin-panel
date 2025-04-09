using APIs.Automation.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Automation;

namespace APIs.Automation.Models.Business.AutoMapper.Automation
{
    public class MyDepartmentsProfile : Profile
    {
        public MyDepartmentsProfile()
        {
            CreateMap<MyDepartments, MyDepartmentsVM>()/*
                .ForMember(x => x.EstablishedDateTime, opt => opt.Ignore())
                .ForMember(x => x.DirectorName, opt => opt.Ignore())
                .ForMember(x => x.MyCompaniesVM, opt => opt.Ignore())*/;
            CreateMap<MyDepartmentsVM, MyDepartments>();
        }
    }
}
