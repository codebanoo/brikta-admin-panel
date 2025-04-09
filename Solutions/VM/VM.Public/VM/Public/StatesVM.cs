using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Public
{
    public class StatesVM : BaseEntity
    {
        public StatesVM()
        {
            CitiesVM = new HashSet<CitiesVM>();
        }

        public long StateId { get; set; }

        public int? CountryId { get; set; }

        public string StateName { get; set; }

        public string StateCode { get; set; }

        //public CountriesVM CountryVM { get; set; }

        public ICollection<CitiesVM> CitiesVM { get; set; }
    }
}
