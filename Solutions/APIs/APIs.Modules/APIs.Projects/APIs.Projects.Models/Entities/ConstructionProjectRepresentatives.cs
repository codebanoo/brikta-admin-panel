using FrameWork;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectRepresentatives : BaseEntity
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long ConstructionProjectRepresentativeId { get; set; }

        public long RepresentativeId { get; set; }

        public long OwnerUserId { get; set; }


        [ForeignKey("ConstructionProjects")]
        public long ConstructionProjectId { get; set; }
    }
}
