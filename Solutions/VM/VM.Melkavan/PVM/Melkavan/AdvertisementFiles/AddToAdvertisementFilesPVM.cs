using System.Collections.Generic;
using VM.Melkavan;
using VM.PVM.Base;


namespace VM.PVM.Melkavan
{
    public class AddToAdvertisementFilesPVM : BPVM
    {
        public List<AdvertisementFilesVM> AdvertisementFilesVMList { get; set; }
        public List<int>? DeletedPhotosIDs { get; set; }
        public bool? IsMainChanged { get; set; }
        public long? AdvertisementId { get; set; }

        //public List<AdvertismentFilesPVM> AdvertismentFilesPVMList { get; set; }
    }
}
