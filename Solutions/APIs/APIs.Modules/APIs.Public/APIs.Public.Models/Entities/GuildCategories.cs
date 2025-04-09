using FrameWork;


namespace APIs.Public.Models.Entities
{
    public partial class GuildCategories : BaseEntity
    {
        public int GuildCategoryId { get; set; }
        public string? GuildCategoryName { get; set; }
    }
}
