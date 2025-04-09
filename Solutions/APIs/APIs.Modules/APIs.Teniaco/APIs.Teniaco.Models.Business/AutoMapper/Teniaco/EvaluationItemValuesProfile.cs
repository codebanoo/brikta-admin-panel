using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class EvaluationItemValuesProfile : Profile
    {
        public EvaluationItemValuesProfile()
        {

            CreateMap<EvaluationItemValues, EvaluationItemValuesVM>();
            CreateMap<EvaluationItemValuesVM, EvaluationItemValues>();
        }
    }
}
