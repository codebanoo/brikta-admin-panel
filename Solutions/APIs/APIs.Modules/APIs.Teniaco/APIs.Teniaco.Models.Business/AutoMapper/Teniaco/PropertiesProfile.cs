using APIs.Teniaco.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Teniaco;
using Properties = APIs.Teniaco.Models.Entities.Properties;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class PropertiesProfile : Profile
    {
        public PropertiesProfile()
        {
            CreateMap<Properties, PropertiesVM>();
            CreateMap<PropertiesVM, Properties>();
        }
    }
}
