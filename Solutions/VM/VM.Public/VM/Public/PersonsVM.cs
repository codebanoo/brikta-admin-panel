using FrameWork;
using System;
using System.Collections.Generic;
using System.Text;

namespace VM.Public
{
    public class PersonsVM : BaseEntity
    {
        private string lang = "fa";

        public PersonsVM()
        {

        }

        public PersonsVM(string _lang)
        {
            lang = _lang;
        }

        public long PersonId { get; set; }
        public int? PersonTypeId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public bool Sexuality { get; set; }
        public string PostalCode { get; set; }
        public string Mobail { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public string CountryName { get; set; }
        public long? StateId { get; set; }
        public string StateName { get; set; }
        public long? CityId { get; set; }
        public string CityName { get; set; }
        public string Address { get; set; }


        public DateTime? BirthDateTimeEn { get; set; }
        public string BirthDateTime
        {
            get
            {

                if (this.BirthDateTimeEn.HasValue)
                    return DateManager.ConvertFromDate(lang, this.BirthDateTimeEn);
                else
                    return "";

            }
            set
            {
                if (!string.IsNullOrEmpty(lang) && !string.IsNullOrEmpty(value))
                    this.BirthDateTimeEn = DateManager.ConvertToDate(lang, value);
            }
        }
    }
}
