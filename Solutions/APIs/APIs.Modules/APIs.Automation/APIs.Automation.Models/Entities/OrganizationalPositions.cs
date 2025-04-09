using FrameWork;
using System.ComponentModel.DataAnnotations;


namespace APIs.Automation.Models.Entities
{
    public partial class OrganizationalPositions : BaseEntity
    {
        [Key]
        public int OrganizationalPositionId { get; set; }
        public string OrganizationalPositionName { get; set; }
    }
}
