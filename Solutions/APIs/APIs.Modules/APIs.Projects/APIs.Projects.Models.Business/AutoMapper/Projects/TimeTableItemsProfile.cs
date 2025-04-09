using APIs.Projects.Models.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Projects;

namespace APIs.Projects.Models.Business.AutoMapper.Projects
{
    public class TimeTableItemsProfile : Profile
    {
        public TimeTableItemsProfile()
        {

            CreateMap<TimeTableItems, TimeTableItemsVM>();
            CreateMap<TimeTableItemsVM, TimeTableItems>();
        }
    }
}
