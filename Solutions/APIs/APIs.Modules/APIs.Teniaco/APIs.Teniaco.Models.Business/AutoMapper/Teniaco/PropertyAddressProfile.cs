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
    public class PropertyAddressProfile : Profile
    {
        public PropertyAddressProfile()
        {
            CreateMap<PropertyAddress, PropertyAddressVM>();
            CreateMap<PropertyAddressVM, PropertyAddress>();

            CreateMap<PropertyAddress, MyPropertyAddressVM>();
            CreateMap<MyPropertyAddressVM, PropertyAddress>();
        }
    }
}
