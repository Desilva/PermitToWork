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
    
    public partial class excavation_disposal_location
    {
        public int id_permit { get; set; }
        public int id_disposal_location { get; set; }
        public Nullable<double> volume { get; set; }
    
        public virtual excavation excavation { get; set; }
        public virtual mst_ex_disposal_location mst_ex_disposal_location { get; set; }
    }
}
