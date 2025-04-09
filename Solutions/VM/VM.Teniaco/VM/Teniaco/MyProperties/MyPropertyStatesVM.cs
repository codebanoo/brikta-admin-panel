using FrameWork;


namespace VM.Teniaco
{
    public class MyPropertyStatesVM : BaseEntity
    {
        public MyPropertyStatesVM()
        {
            MyPropertiesVM = new HashSet<MyPropertiesVM>();
        }

        public int PropertyStateId { get; set; }

        public string PropertyStateTitle { get; set; }

        public virtual ICollection<MyPropertiesVM> MyPropertiesVM { get; set; }
    }
}
