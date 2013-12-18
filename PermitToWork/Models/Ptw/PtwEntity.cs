using PermitToWork.Models.Hira;
using PermitToWork.Models.Hw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PermitToWork.Models.Ptw
{
    public class PtwEntity
    {
        public int id { get; set; }
        public string ptw_no { get; set; }
        public Nullable<System.DateTime> proposed_period_start { get; set; }
        public Nullable<System.DateTime> proposed_period_end { get; set; }
        public string dept_requestor { get; set; }
        public string section { get; set; }
        public string total_crew { get; set; }
        public string requestor_ptw_holder_no { get; set; }
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
        public int? status { get; set; }

        public byte is_extend { get; set; }
        public List<HiraEntity> hira_document { get; set; }
        public bool has_extend { get; set; }
        public string extend_ptw_no { get; set; }
        public int hw_need { get; set; }
        public string hw_no { get; set; }
        public bool has_clearance { get; set; }

        public HwEntity hw { get; set; }
        public string ptw_status { get; set; }

        public enum statusPtw
        {
            CREATE,
            CLEARANCECOMPLETE,
            ACCSPV,
            ACCASS,
            ACCFO,
            CANCEL,
            CANREQ,
            CANSPV,
            CANASS,
            CANFO
        };

        public enum statusClearance
        {
            NOTCOMPLETE,
            COMPLETE,
            CLOSE
        };

        public enum clearancePermit
        {
            LOCKOUTTAGOUT,
            CONFIRMSPACE,
            HOTWORK,
            FIREIMPAIRMENT,
            EXCAVATION,
            WORKINGHEIGHT,
            RADIOGRAPHY
        };

        private star_energy_ptwEntities db;

        public PtwEntity() {
            this.db = new star_energy_ptwEntities();
        }

        public PtwEntity(int id,star_energy_ptwEntities db = null)
        {
            if (db == null)
            {
                this.db = new star_energy_ptwEntities();
            }
            else
            {
                this.db = db;
            }
            permit_to_work ptw = this.db.permit_to_work.Find(id);
            this.id = ptw.id;
            this.ptw_no = ptw.ptw_no;
            this.proposed_period_start = ptw.proposed_period_start;
            this.proposed_period_end = ptw.proposed_period_end;
            this.dept_requestor = ptw.dept_requestor;
            this.section = ptw.section;
            this.total_crew = ptw.total_crew;
            this.requestor_ptw_holder_no = ptw.requestor_ptw_holder_no;
            this.area = ptw.area;
            this.work_location = ptw.work_location;
            this.area_code = ptw.area_code;
            this.work_order_no = ptw.work_order_no;
            this.work_description = ptw.work_description;
            this.notes = ptw.notes;
            this.pre_job_1_spv = ptw.pre_job_1_spv;
            this.pre_job_2_spv = ptw.pre_job_2_spv;
            this.pre_job_3_spv = ptw.pre_job_3_spv;
            this.pre_job_4_spv = ptw.pre_job_4_spv;
            this.pre_job_5_spv = ptw.pre_job_5_spv;
            this.pre_job_6_spv = ptw.pre_job_6_spv;
            this.pre_job_7_spv = ptw.pre_job_7_spv;
            this.pre_job_1_fo = ptw.pre_job_1_fo;
            this.pre_job_2_fo = ptw.pre_job_2_fo;
            this.pre_job_3_fo = ptw.pre_job_3_fo;
            this.pre_job_4_fo = ptw.pre_job_4_fo;
            this.pre_job_5_fo = ptw.pre_job_5_fo;
            this.pre_job_6_fo = ptw.pre_job_6_fo;
            this.pre_job_7_fo = ptw.pre_job_7_fo;
            this.cancel_1_spv = ptw.cancel_1_spv;
            this.cancel_2_spv = ptw.cancel_2_spv;
            this.cancel_3_spv = ptw.cancel_3_spv;
            this.cancel_4_spv = ptw.cancel_4_spv;
            this.cancel_5_spv = ptw.cancel_5_spv;
            this.cancel_6_spv = ptw.cancel_6_spv;
            this.cancel_7_spv = ptw.cancel_7_spv;
            this.cancel_1_fo = ptw.cancel_1_fo;
            this.cancel_2_fo = ptw.cancel_2_fo;
            this.cancel_3_fo = ptw.cancel_3_fo;
            this.cancel_4_fo = ptw.cancel_4_fo;
            this.cancel_5_fo = ptw.cancel_5_fo;
            this.cancel_6_fo = ptw.cancel_6_fo;
            this.cancel_7_fo = ptw.cancel_7_fo;
            this.pre_job_notes = ptw.pre_job_notes;
            this.cancel_notes = ptw.cancel_notes;
            this.validity_period_start = ptw.validity_period_start;
            this.validity_period_end = ptw.validity_period_end;
            this.cancellation_date = ptw.cancellation_date;
            this.loto_id = ptw.loto_id;
            this.loto_status = ptw.loto_status;
            this.csep_id = ptw.csep_id;
            this.csep_status = ptw.csep_status;
            this.hw_id = ptw.hw_id;
            this.hw_status = ptw.hw_status;
            this.fi_id = ptw.fi_id;
            this.fi_status = ptw.fi_status;
            this.ex_id = ptw.ex_id;
            this.ex_status = ptw.ex_status;
            this.wh_id = ptw.wh_id;
            this.wh_status = ptw.wh_status;
            this.rad_id = ptw.rad_id;
            this.rad_status = ptw.rad_status;
            this.acc_ptw_requestor = ptw.acc_ptw_requestor;
            this.acc_ptw_requestor_approve = ptw.acc_ptw_requestor_approve;
            this.acc_supervisor = ptw.acc_supervisor;
            this.acc_supervisor_delegate = ptw.acc_supervisor_delegate;
            this.acc_supervisor_approve = ptw.acc_supervisor_approve;
            this.acc_assessor = ptw.acc_assessor;
            this.acc_assessor_delegate = ptw.acc_assessor_delegate;
            this.acc_assessor_approve = ptw.acc_assessor_approve;
            this.acc_fo = ptw.acc_fo;
            this.acc_fo_delegate = ptw.acc_fo_delegate;
            this.acc_fo_approve = ptw.acc_fo_approve;
            this.can_ptw_requestor = ptw.can_ptw_requestor;
            this.can_ptw_requestor_approve = ptw.can_ptw_requestor_approve;
            this.can_supervisor = ptw.can_supervisor;
            this.can_supervisor_delegate = ptw.can_supervisor_delegate;
            this.can_supervisor_approve = ptw.can_supervisor_approve;
            this.can_assessor = ptw.can_assessor;
            this.can_assessor_delegate = ptw.can_assessor_delegate;
            this.can_assessor_approve = ptw.can_assessor_approve;
            this.can_fo = ptw.can_fo;
            this.can_fo_delegate = ptw.can_fo_delegate;
            this.can_fo_approve = ptw.can_fo_approve;
            this.id_parent_ptw = ptw.id_parent_ptw;
            this.is_extend = (byte)(ptw.id_parent_ptw != null ? 1 : 0);
            this.status = ptw.status;
            this.has_extend = ptw.permit_to_work1.Count > 0;
            this.extend_ptw_no = ptw.permit_to_work2 != null ? ptw.permit_to_work2.ptw_no : "";

            if (this.hw_id != null)
            {
                this.hw = new HwEntity(this.hw_id.Value);
                this.hw_no = ptw.hot_work.ElementAt(0).hw_no;
                this.has_clearance = true;
            }

            this.ptw_status = getPtwStatus();

            this.hira_document = new ListHira(this.id,this.db).listHira;
        }

        public int addPtw(int stat = 0)
        {
            this.db = new star_energy_ptwEntities();
            permit_to_work ptw = new permit_to_work
            {
                id = id,
                ptw_no = ptw_no,
                proposed_period_start = proposed_period_start,
                proposed_period_end = proposed_period_end,
                dept_requestor = dept_requestor,
                section = section,
                total_crew = total_crew,
                requestor_ptw_holder_no = requestor_ptw_holder_no,
                area = area,
                work_location = work_location,
                area_code = area_code,
                work_order_no = work_order_no,
                work_description = work_description,
                id_parent_ptw = id_parent_ptw,
                acc_ptw_requestor = acc_ptw_requestor,
                can_ptw_requestor = can_ptw_requestor,
                status = (int)statusPtw.CREATE,
            };

            if (stat == 1)
            {
                
                ptw.acc_supervisor = acc_supervisor;
                ptw.acc_supervisor_delegate = acc_supervisor_delegate;
                ptw.acc_assessor = acc_assessor;
                ptw.acc_assessor_delegate = acc_assessor_delegate;
                ptw.acc_fo = acc_fo;
                ptw.acc_fo_delegate = acc_fo_delegate;
                ptw.can_supervisor = can_supervisor;
                ptw.can_supervisor_delegate = can_supervisor_delegate;
                ptw.can_assessor = can_assessor;
                ptw.can_assessor_delegate = can_assessor_delegate;
                ptw.can_fo = can_fo;
                ptw.can_fo_delegate = can_fo_delegate;
            }

            db.permit_to_work.Add(ptw);
            int retVal = this.db.SaveChanges();
            this.id = ptw.id;
            return retVal;
        }

        public int editPtw()
        {
            permit_to_work ptw = db.permit_to_work.Find(id);
            ptw.id = id;
            ptw.proposed_period_start = proposed_period_start;
            ptw.proposed_period_end = proposed_period_end;
            ptw.dept_requestor = dept_requestor;
            ptw.section = section;
            ptw.total_crew = total_crew;
            ptw.requestor_ptw_holder_no = requestor_ptw_holder_no;
            ptw.area = area;
            ptw.work_location = work_location;
            ptw.area_code = area_code;
            ptw.work_order_no = work_order_no;
            ptw.work_description = work_description;
            ptw.notes = notes;
            ptw.pre_job_1_spv = pre_job_1_spv;
            ptw.pre_job_2_spv = pre_job_2_spv;
            ptw.pre_job_3_spv = pre_job_3_spv;
            ptw.pre_job_4_spv = pre_job_4_spv;
            ptw.pre_job_5_spv = pre_job_5_spv;
            ptw.pre_job_6_spv = pre_job_6_spv;
            ptw.pre_job_7_spv = pre_job_7_spv;
            ptw.pre_job_1_fo = pre_job_1_fo;
            ptw.pre_job_2_fo = pre_job_2_fo;
            ptw.pre_job_3_fo = pre_job_3_fo;
            ptw.pre_job_4_fo = pre_job_4_fo;
            ptw.pre_job_5_fo = pre_job_5_fo;
            ptw.pre_job_6_fo = pre_job_6_fo;
            ptw.pre_job_7_fo = pre_job_7_fo;
            ptw.cancel_1_spv = cancel_1_spv;
            ptw.cancel_2_spv = cancel_2_spv;
            ptw.cancel_3_spv = cancel_3_spv;
            ptw.cancel_4_spv = cancel_4_spv;
            ptw.cancel_5_spv = cancel_5_spv;
            ptw.cancel_6_spv = cancel_6_spv;
            ptw.cancel_7_spv = cancel_7_spv;
            ptw.cancel_1_fo = cancel_1_fo;
            ptw.cancel_2_fo = cancel_2_fo;
            ptw.cancel_3_fo = cancel_3_fo;
            ptw.cancel_4_fo = cancel_4_fo;
            ptw.cancel_5_fo = cancel_5_fo;
            ptw.cancel_6_fo = cancel_6_fo;
            ptw.cancel_7_fo = cancel_7_fo;
            ptw.pre_job_notes = pre_job_notes;
            ptw.cancel_notes = cancel_notes;
            ptw.validity_period_start = validity_period_start;
            ptw.validity_period_end = validity_period_end;
            ptw.cancellation_date = cancellation_date;
            //ptw.acc_ptw_requestor = acc_ptw_requestor;
            //ptw.acc_supervisor = acc_supervisor;
            //ptw.acc_supervisor_delegate = acc_supervisor_delegate;
            //ptw.acc_assessor = acc_assessor;
            //ptw.acc_assessor_delegate = acc_assessor_delegate;
            //ptw.acc_fo = acc_fo;
            //ptw.acc_fo_delegate = acc_fo_delegate;
            //ptw.can_ptw_requestor = can_ptw_requestor;
            //ptw.can_supervisor = can_supervisor;
            //ptw.can_supervisor_delegate = can_supervisor_delegate;
            //ptw.can_assessor = can_assessor;
            //ptw.can_assessor_delegate = can_assessor_delegate;
            //ptw.can_fo = can_fo;
            //ptw.can_fo_delegate = can_fo_delegate;
            //ptw.id_parent_ptw = id_parent_ptw;

            this.db.Entry(ptw).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int extendPtw(PtwEntity ptw)
        {
            this.generatePtwNumber(ptw.ptw_no, true);
            this.proposed_period_start = ptw.validity_period_end.Value.AddDays(1);
            this.proposed_period_end = ptw.validity_period_end.Value.AddDays(8);
            this.dept_requestor = ptw.dept_requestor;
            this.section = ptw.section;
            this.total_crew = ptw.total_crew;
            this.requestor_ptw_holder_no = ptw.requestor_ptw_holder_no;
            this.area = ptw.area;
            this.work_location = ptw.work_location;
            this.area_code = ptw.area_code;
            this.work_order_no = ptw.work_order_no;
            this.work_description = ptw.work_description;
            this.notes = ptw.notes;
            this.acc_ptw_requestor = ptw.acc_ptw_requestor;
            this.acc_supervisor = ptw.acc_supervisor;
            this.acc_supervisor_delegate = ptw.acc_supervisor_delegate;
            this.acc_assessor = ptw.acc_assessor;
            this.acc_assessor_delegate = ptw.acc_assessor_delegate;
            this.acc_fo = ptw.acc_fo;
            this.acc_fo_delegate = ptw.acc_fo_delegate;
            this.can_ptw_requestor = ptw.can_ptw_requestor;
            this.can_supervisor = ptw.can_supervisor;
            this.can_supervisor_delegate = ptw.can_supervisor_delegate;
            this.can_assessor = ptw.can_assessor;
            this.can_assessor_delegate = ptw.can_assessor_delegate;
            this.can_fo = ptw.can_fo;
            this.can_fo_delegate = ptw.can_fo_delegate;
            this.id_parent_ptw = ptw.id;
            return this.addPtw(1);
        }

        public string cancelPtw(UserEntity user)
        {
            if (user.id.ToString() == this.acc_ptw_requestor)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_ptw_requestor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CANREQ;
                ptw.cancellation_date = DateTime.Today;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.cancelPtw(user);
                }
                return "200";
            }
            else
            {
                return "404";
            }
        }

        public string getPtwStatus()
        {
            string retVal = "";

            if (this.status == (int)statusPtw.CREATE && this.has_clearance && !isAllClearanceComplete()) {
                retVal = "Waiting Clearance Permit Completed";
            } else if (this.status == (int)statusPtw.CREATE && this.has_clearance && !isAllClearanceComplete()) {
                retVal = "Waiting Approval by Requestor";
            } else if (this.status == (int)statusPtw.CREATE && !this.has_clearance) {
                retVal = "Waiting Approval by Requestor";
            } else if (this.status == (int)statusPtw.CLEARANCECOMPLETE) {
                retVal = "Waiting Approval by Supervisor";
            } else if (this.status == (int)statusPtw.ACCSPV) {
                retVal = "Waiting Approval by Assessor";
            } else if (this.status == (int)statusPtw.ACCASS) {
                retVal = "Waiting Approval by Facility Owner";
            } else if (this.status == (int)statusPtw.ACCFO) {
                retVal = "Completed. Has been approved by Facility Owner";
            } else if (this.status == (int)statusPtw.CANCEL) {
                retVal = "Cancellation Permit to Work. Waiting checking by requestor";
            } else if (this.status == (int)statusPtw.CANREQ) {
                retVal = "Waiting Cancellation Approval by Supervisor";
            } else if (this.status == (int)statusPtw.CANSPV) {
                retVal = "Waiting Cancellation Approval by Assessor";
            } else if (this.status == (int)statusPtw.CANASS) {
                retVal = "Waiting Cancellation Approval by Facility Owner";
            } else if (this.status == (int)statusPtw.CANFO) {
                retVal = "Cancelled. Permit has been approved to cancel by Facility Owner";
            }

            return retVal;            
        }

        #region generate ptw_number

        public void generatePtwNumber(string lastNumber, bool isExtend = false)
        {
            string result = "PTW-A-B-C-";
            int thisYear = DateTime.Today.Year;
            int lastNo = 0;
            int extension = 0;
            if (lastNumber != null)
            {
                string[] lastPtwNumberPart = lastNumber.Split('-');
                if (lastPtwNumberPart.Length == 6)
                {
                    lastNo = Int32.Parse(lastPtwNumberPart[5]) - 1;
                    if (Int32.Parse(lastPtwNumberPart[4]) == thisYear)
                    {
                        if (isExtend == false)
                        {
                            lastNo = Int32.Parse(lastPtwNumberPart[5]);
                        }
                    }
                    else
                    {
                        if (isExtend == false)
                        {
                            lastNo = 0;
                        }
                    }
                }
                else if (lastPtwNumberPart.Length == 7)
                {
                    if (Int32.Parse(lastPtwNumberPart[4]) == thisYear)
                    {
                        if (isExtend == true)
                        {
                            extension = lastPtwNumberPart[6][0] - 65 + 1;
                            lastNo = Int32.Parse(lastPtwNumberPart[5]) - 1;
                        }
                        else
                        {
                            lastNo = Int32.Parse(lastPtwNumberPart[5]);
                        }
                    }
                    else
                    {
                        if (isExtend == true)
                        {
                            extension = lastPtwNumberPart[6][0] - 65 + 1;
                            lastNo = Int32.Parse(lastPtwNumberPart[5]) - 1;
                        }
                        else
                        {
                            lastNo = 0;
                        }
                    }
                }
            }

            result += thisYear + "-" + (lastNo + 1).ToString().PadLeft(4, '0');
            if (isExtend)
            {
                result += "-" + (char)(extension + 65);
            }

            this.ptw_no = result;
        }

        #endregion

        #region check user in PTW

        public bool isRequestor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_ptw_requestor == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_supervisor == user_id || this.acc_supervisor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccAssessor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_assessor == user_id || this.acc_assessor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFO(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_fo == user_id || this.acc_fo_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_supervisor == user_id || this.can_supervisor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanAssessor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_assessor == user_id || this.can_assessor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanFO(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_fo == user_id || this.can_fo_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanEdit(UserEntity user)
        {
            bool isCanEdit = false;
            if (isRequestor(user) && this.status < (int)statusPtw.CLEARANCECOMPLETE) {
                isCanEdit = true;
            }

            if (isAccSupervisor(user) && this.status == (int)statusPtw.CLEARANCECOMPLETE)
            {
                isCanEdit = true;
            }

            if (isAccAssessor(user) && this.status == (int)statusPtw.ACCSPV)
            {
                isCanEdit = true;
            }

            if (isAccFO(user) && this.status == (int)statusPtw.ACCASS)
            {
                isCanEdit = true;
            }

            if (this.id == 0)
            {
                isCanEdit = true;
            }

            return isCanEdit;
        }

        public bool isCancelPtw()
        {
            return this.status == (int)statusPtw.CANCEL;
        }

        public int assignSupervisor(UserEntity supervisor)
        {
            this.acc_supervisor = supervisor.id.ToString();
            this.can_supervisor = supervisor.id.ToString();
            this.acc_supervisor_delegate = supervisor.employee_delegate.ToString();
            this.can_supervisor_delegate = supervisor.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_supervisor = this.acc_supervisor;
            ptw.can_supervisor = this.can_supervisor;
            ptw.acc_supervisor_delegate = this.acc_supervisor_delegate;
            ptw.can_supervisor_delegate = this.can_supervisor_delegate;

            this.db.Entry(ptw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignAssessor(UserEntity assessor)
        {
            this.acc_assessor = assessor.id.ToString();
            this.can_assessor = assessor.id.ToString();
            this.acc_assessor_delegate = assessor.employee_delegate.ToString();
            this.can_assessor_delegate = assessor.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_assessor = this.acc_assessor;
            ptw.can_assessor = this.can_assessor;
            ptw.acc_assessor_delegate = this.acc_assessor_delegate;
            ptw.can_assessor_delegate = this.can_assessor_delegate;

            this.db.Entry(ptw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignFO(UserEntity fo)
        {
            this.acc_fo = fo.id.ToString();
            this.can_fo = fo.id.ToString();
            this.acc_fo_delegate = fo.employee_delegate.ToString();
            this.can_fo_delegate = fo.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_fo = this.acc_fo;
            ptw.can_fo = this.can_fo;
            ptw.acc_fo_delegate = this.acc_fo_delegate;
            ptw.can_fo_delegate = this.can_fo_delegate;

            this.db.Entry(ptw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public bool isUserInPtw(UserEntity user)
        {
            return (isRequestor(user) || isAccSupervisor(user) || isAccAssessor(user) || isAccFO(user) ||
                    isCanSupervisor(user) || isAccAssessor(user) || isCanFO(user));
        }

        #endregion

        #region check all clearence complete

        public bool isAllClearanceComplete()
        {
            var isComplete = true;

            // check if LOTO
            if (this.loto_id != null && this.loto_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check CSEP
            if (this.csep_id != null && this.csep_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check HW
            if (this.hw_id != null && this.hw_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check FI
            if (this.fi_id != null && this.fi_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check Ex
            if (this.ex_id != null && this.ex_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check WH
            if (this.wh_id != null && this.wh_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            // check Rad
            if (this.rad_id != null && this.rad_status != (int)statusClearance.COMPLETE)
            {
                isComplete = false;
            }

            return isComplete;
        }

        public bool isAllClearanceClose()
        {
            var isClose = true;

            // check if LOTO
            if (this.loto_id != null && this.loto_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check CSEP
            if (this.csep_id != null && this.csep_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check HW
            if (this.hw_id != null && this.hw_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check FI
            if (this.fi_id != null && this.fi_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check Ex
            if (this.ex_id != null && this.ex_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check WH
            if (this.wh_id != null && this.wh_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check Rad
            if (this.rad_id != null && this.rad_status != (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            return isClose;
        }

        #endregion

        #region approval rejection

        public string requestorAccApproval(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            if (user.id.ToString() == this.acc_ptw_requestor)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_ptw_requestor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CLEARANCECOMPLETE;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string supervisorAccApproval(UserEntity user)
        {
            // supervisor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}
            //               401 {not select assessor}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            if (ptw.acc_assessor == null)
            {
                return "401";
            }

            if (user.id.ToString() == this.acc_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.acc_supervisor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.ACCSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            } 
            else if (user.id.ToString() == this.acc_supervisor_delegate) 
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.acc_supervisor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.ACCSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string supervisorAccReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_supervisor || user.id.ToString() == this.acc_supervisor_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_ptw_requestor_approve = null;
                ptw.status = (int)statusPtw.CREATE;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string assessorAccApproval(UserEntity user)
        {
            // assessor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}
            //               401 {not select facility owner}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            if (ptw.acc_fo == null)
            {
                return "401";
            }
            
            if (user.id.ToString() == this.acc_assessor)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_assessor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.ACCASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.acc_assessor_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_assessor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.ACCASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string assessorAccReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_assessor || user.id.ToString() == this.acc_assessor_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_supervisor_approve = null;
                ptw.status = (int)statusPtw.CLEARANCECOMPLETE;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fOAccApproval(UserEntity user)
        {
            // FO approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            if (user.id.ToString() == this.acc_fo)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_fo_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.ACCFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.acc_fo_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_fo_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.ACCFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string fOAccReject(UserEntity user, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_fo || user.id.ToString() == this.acc_fo_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_assessor_approve = null;
                ptw.status = (int)statusPtw.ACCSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string requestorCanApproval(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            if (user.id.ToString() == this.can_ptw_requestor)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_ptw_requestor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CANREQ;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.requestorCanApproval(user);
                }
                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string supervisorCanApproval(UserEntity user)
        {
            // supervisor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}
            string retVal = "";
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            this.id_parent_ptw = ptw.id_parent_ptw;
            if (user.id.ToString() == this.can_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.can_supervisor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CANSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.supervisorCanApproval(user);
                }
                retVal = "200";
            }
            else if (user.id.ToString() == this.can_supervisor_delegate)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.can_supervisor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.supervisorCanApproval(user);
                }
                retVal = "201";
            }
            else
            {
                retVal = "400";
            }
            return retVal;
        }

        public string supervisorCanReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_supervisor || user.id.ToString() == this.can_supervisor_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_ptw_requestor_approve = null;
                ptw.status = (int)statusPtw.CANCEL;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.supervisorCanReject(user,comment);
                }
                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string assessorCanApproval(UserEntity user)
        {
            // assessor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}
            //               401 {not select facility owner}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            this.id_parent_ptw = ptw.id_parent_ptw;
            if (user.id.ToString() == this.can_assessor)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                
                ptw.can_assessor_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CANASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.assessorCanApproval(user);
                }
                return "200";
            }
            else if (user.id.ToString() == this.can_assessor_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.can_assessor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.assessorCanApproval(user);
                }
                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string assessorCanReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_assessor || user.id.ToString() == this.can_assessor_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_supervisor_approve = null;
                ptw.status = (int)statusPtw.CANREQ;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.assessorCanReject(user,comment);
                }
                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fOCanApproval(UserEntity user)
        {
            // FO approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            this.id_parent_ptw = ptw.id_parent_ptw;
            if (user.id.ToString() == this.can_fo)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.can_fo_approve = "a" + user.signature;
                ptw.status = (int)statusPtw.CANFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.fOCanApproval(user);
                }
                return "200";
            }
            else if (user.id.ToString() == this.can_fo_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.can_fo_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.fOCanApproval(user);
                }
                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string fOCanReject(UserEntity user, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_fo || user.id.ToString() == this.can_fo_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);

                ptw.can_assessor_approve = null;
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.status = (int)statusPtw.CANSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value);
                    pt.fOCanReject(user,comment);
                }
                return "200";
            }
            else
            {
                return "400";
            }
        }

        #endregion

        #region send email

        public string sendEmailSpv(List<UserEntity> listSpv, string serverUrl)
        {
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailsupervisor";
            SendEmail sendEmail = new SendEmail();
            foreach (UserEntity spv in listSpv)
            {
                List<string> s = new List<string>();

                s.Add("septu.jamasoka@gmail.com");

                string encodedValue = salt + spv.id + val + this.id;
                string encodedElement = Base64.Base64Encode(encodedValue);

                string seal = Base64.MD5Seal(timestamp + salt + val);

                string message = serverUrl + "Ptw/SetSupervisor?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                sendEmail.Send(s, message, "ptw supervisor");
            }

            return "200";
        }

        public string sendEmailRequestor(string serverUrl, int stat = 0, string comment = null)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor));
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Requestor Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailSupervisor(string serverUrl, int pos = 0, int stat = 0, string comment = null)
        {
            int supervisor_id = 0;

            if (pos == 1)
                supervisor_id = Int32.Parse(this.acc_supervisor);
            else
                supervisor_id = Int32.Parse(this.can_supervisor);
            UserEntity supervisor = new UserEntity(supervisor_id);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Supervisor Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailAssessor(string serverUrl, int pos = 0, int stat = 0, string comment = null)
        {
            int assessor_id = 0;

            if (pos == 1)
                assessor_id = Int32.Parse(this.acc_assessor);
            else
                assessor_id = Int32.Parse(this.can_assessor);
            UserEntity assessor = new UserEntity(assessor_id);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(assessor.email);
            s.Add("septu.jamasoka@gmail.com");

            if (assessor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(assessor.employee_delegate.Value);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Assessor Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFo(string serverUrl, int pos = 0, int stat = 0, string comment = null)
        {
            int fo_id = 0;

            if (pos == 1)
                fo_id = Int32.Parse(this.acc_fo);
            else
                fo_id = Int32.Parse(this.can_fo);

            UserEntity facilityOwner = new UserEntity(fo_id);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(facilityOwner.email);
            s.Add("septu.jamasoka@gmail.com");

            if (facilityOwner.employee_delegate != null)
            {
                UserEntity del = new UserEntity(facilityOwner.employee_delegate.Value);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id;
                subject = "Permit to Work Facility Owner Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailRequestorClearance(string serverUrl, int clearance_permit, int status)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor));
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;
            string subject = "Permit to Work Requestor Approve";

            if (status == 1)
            {
                switch (clearance_permit)
                {
                    case (int)clearancePermit.HOTWORK:
                        message = "Hot Work Permit has approved by facility owner.<br />" + serverUrl + "Home?p=Hw/edit/" + this.hw_id;
                        subject = "Hot Work Permit Number " + hw_no + " Approved";
                        break;
                }
            }
            else if (status == 2)
            {
                switch (clearance_permit)
                {
                    case (int)clearancePermit.HOTWORK:
                        message = "Hot Work Permit has approved to close by facility owner.<br />" + serverUrl + "Home?p=Hw/edit/" + this.hw_id;
                        subject = "Hot Work Permit Number " + hw_no + " Closed";
                        break;
                }
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailRequestorClearanceCompleted(string serverUrl, int status)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor));
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = "All clearance permit has been approved, you may continue by approving this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
            string subject = "All Clearance Permit Approved";

            if (status == 1)
            {
                message = "All clearance permit has been approved, you may continue by approving this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "All Clearance Permit Approved";
            }
            else if (status == 2)
            {
                message = "All clearance permit has been closed, you may continue by cancelling this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "All Clearance Permit Closed";
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailRequestorPermitCompleted(string serverUrl, int status)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor));
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = "Permit to Work has been approved, you may start working using this permit.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
            string subject = "Permit to Work Approved";

            if (status == 1)
            {
                message = "Permit to Work has been approved, you may start working using this permit.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "Permit to Work Approved";
            }
            else if (status == 2)
            {
                message = "Permit to Work has been cancelled. You cannot work using this permit again.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "Permit to Work Cancelled";
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        #endregion

        #region clearance permit

        public string setHw(int? hw_id, int? status)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            this.hw_id = hw_id;
            this.hw_status = status;

            ptw.hw_id = this.hw_id;
            ptw.hw_status = this.hw_status;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        public string setHwStatus(int status)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            this.hw_status = status;

            ptw.hw_status = this.hw_status;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        public string hwStatus()
        {
            string retVal = "";
            switch (this.hw_status) {
                case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                case (int)statusClearance.CLOSE: retVal = "<span class='label label-error'>Closed</span>"; break;
            };

            return retVal;
        }

        #endregion
    }
}