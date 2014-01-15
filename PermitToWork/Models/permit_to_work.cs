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
    
    public partial class permit_to_work
    {
        public permit_to_work()
        {
            this.hira_document = new HashSet<hira_document>();
            this.hot_work = new HashSet<hot_work>();
            this.permit_to_work1 = new HashSet<permit_to_work>();
        }
    
        public int id { get; set; }
        public string ptw_no { get; set; }
        public Nullable<System.DateTime> proposed_period_start { get; set; }
        public Nullable<System.DateTime> proposed_period_end { get; set; }
        public Nullable<int> dept_requestor { get; set; }
        public Nullable<int> section { get; set; }
        public string total_crew { get; set; }
        public Nullable<int> requestor_ptw_holder_no { get; set; }
        public string area { get; set; }
        public string work_location { get; set; }
        public string area_code { get; set; }
        public string work_order_no { get; set; }
        public string work_description { get; set; }
        public string notes { get; set; }
        public Nullable<byte> pre_job_1_spv { get; set; }
        public Nullable<byte> pre_job_2_spv { get; set; }
        public Nullable<byte> pre_job_3_spv { get; set; }
        public Nullable<byte> pre_job_4_spv { get; set; }
        public Nullable<byte> pre_job_5_spv { get; set; }
        public Nullable<byte> pre_job_6_spv { get; set; }
        public Nullable<byte> pre_job_7_spv { get; set; }
        public Nullable<byte> pre_job_1_fo { get; set; }
        public Nullable<byte> pre_job_2_fo { get; set; }
        public Nullable<byte> pre_job_3_fo { get; set; }
        public Nullable<byte> pre_job_4_fo { get; set; }
        public Nullable<byte> pre_job_5_fo { get; set; }
        public Nullable<byte> pre_job_6_fo { get; set; }
        public Nullable<byte> pre_job_7_fo { get; set; }
        public Nullable<byte> cancel_1_spv { get; set; }
        public Nullable<byte> cancel_2_spv { get; set; }
        public Nullable<byte> cancel_3_spv { get; set; }
        public Nullable<byte> cancel_4_spv { get; set; }
        public Nullable<byte> cancel_5_spv { get; set; }
        public Nullable<byte> cancel_6_spv { get; set; }
        public Nullable<byte> cancel_7_spv { get; set; }
        public Nullable<byte> cancel_1_fo { get; set; }
        public Nullable<byte> cancel_2_fo { get; set; }
        public Nullable<byte> cancel_3_fo { get; set; }
        public Nullable<byte> cancel_4_fo { get; set; }
        public Nullable<byte> cancel_5_fo { get; set; }
        public Nullable<byte> cancel_6_fo { get; set; }
        public Nullable<byte> cancel_7_fo { get; set; }
        public string pre_job_notes { get; set; }
        public string cancel_notes { get; set; }
        public Nullable<System.DateTime> validity_period_start { get; set; }
        public Nullable<System.DateTime> validity_period_end { get; set; }
        public Nullable<System.DateTime> cancellation_date { get; set; }
        public Nullable<int> loto_id { get; set; }
        public Nullable<int> loto_status { get; set; }
        public Nullable<int> csep_id { get; set; }
        public Nullable<int> csep_status { get; set; }
        public Nullable<int> hw_id { get; set; }
        public Nullable<int> hw_status { get; set; }
        public Nullable<int> fi_id { get; set; }
        public Nullable<int> fi_status { get; set; }
        public Nullable<int> ex_id { get; set; }
        public Nullable<int> ex_status { get; set; }
        public Nullable<int> wh_id { get; set; }
        public Nullable<int> wh_status { get; set; }
        public Nullable<int> rad_id { get; set; }
        public Nullable<int> rad_status { get; set; }
        public string acc_ptw_requestor { get; set; }
        public string acc_ptw_requestor_approve { get; set; }
        public string acc_supervisor { get; set; }
        public string acc_supervisor_delegate { get; set; }
        public string acc_supervisor_approve { get; set; }
        public string acc_assessor { get; set; }
        public string acc_assessor_delegate { get; set; }
        public string acc_assessor_approve { get; set; }
        public string acc_fo { get; set; }
        public string acc_fo_delegate { get; set; }
        public string acc_fo_approve { get; set; }
        public string can_ptw_requestor { get; set; }
        public string can_ptw_requestor_approve { get; set; }
        public string can_supervisor { get; set; }
        public string can_supervisor_delegate { get; set; }
        public string can_supervisor_approve { get; set; }
        public string can_assessor { get; set; }
        public string can_assessor_delegate { get; set; }
        public string can_assessor_approve { get; set; }
        public string can_fo { get; set; }
        public string can_fo_delegate { get; set; }
        public string can_fo_approve { get; set; }
        public Nullable<int> id_parent_ptw { get; set; }
        public Nullable<int> status { get; set; }
    
        public virtual ICollection<hira_document> hira_document { get; set; }
        public virtual ICollection<hot_work> hot_work { get; set; }
        public virtual mst_department mst_department { get; set; }
        public virtual mst_ptw_holder_no mst_ptw_holder_no { get; set; }
        public virtual mst_section mst_section { get; set; }
        public virtual ICollection<permit_to_work> permit_to_work1 { get; set; }
        public virtual permit_to_work permit_to_work2 { get; set; }
    }
}
