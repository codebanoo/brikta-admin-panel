using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class ProjectStates : BaseEntity
    {
        public ProjectStates()
        {
            //Projects = new HashSet<Projects>();
        }

        [Key]
        public int ProjectStateId { get; set; }

        [Required]
        [StringLength(50)]
        public string ProjectStateTitle { get; set; }

        [Required]
        [StringLength(20)]
        public string ProjectStateColor { get; set; }

        //public virtual ICollection<Projects> Projects { get; set; }
    }
}
