using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Projects.Models.Entities
{
    public partial class ConstructionProjectOwnerUsers : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ConstructionProjectOwnerUserId { get; set; }

        //مالک
        public long OwnerUserId { get; set; }

        [ForeignKey("ConstructionProjects")]
        public long ConstructionProjectId { get; set; }

        //درصد دنگ
        public double SharePercent { get; set; }

        public virtual ConstructionProjects ConstructionProjects { get; set; }
    }
}
