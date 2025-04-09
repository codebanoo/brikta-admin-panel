using System;
using System.Collections.Generic;
using System.Text;
using FrameWork;

namespace VM.Public
{
    public class CountriesVM : BaseEntity
    {
        public CountriesVM()
        {
            StatesVM = new HashSet<StatesVM>();
        }

        public int CountryId { get; set; }

        public string CountryName { get; set; }

        public string CountryLatinName { get; set; }

        public string CountryAbbreviationName { get; set; }

        public string CountryCode { get; set; }

        public string CountryFlagPath { get; set; }

        public ICollection<StatesVM> StatesVM { get; set; }
    }
}
