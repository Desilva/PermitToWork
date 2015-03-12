using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hira;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Master;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.SafetyBriefing;
using PermitToWork.Models.User;
using PermitToWork.Models.Workflow;
using PermitToWork.Models.WorkingHeight;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PermitToWork.Models.Ptw
{
    public class PtwEntity : permit_to_work
    {
        public override ICollection<confined_space> confined_space { get { return null; } set { } }
        public override ICollection<excavation> excavations { get { return null; } set { } }
        public override ICollection<fire_impairment> fire_impairment { get { return null; } set { } }
        public override ICollection<hira_document> hira_document { get { return null; } set { } }
        public override ICollection<hot_work> hot_work { get { return null; } set { } }
        public override loto_glarf loto_glarf { get { return null; } set { } }
        public override mst_department mst_department { get { return null; } set { } }
        public override mst_ptw_holder_no mst_ptw_holder_no { get { return null; } set { } }
        public override mst_section mst_section { get { return null; } set { } }
        public override ICollection<permit_to_work> permit_to_work1 { get { return null; } set { } }
        public override permit_to_work permit_to_work2 { get { return null; } set { } }
        public override ICollection<radiography> radiographies { get { return null; } set { } }
        public override ICollection<safety_briefing> safety_briefing { get { return null; } set { } }
        public override ICollection<working_height> working_height { get { return null; } set { } }
        public override ICollection<loto_permit> loto_permit { get { return null; } set { } }


        public byte is_extend { get; set; }
        // public List<HiraEntity> hira_document { get; set; }
        public bool has_extend { get; set; }
        public string extend_ptw_no { get; set; }
        public int hw_need { get; set; }
        public string hw_no { get; set; }
        public int fi_need { get; set; }
        public string fi_no { get; set; }
        public int rad_need { get; set; }
        public string rad_no { get; set; }
        public int wh_need { get; set; }
        public string wh_no { get; set; }
        public int ex_need { get; set; }
        public string ex_no { get; set; }
        public int csep_need { get; set; }
        public string csep_no { get; set; }
        public string loto_no { get; set; }
        public string loto_statusText { get; set; }
        public bool has_clearance { get; set; }
        public int? id_safety_briefing { get; set; }
        public int? safety_briefing_status { get; set; }
        public bool isNeedClose { get; set; }
        public bool isUser { get; set; }

        public bool isUserPtw { get; set; }
        public bool will_overdue { get; set; }

        public List<FIEntity> fireImpairments { get; set; }

        public MstDepartmentEntity department { get; set; }
        public MstSectionEntity section1 { get; set; }
        public MstPtwHolderNoEntity ptw_holder_no { get; set; }

        //public HwEntity hw { get; set; }
        //public FIEntity fi { get; set; }
        //public RadEntity rad { get; set; }

        public Dictionary<string, IClearancePermitEntity> cPermit { get; set; }
        public List<LotoEntity> lotoPermit { get; set; }
        public string ptw_status { get; set; }

        public Dictionary<string, UserEntity> userInPTW { get; set; }

        public WorkflowNodeServiceModel workflowNodeService { get; set; }

        public enum statusPtw
        {
            CREATE, // 0
            GUESTCOMPLETE,// 1
            CLEARANCECOMPLETE,// 2
            ACCSPV,// 3
            CHOOSEASS,// 4
            ACCASS,// 5
            ACCFO,// 6
            CANCEL,// 7
            CANREQ,// 8
            CANSPV,// 9
            CANASS,// 10
            CANFO,// 11
            CANCELLED// 12
        };

        public enum statusClearance
        {
            NOTCOMPLETE,
            COMPLETE,
            CLOSE,
            REQUESTORCANCELLED
        };

        public enum clearancePermit
        {
            LOCKOUTTAGOUT,
            CONFINEDSPACE,
            HOTWORK,
            FIREIMPAIRMENT,
            EXCAVATION,
            WORKINGHEIGHT,
            RADIOGRAPHY
        };

        public enum UserInPTW
        {
            REQUESTOR,
            SUPERVISOR,
            ASSESSOR,
            FACILITYOWNER,
            REQUESTORDELEGATE,
            SUPERVISORDELEGATE,
            ASSESSORDELEGATE,
            FACILITYOWNERDELEGATE,
            CANASSESSOR,
            CANASSESSORDELEGATE,
        }

        private star_energy_ptwEntities db;

        public PtwEntity() {
            this.db = new star_energy_ptwEntities();
            this.cPermit = new Dictionary<string, IClearancePermitEntity>();
            this.userInPTW = new Dictionary<string, UserEntity>();
            this.workflowNodeService = new WorkflowNodeServiceModel();
        }

        public PtwEntity(int id, UserEntity user, star_energy_ptwEntities db = null)
            : this()
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
            ModelUtilization.Clone(ptw, this);
            this.is_extend = (byte)(ptw.id_parent_ptw != null ? 1 : 0);
            this.has_extend = ptw.permit_to_work1.Count > 0;
            this.extend_ptw_no = ptw.permit_to_work2 != null ? ptw.permit_to_work2.ptw_no : "";
            if (this.is_guest != 1)
            {
                if (ptw.mst_ptw_holder_no != null)
                {
                    this.ptw_holder_no = new MstPtwHolderNoEntity(ptw.mst_ptw_holder_no);
                }
            }
            this.section1 = new MstSectionEntity(ptw.mst_section);
            this.department = new MstDepartmentEntity(ptw.mst_department);

            if (this.hw_id != null)
            {
                IClearancePermitEntity hw = (IClearancePermitEntity)new HwEntity(this.hw_id.Value);
                hw.ids = this.hw_id.Value;
                hw.statusText = ((HwEntity)hw).getStatus();
                this.cPermit.Add(clearancePermit.HOTWORK.ToString(), hw);
                this.hw_no = ptw.hot_work.ElementAt(0).hw_no;
                this.has_clearance = true;
            }

            if (ptw.fire_impairment.Count > 0)
            {
                fireImpairments = new List<FIEntity>();
                foreach (fire_impairment fi in ptw.fire_impairment)
                {
                    fireImpairments.Add(new FIEntity(fi,user));
                }
            }

            if (this.fi_id != null)
            {
                IClearancePermitEntity fi = (IClearancePermitEntity)new FIEntity(this.fi_id.Value, user);
                fi.ids = this.fi_id.Value;
                fi.statusText = ((FIEntity)fi).getStatus();
                this.cPermit.Add(clearancePermit.FIREIMPAIRMENT.ToString(), fi);
                this.fi_no = ptw.fire_impairment.ElementAt(0).fi_no;
                this.has_clearance = true;
            }

            if (this.rad_id != null)
            {
                IClearancePermitEntity rad = (IClearancePermitEntity)new RadEntity(this.rad_id.Value, user);
                rad.ids = this.rad_id.Value;
                this.cPermit.Add(clearancePermit.RADIOGRAPHY.ToString(), rad);
                this.rad_no = ptw.radiographies.ElementAt(0).rg_no;
                this.has_clearance = true;
            }

            if (this.wh_id != null)
            {
                IClearancePermitEntity wh = (IClearancePermitEntity)new WorkingHeightEntity(this.wh_id.Value, user);
                wh.ids = this.wh_id.Value;
                this.cPermit.Add(clearancePermit.WORKINGHEIGHT.ToString(), wh);
                this.wh_no = ptw.working_height.ElementAt(0).wh_no;
                this.has_clearance = true;
            }

            if (this.ex_id != null)
            {
                IClearancePermitEntity ex = (IClearancePermitEntity)new ExcavationEntity(this.ex_id.Value, user);
                ex.ids = this.ex_id.Value;
                this.cPermit.Add(clearancePermit.EXCAVATION.ToString(), ex);
                this.ex_no = ptw.excavations.ElementAt(0).ex_no;
                this.has_clearance = true;
            }

            if (this.csep_id != null)
            {
                IClearancePermitEntity csep = (IClearancePermitEntity)new CsepEntity(this.csep_id.Value, user);
                csep.ids = this.csep_id.Value;
                this.cPermit.Add(clearancePermit.CONFINEDSPACE.ToString(), csep);
                this.csep_no = ptw.confined_space.ElementAt(0).csep_no;
                this.has_clearance = true;
            }

            if (this.loto_need != null && this.loto_need != 0)
            {
                this.lotoPermit = new List<LotoEntity>();
                foreach (loto_permit loto in ptw.loto_permit)
                {
                    this.lotoPermit.Add(new LotoEntity(loto.id, user, this.acc_ptw_requestor));
                }
                this.loto_no = "";
                this.loto_statusText = this.loto_status == 0 ? "LOTO Permit is being edited by Requestor" : (this.loto_status == 1 ? "LOTO Permit is Approved" : (this.loto_status == 2 ? "LOTO Permit is Cancelled" : ""));
                this.has_clearance = true;
            }

            if (ptw.safety_briefing.Count > 0)
            {
                this.id_safety_briefing = ptw.safety_briefing.ElementAt(0).id;
                this.safety_briefing_status = ptw.safety_briefing.ElementAt(0).status;
            }

            this.ptw_status = getPtwStatus();
            if (this.acc_ptw_requestor != null || this.acc_supervisor != null)
            {
                var ptwRequestor = this.is_guest != 1 ? new UserEntity(Int32.Parse(this.acc_ptw_requestor), user.token, user) : new UserEntity(Int32.Parse(this.acc_supervisor), user.token, user);
                this.isNeedClose = this.status == (int)statusPtw.ACCFO && this.validity_period_end != null && this.validity_period_end.Value.CompareTo(DateTime.Now) < 0 && this.is_extend != 1 && (ptwRequestor.id == user.id || ptwRequestor.employee_delegate == user.id) ? true : false;
            }

            this.acc_fo = ptw.acc_fo;
            this.generateUserInPTW(user);
            // this.hira_document = new ListHira(this.id,this.db).listHira;
        }

        public PtwEntity(int id, UserEntity user, ListUser listUser, star_energy_ptwEntities db = null)
            : this()
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
            ModelUtilization.Clone(ptw, this);
            this.is_extend = (byte)(ptw.id_parent_ptw != null ? 1 : 0);
            this.has_extend = ptw.permit_to_work1.Count > 0;
            this.extend_ptw_no = ptw.permit_to_work2 != null ? ptw.permit_to_work2.ptw_no : "";
            if (this.is_guest != 1)
            {
                if (ptw.mst_ptw_holder_no != null)
                {
                    this.ptw_holder_no = new MstPtwHolderNoEntity(ptw.mst_ptw_holder_no);
                }
            }
            this.section1 = new MstSectionEntity(ptw.mst_section);
            this.department = new MstDepartmentEntity(ptw.mst_department);

            if (this.hw_id != null)
            {
                IClearancePermitEntity hw = (IClearancePermitEntity)new HwEntity(this.hw_id.Value);
                hw.ids = this.hw_id.Value;
                hw.statusText = ((HwEntity)hw).getStatus();
                this.cPermit.Add(clearancePermit.HOTWORK.ToString(), hw);
                this.hw_no = ptw.hot_work.ElementAt(0).hw_no;
                this.has_clearance = true;
            }

            //if (ptw.fire_impairment.Count > 0)
            //{
            //    fireImpairments = new List<FIEntity>();
            //    foreach (fire_impairment fi in ptw.fire_impairment)
            //    {
            //        fireImpairments.Add(new FIEntity(fi, user));
            //    }
            //}

            if (this.fi_id != null)
            {
                IClearancePermitEntity fi = (IClearancePermitEntity)new FIEntity(this.fi_id.Value, user);
                fi.ids = this.fi_id.Value;
                fi.statusText = ((FIEntity)fi).getStatus();
                this.cPermit.Add(clearancePermit.FIREIMPAIRMENT.ToString(), fi);
                this.fi_no = ptw.fire_impairment.ElementAt(0).fi_no;
                this.has_clearance = true;
            }

            if (this.rad_id != null)
            {
                IClearancePermitEntity rad = (IClearancePermitEntity)new RadEntity(this.rad_id.Value, user);
                rad.ids = this.rad_id.Value;
                this.cPermit.Add(clearancePermit.RADIOGRAPHY.ToString(), rad);
                this.rad_no = ptw.radiographies.ElementAt(0).rg_no;
                this.has_clearance = true;
            }

            if (this.wh_id != null)
            {
                IClearancePermitEntity wh = (IClearancePermitEntity)new WorkingHeightEntity(this.wh_id.Value, user);
                wh.ids = this.wh_id.Value;
                this.cPermit.Add(clearancePermit.WORKINGHEIGHT.ToString(), wh);
                this.wh_no = ptw.working_height.ElementAt(0).wh_no;
                this.has_clearance = true;
            }

            if (this.ex_id != null)
            {
                IClearancePermitEntity ex = (IClearancePermitEntity)new ExcavationEntity(this.ex_id.Value, user);
                ex.ids = this.ex_id.Value;
                this.cPermit.Add(clearancePermit.EXCAVATION.ToString(), ex);
                this.ex_no = ptw.excavations.ElementAt(0).ex_no;
                this.has_clearance = true;
            }

            if (this.csep_id != null)
            {
                IClearancePermitEntity csep = (IClearancePermitEntity)new CsepEntity(this.csep_id.Value, user);
                csep.ids = this.csep_id.Value;
                this.cPermit.Add(clearancePermit.CONFINEDSPACE.ToString(), csep);
                this.csep_no = ptw.confined_space.ElementAt(0).csep_no;
                this.has_clearance = true;
            }

            if (this.loto_need != null && this.loto_need != 0)
            {
                this.lotoPermit = new List<LotoEntity>();
                foreach (loto_permit loto in ptw.loto_permit)
                {
                    this.lotoPermit.Add(new LotoEntity(loto.id, user, this.acc_ptw_requestor));
                }
                this.loto_no = "";
                this.loto_statusText = this.loto_status == 0 ? "LOTO Permit is being edited by Requestor" : (this.loto_status == 1 ? "LOTO Permit is Approved" : (this.loto_status == 2 ? "LOTO Permit is Cancelled" : ""));
                this.has_clearance = true;
            }

            if (ptw.safety_briefing.Count > 0)
            {
                this.id_safety_briefing = ptw.safety_briefing.ElementAt(0).id;
                this.safety_briefing_status = ptw.safety_briefing.ElementAt(0).status;
            }

            this.ptw_status = getPtwStatus();
            if (this.acc_ptw_requestor != null || this.acc_supervisor != null)
            {
                var ptwRequestor = this.is_guest != 1 ? new UserEntity(Int32.Parse(this.acc_ptw_requestor), user.token, user) : new UserEntity(Int32.Parse(this.acc_supervisor), user.token, user);
                this.isNeedClose = this.status == (int)statusPtw.ACCFO && this.validity_period_end != null && this.validity_period_end.Value.CompareTo(DateTime.Now) < 0 && this.is_extend != 1 && (ptwRequestor.id == user.id || ptwRequestor.employee_delegate == user.id) ? true : false;
            }

            this.acc_fo = ptw.acc_fo;
            this.generateUserInPTW(user, listUser);
            // this.hira_document = new ListHira(this.id,this.db).listHira;
        }

        public PtwEntity(permit_to_work ptw, UserEntity user, ListUser listUser, bool isProd = false)
            : this()
        {
            // ModelUtilization.Clone(ptw, this);
            this.id = ptw.id;
            this.work_description = ptw.work_description;
            this.ptw_no = ptw.ptw_no;
            this.hw_id = ptw.hw_id;
            this.csep_id = ptw.csep_id;
            this.fi_id = ptw.fi_id;
            this.wh_id = ptw.wh_id;
            this.ex_id = ptw.ex_id;
            this.rad_id = ptw.rad_id;
            this.loto_need = ptw.loto_need;
            this.acc_ptw_requestor = ptw.acc_ptw_requestor;
            this.acc_assessor = ptw.acc_assessor;
            this.acc_supervisor = ptw.acc_supervisor;
            this.acc_fo = ptw.acc_fo;
            this.can_assessor = ptw.can_assessor;
            this.proposed_period_end = ptw.proposed_period_end;
            this.validity_period_end = ptw.validity_period_end;
            this.acc_supervisor_delegate = ptw.acc_supervisor_delegate;
            this.acc_assessor_delegate = ptw.acc_assessor_delegate;
            this.acc_fo_delegate = ptw.acc_fo_delegate;
            this.can_assessor_delegate = ptw.can_assessor_delegate;
            this.status = ptw.status;
            this.department = new MstDepartmentEntity(ptw.mst_department);
            this.is_extend = (byte)(ptw.id_parent_ptw != null ? 1 : 0);
            this.has_extend = ptw.permit_to_work1.Count > 0;
            Debug.WriteLine("d = " + DateTime.Now.TimeOfDay.TotalMilliseconds);
            this.isUserPtw = false;

            if (this.hw_id != null)
            {
                IClearancePermitEntity hw = (IClearancePermitEntity)new HwEntity(ptw.hot_work.ElementAt(0));
                hw.ids = this.hw_id.Value;
                hw.statusText = ((HwEntity)hw).getStatus();
                this.cPermit.Add(clearancePermit.HOTWORK.ToString(), hw);
                this.hw_no = ptw.hot_work.ElementAt(0).hw_no;
                this.has_clearance = true;
            }

            if (this.fi_id != null)
            {
                IClearancePermitEntity fi = (IClearancePermitEntity)new FIEntity(ptw.fire_impairment.ElementAt(0), listUser, user);
                fi.ids = this.fi_id.Value;
                this.isUserPtw = this.isUserPtw || fi.isUser;
                fi.statusText = ((FIEntity)fi).getStatus();
                this.cPermit.Add(clearancePermit.FIREIMPAIRMENT.ToString(), fi);
                this.fi_no = ptw.fire_impairment.ElementAt(0).fi_no;
                this.has_clearance = true;
            }

            if (this.rad_id != null)
            {
                IClearancePermitEntity rad = (IClearancePermitEntity)new RadEntity(ptw.radiographies.ElementAt(0), listUser, user);
                rad.ids = this.rad_id.Value;
                this.isUserPtw = this.isUserPtw || rad.isUser;
                this.cPermit.Add(clearancePermit.RADIOGRAPHY.ToString(), rad);
                this.rad_no = ptw.radiographies.ElementAt(0).rg_no;
                this.has_clearance = true;
            }

            if (this.wh_id != null)
            {
                IClearancePermitEntity wh = (IClearancePermitEntity)new WorkingHeightEntity(ptw.working_height.ElementAt(0), listUser, user);
                wh.ids = this.wh_id.Value;
                this.isUserPtw = this.isUserPtw || wh.isUser;
                this.cPermit.Add(clearancePermit.WORKINGHEIGHT.ToString(), wh);
                this.wh_no = ptw.working_height.ElementAt(0).wh_no;
                this.has_clearance = true;
            }

            if (this.ex_id != null)
            {
                IClearancePermitEntity ex = (IClearancePermitEntity)new ExcavationEntity(ptw.excavations.ElementAt(0), listUser, user);
                ex.ids = this.ex_id.Value;
                this.isUserPtw = this.isUserPtw || ex.isUser;
                this.cPermit.Add(clearancePermit.EXCAVATION.ToString(), ex);
                this.ex_no = ptw.excavations.ElementAt(0).ex_no;
                this.has_clearance = true;
            }

            if (this.csep_id != null)
            {
                IClearancePermitEntity csep = (IClearancePermitEntity)new CsepEntity(ptw.confined_space.ElementAt(0), user);
                csep.ids = this.csep_id.Value;
                this.cPermit.Add(clearancePermit.CONFINEDSPACE.ToString(), csep);
                this.csep_no = ptw.confined_space.ElementAt(0).csep_no;
                this.has_clearance = true;
            }

            if (this.loto_need != null && this.loto_need != 0)
            {
                //this.lotoPermit = new List<LotoEntity>();
                //foreach (loto_permit loto in ptw.loto_permit)
                //{
                //    this.lotoPermit.Add(new LotoEntity(loto, user, this.acc_ptw_requestor, listUser));
                //}
                //this.loto_no = "";
                //this.loto_statusText = this.loto_status == 0 ? "LOTO Permit is being edited by Requestor" : (this.loto_status == 1 ? "LOTO Permit is Approved" : (this.loto_status == 2 ? "LOTO Permit is Cancelled" : ""));
                //this.has_clearance = true;
            }

            this.ptw_status = getPtwStatus();
            if (this.acc_ptw_requestor != null || this.acc_supervisor != null)
            {
                var ptwRequestor = ptw.is_guest != 1 ? listUser.listUser.Find(p => p.id == Int32.Parse(this.acc_ptw_requestor)) : listUser.listUser.Find(p => p.id == Int32.Parse(this.acc_supervisor));
                // this.isNeedClose = this.status == (int)statusPtw.ACCFO && ptw.validity_period_end != null && ptw.validity_period_end.Value.CompareTo(DateTime.Now) < 0 && ptw.id_parent_ptw != null && (ptwRequestor.id == user.id || ptwRequestor.employee_delegate == user.id) ? true : false;
            }
            this.will_overdue = !this.has_extend && this.validity_period_end != null && this.validity_period_end.Value.Subtract(DateTime.Today).TotalDays < 2 && this.status >= (int)statusPtw.ACCFO && this.status < (int)statusPtw.CANFO;
            Debug.WriteLine(this.will_overdue);
            this.generateUserInPTW(user,listUser);
            Debug.WriteLine("e = " + DateTime.Now.TimeOfDay.TotalMilliseconds);
        }

        /// <summary>
        /// for checking if need to be closed first
        /// </summary>
        /// <param name="ptw"></param>
        /// <param name="user"></param>
        /// <param name="isCheck"></param>
        /// <param name="listUser"></param>
        public PtwEntity(permit_to_work ptw, UserEntity user, bool isCheck, ListUser listUser)
            : this()
        {
            // ModelUtilization.Clone(ptw, this);
            this.id = ptw.id;
            this.work_description = ptw.work_description;
            this.ptw_no = ptw.ptw_no;
            this.acc_ptw_requestor = ptw.acc_ptw_requestor;
            this.acc_assessor = ptw.acc_assessor;
            this.acc_supervisor = ptw.acc_supervisor;
            this.acc_fo = ptw.acc_fo;
            this.can_assessor = ptw.can_assessor;
            this.proposed_period_end = ptw.proposed_period_end;
            this.validity_period_end = ptw.validity_period_end;
            this.acc_supervisor_delegate = ptw.acc_supervisor_delegate;
            this.acc_assessor_delegate = ptw.acc_assessor_delegate;
            this.acc_fo_delegate = ptw.acc_fo_delegate;
            this.can_assessor_delegate = ptw.can_assessor_delegate;
            this.status = ptw.status;
            this.department = new MstDepartmentEntity(ptw.mst_department);
            this.is_extend = (byte)(ptw.id_parent_ptw != null ? 1 : 0);
            this.has_extend = ptw.permit_to_work1.Count > 0;
           

            this.ptw_status = getPtwStatus();
            if (this.acc_ptw_requestor != null || this.acc_supervisor != null)
            {
                var ptwRequestor = ptw.is_guest != 1 ? listUser.listUser.Find(p => p.id == Int32.Parse(this.acc_ptw_requestor)) : listUser.listUser.Find(p => p.id == Int32.Parse(this.acc_supervisor));
                this.isNeedClose = this.status == (int)statusPtw.ACCFO && (ptw.validity_period_end != null && ptw.validity_period_end.Value.CompareTo(DateTime.Now) < 0) && !this.has_extend && (ptwRequestor != null && (ptwRequestor.id == user.id || ptwRequestor.employee_delegate == user.id)) ? true : false;
            }
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
                loto_need = loto_need,
                hira_docs = this.hira_docs,
                is_guest = is_guest,
                acc_supervisor = acc_supervisor,
                acc_supervisor_delegate = acc_supervisor_delegate,
                can_supervisor = can_supervisor,
                can_supervisor_delegate = can_supervisor_delegate,
                guest_holder_no = guest_holder_no
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
            ptw.ptw_no = ptw_no;
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
            ptw.loto_need = loto_need;
            ptw.pre_job_notes = pre_job_notes;
            ptw.cancel_notes = cancel_notes;
            ptw.validity_period_start = validity_period_start;
            ptw.validity_period_end = validity_period_end;
            ptw.cancellation_date = cancellation_date;
            ptw.hira_docs = hira_docs;
            ptw.guest_holder_no = guest_holder_no;
            ptw.acc_notes_fo_ass = acc_notes_fo_ass;
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

        public int saveNotesAssFo(string notes, int pos)
        {
            permit_to_work ptw = db.permit_to_work.Find(id);
            if (pos == 0)
            {
                ptw.acc_notes_ass_fo = notes;
            }
            else
            {
                ptw.can_notes_ass_fo = notes;
            }

            this.db.Entry(ptw).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int extendPtw(PtwEntity ptw)
        {
            string ptw_no = ptw.ptw_no;
            var ptw_nos = ptw_no.Split('-');
            this.generatePtwNumber(ptw.ptw_no, ptw_nos[0], true);
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
            this.acc_fo = ptw.acc_fo;
            this.guest_holder_no = ptw.guest_holder_no;
            this.is_guest = ptw.is_guest;
            if (this.is_guest == 1)
            {
                this.acc_ptw_requestor_approve = ptw.acc_ptw_requestor_approve;
            }
            //this.acc_assessor = ptw.acc_assessor;
            //this.acc_assessor_delegate = ptw.acc_assessor_delegate;
            //this.acc_fo = ptw.acc_fo;
            //this.acc_fo_delegate = ptw.acc_fo_delegate;
            this.can_ptw_requestor = ptw.can_ptw_requestor;
            if (this.is_guest == 1)
            {
                this.can_ptw_requestor_approve = ptw.acc_ptw_requestor_approve;
            }
            this.can_supervisor = ptw.can_supervisor;
            this.can_supervisor_delegate = ptw.can_supervisor_delegate;
            //this.can_assessor = ptw.can_assessor;
            //this.can_assessor_delegate = ptw.can_assessor_delegate;
            this.can_fo = ptw.can_fo;
            this.can_fo_delegate = ptw.can_fo_delegate;
            this.id_parent_ptw = ptw.id;
            return this.addPtw(1);
        }

        public string cancelPtw(UserEntity user)
        {
            if ((this.is_guest != 1 && user.id.ToString() == this.acc_ptw_requestor) || user.id.ToString() == this.acc_supervisor)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.status = (int)statusPtw.CANCEL;
                ptw.cancellation_date = DateTime.Today;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();

                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.cancelPtw(user);
                }

                if (this.id_safety_briefing != null)
                {
                    SafetyBriefingEntity sb = new SafetyBriefingEntity(this.id_safety_briefing.Value, user);
                    sb.setCancel();
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
            } else if (this.status == (int)statusPtw.CREATE && this.has_clearance && isAllClearanceComplete()) {
                retVal = "Waiting Approval by Requestor";
            } else if (this.status == (int)statusPtw.CREATE && !this.has_clearance) {
                retVal = "Waiting Approval by Requestor";
            } else if (this.status == (int)statusPtw.CLEARANCECOMPLETE) {
                retVal = "Waiting Approval by Supervisor";
            } else if (this.status == (int)statusPtw.ACCSPV) {
                retVal = "Waiting for choosing Assessor by Facility Owner";
            } else if (this.status == (int)statusPtw.CHOOSEASS) {
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
            else if (this.status == (int)statusPtw.CANCELLED)
            {
                retVal = "Permit Request Cancelled. This Permit will not be used.";
            }

            return retVal;            
        }

        public int setStatus(int status)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            ptw.status = status;
            this.db.Entry(ptw).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int setGuestHolderNo()
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            ptw.guest_holder_no = guest_holder_no;
            this.db.Entry(ptw).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int changeCanAssessor(int id, UserEntity user)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            UserEntity assessor = new UserEntity(id, user.token, user);
            ptw.can_assessor = id.ToString();
            ptw.can_assessor_delegate = assessor.employee_delegate != null ? assessor.employee_delegate.ToString() : null;
            this.db.Entry(ptw).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        private void generateUserInPTW(UserEntity user, ListUser listUsers = null)
        {
            ListUser listUser = listUsers != null ? listUsers : new ListUser(user.token, user.id);
            int userId = 0;

            if (is_guest == 1)
            {
                UserEntity userGuest = new UserEntity();
                userGuest.alpha_name = this.acc_ptw_requestor;
                this.userInPTW.Add(UserInPTW.REQUESTOR.ToString(), userGuest);
            }
            else
            {
                Int32.TryParse(this.acc_ptw_requestor, out userId);
                isUser = isUser || user.id == userId;
                this.userInPTW.Add(UserInPTW.REQUESTOR.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.acc_supervisor, out userId);
            isUser = isUser || user.id == userId;
            this.userInPTW.Add(UserInPTW.SUPERVISOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.acc_assessor, out userId);
            isUser = isUser || user.id == userId;
            this.userInPTW.Add(UserInPTW.ASSESSOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.acc_fo, out userId);
            isUser = isUser || user.id == userId;
            this.userInPTW.Add(UserInPTW.FACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.acc_supervisor_delegate, out userId);
            if (userId != 0)
            {
                isUser = isUser || user.id == userId;
                this.userInPTW.Add(UserInPTW.SUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.acc_assessor_delegate, out userId);
            if (userId != 0)
            {
                isUser = isUser || user.id == userId;
                this.userInPTW.Add(UserInPTW.ASSESSORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.acc_fo_delegate, out userId);
            if (userId != 0)
            {
                isUser = isUser || user.id == userId;
                this.userInPTW.Add(UserInPTW.FACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_assessor, out userId);
            isUser = isUser || user.id == userId;
            this.userInPTW.Add(UserInPTW.CANASSESSOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.can_assessor_delegate, out userId);
            if (userId != 0)
            {
                isUser = isUser || user.id == userId;
                this.userInPTW.Add(UserInPTW.CANASSESSORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
        }

        #region generate ptw_number

        public void generatePtwNumber(string lastNumber, string fo_code = null, bool isExtend = false)
        {
            string result = (fo_code == null ? "XXX" : fo_code) + "-";
            int thisYear = DateTime.Today.Year;
            int lastNo = 0;
            int extension = 0;
            if (lastNumber != null)
            {
                string[] lastPtwNumberPart = lastNumber.Split('-');
                if (lastPtwNumberPart.Length == 2)
                {
                    lastNo = Int32.Parse(lastPtwNumberPart[1]) - 1;
                    //if (Int32.Parse(lastPtwNumberPart[4]) == thisYear)
                    //{
                    if (isExtend == false)
                    {
                        lastNo = Int32.Parse(lastPtwNumberPart[1]);
                    }
                    //}
                    //else
                    //{
                    //    if (isExtend == false)
                    //    {
                    //        lastNo = 0;
                    //    }
                    //}
                }
                else if (lastPtwNumberPart.Length == 3)
                {
                    //if (Int32.Parse(lastPtwNumberPart[4]) == thisYear)
                    //{
                        if (isExtend == true)
                        {
                            extension = lastPtwNumberPart[2][0] - 65 + 1;
                            lastNo = Int32.Parse(lastPtwNumberPart[1]) - 1;
                        }
                        else
                        {
                            lastNo = Int32.Parse(lastPtwNumberPart[1]);
                        }
                    //}
                    //else
                    //{
                    //    if (isExtend == true)
                    //    {
                    //        extension = lastPtwNumberPart[6][0] - 65 + 1;
                    //        lastNo = Int32.Parse(lastPtwNumberPart[5]) - 1;
                    //    }
                    //    else
                    //    {
                    //        lastNo = 0;
                    //    }
                    //}
                }
            }

            result += (lastNo + 1).ToString().PadLeft(6, '0');
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
            if (this.userInPTW[UserInPTW.REQUESTOR.ToString()] != null && (this.userInPTW[UserInPTW.REQUESTOR.ToString()].id == user.id))
            //if ((this.acc_ptw_requestor == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if (this.userInPTW[UserInPTW.SUPERVISOR.ToString()] != null && (this.userInPTW[UserInPTW.SUPERVISOR.ToString()].id == user.id || this.userInPTW[UserInPTW.SUPERVISOR.ToString()].employee_delegate == user.id))
            //if ((this.acc_supervisor == user_id || this.acc_supervisor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccAssessor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if (this.userInPTW[UserInPTW.ASSESSOR.ToString()] != null && (this.userInPTW[UserInPTW.ASSESSOR.ToString()].id == user.id || this.userInPTW[UserInPTW.ASSESSOR.ToString()].employee_delegate == user.id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFO(UserEntity user, ListUser listUser = null)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            int foid = 0;
            Int32.TryParse(this.acc_fo, out foid);
            int assId = 0;
            Int32.TryParse(this.acc_assessor, out assId);
            List<MstDelegateFOEntity> delegates = new MstDelegateFOEntity().getListByFO(foid, user, listUser);
            if (this.userInPTW[UserInPTW.FACILITYOWNER.ToString()] != null && (this.userInPTW[UserInPTW.FACILITYOWNER.ToString()].id == user.id || this.userInPTW[UserInPTW.FACILITYOWNER.ToString()].employee_delegate == user.id))
            {
                retVal = true;
            }
            else
            {
                var e = delegates.GetEnumerator();
                while (e.MoveNext() && !retVal)
                {
                    MstDelegateFOEntity del = e.Current;
                    if (del.user_delegate_id.ToString() == user_id && del.user_delegate_id != assId)
                    {
                        retVal = true;
                    }
                }
            }

            return retVal;
        }

        public bool isCanSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if (this.userInPTW[UserInPTW.SUPERVISOR.ToString()] != null && (this.userInPTW[UserInPTW.SUPERVISOR.ToString()].id == user.id || this.userInPTW[UserInPTW.SUPERVISOR.ToString()].employee_delegate == user.id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanAssessor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if (this.userInPTW[UserInPTW.CANASSESSOR.ToString()] != null && (this.userInPTW[UserInPTW.CANASSESSOR.ToString()].id == user.id || this.userInPTW[UserInPTW.CANASSESSOR.ToString()].employee_delegate == user.id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanFO(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            int foid = 0;
            Int32.TryParse(this.can_fo, out foid);
            int assId = 0;
            Int32.TryParse(this.can_assessor, out assId);
            List<MstDelegateFOEntity> delegates = new MstDelegateFOEntity().getListByFO(foid, user);
            if (this.userInPTW[UserInPTW.FACILITYOWNER.ToString()] != null && (this.userInPTW[UserInPTW.FACILITYOWNER.ToString()].id == user.id || this.userInPTW[UserInPTW.FACILITYOWNER.ToString()].employee_delegate == user.id))
            {
                retVal = true;
            }
            else
            {
                var e = delegates.GetEnumerator();
                while (e.MoveNext() && !retVal)
                {
                    MstDelegateFOEntity del = e.Current;
                    if (del.user_delegate_id.ToString() == user_id && del.user_delegate_id != assId)
                    {
                        retVal = true;
                    }
                }
            }

            return retVal;
        }

        public bool isCanEdit(UserEntity user, out bool isFo)
        {
            bool isCanEdit = false;
            isFo = false;
            if (isRequestor(user) && this.status < (int)statusPtw.GUESTCOMPLETE) {
                isCanEdit = true;
            }

            if (isAccSupervisor(user) && (this.status == (int)statusPtw.CLEARANCECOMPLETE || this.status == (int)statusPtw.GUESTCOMPLETE))
            {
                isCanEdit = true;
            }

            if (isAccFO(user))
            {
                isFo = true;
            }

            if (isFo && this.status == (int)statusPtw.ACCSPV)
            {
                isCanEdit = true;
            }

            if (isAccAssessor(user) && this.status == (int)statusPtw.CHOOSEASS)
            {
                isCanEdit = true;
            }

            if (isFo && this.status == (int)statusPtw.ACCASS)
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
            //this.acc_supervisor_delegate = supervisor.employee_delegate.ToString();
            //this.can_supervisor_delegate = supervisor.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_supervisor = this.acc_supervisor;
            ptw.can_supervisor = this.can_supervisor;
            //ptw.acc_supervisor_delegate = this.acc_supervisor_delegate;
            //ptw.can_supervisor_delegate = this.can_supervisor_delegate;

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
            // create node
            workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                WorkflowNodeServiceModel.GeneralPermitNodeName.CHOOSING_ASSESSOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

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

        public int assignFOAcc(UserEntity fo)
        {
            this.acc_fo = fo.id.ToString();
            this.acc_fo_delegate = fo.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_fo = this.acc_fo;
            ptw.acc_fo_delegate = this.acc_fo_delegate;

            this.db.Entry(ptw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignFOCan(UserEntity fo)
        {
            this.can_fo = fo.id.ToString();
            this.can_fo_delegate = fo.employee_delegate.ToString();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.can_fo = this.can_fo;
            ptw.can_fo_delegate = this.can_fo_delegate;

            this.db.Entry(ptw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public bool isUserInPtw(UserEntity user, ListUser listUser)
        {
            return (isRequestor(user) || isAccSupervisor(user) || isAccAssessor(user) || isAccFO(user, listUser) || isCanAssessor(user));
        }

        #endregion

        #region check all clearence complete

        public bool isAllClearanceComplete()
        {
            var isComplete = true;

            // check if LOTO
            if (this.loto_need != null && this.loto_need != 0 && this.loto_status != (int)statusClearance.COMPLETE && this.status == (int)statusPtw.CLEARANCECOMPLETE)
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
            if (this.loto_need != null && this.loto_need != 0 && this.loto_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check CSEP
            if (this.csep_id != null && this.csep_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check HW
            if (this.hw_id != null && this.hw_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check FI
            if (this.fi_id != null && this.fi_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check Ex
            if (this.ex_id != null && this.ex_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check WH
            if (this.wh_id != null && this.wh_status < (int)statusClearance.CLOSE)
            {
                isClose = false;
            }

            // check Rad
            if (this.rad_id != null && this.rad_status < (int)statusClearance.CLOSE)
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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.REQUESTOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

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
            //if (ptw.acc_assessor == null)
            //{
            //    return "401";
            //}

            if (user.id.ToString() == this.acc_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.acc_supervisor_approve = "a" + user.signature;
                if (ptw.acc_assessor != null)
                {
                    ptw.status = (int)statusPtw.CHOOSEASS;
                }
                else
                {
                    ptw.status = (int)statusPtw.ACCSPV;
                }
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                return "200";
            } 
            else
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.acc_supervisor_delegate = user.id.ToString();
                ptw.acc_supervisor_approve = "d" + user.signature;
                if (ptw.acc_assessor != null)
                {
                    ptw.status = (int)statusPtw.CHOOSEASS;
                }
                else
                {
                    ptw.status = (int)statusPtw.ACCSPV;
                }
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                return "201";
            }
        }

        public string supervisorAccReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_supervisor || user.id == this.userInPTW[UserInPTW.SUPERVISOR.ToString()].employee_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_ptw_requestor_approve = null;
                ptw.status = (int)statusPtw.CREATE;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);

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
                try
                {
                    //ptw.acc_fo = this.acc_fo;
                    //ptw.acc_fo_delegate = this.acc_fo_delegate;
                    //ptw.can_fo = this.acc_fo;
                    //ptw.can_fo_delegate = this.acc_fo_delegate;
                    ptw.acc_assessor_approve = "a" + user.signature;
                    ptw.status = (int)statusPtw.ACCASS;
                    this.db.Entry(ptw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    // create node
                    workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                        WorkflowNodeServiceModel.GeneralPermitNodeName.ASSESSOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                return "200";
            }
            else
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_assessor_delegate = user.id.ToString();
                ptw.acc_assessor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.ACCASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.ASSESSOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                return "201";
            }
        }

        public string assessorAccReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_assessor || user.id == this.userInPTW[UserInPTW.ASSESSOR.ToString()].employee_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                ptw.acc_supervisor_approve = null;
                ptw.acc_supervisor_delegate = null;
                ptw.status = (int)statusPtw.CLEARANCECOMPLETE;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);

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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.FACILITY_OWNER_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                return "200";
            }
            else
            {
                //ptw.acc_fo = this.acc_fo;
                ptw.acc_fo_delegate = user.id.ToString();
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.acc_fo_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.ACCFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.FACILITY_OWNER_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                return "200";
            }
        }

        public string fOAccReject(UserEntity user, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_supervisor_approve = null;
            ptw.acc_assessor_approve = null;
            ptw.acc_assessor_delegate = null;
            ptw.acc_supervisor_delegate = null;
            ptw.status = (int)statusPtw.CLEARANCECOMPLETE;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();
            // create node
            workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                WorkflowNodeServiceModel.GeneralPermitNodeName.FACILITY_OWNER_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);

            return "200";
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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_REQUESTOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_SUPERVISOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.supervisorCanApproval(user);
                }
                retVal = "200";
            }
            else
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                ptw.can_supervisor_delegate = user.id.ToString();
                ptw.can_supervisor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANSPV;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_SUPERVISOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.supervisorCanApproval(user);
                }
                retVal = "201";
            }
            return retVal;
        }

        public string supervisorCanReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_supervisor || user.id == this.userInPTW[UserInPTW.SUPERVISOR.ToString()].employee_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_ptw_requestor_approve = null;
                ptw.status = (int)statusPtw.CANCEL;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_SUPERVISOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_ASSESSOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.assessorCanApproval(user);
                }
                return "200";
            }
            else
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                ptw.can_assessor_delegate = user.id.ToString();
                ptw.can_assessor_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANASS;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_ASSESSOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.assessorCanApproval(user);
                }
                return "201";
            }
        }

        public string assessorCanReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_assessor || user.id == this.userInPTW[UserInPTW.CANASSESSOR.ToString()].employee_delegate)
            {
                permit_to_work ptw = this.db.permit_to_work.Find(this.id);
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.can_supervisor_approve = null;
                ptw.can_supervisor_delegate = null;
                ptw.status = (int)statusPtw.CANREQ;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_ASSESSOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
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
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_FACILITY_OWNER.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.fOCanApproval(user);
                }
                return "200";
            }
            else
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                ptw.can_fo_delegate = user.id.ToString();
                ptw.can_fo_approve = "d" + user.signature;
                ptw.status = (int)statusPtw.CANFO;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_FACILITY_OWNER.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.fOCanApproval(user);
                }
                return "201";
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
                ptw.can_assessor_delegate = null;
                ptw.can_supervisor_approve = null;
                ptw.can_supervisor_delegate = null;
                this.id_parent_ptw = ptw.id_parent_ptw;
                ptw.status = (int)statusPtw.CANREQ;
                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
                // create node
                workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                    WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_FACILITY_OWNER.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                if (this.id_parent_ptw != null)
                {
                    PtwEntity pt = new PtwEntity(this.id_parent_ptw.Value, user);
                    pt.fOCanReject(user,comment);
                }
                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string CancelPTWRequest(UserEntity user)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            ptw.status = (int)statusPtw.CANCELLED;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        #endregion

        #region send email

        public string sendEmailRequestNo(UserEntity userLogin, string serverUrl)
        {
            ListUser listUser = new ListUser();
            var users = listUser.GetAdminSHE(userLogin.token, userLogin.id);
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            SendEmail sendEmail = new SendEmail();

            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.requested_no = 1;
            db.Entry(ptw).State = EntityState.Modified;
            db.SaveChanges();

            foreach (UserEntity user in users)
            {
                s.Add(user.email);
                userId.Add(user.id);
            }

            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_supervisor), userLogin.token, userLogin);
            string message = requestor.alpha_name + " has requested to give PTW Holder No to " + this.acc_ptw_requestor + " for PTW No: " + this.ptw_no;
            string subject = "Request PTW Holder No with PTW No " + this.ptw_no;
            sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Give PTW Holder No to " + this.acc_ptw_requestor + " for PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailRequestNoSet(UserEntity userLogin, string serverUrl)
        {
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_supervisor), userLogin.token, userLogin);
            List<string> s = new List<string>();
            SendEmail sendEmail = new SendEmail();
            List<int> userId = new List<int>();
            s.Add(requestor.email);
            userId.Add(requestor.id);

            string message = "Requestor's PTW Holder No has been set.";
            string subject = "Request PTW Holder No with PTW No " + this.ptw_no + " Set";
            sendEmail.SendToNotificationCenter(userId, "General Permit", "PTW Holder Number has been given. PTW No. " + this.ptw_no + " can be proceed.", serverUrl + "Home?p=ptw/edit/" + this.id);

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFO(List<MstFOEntity> listFO, string serverUrl)
        {
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailfo";
            SendEmail sendEmail = new SendEmail();
            foreach (MstFOEntity fo in listFO)
            {
                List<string> s = new List<string>();

                //s.Add("septu.jamasoka@gmail.com");
                if (fo.user != null)
                {
                    s.Add(fo.user.email);
                    string encodedValue = salt + fo.user.id + val + this.id;
                    string encodedElement = Base64.Base64Encode(encodedValue);

                    string seal = Base64.MD5Seal(timestamp + salt + val);

                    string message = serverUrl + "Ptw/SetFacilityOwner?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                    sendEmail.Send(s, message, "PTW Facility Owner");
                }
            }

            return "200";
        }

        public string sendEmailSpv(List<UserEntity> listSpv, string serverUrl)
        {
            string timestamp = DateTime.UtcNow.Ticks.ToString();
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailsupervisor";
            SendEmail sendEmail = new SendEmail();
            foreach (UserEntity spv in listSpv)
            {
                List<string> s = new List<string>();

                //s.Add("septu.jamasoka@gmail.com");
                s.Add(spv.email);
                string encodedValue = salt + spv.id + val + this.id;
                string encodedElement = Base64.Base64Encode(encodedValue);

                string seal = Base64.MD5Seal(timestamp + salt + val);

                string message = serverUrl + "Ptw/SetSupervisor?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                sendEmail.Send(s, message, "ptw supervisor");
            }

            return "200";
        }

        public string sendEmailRequestor(string serverUrl, string token, UserEntity user, int stat = 0, int cancel = 0, string comment = null)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            s.Add(requestor.email);
            userId.Add(requestor.id);
            //s.Add("septu.jamasoka@gmail.com");

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Requestor Approve";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Cancellation of PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailSupervisor(string serverUrl, string token, UserEntity user, int pos = 1, int stat = 0, int cancel = 0, string comment = null)
        {
            int supervisor_id = 0;

            if (this.acc_supervisor != null)
                supervisor_id = Int32.Parse(this.acc_supervisor);
            // else
                // supervisor_id = Int32.Parse(this.can_supervisor);
            UserEntity supervisor = new UserEntity(supervisor_id, token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            if (supervisor.email != null)
            {
                s.Add(supervisor.email);
                userId.Add(supervisor.id);
                //s.Add("septu.jamasoka@gmail.com");

            }
            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value, token, user);
                s.Add(del.email);
                userId.Add(del.id);
                //s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Supervisor Approve";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Cancellation of PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailAssessor(string serverUrl, string token, UserEntity user, int pos = 0, int stat = 0, int cancel = 0, string comment = null)
        {
            int assessor_id = 0;

            if (pos == 0)
                assessor_id = Int32.Parse(this.acc_assessor);
            else
                assessor_id = Int32.Parse(this.can_assessor);
            UserEntity assessor = new UserEntity(assessor_id, token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            s.Add(assessor.email);
            userId.Add(assessor.id);
            //s.Add("septu.jamasoka@gmail.com");

            if (assessor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(assessor.employee_delegate.Value, token, user);
                s.Add(del.email);
                userId.Add(del.id);
                //s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id;
                subject = "Permit to Work Assessor Approve";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=ptw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Cancellation of PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFo(string serverUrl, string token, UserEntity user, int pos = 0, int stat = 0, int cancel = 0, string comment = null)
        {
            int fo_id = 0;

            if (pos == 1)
                fo_id = Int32.Parse(this.acc_fo);
            else
                fo_id = Int32.Parse(this.can_fo);

            UserEntity facilityOwner = new UserEntity(fo_id, token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            s.Add(facilityOwner.email);
            userId.Add(facilityOwner.id);
            //s.Add("septu.jamasoka@gmail.com");

            if (facilityOwner.employee_delegate != null)
            {
                UserEntity del = new UserEntity(facilityOwner.employee_delegate.Value, token, user);
                s.Add(del.email);
                userId.Add(del.id);
                //s.Add("septu.jamasoka@gmail.com");
            }

            List<UserEntity> listDel = facilityOwner.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
#if (!DEBUG)
                s.Add(u.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(u.id);
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Facility Owner Approve";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Approval Rejection";
                if (cancel == 0)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (cancel == 1)
                {
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "Cancellation of PTW No. " + this.ptw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=ptw/edit/" + this.id);
                }
            }
            else if (stat == 2)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Permit to Work Choosing Assessor";
                sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Choose Assessor of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailRequestorClearance(string serverUrl, string token, UserEntity user, int clearance_permit, int status)
        {
            //if (extension == 0)
            //{

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            if (this.is_guest == 1)
            {
                UserEntity requestor = new UserEntity(Int32.Parse(this.acc_supervisor), token, user);
                s.Add(requestor.email);
                userId.Add(requestor.id);
            }
            else
            {
                UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor), token, user);
                s.Add(requestor.email);
                userId.Add(requestor.id);
            }
            //s.Add("septu.jamasoka@gmail.com");

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;
            string subject = "Permit to Work Requestor Approve";

            if (status == 1)
            {
                switch (clearance_permit)
                {
                    case (int)clearancePermit.HOTWORK:
                        message = "Hot Work Permit has approved by facility owner.<br />" + serverUrl + "Home?p=Hw/edit/" + this.hw_id;
                        subject = "Hot Work Permit Number " + hw_no + " Has Been Approved";
                        sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit has been approved by facility owner.", serverUrl + "Home?p=Hw/edit/" + this.hw_id);
                        break;
                    case (int)clearancePermit.CONFINEDSPACE:
                        message = "Confined Space Entry Permit has approved by facility owner.<br />" + serverUrl + "Home?p=Csep/edit/" + this.csep_id;
                        subject = "Confined Space Entry Permit Number " + csep_no + " Has Been Approved";
                        sendEmail.SendToNotificationCenter(userId, "Confined Space Entry Permit", "Confined Space Entry Permit has been approved by facility owner.", serverUrl + "Home?p=Csep/edit/" + this.csep_id);
                        break;
                }
            }
            else if (status == 2)
            {
                switch (clearance_permit)
                {
                    case (int)clearancePermit.HOTWORK:
                        message = "Hot Work Permit has approved to close by facility owner.<br />" + serverUrl + "Home?p=Hw/edit/" + this.hw_id;
                        subject = "Hot Work Permit Number " + hw_no + " Has Been Closed";
                        sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit has been cancelled by facility owner.", serverUrl + "Home?p=Hw/edit/" + this.hw_id);
                        break;
                    case (int)clearancePermit.CONFINEDSPACE:
                        message = "Confined Space Entry Permit has approved to close by facility owner.<br />" + serverUrl + "Home?p=Csep/edit/" + this.csep_id;
                        subject = "Confined Space Entry Permit Number " + csep_no + " Has Been Closed";
                        sendEmail.SendToNotificationCenter(userId, "Confined Space Entry Permit", "Confined Space Entry Permit has been cancelled by facility owner.", serverUrl + "Home?p=Csep/edit/" + this.csep_id);
                        break;
                }
            }
            else if (status == 3)
            {
                switch (clearance_permit)
                {
                    case (int)clearancePermit.HOTWORK:
                        message = "Hot Work Permit has approved to close by supervisor.<br />" + serverUrl + "Home?p=Hw/edit/" + this.hw_id;
                        subject = "Hot Work Permit Number " + hw_no + " Has Been Cancelled by Supervisor";
                        sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit has been cancelled by Supervisor.", serverUrl + "Home?p=Hw/edit/" + this.hw_id);
                        break;
                    case (int)clearancePermit.CONFINEDSPACE:
                        message = "Confined Space Entry Permit has approved to close by supervisor.<br />" + serverUrl + "Home?p=Csep/edit/" + this.csep_id;
                        subject = "Confined Space Entry Permit Number " + csep_no + " Has Been Cancelled by Supervisor";
                        sendEmail.SendToNotificationCenter(userId, "Confined Space Entry Permit", "Confined Space Entry Permit has been cancelled by Supervisor.", serverUrl + "Home?p=Csep/edit/" + this.csep_id);
                        break;
                }
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailSupervisorLOTO(string serverUrl, string token, UserEntity user) {
            int supervisor_id = 0;

            if (this.acc_supervisor != null)
                supervisor_id = Int32.Parse(this.acc_supervisor);
            // else
            // supervisor_id = Int32.Parse(this.can_supervisor);
            UserEntity supervisor = new UserEntity(supervisor_id, token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            if (supervisor.email != null)
            {
                s.Add(supervisor.email);
                userId.Add(supervisor.id);
                //s.Add("septu.jamasoka@gmail.com");

            }
            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value, token, user);
                s.Add(del.email);
                userId.Add(del.id);
                //s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            message = serverUrl + "Home?p=ptw/edit/" + this.id;
            subject = "LOTO Permit Creation";

            sendEmail.SendToNotificationCenter(userId, "General Permit", "Please Create LOTO Permit for this PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailRequestorClearanceCompleted(string serverUrl, string token, UserEntity user, int status)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = is_guest == 1 ? new UserEntity(Int32.Parse(this.acc_supervisor), token, user) : new UserEntity(Int32.Parse(this.acc_ptw_requestor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            s.Add(requestor.email);
            userId.Add(requestor.id);
            //s.Add("septu.jamasoka@gmail.com");

            string message = "All clearance permit has been approved, you may continue by approving this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
            string subject = "All Clearance Permit Approved";

            if (status == 1)
            {
                message = "All clearance permit has been approved, you may continue by approving this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "All Clearance Permit Approved";
                sendEmail.SendToNotificationCenter(userId, "General Permit", "All clearance permit has been approved. Please approve PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
            }
            else if (status == 2)
            {
                message = "All clearance permit has been closed, you may continue by cancelling this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "All Clearance Permit Closed";
                sendEmail.SendToNotificationCenter(userId, "General Permit", "All clearance permit has been cancelled. Please approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
            }
            else if (status == 3)
            {
                message = "All clearance permit has been cancelled by supervisor, you may continue by cancelling this Permit to Work.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                subject = "All Clearance Permit Cancelled by Supervisor";
                sendEmail.SendToNotificationCenter(userId, "General Permit", "All clearance permit has been cancelled by Supervisor. Please approve Cancellation of PTW No. " + this.ptw_no, serverUrl + "Home?p=ptw/edit/" + this.id);
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailRequestorPermitCompleted(string serverUrl, string token, UserEntity user, int status)
        {
            //if (extension == 0)
            //{
            if (this.is_guest != 1) {
                UserEntity requestor = new UserEntity(Int32.Parse(this.acc_ptw_requestor), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
                s.Add(requestor.email);
                userId.Add(requestor.id);
                //s.Add("septu.jamasoka@gmail.com");

                string message = "Permit to Work has been approved, you may start working using this permit.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                string subject = "Permit to Work Approved";

                if (status == 1)
                {
                    message = "Permit to Work has been approved, you may start working using this permit.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                    subject = "Permit to Work Approved";
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "General Permit (" + this.ptw_no + ") has been approved by Facility Owner.", serverUrl + "Home?p=ptw/edit/" + this.id);
                }
                else if (status == 2)
                {
                    message = "Permit to Work has been cancelled. You cannot work using this permit again.<br />" + serverUrl + "Home?p=Ptw/edit/" + this.id;
                    subject = "Permit to Work Cancelled";
                    sendEmail.SendToNotificationCenter(userId, "General Permit", "General Permit (" + this.ptw_no + ") has been cancelled by Facility Owner.", serverUrl + "Home?p=ptw/edit/" + this.id);
                }

                sendEmail.Send(s, message, subject);
            }
            //}

            return "200";
        }

        #endregion

        #region clearance permit

        public string setClearancePermit(int? clearance_id, int? status, string typeClerancePermit)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            if (typeClerancePermit == clearancePermit.HOTWORK.ToString())
            {
                this.hw_id = clearance_id;
                this.hw_status = status;

                ptw.hw_id = this.hw_id;
                ptw.hw_status = this.hw_status;
            }
            else if (typeClerancePermit == clearancePermit.FIREIMPAIRMENT.ToString())
            {
                this.fi_id = clearance_id;
                this.fi_status = status;

                ptw.fi_id = this.fi_id;
                ptw.fi_status = this.fi_status;
            }
            else if (typeClerancePermit == clearancePermit.RADIOGRAPHY.ToString())
            {
                this.rad_id = clearance_id;
                this.rad_status = status;

                ptw.rad_id = this.rad_id;
                ptw.rad_status = this.rad_status;
            }
            else if (typeClerancePermit == clearancePermit.WORKINGHEIGHT.ToString())
            {
                this.wh_id = clearance_id;
                this.wh_status = status;

                ptw.wh_id = this.wh_id;
                ptw.wh_status = this.wh_status;
            }
            else if (typeClerancePermit == clearancePermit.EXCAVATION.ToString())
            {
                this.ex_id = clearance_id;
                this.ex_status = status;

                ptw.ex_id = this.ex_id;
                ptw.ex_status = this.ex_status;
            }
            else if (typeClerancePermit == clearancePermit.CONFINEDSPACE.ToString())
            {
                this.csep_id = clearance_id;
                this.csep_status = status;

                ptw.csep_id = this.csep_id;
                ptw.csep_status = this.csep_status;
            }
            else if (typeClerancePermit == clearancePermit.LOCKOUTTAGOUT.ToString())
            {
                this.loto_id = clearance_id;
                this.loto_status = status;

                ptw.loto_id = this.loto_id;
                ptw.loto_status = this.loto_status;
            }
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        public string setClerancePermitStatus(int status, string typeClerancePermit)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);

            if (typeClerancePermit == clearancePermit.HOTWORK.ToString())
            {
                this.hw_status = status;

                ptw.hw_status = this.hw_status;
            }
            else if (typeClerancePermit == clearancePermit.FIREIMPAIRMENT.ToString())
            {
                this.fi_status = status;

                ptw.fi_status = this.fi_status;
            }
            else if (typeClerancePermit == clearancePermit.RADIOGRAPHY.ToString())
            {
                this.rad_status = status;

                ptw.rad_status = this.rad_status;
            }
            else if (typeClerancePermit == clearancePermit.WORKINGHEIGHT.ToString())
            {
                this.wh_status = status;

                ptw.wh_status = this.wh_status;
            }
            else if (typeClerancePermit == clearancePermit.EXCAVATION.ToString())
            {
                this.ex_status = status;

                ptw.ex_status = this.ex_status;
            }
            else if (typeClerancePermit == clearancePermit.CONFINEDSPACE.ToString())
            {
                this.csep_status = status;

                ptw.csep_status = this.csep_status;
            }
            else if (typeClerancePermit == clearancePermit.LOCKOUTTAGOUT.ToString())
            {
                this.loto_status = status;

                ptw.loto_status = this.loto_status;
            }

            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        public string clearancePermitStatus(string typeClerancePermit)
        {
            string retVal = "";

            if (typeClerancePermit == clearancePermit.HOTWORK.ToString())
            {
                switch (this.hw_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.FIREIMPAIRMENT.ToString())
            {
                switch (this.fi_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.RADIOGRAPHY.ToString())
            {
                switch (this.rad_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.WORKINGHEIGHT.ToString())
            {
                switch (this.wh_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.EXCAVATION.ToString())
            {
                switch (this.ex_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.CONFINEDSPACE.ToString())
            {
                switch (this.csep_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }
            else if (typeClerancePermit == clearancePermit.LOCKOUTTAGOUT.ToString())
            {
                switch (this.loto_status)
                {
                    case (int)statusClearance.NOTCOMPLETE: retVal = "<span class='label'>Not Completed</span>"; break;
                    case (int)statusClearance.COMPLETE: retVal = "<span class='label label-success'>Completed</span>"; break;
                    case (int)statusClearance.CLOSE: retVal = "<span class='label label-danger'>Closed</span>"; break;
                    case (int)statusClearance.REQUESTORCANCELLED: retVal = "<span class='label label-warning'>Requestor's Cancelled</span>"; break;
                };
            }

            return retVal;
        }

        #endregion

        #region guest

        public string guestAccApproval(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.acc_ptw_requestor_approve = "a" + this.acc_ptw_requestor_approve;
            ptw.status = (int)statusPtw.GUESTCOMPLETE;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        public string guestCanApproval(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            ptw.can_ptw_requestor_approve = ptw.acc_ptw_requestor_approve;
            ptw.status = (int)statusPtw.CANREQ;
            this.db.Entry(ptw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        #endregion

        internal void addLoto(LotoEntity loto)
        {
            permit_to_work ptw = this.db.permit_to_work.Find(this.id);
            if (ptw != null)
            {
                loto_permit lot = this.db.loto_permit.Find(loto.id);
                ptw.loto_permit.Add(lot);

                this.db.Entry(ptw).State = EntityState.Modified;
                this.db.SaveChanges();
            }
        }

        internal void checkLotoCancellationComplete(UserEntity user)
        {
            bool complete = true;
            foreach (LotoEntity loto in this.lotoPermit)
            {
                if (user.id == loto.listUserInLOTO[LotoEntity.userInLOTO.SUPERVISOR.ToString()].id || user.id == loto.listUserInLOTO[LotoEntity.userInLOTO.SUPERVISOR.ToString()].employee_delegate)
                {
                    if (loto.cancellation_supervisor_signature != null)
                    {
                        complete = complete && true; //  && loto.status != (int)LotoEntity.LOTOStatus.CANCELSPV;
                    }
                    else
                    {
                        complete = complete && false;
                    }
                }
                else if (user.id == loto.listUserInLOTO[LotoEntity.userInLOTO.CANCELLATIONFACILITYOWNER.ToString()].id || user.id == loto.listUserInLOTO[LotoEntity.userInLOTO.CANCELLATIONFACILITYOWNER.ToString()].employee_delegate)
                {
                    if (loto.cancellation_supervisor_signature != null)
                    {
                        complete = complete && loto.status != (int)LotoEntity.LOTOStatus.CANCELSPV;
                    }
                    else
                    {
                        complete = complete && false;
                    }
                }

                foreach (LotoComingHolderEntity comingHolder in loto.lotoComingHolder)
                {
                    if (user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate)
                    {
                        if (comingHolder.isCancel())
                        {
                            complete = complete && loto.status != (int)LotoEntity.LOTOStatus.CANCELSPV;
                        }
                        else
                        {
                            complete = complete && false;
                        }
                    }
                }
            }

            if (complete)
            {
                setClerancePermitStatus((int)statusClearance.CLOSE, clearancePermit.LOCKOUTTAGOUT.ToString());
            }
        }
    }
}