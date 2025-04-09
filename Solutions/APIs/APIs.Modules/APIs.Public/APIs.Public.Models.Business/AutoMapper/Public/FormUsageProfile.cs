using APIs.Public.Models.Entities;
using AutoMapper;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class FormUsageProfile : Profile

    {
        public FormUsageProfile()
        {
            CreateMap<FormUsages, FormUsagesVM>();
            CreateMap<FormUsagesVM, FormUsages>();
        }
    }
}
