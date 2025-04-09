using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertyAddress : BaseEntity
    {
        [Key]
        [ForeignKey("Properties")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long PropertyId { get; set; }

        public long? StateId { get; set; }

        public long? CityId { get; set; }

        //منطقه
        public long? ZoneId { get; set; }
        public long? DistrictId { get; set; }

   
        [StringLength(500)]
        public string? Address { get; set; }

        public double? LocationLat { get; set; }

        public double? LocationLon { get; set; }

        public string? TownName { get; set; }

        public virtual Properties Properties { get; set; }
    }
}
