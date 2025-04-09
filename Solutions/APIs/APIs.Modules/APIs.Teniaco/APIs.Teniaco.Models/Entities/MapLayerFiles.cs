
using FrameWork;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APIs.Teniaco.Models.Entities
{
    public partial class MapLayerFiles :BaseEntity
    {
        [Key]
        public int MapLayerId { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public class crs
        {
            public string type { get; set; }
            public class properties
            {
                public string name { get; set; }
            }
        }
        public class features
        {
            public string type { get; set; }
            public class properties
            {
                public int landuse { get; set; }
                public int ORIG_FID { get; set; }
            }
            public class geometry
            {
                public string type { get; set; }
                public string[] coordinates { get; set; }
            }
        }
    }
}
