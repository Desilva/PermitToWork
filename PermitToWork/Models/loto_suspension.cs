//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PermitToWork.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class loto_suspension
    {
        public loto_suspension()
        {
            this.loto_suspension_holder = new HashSet<loto_suspension_holder>();
        }
    
        public int id { get; set; }
        public Nullable<int> id_loto { get; set; }
        public string requestor { get; set; }
        public string facility_owner { get; set; }
        public string requestor_signature { get; set; }
        public string fo_signature { get; set; }
        public Nullable<int> suspend_no { get; set; }
        public string notes { get; set; }
        public Nullable<int> status { get; set; }
        public string can_requestor_signature { get; set; }
        public string fo_delegate { get; set; }
    
        public virtual ICollection<loto_suspension_holder> loto_suspension_holder { get; set; }
        public virtual loto_permit loto_permit { get; set; }
    }
}
