using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class EvaluationsProfile : Profile
    {
        public EvaluationsProfile()
        {
            CreateMap<Evaluations, EvaluationsVM>();
            CreateMap<EvaluationsVM, Evaluations>();
        }

    }
}
