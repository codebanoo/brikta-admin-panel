using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class ContractorsProfile : Profile
    {
        public ContractorsProfile()
        {

            CreateMap<Contractors, ContractorsVM>();
            CreateMap<ContractorsVM, Contractors>();
        }
    }
}
