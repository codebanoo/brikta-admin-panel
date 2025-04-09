using APIs.Automation.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using VM.Automation;

namespace APIs.Automation.Models.Business.AutoMapper.Automation
{
    public class MyDepartmentsDirectorsProfile : Profile
    {
        public MyDepartmentsDirectorsProfile()
        {
            CreateMap<MyDepartmentsDirectors, MyDepartmentsDirectorsVM>();
            CreateMap<MyDepartmentsDirectorsVM, MyDepartmentsDirectors>();
        }
    }
}
