using FrameWork;

namespace VM.Teniaco
{
    public class MapLayerFilesVM :BaseEntity
    {
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
