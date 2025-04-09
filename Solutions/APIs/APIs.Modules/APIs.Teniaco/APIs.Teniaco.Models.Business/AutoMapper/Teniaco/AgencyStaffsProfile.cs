using APIs.Teniaco.Models.Entities;
using AutoMapper;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class AgencyStaffsProfile : Profile
    {
        public AgencyStaffsProfile()
        {

            CreateMap<AgencyStaffs, AgencyStaffsVM>();
            CreateMap<AgencyStaffsVM, AgencyStaffs>();
        }
    }
}
