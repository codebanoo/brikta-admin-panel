using FrameWork;
using System.Collections.Generic;

namespace VM.Melkavan
{
    public class AdvertisementTypesVM : BaseEntity
    {
        public AdvertisementTypesVM()
        {

            AdvertisementFeaturesValuesVM = new HashSet<AdvertisementFeaturesValuesVM>();
           AdvertisementVM = new HashSet<AdvertisementVM>();
        }


        //نوع آگهی
        //1: اجاره
        //2: فروش
        public int AdvertisementTypeId { get; set; }

        public string AdvertisementTypeTilte { get; set; }

        public virtual ICollection<AdvertisementFeaturesValuesVM> AdvertisementFeaturesValuesVM { get; set; }

        public virtual ICollection<AdvertisementVM> AdvertisementVM { get; set; }
    }
}
