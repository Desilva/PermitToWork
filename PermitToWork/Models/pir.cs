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
    
    public partial class pir
    {
        public pir()
        {
            this.audit_log = new HashSet<audit_log>();
            this.iir_recommendations = new HashSet<iir_recommendations>();
            this.pir_log = new HashSet<pir_log>();
            this.rcas = new HashSet<rca>();
        }
    
        public int id { get; set; }
        public string no { get; set; }
        public string improvement_request { get; set; }
        public Nullable<System.DateTime> date_rise { get; set; }
        public string initiate_by { get; set; }
        public string title { get; set; }
        public string reference { get; set; }
        public string procedure_reference { get; set; }
        public Nullable<System.DateTime> initiator_sign_date { get; set; }
        public Nullable<System.DateTime> kpbo_sign_date_init { get; set; }
        public Nullable<System.DateTime> target_completion_init { get; set; }
        public string desc_prob { get; set; }
        public string investigator { get; set; }
        public Nullable<System.DateTime> investigator_date { get; set; }
        public string improvement_plant { get; set; }
        public Nullable<System.DateTime> start_implement_date { get; set; }
        public string process_owner { get; set; }
        public Nullable<System.DateTime> target_completion_process { get; set; }
        public string action_by { get; set; }
        public string require_dokument { get; set; }
        public string hira_require { get; set; }
        public Nullable<System.DateTime> kpbo_sign_date_process { get; set; }
        public Nullable<System.DateTime> review_date { get; set; }
        public string result_of_action { get; set; }
        public Nullable<System.DateTime> kpbo_sign_date_process_result { get; set; }
        public Nullable<System.DateTime> sign_date_verified { get; set; }
        public string verified_desc { get; set; }
        public Nullable<System.DateTime> initiator_verified_date { get; set; }
        public Nullable<System.DateTime> review_mgmt_verified_date { get; set; }
        public string description { get; set; }
        public string status { get; set; }
        public string pir_category { get; set; }
        public Nullable<int> process_user { get; set; }
        public Nullable<byte> from { get; set; }
        public string investigator_sign { get; set; }
        public string mgmt { get; set; }
        public string mgmt_sign { get; set; }
    
        public virtual ICollection<audit_log> audit_log { get; set; }
        public virtual ICollection<iir_recommendations> iir_recommendations { get; set; }
        public virtual ICollection<pir_log> pir_log { get; set; }
        public virtual ICollection<rca> rcas { get; set; }
    }
}
