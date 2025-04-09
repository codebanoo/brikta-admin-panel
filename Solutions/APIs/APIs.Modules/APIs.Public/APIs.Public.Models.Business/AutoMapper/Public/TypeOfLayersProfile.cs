using APIs.Public.Models.Entities;
using AutoMapper;
using VM.Public;

namespace APIs.Public.Models.Business.AutoMapper.Public
{
    public class TypeOfUseLayersProfile : Profile
    {
        public TypeOfUseLayersProfile()
        {
            CreateMap<TypeOfUseLayers, TypeOfUseLayersVM>();
            CreateMap<TypeOfUseLayersVM, TypeOfUseLayers>();
        }
    }
}
