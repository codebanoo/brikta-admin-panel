using APIs.Projects.Models.Entities;
using AutoMapper;
using VM.Projects;

namespace APIs.Projects.Models.Business.AutoMapper.Projects
{
    public class ConfirmationProfile : Profile
    {
        public ConfirmationProfile()
        {

            CreateMap<ConfirmationAgreements, ConfirmationAgreementsVM>();
            CreateMap<ConfirmationAgreementsVM, ConfirmationAgreements>();
        }
    }
}
