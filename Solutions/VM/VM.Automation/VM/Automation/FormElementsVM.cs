using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Automation
{
    public class FormElementsVM : BaseEntity
    {
        public FormElementsVM()
        {
            FormElementOptionsVM = new HashSet<FormElementOptionsVM>();
        }

        public int FormElementId { get; set; }

        public int FormId { get; set; }

        public string? FormElementTitle { get; set; }

        //1 = متنی
        //2 = تک انتخابی
        //3 = چند انتخابی
        //4 = توضیحات
        //5 = چک باکس
        //6 = تاریخ
        //7 = ساعت
        //8 = ویرایشگر متن
        //9 = عددی
        //10 = فایل
        public int ElementTypeId { get; set; }

        public string? DefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public int FormElementOrder { get; set; }

        public int FormElementSize { get; set; }//1,2,3,4,5,6,7,8,9,10,11,12 - bootstrap col

        public FormsVM? FormsVM { get; set; }

        public ICollection<FormElementOptionsVM> FormElementOptionsVM { get; set; }
    }
}
