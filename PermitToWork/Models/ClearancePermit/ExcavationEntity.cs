using PermitToWork.Models.Hira;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Models.Workflow;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class ExcavationEntity : excavation, IClearancePermitEntity
    {
        public override permit_to_work permit_to_work { get { return null; } set { } }

        // ptw entity for PTW reference
        private PtwEntity ptw { get; set; }

        public string[] pre_screening_spv_arr { get; set; }
        public string[] pre_screening_ei_arr { get; set; }
        public string[] pre_screening_fac_arr { get; set; }
        public string[] pre_screening_fo_arr { get; set; }

        public Dictionary<string, UserEntity> userInExcavation { get; set; }

        public MstFacilitiesEntity facilitiesUser { get; set; }
        public MstEIEntity eiUser { get; set; }

        public Dictionary<string, List<string>> listDocumentUploaded { get; set; }

        public int ids { get; set; }
        public string statusText { get; set; }

         // HIRA Document related
         public List<HiraEntity> hira_document { get; set; }
         public string hira_no { get; set; }

        public string rad_status { get; set; }

        public bool is_guest { get; set; }
        public bool isUser { get; set; }

        public WorkflowNodeServiceModel workflowNodeService { get; set; }

        private star_energy_ptwEntities db;

        public enum ExStatus
        {
            CREATE,
            EDITANDSEND,
            SPVAPPROVE,
            SHEAPPROVE,
            EIFACAPPROVE,
            REQUESTORAPPROVE,
            FOAPPROVE,
            CLOSING,
            CANREQUESTORAPPROVE,
            CANSPVAPPROVE,
            CANEIFACAPPROVE,
            CANSHEAPPROVE,
            CANFOAPPROVE
        };

        public enum UserInExcavation
        {
            REQUESTOR,
            SUPERVISOR,
            SAFETYOFFICER,
            FACILITYOWNER,
            FACILITIES,
            EI,
            REQUESTORDELEGATE,
            SUPERVISORDELEGATE,
            SAFETYOFFICERDELEGATE,
            FACILITYOWNERDELEGATE,
            FACILITIESDELEGATE,
            EIDELEGATE,
            CANREQUESTORDELEGATE,
            CANSUPERVISORDELEGATE,
            CANSAFETYOFFICERDELEGATE,
            CANFACILITYOWNERDELEGATE,
            CANFACILITIESDELEGATE,
            CANEIDELEGATE,
        }

        // parameterless constructor for declaring db
        public ExcavationEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
            this.userInExcavation = new Dictionary<string, UserEntity>();
            this.listDocumentUploaded = new Dictionary<string, List<string>>();
            this.workflowNodeService = new WorkflowNodeServiceModel();
            //this.screening_fo_arr = new string[];
        }

        // constructor with id to get object from database
        public ExcavationEntity(int id, UserEntity user)
            : this()
        {
            excavation ex = this.db.excavations.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(ex, this);

            this.is_guest = ex.permit_to_work.is_guest == null ? false : ex.permit_to_work.is_guest == 1;

            this.pre_screening_fo_arr = this.pre_screening_fo.Split('#');
            this.pre_screening_spv_arr = this.pre_screening_spv.Split('#');
            this.pre_screening_fac_arr = this.pre_screening_fac.Split('#');
            this.pre_screening_ei_arr = this.pre_screening_ei.Split('#');;

            int userId = 0;

            if (Int32.TryParse(this.facilities, out userId))
            {
                facilitiesUser = new MstFacilitiesEntity(userId, user);
            }

            if (Int32.TryParse(this.ei, out userId))
            {
                eiUser = new MstEIEntity(userId, user);
            }

            generateUserInExcavation(ex, user);

            this.statusText = getStatus();

            this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        // constructor with id to get object from database
        public ExcavationEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            excavation ex = this.db.excavations.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(ex, this);

            this.is_guest = ex.permit_to_work.is_guest == null ? false : ex.permit_to_work.is_guest == 1;

            this.pre_screening_fo_arr = this.pre_screening_fo.Split('#');
            this.pre_screening_spv_arr = this.pre_screening_spv.Split('#');
            this.pre_screening_fac_arr = this.pre_screening_fac.Split('#');
            this.pre_screening_ei_arr = this.pre_screening_ei.Split('#'); ;

            int userId = 0;

            if (Int32.TryParse(this.facilities, out userId))
            {
                facilitiesUser = new MstFacilitiesEntity(userId, user);
            }

            if (Int32.TryParse(this.ei, out userId))
            {
                eiUser = new MstEIEntity(userId, user);
            }

            generateUserInExcavation(ex, user, listUser);

            this.statusText = getStatus();

            this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        public ExcavationEntity(excavation ex, ListUser listUser, UserEntity user)
            : this()
        {
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(ex, this);

            this.is_guest = ex.permit_to_work.is_guest == 1;
            this.isUser = false;
            int userId = 0;

            if (Int32.TryParse(this.facilities, out userId))
            {
                facilitiesUser = new MstFacilitiesEntity(userId, user, listUser);
            }

            if (Int32.TryParse(this.ei, out userId))
            {
                eiUser = new MstEIEntity(userId, user, listUser);
            }

            generateUserInExcavation(ex, user, listUser);

            this.statusText = getStatus();
        }

        private string getStatus()
        {
            string retVal = "";
            switch (this.status)
            {
                case (int)ExStatus.CREATE: retVal = "Excavation Permit is still edited by Requestor"; break;
                case (int)ExStatus.EDITANDSEND: retVal = "Waiting for Supervisor Screening and Approval"; break;
                //case (int)FIStatus.SPVSCREENING: retVal = "Waiting for SO Pre-job Screening"; break;
                //case (int)FIStatus.SOSCREENING: retVal = "Waiting for FO Pre-job Screening"; break;
                //case (int)FIStatus.FOSCREENING: retVal = "Waiting for Approval by Requestor"; break;
                case (int)ExStatus.SPVAPPROVE: retVal = "Waiting for SHE Official Approval"; break;
                case (int)ExStatus.SHEAPPROVE: retVal = "Waiting for E&I and Facilities Screening and Approval"; break;
                case (int)ExStatus.EIFACAPPROVE: retVal = "Waiting for Requestor Approval"; break;
                case (int)ExStatus.REQUESTORAPPROVE: retVal = "Waiting for Facility Owner Screening and Approval"; break;
                case (int)ExStatus.FOAPPROVE: retVal = "Completed. Excavation Permit has been approved by Facility Owner"; break;
                case (int)ExStatus.CLOSING: retVal = "Radiography Permit is cancelled by Requestor. Waiting for Requestor Cancellation Approval"; break;
                case (int)ExStatus.CANREQUESTORAPPROVE: retVal = "Waiting for Supervisor Cancellation Approval"; break;
                case (int)ExStatus.CANSPVAPPROVE: retVal = "Waiting for E&I and Facilities Cancellation Approval"; break;
                case (int)ExStatus.CANEIFACAPPROVE: retVal = "Waiting for SHE Official Cancellation Approval"; break;
                case (int)ExStatus.CANSHEAPPROVE: retVal = "Waiting for Facility Owner Cancellation Approval"; break;
                case (int)ExStatus.CANFOAPPROVE: retVal = "Cancelled. Excavation Permit has been cancelled"; break;
            };

            return retVal;
        }

        public ExcavationEntity(int ptw_id, string requestor, string purpose, string acc_fo, string work_location, string total_crew, DateTime? start, DateTime? end, string acc_supervisor)
            : this()
        {
            // TODO: Complete member initialization
            this.purpose = purpose;
            this.id_ptw = ptw_id;
            this.requestor = requestor;
            this.facility_owner = acc_fo;
            this.work_location = work_location;
            this.estimate_start_date = start;
            this.estimate_end_date = end;
            this.total_crew = Int32.Parse(total_crew);
            this.supervisor = acc_supervisor;

            this.pre_screening_fo = "##########";
            this.pre_screening_spv = "##########";
            this.pre_screening_fac = "##########";
            this.pre_screening_ei = "##########";
        }

        // function insert data to database
        public int create()
        {
            excavation ex = new excavation();
            this.status = (int)ExStatus.CREATE;
            ModelUtilization.Clone(this, ex);
            this.db.excavations.Add(ex);
            int retVal = this.db.SaveChanges();
            this.id = ex.id;
            return retVal;
        }

        // function for editing by requestor
        public int edit()
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.purpose = this.purpose;
                ex.work_location = this.work_location;
                ex.total_crew = this.total_crew;
                ex.estimate_start_date = this.estimate_start_date;
                ex.estimate_end_date = this.estimate_end_date;
                ex.excavation_performed = this.excavation_performed;
                ex.excavation_method = this.excavation_method;
                ex.equipment = this.equipment;
                ex.soil_volume = this.soil_volume;
                ex.disposal_location = this.disposal_location;
                ex.disposal_type = this.disposal_type;

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int delete()
        {
            excavation ex = this.db.excavations.Find(this.id);
            this.db.excavations.Remove(ex);
            int retVal = this.db.SaveChanges();
            return retVal;
        }

        public void generateNumber(string ptw_no)
        {
            string result = "EX-" + ptw_no;

            this.ex_no = result;
        }

        public int savePreScreening(int who)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.screening_remark = this.screening_remark;
                switch (who)
                {
                    case 1 /* Supervisor */:
                        ex.pre_screening_spv = this.pre_screening_spv;
                        break;
                    case 2 /* E&I */:
                        ex.pre_screening_ei = this.pre_screening_ei;
                        break;
                    case 3 /* Facilities */:
                        ex.pre_screening_fac = this.pre_screening_fac;
                        break;
                    case 4 /* Facility Owner */:
                        ex.pre_screening_fo = this.pre_screening_fo;
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int saveNote(int type)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                switch (type)
                {
                    case 1 /* Approval */:
                        ex.approval_note = this.approval_note;
                        break;
                    case 2 /* Cancellation */:
                        ex.cancellation_note = this.cancellation_note;
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int saveAsDraft(int who)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:
                        retVal = edit();
                        break;
                    case 2 /* Supervisor */:
                        retVal = savePreScreening(1);
                        retVal = retVal & saveNote(1);
                        break;
                    case 3 /* SHE */:
                        retVal = saveNote(1);
                        break;
                    case 4 /* Facilities */:
                        retVal = savePreScreening(3);
                        retVal = retVal & saveNote(1);
                        break;
                    case 5 /* E&I */:
                        retVal = savePreScreening(2);
                        retVal = retVal & saveNote(1);
                        break;
                    case 6 /* Requestor */:
                        retVal = saveNote(1);
                        break;
                    case 7 /* Facility Owner */:
                        retVal = savePreScreening(4);
                        retVal = retVal & saveNote(1);
                        break;
                }
            }
            return retVal;
        }

        public int signClearance(int who, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            if (ex != null)
            {
                ex.approval_note = this.approval_note;
                switch (who)
                {
                    case 1 /* Requestor */:
                        ex.status = (int)ExStatus.EDITANDSEND;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_INPUT.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        break;
                    case 2 /* Supervisor */:
                        userEx = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.supervisor_signature = "d" + user.signature;
                            ex.supervisor_delegate = user.id.ToString();
                        }
                        ex.supervisor_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.SHEAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        break;
                    case 3 /* SHE */:
                        userEx = this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.safety_officer_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.safety_officer_signature = "d" + user.signature;
                            ex.safety_officer_delegate = user.id.ToString();
                        }
                        ex.safety_officer_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.SHEAPPROVE;
                        break;
                    case 4 /* Facilities */:
                        userEx = this.userInExcavation[UserInExcavation.FACILITIES.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.facilities_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.facilities_signature = "d" + user.signature;
                            ex.facilities_delegate = user.id.ToString();
                        }
                        ex.facilities_signature_date = DateTime.Now;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        if (ex.ei_signature != null)
                        {
                            ex.status = (int)ExStatus.EIFACAPPROVE;
                        }
                        break;
                    case 5 /* E&I */:
                        userEx = this.userInExcavation[UserInExcavation.EI.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.ei_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.ei_signature = "d" + user.signature;
                            ex.ei_delegate = user.id.ToString();
                        }
                        ex.ei_signature_date = DateTime.Now;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        if (ex.facilities_signature != null)
                        {
                            ex.status = (int)ExStatus.EIFACAPPROVE;
                        }
                        break;
                    case 6 /* Requestor */:
                        if (is_guest)
                        {
                            userEx = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()];
                            if (user.id == userEx.id || user.id == userEx.employee_delegate)
                            {
                                ex.requestor_signature = ex.permit_to_work.acc_ptw_requestor_approve;
                            }
                        } else {
                            userEx = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()];
                            if (user.id == userEx.id)
                            {
                                ex.requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == userEx.employee_delegate)
                            {
                                ex.requestor_signature = "d" + user.signature;
                                ex.requestor_delegate = user.id.ToString();
                            }
                        } 
                        ex.requestor_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.REQUESTORAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        break;
                    case 7 /* Facility Owner */:
                        userEx = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.facility_owner_signature = "a" + user.signature;
                        }
                        else
                        {
                            ex.facility_owner_signature = "d" + user.signature;
                            ex.facility_owner_delegate = user.id.ToString();
                        }
                        ex.facility_owner_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.FOAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.FACILITY_OWNER_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                        this.ptw = new PtwEntity(ex.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.EXCAVATION.ToString());
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int rejectClearance(int who)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            if (ex != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:
                        ex.status = (int)ExStatus.CREATE;
                        break;
                    case 2 /* Supervisor */:
                        ex.status = (int)ExStatus.CREATE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.SUPERVISOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 3 /* SHE */:
                        ex.supervisor_signature = null;
                        ex.supervisor_delegate = null;
                        ex.status = (int)ExStatus.EDITANDSEND;
                        break;
                    case 4 /* Facilities */:
                        ex.safety_officer_signature = null;
                        ex.safety_officer_signature = null;
                        ex.status = (int)ExStatus.EDITANDSEND;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 5 /* E&I */:
                        ex.safety_officer_signature = null;
                        ex.safety_officer_signature = null;
                        ex.status = (int)ExStatus.EDITANDSEND;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 6 /* Requestor */:
                        ex.ei_signature = null;
                        ex.ei_delegate = null;
                        ex.facilities_signature = null;
                        ex.facilities_delegate = null;
                        ex.status = (int)ExStatus.SHEAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 7 /* Facility Owner */:
                        ex.requestor_signature = null;
                        ex.requestor_delegate = null;
                        ex.status = (int)ExStatus.EIFACAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.FACILITY_OWNER_APPROVE.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToUser(int who, int stat, string serverUrl, UserEntity user, string comment = "")
        {
            int retVal = 0;
            int? userId = null;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            List<string> listEmail = new List<string>();
            SendEmail sendEmail = new SendEmail();
            string message = "";
            string title = "";
            List<int> userIds = new List<int>();
            if (ex != null)
            {
#if DEBUG
                listEmail.Add("septujamasoka@gmail.com");
#endif
                switch (who)
                {
                    case 1:
                        if (stat == 1)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }

                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Supervisor";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                // send email supervisor
                            }
#endif
                        }
                        retVal = 1;
                        break;
                    case 2:
                        if (stat == 1)
                        {
                            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()) && this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.EI.ToString()))
                            //if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString() && p == UserInExcavation.EI.ToString()))
                            {
#if !DEBUG
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }

                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from E&I and Civil";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                    List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                    foreach (UserEntity u in listDel)
                                    {
                                        listEmail.Add(u.email);
                                        userIds.Add(u.id);
                                    }
                                }
