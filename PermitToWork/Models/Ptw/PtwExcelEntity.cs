using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermitToWork.Models.Master;
using PermitToWork.Models.User;

namespace PermitToWork.Models.Ptw
{
    public class PtwExcelEntity
    {
        public string ptw_no { get; set; }
        public string proposed_period_start { get; set; }
        public string proposed_period_end { get; set; }
        public string dept_requestor { get; set; }
        public string section { get; set; }
        public string total_crew { get; set; }
        public string requestor_ptw_holder_no { get; set; }
        public string area { get; set; }
        public string work_location { get; set; }
        public string area_code { get; set; }
        public string work_order_no { get; set; }
        public string work_description { get; set; }
        public string hira_docs { get; set; }
        public string loto_need { get; set; }
        public string csep_need { get; set; }
        public string hw_need { get; set; }
        public string fi_need { get; set; }
        public string ex_need { get; set; }
        public string wh_need { get; set; }
        public string rad_need { get; set; }
        public string pre_job_1_spv { get; set; }
        public string pre_job_2_spv { get; set; }
        public string pre_job_3_spv { get; set; }
        public string pre_job_4_spv { get; set; }
        public string pre_job_5_spv { get; set; }
        public string pre_job_6_spv { get; set; }
        public string pre_job_7_spv { get; set; }
        public string pre_job_1_fo { get; set; }
        public string pre_job_2_fo { get; set; }
        public string pre_job_3_fo { get; set; }
        public string pre_job_4_fo { get; set; }
        public string pre_job_5_fo { get; set; }
        public string pre_job_6_fo { get; set; }
        public string pre_job_7_fo { get; set; }
        public string pre_job_notes { get; set; }
        public string acc_notes_fo_ass { get; set; }
        public string acc_notes_ass_fo { get; set; }
        public string validity_period_start { get; set; }
        public string validity_period_end { get; set; }
        public string acc_ptw_requestor { get; set; }
        public string acc_supervisor { get; set; }
        public string acc_supervisor_delegate { get; set; }
        public string acc_assessor { get; set; }
        public string acc_assessor_delegate { get; set; }
        public string acc_fo { get; set; }
        public string acc_fo_delegate { get; set; }
        public string cancel_1_spv { get; set; }
        public string cancel_2_spv { get; set; }
        public string cancel_3_spv { get; set; }
        public string cancel_4_spv { get; set; }
        public string cancel_5_spv { get; set; }
        public string cancel_6_spv { get; set; }
        public string cancel_7_spv { get; set; }
        public string cancel_1_fo { get; set; }
        public string cancel_2_fo { get; set; }
        public string cancel_3_fo { get; set; }
        public string cancel_4_fo { get; set; }
        public string cancel_5_fo { get; set; }
        public string cancel_6_fo { get; set; }
        public string cancel_7_fo { get; set; }
        public string cancel_notes { get; set; }
        public string can_notes_ass_fo { get; set; }
        public string cancellation_date { get; set; }
        public string can_ptw_requestor { get; set; }
        public string can_supervisor { get; set; }
        public string can_supervisor_delegate { get; set; }
        public string can_assessor { get; set; }
        public string can_assessor_delegate { get; set; }
        public string can_fo { get; set; }
        public string can_fo_delegate { get; set; }
        public string status { get; set; }

