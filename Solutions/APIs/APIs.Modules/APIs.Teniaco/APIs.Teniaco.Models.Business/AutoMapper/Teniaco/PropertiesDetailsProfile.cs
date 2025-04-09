using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class PropertiesDetailsProfile : Profile
    {
        public PropertiesDetailsProfile()
        {

            CreateMap<PropertiesDetails, PropertiesDetailsVM>();
            CreateMap<PropertiesDetailsVM, PropertiesDetails>();
        }
    }
}
