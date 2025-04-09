using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class PropertyFiles : BaseEntity
    {
        [Key]
        public int PropertyFileId { get; set; }

        [ForeignKey("Properties")]
        public long PropertyId { get; set; }

        //[Required]
        //[StringLength(50)]
        public string? PropertyFileTitle { get; set; }

        [Required]
        [StringLength(200)]
        public string PropertyFilePath { get; set; }

        [Required]
        [StringLength(5)]
        public string PropertyFileExt { get; set; }

        public string PropertyFileType { get; set; }

        public int PropertyFileOrder { get; set; }

        public virtual Properties Properties { get; set; }
    }
}