        public PtwExcelEntity(permit_to_work ptw, UserEntity user, ListUser listUser)
        {
            // ModelUtilization.Clone(ptw, this);
            this.work_description = ptw.work_description;
            this.ptw_no = ptw.ptw_no;
            this.proposed_period_start = ptw.proposed_period_start != null ? ptw.proposed_period_start.Value.ToString("MM/dd/yyyy hh:mm tt") : "-";
            this.proposed_period_end = ptw.proposed_period_end != null ? ptw.proposed_period_end.Value.ToString("MM/dd/yyyy hh:mm tt") : "-";
            this.dept_requestor = ptw.dept_requestor != null ? new MstDepartmentEntity(ptw.mst_department).department : "-";
            this.section = ptw.section != null ? new MstSectionEntity(ptw.mst_section).section : "-";
            this.total_crew = ptw.total_crew;
            this.requestor_ptw_holder_no = ptw.is_guest == 1 ? ptw.guest_holder_no : (ptw.mst_ptw_holder_no != null ? new MstPtwHolderNoEntity(ptw.mst_ptw_holder_no).ptw_holder_no : "");
            this.area = ptw.area == "1" ? "Power Station" : (ptw.area == "2" ? "Steam Field" : ptw.area);
            this.work_location = ptw.work_location;
            this.area_code = ptw.area_code;
            this.work_order_no = ptw.work_order_no;
            this.work_description = ptw.work_description;

            this.loto_need = ptw.loto_need == 1 ? "Yes" : "No";
            this.csep_need = ptw.csep_id != null ? "Yes" : "No";
            this.hw_need = ptw.hw_id != null ? "Yes" : "No";
            this.fi_need = ptw.fi_id != null ? "Yes" : "No";
            this.ex_need = ptw.ex_id != null ? "Yes" : "No";
            this.wh_need = ptw.wh_id != null ? "Yes" : "No";
            this.rad_need = ptw.rad_id != null ? "Yes" : "No";

            this.pre_job_1_spv = ptw.pre_job_1_spv == 1 ? "Yes" : (ptw.pre_job_1_spv == 2 ? "No" : (ptw.pre_job_1_spv == 0 ? "N/A" : ""));
            this.pre_job_2_spv = ptw.pre_job_2_spv == 1 ? "Yes" : (ptw.pre_job_2_spv == 2 ? "No" : (ptw.pre_job_2_spv == 0 ? "N/A" : ""));
            this.pre_job_3_spv = ptw.pre_job_3_spv == 1 ? "Yes" : (ptw.pre_job_3_spv == 2 ? "No" : (ptw.pre_job_3_spv == 0 ? "N/A" : ""));
            this.pre_job_4_spv = ptw.pre_job_4_spv == 1 ? "Yes" : (ptw.pre_job_4_spv == 2 ? "No" : (ptw.pre_job_4_spv == 0 ? "N/A" : ""));
            this.pre_job_5_spv = ptw.pre_job_5_spv == 1 ? "Yes" : (ptw.pre_job_5_spv == 2 ? "No" : (ptw.pre_job_5_spv == 0 ? "N/A" : ""));
            this.pre_job_6_spv = ptw.pre_job_6_spv == 1 ? "Yes" : (ptw.pre_job_6_spv == 2 ? "No" : (ptw.pre_job_6_spv == 0 ? "N/A" : ""));
            this.pre_job_7_spv = ptw.pre_job_7_spv == 1 ? "Yes" : (ptw.pre_job_7_spv == 2 ? "No" : (ptw.pre_job_7_spv == 0 ? "N/A" : ""));
            this.pre_job_1_fo = ptw.pre_job_1_fo == 1 ? "Yes" : (ptw.pre_job_1_fo == 2 ? "No" : (ptw.pre_job_1_fo == 0 ? "N/A" : ""));
            this.pre_job_2_fo = ptw.pre_job_2_fo == 1 ? "Yes" : (ptw.pre_job_2_fo == 2 ? "No" : (ptw.pre_job_2_fo == 0 ? "N/A" : ""));
            this.pre_job_3_fo = ptw.pre_job_3_fo == 1 ? "Yes" : (ptw.pre_job_3_fo == 2 ? "No" : (ptw.pre_job_3_fo == 0 ? "N/A" : ""));
            this.pre_job_4_fo = ptw.pre_job_4_fo == 1 ? "Yes" : (ptw.pre_job_4_fo == 2 ? "No" : (ptw.pre_job_4_fo == 0 ? "N/A" : ""));
            this.pre_job_5_fo = ptw.pre_job_5_fo == 1 ? "Yes" : (ptw.pre_job_5_fo == 2 ? "No" : (ptw.pre_job_5_fo == 0 ? "N/A" : ""));
            this.pre_job_6_fo = ptw.pre_job_6_fo == 1 ? "Yes" : (ptw.pre_job_6_fo == 2 ? "No" : (ptw.pre_job_6_fo == 0 ? "N/A" : ""));
            this.pre_job_7_fo = ptw.pre_job_7_fo == 1 ? "Yes" : (ptw.pre_job_7_fo == 2 ? "No" : (ptw.pre_job_7_fo == 0 ? "N/A" : ""));
            this.cancel_1_spv = ptw.cancel_1_spv == 1 ? "Yes" : (ptw.cancel_1_spv == 2 ? "No" : (ptw.cancel_1_spv == 0 ? "N/A" : ""));
            this.cancel_2_spv = ptw.cancel_2_spv == 1 ? "Yes" : (ptw.cancel_2_spv == 2 ? "No" : (ptw.cancel_2_spv == 0 ? "N/A" : ""));
            this.cancel_3_spv = ptw.cancel_3_spv == 1 ? "Yes" : (ptw.cancel_3_spv == 2 ? "No" : (ptw.cancel_3_spv == 0 ? "N/A" : ""));
            this.cancel_4_spv = ptw.cancel_4_spv == 1 ? "Yes" : (ptw.cancel_4_spv == 2 ? "No" : (ptw.cancel_4_spv == 0 ? "N/A" : ""));
            this.cancel_5_spv = ptw.cancel_5_spv == 1 ? "Yes" : (ptw.cancel_5_spv == 2 ? "No" : (ptw.cancel_5_spv == 0 ? "N/A" : ""));
            this.cancel_6_spv = ptw.cancel_6_spv == 1 ? "Yes" : (ptw.cancel_6_spv == 2 ? "No" : (ptw.cancel_6_spv == 0 ? "N/A" : ""));
            this.cancel_7_spv = ptw.cancel_7_spv == 1 ? "Yes" : (ptw.cancel_7_spv == 2 ? "No" : (ptw.cancel_7_spv == 0 ? "N/A" : ""));
            this.cancel_1_fo = ptw.cancel_1_fo == 1 ? "Yes" : (ptw.cancel_1_fo == 2 ? "No" : (ptw.cancel_1_fo == 0 ? "N/A" : ""));
            this.cancel_2_fo = ptw.cancel_2_fo == 1 ? "Yes" : (ptw.cancel_2_fo == 2 ? "No" : (ptw.cancel_2_fo == 0 ? "N/A" : ""));
            this.cancel_3_fo = ptw.cancel_3_fo == 1 ? "Yes" : (ptw.cancel_3_fo == 2 ? "No" : (ptw.cancel_3_fo == 0 ? "N/A" : ""));
            this.cancel_4_fo = ptw.cancel_4_fo == 1 ? "Yes" : (ptw.cancel_4_fo == 2 ? "No" : (ptw.cancel_4_fo == 0 ? "N/A" : ""));
            this.cancel_5_fo = ptw.cancel_5_fo == 1 ? "Yes" : (ptw.cancel_5_fo == 2 ? "No" : (ptw.cancel_5_fo == 0 ? "N/A" : ""));
            this.cancel_6_fo = ptw.cancel_6_fo == 1 ? "Yes" : (ptw.cancel_6_fo == 2 ? "No" : (ptw.cancel_6_fo == 0 ? "N/A" : ""));
            this.cancel_7_fo = ptw.cancel_7_fo == 1 ? "Yes" : (ptw.cancel_7_fo == 2 ? "No" : (ptw.cancel_7_fo == 0 ? "N/A" : ""));

            this.pre_job_notes = ptw.pre_job_notes;
            this.cancel_notes = ptw.cancel_notes;
            this.validity_period_start = ptw.validity_period_start != null ? ptw.validity_period_start.Value.ToString("MM/dd/yyyy hh:mm tt") : "-";
            this.validity_period_end = ptw.validity_period_end != null ? ptw.validity_period_end.Value.ToString("MM/dd/yyyy hh:mm tt") : "-";
            this.cancellation_date = ptw.cancellation_date != null ? ptw.cancellation_date.Value.ToString("MM/dd/yyyy hh:mm tt") : "-";
            this.acc_notes_ass_fo = ptw.acc_notes_ass_fo;
            this.acc_notes_fo_ass = ptw.acc_notes_fo_ass;
            this.can_notes_ass_fo = ptw.can_notes_ass_fo;

            int userId = 0;
            UserEntity users = null;
            Int32.TryParse(ptw.acc_ptw_requestor, out userId);
            this.acc_ptw_requestor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            userId = 0;
            Int32.TryParse(ptw.acc_supervisor, out userId);
            this.acc_supervisor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.acc_supervisor_approve != null && ptw.acc_supervisor_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.acc_supervisor_delegate, out userId);
                this.acc_supervisor_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            userId = 0;
            Int32.TryParse(ptw.acc_assessor, out userId);
            this.acc_assessor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.acc_assessor_approve != null && ptw.acc_assessor_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.acc_assessor_delegate, out userId);
                this.acc_assessor_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            userId = 0;
            Int32.TryParse(ptw.acc_fo, out userId);
            this.acc_fo = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.acc_fo_approve != null && ptw.acc_fo_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.acc_fo_delegate, out userId);
                this.acc_fo_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            userId = 0;
            Int32.TryParse(ptw.can_ptw_requestor, out userId);
            this.can_ptw_requestor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            userId = 0;
            Int32.TryParse(ptw.can_supervisor, out userId);
            this.can_supervisor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.can_supervisor_approve != null && ptw.can_supervisor_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.can_supervisor_delegate, out userId);
                this.can_supervisor_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            userId = 0;
            Int32.TryParse(ptw.can_assessor, out userId);
            this.can_assessor = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.can_assessor_approve != null && ptw.can_assessor_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.can_assessor_delegate, out userId);
                this.can_assessor_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            userId = 0;
            Int32.TryParse(ptw.can_fo, out userId);
            this.can_fo = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";

            if (ptw.can_fo_approve != null && ptw.can_fo_approve.ElementAt(0) == 'd')
            {
                userId = 0;
                Int32.TryParse(ptw.can_fo_delegate, out userId);
                this.can_fo_delegate = (users = listUser.listUser.Find(p => p.id == userId)) != null ? users.alpha_name : "";
            }

            if (ptw.status < (int)PtwEntity.statusPtw.ACCFO)
            {
                this.status = "ON GOING";
            }
            else if (ptw.status == (int)PtwEntity.statusPtw.ACCFO && ptw.permit_to_work1.Count == 0)
            {
                this.status = "COMPLETED";
            }
            else if (ptw.status == (int)PtwEntity.statusPtw.ACCFO && ptw.permit_to_work1.Count > 0)
            {
                this.status = "EXTENDED";
            }
            else if (ptw.status == (int)PtwEntity.statusPtw.CANREQ)
            {
                this.status = "CANCEL BY REQ.";
            }
            else if (ptw.status == (int)PtwEntity.statusPtw.CANFO)
            {
                this.status = "CANCELLED";
            }
            else if (ptw.status == (int)PtwEntity.statusPtw.CANCELLED)
            {
                this.status = "REQUEST CANCELLED";
            }
        }
    }
}