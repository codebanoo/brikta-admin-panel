using FrameWork;

namespace VM.Teniaco
{
    public class MapLayerCategoriesVM : BaseEntity
    {
        public int MapLayerCategoryId { get; set; }
        public string MapLayerCategoryTitle { get; set; }
        public int? ParentMapLayerCategoryId { get; set; }
        public string? Description { get; set; }
        public string? NewDescription
        {
            get
            {
                if (!(string.IsNullOrEmpty(this.Description)))
                {
                    if (this.Description.Length <= 50)
                    {
                        return this.Description;
                    }
                    return this.Description.Substring(0, 50);
                }
                else { return this.Description; }
            }
        }
        public string? Color { get; set; }
    }
}
