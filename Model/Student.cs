//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Student
    {
        public Student()
        {
            this.SelectSub = new HashSet<SelectSub>();
        }
    
        public int StuID { get; set; }
        public string StuName { get; set; }
        public string StuNum { get; set; }
        public string StuPwd { get; set; }
        public int ClassForm { get; set; }
        public string StuPicture { get; set; }
    
        public virtual ICollection<SelectSub> SelectSub { get; set; }
        public virtual Class Class { get; set; }
    }
}