#endif
                                title = "[URGENT] Excavation Clearance Permit (" + this.ex_no + ") E&I and Civil hasn't been Chosen";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                //sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please choose E&I and Civil for Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (is_guest)
                            {
                                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
                            else
                            {
                                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from Supervisor";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 3:
                        if (stat == 1)
                        {
                            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString() && p == UserInExcavation.EI.ToString()))
                            {
#if !DEBUG
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }

                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from E&I and Civil";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                    List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                    foreach (UserEntity u in listDel)
                                    {
                                        listEmail.Add(u.email);
                                        userIds.Add(u.id);
                                    }
                                }
#endif
                                title = "[URGENT] Excavation Clearance Permit (" + this.ex_no + ") E&I and Civil hasn't been Chosen";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please choose E&I and Civil for Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from SHE";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 4:
                        if (stat == 1)
                        {
                            if (ex.ei_signature != null)
                            {
#if !DEBUG
                                if (is_guest)
                                {
                                    if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                    }
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Requestor";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.EI.ToString()))
                                {
#if !DEBUG
                                    listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
#endif
                                    title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from E&I";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                                else
                                {
#if !DEBUG
                                    if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                        List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                        foreach (UserEntity u in listDel)
                                        {
                                            listEmail.Add(u.email);
                                            userIds.Add(u.id);
                                        }
                                    }
#endif
                                    title = "[URGENT] Excavation Clearance Permit (" + this.ex_no + ") E&I hasn't been Chosen";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please choose E&I for Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from Civil";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 5:
                        if (stat == 1)
                        {
                            if (ex.facilities_signature != null)
                            {
#if !DEBUG
                                if (is_guest)
                                {
                                    if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                    }
                                }
                                else
                                {
                                    if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                    }
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Requestor";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()))
                                {
#if !DEBUG
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
#endif
                                    title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Civil";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                                else
                                {
#if !DEBUG
                                    if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                    {
                                        listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                        userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                        if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                        {
                                            userEx = new UserEntity(userId.Value, user.token, user);
                                            listEmail.Add(userEx.email);
                                            userIds.Add(userEx.id);
                                        }
                                    }
                                    List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                    foreach (UserEntity u in listDel)
                                    {
                                        listEmail.Add(u.email);
                                        userIds.Add(u.id);
                                    }
#endif
                                    title = "[URGENT] Excavation Clearance Permit (" + this.ex_no + ") Civil hasn't been Chosen";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please choose Civil for Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from E&I";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 6:
                        if (stat == 1)
                        {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                    userIds.Add(u.id);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Facility Owner";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.FACILITIES.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from Requestor";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);

                        }
                        retVal = 1;
                        break;
                    case 7:
                        if (stat == 1)
                        {
#if !DEBUG
                            if (is_guest)
                            {
                                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
                            else
                            {
                                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Completed and Approved";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "  has been approved by Facility Owner.", serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (is_guest)
                            {
                                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
                            else
                            {
                                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from Facility Owner";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                }

                sendEmail.Send(listEmail, message, title);
            }

            return retVal;
        }

        public int signClearanceCancel(int who, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            if (ex != null)
            {
                ex.cancellation_note = this.cancellation_note;
                switch (who)
                {
                    case 1 /* Requestor */:
                        if (is_guest)
                        {
                            userEx = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()];
                            if (user.id == userEx.id || user.id == userEx.employee_delegate)
                            {
                                ex.can_requestor_signature = ex.permit_to_work.acc_ptw_requestor_approve;
                            }
                        }
                        else
                        {
                            userEx = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()];
                            if (user.id == userEx.id)
                            {
                                ex.can_requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == userEx.employee_delegate)
                            {
                                ex.can_requestor_signature = "d" + user.signature;
                                ex.can_requestor_delegate = user.id.ToString();
                            }
                        }
                        ex.can_requestor_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.CANREQUESTORAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_REQUESTOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        break;
                    case 2 /* Supervisor */:
                        userEx = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.can_supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.can_supervisor_signature = "d" + user.signature;
                            ex.can_supervisor_delegate = user.id.ToString();
                        }
                        ex.can_supervisor_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.CANSPVAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_SUPERVISOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                        this.ptw = new PtwEntity(ex.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.REQUESTORCANCELLED, PtwEntity.clearancePermit.EXCAVATION.ToString());
                        break;
                    case 3 /* Facilities */:
                        userEx = this.userInExcavation[UserInExcavation.FACILITIES.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.can_facilities_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.can_facilities_signature = "d" + user.signature;
                            ex.can_facilities_delegate = user.id.ToString();
                        }
                        ex.can_facilities_signature_date = DateTime.Now;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        if (ex.can_ei_signature != null)
                        {
                            ex.status = (int)ExStatus.CANSHEAPPROVE;
                        }
                        break;
                    case 4 /* E&I */:
                        userEx = this.userInExcavation[UserInExcavation.EI.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.can_ei_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.can_ei_signature = "d" + user.signature;
                            ex.can_ei_delegate = user.id.ToString();
                        }
                        ex.can_ei_signature_date = DateTime.Now;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);
                        if (ex.can_facilities_signature != null)
                        {
                            ex.status = (int)ExStatus.CANSHEAPPROVE;
                        }
                        break;
                    case 5 /* SHE */:
                        userEx = this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.can_safety_officer_signature = "a" + user.signature;
                        }
                        else if (user.id == userEx.employee_delegate)
                        {
                            ex.can_safety_officer_signature = "d" + user.signature;
                            ex.can_safety_officer_delegate = user.id.ToString();
                        }
                        ex.can_safety_officer_signature_date1 = DateTime.Now;
                        ex.status = (int)ExStatus.CANSHEAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        userEx = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()];
                        if (user.id == userEx.id)
                        {
                            ex.can_facility_owner_signature = "a" + user.signature;
                        }
                        else
                        {
                            ex.can_facility_owner_signature = "d" + user.signature;
                            ex.can_facility_owner_delegate = user.id.ToString();
                        }
                        ex.can_facility_owner_signature_date = DateTime.Now;
                        ex.status = (int)ExStatus.CANFOAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_FACILITY_OWNER.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED);

                        this.ptw = new PtwEntity(ex.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.EXCAVATION.ToString());
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int rejectClearanceCancel(int who)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            if (ex != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:
                        ex.status = (int)ExStatus.CLOSING;
                        break;
                    case 2 /* Supervisor */:
                        ex.can_requestor_signature = null;
                        ex.can_requestor_delegate = null;
                        ex.status = (int)ExStatus.CLOSING;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_SUPERVISOR.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 3 /* Facilities */:
                        ex.can_supervisor_signature = null;
                        ex.can_supervisor_delegate = null;
                        ex.status = (int)ExStatus.CANREQUESTORAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 4 /* E&I */:
                        ex.can_supervisor_signature = null;
                        ex.can_supervisor_delegate = null;
                        ex.status = (int)ExStatus.CANREQUESTORAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                    case 5 /* SHE */:
                        ex.can_facilities_signature = null;
                        ex.can_facilities_delegate = null;
                        ex.can_ei_signature = null;
                        ex.can_ei_delegate = null;
                        ex.status = (int)ExStatus.CANSPVAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        ex.can_facilities_signature = null;
                        ex.can_facilities_delegate = null;
                        ex.can_ei_signature = null;
                        ex.can_ei_delegate = null;
                        ex.status = (int)ExStatus.CANSPVAPPROVE;
                        // create node
                        workflowNodeService.CreateNode(this.id, WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_FACILITY_OWNER.ToString(), (byte)WorkflowNodeServiceModel.NodeStatus.REJECTED);
                        break;
                }

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToUserCancel(int who, int stat, string serverUrl, UserEntity user, string comment = "")
        {
            int retVal = 0;
            int? userId = null;
            excavation ex = this.db.excavations.Find(this.id);
            UserEntity userEx = null;
            List<string> listEmail = new List<string>();
            SendEmail sendEmail = new SendEmail();
            string message = "";
            string title = "";
            List<int> userIds = new List<int>();
            if (ex != null)
            {
#if DEBUG
                listEmail.Add("septujamasoka@gmail.com");
#endif
                switch (who)
                {
                    case 1:
                        if (stat == 1)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }

                                title = "Excavation Clearance Permit (" + this.ex_no + ") Need Review and Approval from Supervisor";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                // send email supervisor
                            }
#endif
                        }
                        retVal = 1;
                        break;
                    case 2:
                        if (stat == 1)
                        {
                            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()) && this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.EI.ToString()))
                            {
#if !DEBUG
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }

                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from E&I and Civil";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (is_guest)
                            {
                                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
                            else
                            {
                                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Rejected from Supervisor";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 3:
                        if (stat == 1)
                        {
                            if (ex.can_ei_signature != null)
                            {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from Facility Owner";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.EI.ToString()))
                                {
#if !DEBUG
                                    listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
#endif
                                    title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from E&I";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Rejected from Civil";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 4:
                        if (stat == 1)
                        {
                            if (ex.can_facilities_signature != null)
                            {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                    userIds.Add(u.id);
                                }
#endif
                                title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from Facility Owner";
                                message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                            }
                            else
                            {
                                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()))
                                {
#if !DEBUG
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
#endif
                                    title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from Civil";
                                    message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                                    sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                                }
                            }
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Rejected from E&I";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 5:
                        if (stat == 1)
                        {
#if !DEBUG
                                if (this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                    userIds.Add(u.id);
                                }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Need Review and Approval from Facility Owner";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Please approve cancellation of Excavation Permit No. " + this.ex_no, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.FACILITIES.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from SHE Official";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);

                        }
                        retVal = 1;
                        break;
                    case 6:
                        if (stat == 1)
                        {
#if !DEBUG
                            if (is_guest)
                            {
                                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
                            else
                            {
                                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                                {
                                    listEmail.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].email);
                                    userIds.Add(this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id);
                                    if ((userId = this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate) != null)
                                    {
                                        userEx = new UserEntity(userId.Value, user.token, user);
                                        listEmail.Add(userEx.email);
                                        userIds.Add(userEx.id);
                                    }
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Cancellation Completed and Approved";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + " has been completed.", serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
#if !DEBUG
                            if (this.userInExcavation[UserInExcavation.FACILITIES.ToString()] != null)
                            {
                                listEmail.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                                listEmail.Add(this.userInExcavation[UserInExcavation.EI.ToString()].email);
                                userIds.Add(this.userInExcavation[UserInExcavation.EI.ToString()].id);
                                if ((userId = this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate) != null)
                                {
                                    userEx = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userEx.email);
                                    userIds.Add(userEx.id);
                                }
                            }
#endif
                            title = "Excavation Clearance Permit (" + this.ex_no + ") Rejected from Facility Owner";
                            message = serverUrl + "Home?p=Excavation/edit/" + this.id + "<br />Comment: " + comment;
                            sendEmail.SendToNotificationCenter(userIds, "Excavation Permit", "Cancellation of Excavation Permit No. " + this.ex_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Excavation/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                }

                sendEmail.Send(listEmail, message, title);
            }

            return retVal;
        }

        #region internal function

        private void generateUserInExcavation(excavation ex,UserEntity user, ListUser listUsers = null)
        {
 	        ListUser listUser = listUsers != null ? listUsers : new ListUser(user.token, user.id);
            int userId = 0;

            if (this.is_guest)
            {
                UserEntity userGuest = new UserEntity();
                userGuest.alpha_name = this.requestor;
                this.userInExcavation.Add(UserInExcavation.REQUESTOR.ToString(), userGuest);
            }
            else
            {
                Int32.TryParse(this.requestor, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.REQUESTOR.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.supervisor, out userId);
            this.isUser = this.isUser || userId == user.id;
            this.userInExcavation.Add(UserInExcavation.SUPERVISOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            if (facilitiesUser != null)
            {
                this.isUser = this.isUser || facilitiesUser.user.id == user.id;
                this.userInExcavation.Add(UserInExcavation.FACILITIES.ToString(), facilitiesUser.user);
            }

            if (eiUser != null)
            {
                this.isUser = this.isUser || eiUser.user.id == user.id;
                this.userInExcavation.Add(UserInExcavation.EI.ToString(), eiUser.user);
            }

            userId = 0;
            Int32.TryParse(this.safety_officer, out userId);
            this.isUser = this.isUser || userId == user.id;
            this.userInExcavation.Add(UserInExcavation.SAFETYOFFICER.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.facility_owner, out userId);
            this.isUser = this.isUser || userId == user.id;
            this.userInExcavation.Add(UserInExcavation.FACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));

            if (this.supervisor_delegate != null) {
                userId = 0;
                Int32.TryParse(this.supervisor_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.SUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.safety_officer_delegate != null) {
                userId = 0;
                Int32.TryParse(this.safety_officer_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.SAFETYOFFICERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.facilities_delegate != null) {
                userId = 0;
                Int32.TryParse(this.facilities_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.FACILITIESDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.ei_delegate != null) {
                userId = 0;
                Int32.TryParse(this.ei_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.EIDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.requestor_delegate != null) {
                userId = 0;
                Int32.TryParse(this.requestor_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.REQUESTORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.facility_owner_delegate != null) {
                userId = 0;
                Int32.TryParse(this.facility_owner_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.FACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_supervisor_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_supervisor_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANSUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_safety_officer_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_safety_officer_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANSAFETYOFFICERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_facilities_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_facilities_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANFACILITIESDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_ei_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_ei_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANEIDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_requestor_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_requestor_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANREQUESTORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            if (this.can_facility_owner_delegate != null) {
                userId = 0;
                Int32.TryParse(this.can_facility_owner_delegate, out userId);
                this.isUser = this.isUser || userId == user.id;
                this.userInExcavation.Add(UserInExcavation.CANFACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
        }

#endregion

        #region is can edit

        public bool isCanEditFormRequestor(UserEntity user) {
            if (this.ptw.is_guest == 1)
            {
                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                {
                    if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CREATE)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInExcavation[UserInExcavation.REQUESTOR.ToString()] != null)
                {
                    if ((this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CREATE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanApproveSpv(UserEntity user)
        {
            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
            {
                if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.EDITANDSEND)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveSHE(UserEntity user)
        {
            if (this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()] != null)
            {
                if ((this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.SPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveEI(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p ==  UserInExcavation.EI.ToString()))
            {
                if ((this.userInExcavation[UserInExcavation.EI.ToString()].id == user.id || this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.SHEAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveFacilities(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()))
            {
                if ((this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id == user.id || this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.SHEAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveRequestor(UserEntity user)
        {
            if (this.ptw.is_guest == 1)
            {
                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                {
                    if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.EIFACAPPROVE)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.REQUESTOR.ToString()))
                {
                    if ((this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.EIFACAPPROVE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanApproveFO(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITYOWNER.ToString()))
            {
                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id == user.id || this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.REQUESTORAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)ExStatus.REQUESTORAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanAssign(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITYOWNER.ToString()))
            {
                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id == user.id || this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate == user.id) && this.status <= (int)ExStatus.SPVAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status <= (int)ExStatus.SPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanCancel(UserEntity user)
        {
            if (this.ptw.is_guest == 1)
            {
                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                {
                    if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status <= (int)ExStatus.FOAPPROVE && this.ptw.status >= (int)PtwEntity.statusPtw.ACCFO)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.REQUESTOR.ToString()))
                {
                    if ((this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate == user.id) && this.status <= (int)ExStatus.FOAPPROVE && this.ptw.status >= (int)PtwEntity.statusPtw.ACCFO)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanApproveSpvCancel(UserEntity user)
        {
            if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
            {
                if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CANREQUESTORAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveSHECancel(UserEntity user)
        {
            if (this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()] != null)
            {
                if ((this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SAFETYOFFICER.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CANEIFACAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveEICancel(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.EI.ToString()))
            {
                if ((this.userInExcavation[UserInExcavation.EI.ToString()].id == user.id || this.userInExcavation[UserInExcavation.EI.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CANSPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveFacilitiesCancel(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITIES.ToString()))
            {
                if ((this.userInExcavation[UserInExcavation.FACILITIES.ToString()].id == user.id || this.userInExcavation[UserInExcavation.FACILITIES.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CANSPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveRequestorCancel(UserEntity user)
        {
            if (this.ptw.is_guest == 1)
            {
                if (this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()] != null)
                {
                    if ((this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.SUPERVISOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CLOSING)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.REQUESTOR.ToString()))
                {
                    if ((this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].id == user.id || this.userInExcavation[UserInExcavation.REQUESTOR.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CLOSING)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanApproveFOCancel(UserEntity user)
        {
            if (this.userInExcavation.Keys.ToList().Exists(p => p == UserInExcavation.FACILITYOWNER.ToString()))
            {
                List<UserEntity> listDel = this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].id == user.id || this.userInExcavation[UserInExcavation.FACILITYOWNER.ToString()].employee_delegate == user.id) && this.status == (int)ExStatus.CANSHEAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)ExStatus.CANSHEAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        #region assignment

        internal int assignSO(string serverUrl, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.safety_officer = this.safety_officer;

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                UserEntity userSO = new UserEntity(Int32.Parse(this.safety_officer), user.token, user);
                List<string> email = new List<string>();
                email.Add(userSO.email);
                // email.Add("septujamasoka@gmail.com");
                SendEmail sendEmail = new SendEmail();

                string message = serverUrl + "Home?p=Excavation/edit/" + this.id;

                sendEmail.Send(email, message, "Assigned as SHE Official for Excavation Clearance Permit (" + ex.ex_no + ")");
            }

            return retVal;
        }

        internal int assignFAC(string serverUrl, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.facilities = this.facilities;

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                MstFacilitiesEntity fac = new MstFacilitiesEntity(Int32.Parse(this.facilities), user);
                List<string> email = new List<string>();
                email.Add(fac.user.email);
                // email.Add("septujamasoka@gmail.com");
                SendEmail sendEmail = new SendEmail();

                string message = serverUrl + "Home?p=Excavation/edit/" + this.id;

                sendEmail.Send(email, message, "Assigned as Facilities for Excavation Clearance Permit (" + ex.ex_no + ")");
            }

            return retVal;
        }

        internal int assignEI(string serverUrl, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.ei = this.ei;

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                MstEIEntity fac = new MstEIEntity(Int32.Parse(this.ei), user);
                List<string> email = new List<string>();
                email.Add(fac.user.email);
                // email.Add("septujamasoka@gmail.com");
                SendEmail sendEmail = new SendEmail();

                string message = serverUrl + "Home?p=Excavation/edit/" + this.id;

                sendEmail.Send(email, message, "Assigned as E&I for Excavation Clearance Permit (" + ex.ex_no + ")");
            }

            return retVal;
        }

        internal int assignSpv(string serverUrl, UserEntity user)
        {
            int retVal = 0;
            excavation ex = this.db.excavations.Find(this.id);
            if (ex != null)
            {
                ex.supervisor = user.id.ToString();

                this.db.Entry(ex).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                UserEntity supervisor = new UserEntity(Int32.Parse(ex.supervisor), user.token, user);
                List<string> email = new List<string>();
                email.Add(supervisor.email);
                // email.Add("septujamasoka@gmail.com");
                SendEmail sendEmail = new SendEmail();

                string message = serverUrl + "Home?p=Excavation/edit/" + this.id;

                sendEmail.Send(email, message, "Assigned as Supervisor for Excavation Clearance Permit (" + ex.ex_no + ")");
            }

            return retVal;
        }

        #endregion

        public bool userInEx(UserEntity user)
        {
            foreach (KeyValuePair<string, UserEntity> entry in userInExcavation)
            {
                UserEntity us = entry.Value;
                if (entry.Key == UserInExcavation.FACILITYOWNER.ToString() || entry.Key == UserInExcavation.SUPERVISOR.ToString() || entry.Key == UserInExcavation.FACILITYOWNER.ToString())
                {
                }
                else if (us != null && (user.id == us.id || user.id == us.employee_delegate))
                {
                    return true;
                }
            }

            return false;
        }

        public string getHiraNo()
        {
            this.hira_no = "";
            if (this.ptw.hira_docs != null)
            {
                string[] s = this.ptw.hira_docs.Split(new string[] { "#@#" }, StringSplitOptions.None);
                foreach (string ss in s)
                {
                    if (!String.IsNullOrEmpty(ss))
                    {
                        string name = ss.Split('/').Last();
                        string fileName = name.Substring(0, name.Length - 4);
                        fileName = HttpUtility.UrlDecode(fileName);
                        this.hira_no += ", " + fileName;
                    }
                }
            }

            if (this.hira_no.Length == 0)
            {
                return this.hira_no;
            }
            else
            {
                this.hira_no = this.hira_no.Substring(2);
                return this.hira_no;
            }
        }

        public void getPtw(UserEntity user)
        {

            this.ptw = new PtwEntity(this.id_ptw.Value, user);
        }
        public void getPtw(UserEntity user, ListUser listUser)
        {

            this.ptw = new PtwEntity(this.id_ptw.Value, user, listUser);
        }
    }
}