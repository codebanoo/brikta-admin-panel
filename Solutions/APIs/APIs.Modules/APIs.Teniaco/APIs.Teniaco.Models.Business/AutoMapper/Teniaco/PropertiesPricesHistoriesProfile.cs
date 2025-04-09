using APIs.Teniaco.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Teniaco;

namespace APIs.Teniaco.Models.Business.AutoMapper.Teniaco
{
    public class PropertiesPricesHistoriesProfile : Profile
    {
        public PropertiesPricesHistoriesProfile()
        {
            CreateMap<PropertiesPricesHistories, PropertiesPricesHistoriesVM>();
            CreateMap<PropertiesPricesHistoriesVM, PropertiesPricesHistories>();

            CreateMap<PropertiesPricesHistories, MyPropertiesPricesHistoriesVM>();
            CreateMap<MyPropertiesPricesHistoriesVM, PropertiesPricesHistories>();
        }
    }
}
