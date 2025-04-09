using APIs.Melkavan.Models.Entities;
using AutoMapper;
using VM.Melkavan;

namespace APIs.Melkavan.Models.Business.AutoMapper.Melkavan
{
    public class SmsesProfile : Profile
    {
        public SmsesProfile()
        {
            CreateMap<Smses, SmsesVM>();
            CreateMap<SmsesVM, Smses>();
        }
    }
}
