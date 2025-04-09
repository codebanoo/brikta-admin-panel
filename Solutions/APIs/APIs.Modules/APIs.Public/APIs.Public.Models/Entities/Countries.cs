using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace APIs.Public.Models.Entities
{
    public partial class Countries : BaseEntity
    {
        public Countries()
        {
            States = new HashSet<States>();
        }

        public int CountryId { get; set; }

        [StringLength(50)]
        public string CountryName { get; set; }

        [StringLength(50)]
        public string CountryLatinName { get; set; }

        [StringLength(10)]
        public string CountryAbbreviationName { get; set; }

        [StringLength(10)]
        public string CountryCode { get; set; }

        [StringLength(300)]
        public string CountryFlagPath { get; set; }

        public ICollection<States> States { get; set; }
    }
}
