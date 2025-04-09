using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using FrameWork;

namespace APIs.Public.Models.Entities
{
    public partial class Persons : BaseEntity
    {
        public long PersonId { get; set; }
        public int? PersonTypeId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string NationalCode { get; set; }
        public bool Sexuality { get; set; }
        public DateTime? BirthDateTimeEn { get; set; }
        public string PostalCode { get; set; }
        public string Mobail { get; set; }
        public string Phone { get; set; }
        public int? CountryId { get; set; }
        public long? StateId { get; set; }
        public long? CityId { get; set; }
        public string Address { get; set; }
    }
}
