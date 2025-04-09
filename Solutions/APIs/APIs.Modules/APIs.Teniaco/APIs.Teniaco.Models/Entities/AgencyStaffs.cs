using APIs.Public.Models.Entities;
using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class AgencyStaffs : BaseEntity
    {
       
        [Key]
        public int AgencyStaffsId { get; set; }

        [ForeignKey("Agencies")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int AgencyId { get; set; }

        //public long PersonId { get; set; }

        public long? UserId { get; set; }

        [ForeignKey("Positions")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PositionId { get; set; }

        public string? SocialNetworks { get; set; }

        public virtual Agencies Agencies { get; set; }

        public virtual Positions Positions { get; set; }

    }
}
