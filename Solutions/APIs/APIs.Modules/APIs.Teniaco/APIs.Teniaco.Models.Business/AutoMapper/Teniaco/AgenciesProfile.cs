using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class AgenciesProfile : Profile
    {
        public AgenciesProfile()
        {

            CreateMap<Agencies, AgenciesVM>();
            CreateMap<AgenciesVM, Agencies>();
        }
    }
}
