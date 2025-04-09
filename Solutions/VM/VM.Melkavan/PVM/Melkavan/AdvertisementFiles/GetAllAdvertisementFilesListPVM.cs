using VM.PVM.Base;

namespace VM.PVM.Melkavan
{
    public class GetAllAdvertisementFilesListPVM : BPVM
    {
        public long AdvertisementId { get; set; }

        public string AdvertisementFileTitle { get; set; }

        public string AdvertisementFileType { get; set; }
    }
}
