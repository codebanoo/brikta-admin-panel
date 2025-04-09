using APIs.Automation.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Automation;

namespace APIs.Automation.Models.Business.AutoMapper.Automation
{
    public class FormElementValuesProfile : Profile
    {
        public FormElementValuesProfile()
        {
            CreateMap<FormElementValues, FormElementValuesVM>();
            CreateMap<FormElementValuesVM, FormElementValues>();
        }
    }
}
