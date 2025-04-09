using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace APIs.Projects.Models.Entities
{
    public partial class PropertyProjects : BaseEntity
    {
        public PropertyProjects()
        {
            ProjectPropertyOperations = new HashSet<OperationOnPropertyProjects>();
        }

        public int PropertyProjectId { get; set; }

        public long PropertyId { get; set; }

        public int? ParentPropertyProjectId { get; set; }

        public int PropertyProjectTypeId { get; set; }

        public bool IsPrivate { get; set; }

        public virtual ICollection<OperationOnPropertyProjects> ProjectPropertyOperations { get; set; }

        public virtual PropertyProjectTypes PropertyProjectTypes { get; set; }
    }
}
