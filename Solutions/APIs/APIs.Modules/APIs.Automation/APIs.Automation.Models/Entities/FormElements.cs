using FrameWork;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIs.Automation.Models.Entities
{
    public partial class FormElements : BaseEntity
    {
        public FormElements()
        {
            FormElementOptions = new HashSet<FormElementOptions>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FormElementId { get; set; }

        [ForeignKey("Forms")]
        public int FormId { get; set; }

        public string FormElementTitle { get; set; }

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

        public string DefaultValue { get; set; }

        public bool IsRequired { get; set; }

        public int FormElementOrder { get; set; }

        public int FormElementSize { get; set; }//1,2,3,4,5,6,7,8,9,10,11,12 - bootstrap col

        public Forms Forms { get; set; }

        public ICollection<FormElementOptions> FormElementOptions { get; set; }
    }
}
