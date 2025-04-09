using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using FrameWork;

namespace APIs.Public.Models.Entities
{
    public partial class States : BaseEntity
    {
        public States()
        {
            Cities = new HashSet<Cities>();
        }

        public long StateId { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }

        [StringLength(50)]
        public string StateName { get; set; }

        [StringLength(10)]
        public string StateCode { get; set; }

        public Countries Country { get; set; }

        public ICollection<Cities> Cities { get; set; }
    }
}
