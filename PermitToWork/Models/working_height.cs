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
    
    public partial class working_height
    {
        public int id { get; set; }
        public Nullable<int> id_ptw { get; set; }
        public string wh_no { get; set; }
        public string description { get; set; }
        public string work_location { get; set; }
        public string requestor { get; set; }
        public Nullable<System.DateTime> from { get; set; }
        public Nullable<System.DateTime> until { get; set; }
        public Nullable<int> total_crew { get; set; }
        public string access { get; set; }
        public string workbox_access { get; set; }
        public string ladder_access { get; set; }
        public string elevated_access { get; set; }
        public Nullable<int> scaffolding { get; set; }
        public string load_capacity { get; set; }
        public string no_person { get; set; }
        public Nullable<int> erector { get; set; }
        public string erector_certificate_no { get; set; }
        public Nullable<System.DateTime> erector_valid_date { get; set; }
        public string scaffold_access { get; set; }
        public Nullable<int> inspector { get; set; }
        public string inspector_certify_no { get; set; }
        public Nullable<System.DateTime> utilization_valid_date { get; set; }
        public string inspector_signature { get; set; }
        public Nullable<System.DateTime> inspector_sign_date { get; set; }
        public string mandatory_fall_prevention { get; set; }
        public string fall_prevention_assess { get; set; }
        public string pre_screening_req { get; set; }
        public string pre_screening_fo { get; set; }
        public string requestor_signature { get; set; }
        public string requestor_delegate { get; set; }
        public Nullable<System.DateTime> requestor_signature_date { get; set; }
        public string supervisor { get; set; }
        public string supervisor_signature { get; set; }
        public string supervisor_delegate { get; set; }
        public Nullable<System.DateTime> supervisor_signature_date { get; set; }
        public string facility_owner { get; set; }
        public string facility_owner_signature { get; set; }
        public string facility_owner_delegate { get; set; }
        public Nullable<System.DateTime> facility_owner_signature_date { get; set; }
        public string can_screening_req { get; set; }
        public string can_screening_fo { get; set; }
        public string can_requestor_signature { get; set; }
        public string can_requestor_delegate { get; set; }
        public Nullable<System.DateTime> can_requestor_signature_date { get; set; }
        public string can_supervisor_signature { get; set; }
        public string can_supervisor_delegate { get; set; }
        public Nullable<System.DateTime> can_supervisor_signature_date { get; set; }
        public string can_facility_owner_signature { get; set; }
        public string can_facility_owner_delegate { get; set; }
        public Nullable<System.DateTime> can_facility_owner_signature_date { get; set; }
        public Nullable<int> status { get; set; }
        public string pre_screening_note { get; set; }
        public string no_inspection { get; set; }
    
        public virtual permit_to_work permit_to_work { get; set; }
    }
}
