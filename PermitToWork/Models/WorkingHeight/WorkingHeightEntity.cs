using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.WorkingHeight
{
    public class WorkingHeightEntity : working_height, IClearancePermitEntity
    {
        public override permit_to_work permit_to_work { get { return null; } set { } }

        // ptw entity for PTW reference
        private PtwEntity ptw { get; set; }

        public string[] workbox_access_arr { get; set; }
        public string[] ladder_access_arr { get; set; }
        public string[] elevated_access_arr { get; set; }

        public string[] scaffold_access_arr { get; set; }

        public string[] fall_prevention_assess_arr { get; set; }

        public string[] screening_req_arr { get; set; }
        public string[] screening_fo_arr { get; set; }

        public string[] can_screening_req_arr { get; set; }
        public string[] can_screening_fo_arr { get; set; }

        public MstErectorEntity erectorUser { get; set; }
        public MstInspectorEntity inspectorUser { get; set; }

        public Dictionary<string, UserEntity> userInWorkingHeight { get; set; }

        public Dictionary<string, List<string>> listDocumentUploaded { get; set; }

        public int ids { get; set; }
        public string statusText { get; set; }

        // HIRA Document related
        // public List<HiraEntity> hira_document { get; set; }
        // public string hira_no { get; set; }

        public string rad_status { get; set; }

        private star_energy_ptwEntities db;

        public enum WHStatus
        {
            CREATE,
            EDITANDSEND,
            INSPECTORSIGN,
            REQUESTORSCREENING,
            FOSCREENING,
            REQUESTORAPPROVE,
            SPVAPPROVE,
            FOAPPROVE,
            CLOSING,
            CANREQUESTORSCREENING,
            CANFOSCREENING,
            CANREQUESTORAPPROVE,
            CANSPVAPPROVE,
            CANFOAPPROVE,
        }

        public enum UserInRadiography
        {
            REQUESTOR,
            ERECTOR,
            INSPECTOR,
            SUPERVISOR,
            FACILITYOWNER,
            REQUESTORDELEGATE,
            ERECTORDELEGATE,
            SUPERVISORDELEGATE,
            FACILITYOWNERDELEGATE,
            CANREQUESTORDELEGATE,
            CANERECTORDELEGATE,
            CANSUPERVISORDELEGATE,
            CANFACILITYOWNERDELEGATE,
        }

        public enum DocumentUploaded
        {
            ATTACHMENT
        }

        // parameterless constructor for declaring db
        public WorkingHeightEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
            this.workbox_access_arr = new string[4];
            this.ladder_access_arr = new string[4];
            this.elevated_access_arr = new string[3];
            this.scaffold_access_arr = new string[3];

            this.fall_prevention_assess_arr = new string[3];

            this.screening_req_arr = new string[3];
            this.screening_fo_arr = new string[3];

            this.can_screening_req_arr = new string[3];
            this.can_screening_fo_arr = new string[3];

            this.userInWorkingHeight = new Dictionary<string, UserEntity>();
            this.listDocumentUploaded = new Dictionary<string, List<string>>();
            //this.screening_fo_arr = new string[];
        }

        // constructor with id to get object from database
        public WorkingHeightEntity(int id, UserEntity user)
            : this()
        {
            working_height rad = this.db.working_height.Find(id);
            if (rad != null)
            {
                // this.ptw = new PtwEntity(fi.id_ptw.Value);
                ModelUtilization.Clone(rad, this);

                this.workbox_access_arr = this.workbox_access.Split('#');
                this.ladder_access_arr = this.ladder_access.Split('#');
                this.elevated_access_arr = this.elevated_access.Split('#');
                this.scaffold_access_arr = this.scaffold_access.Split('#');

                this.screening_req_arr = this.pre_screening_req.Split('#');
                this.screening_fo_arr = this.pre_screening_fo.Split('#');

                this.can_screening_req_arr = this.can_screening_req.Split('#');
                this.can_screening_fo_arr = this.can_screening_fo.Split('#');

                this.fall_prevention_assess_arr = this.fall_prevention_assess.Split('#');

                if (this.erector.HasValue)
                {
                    this.erectorUser = new MstErectorEntity(this.erector.Value, user);
                }

                if (this.inspector.HasValue)
                {
                    this.inspectorUser = new MstInspectorEntity(this.inspector.Value, user);
                }

                generateUserInWorkingHeight(rad, user);

                string path = HttpContext.Current.Server.MapPath("~/Upload/WorkingHeight/" + this.id + "/Attachment");

                DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
                FileInfo[] Files = d.GetFiles(); //Getting Text files

                this.listDocumentUploaded.Add(DocumentUploaded.ATTACHMENT.ToString(), Files.Select(p => p.Name).ToList());

                //this.rad_status = getStatus();

                // generateUserInWorkingHeight(rad, user);

                // this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
            }
        }

        public WorkingHeightEntity(int ptw_id, string requestor, string purpose, string acc_fo)
            : this()
        {
            // TODO: Complete member initialization
            this.description = purpose;
            this.id_ptw = ptw_id;
            this.requestor = requestor;
            this.facility_owner = acc_fo;

            this.pre_screening_fo = "##";
            this.pre_screening_req = "##";
            this.can_screening_fo = "##";
            this.can_screening_req = "##";
            this.workbox_access = "###";
            this.ladder_access = "###";
            this.elevated_access = "##";
            this.scaffold_access = "##";
            this.fall_prevention_assess = "##";
        }

        // function insert data to database
        public int create()
        {
            working_height rad = new working_height();
            this.status = (int)WHStatus.CREATE;
            ModelUtilization.Clone(this, rad);
            this.db.working_height.Add(rad);
            int retVal = this.db.SaveChanges();
            this.id = rad.id;

            string path = HttpContext.Current.Server.MapPath("~/Upload/WorkingHeight/" + this.id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = HttpContext.Current.Server.MapPath("~/Upload/WorkingHeight/" + this.id + "/Attachment");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return retVal;
        }

        // function for editing by requestor
        public int edit()
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            if (wh != null)
            {
                wh.description = this.description;
                wh.work_location = this.work_location;
                wh.from = this.from;
                wh.until = this.until;
                wh.total_crew = this.total_crew;
                wh.access = this.access;
                if (wh.access == 1)
                {
                    wh.workbox_access = this.workbox_access;
                }
                else if (wh.access == 2)
                {
                    wh.ladder_access = this.ladder_access;
                }
                else if (wh.access == 3)
                {
                    wh.elevated_access = this.elevated_access;
                }
                else if (wh.access == 4)
                {
                    wh.scaffolding = this.scaffolding;
                    wh.load_capacity = this.load_capacity;
                    wh.no_person = this.no_person;
                    wh.scaffold_access = this.scaffold_access;
                    if (wh.scaffolding == 1 || wh.scaffolding == 3)
                    {
                        wh.erector = this.erector;
                        wh.erector_certificate_no = this.erector_certificate_no;
                        wh.erector_valid_date = this.erector_valid_date;
                    }
                    else if (wh.scaffolding == 2)
                    {
                        wh.inspector = this.inspector;
                        wh.inspector_certify_no = this.inspector_certify_no;
                        wh.utilization_valid_date = this.utilization_valid_date;
                    }
                }
                wh.mandatory_fall_prevention = this.mandatory_fall_prevention;
                wh.fall_prevention_assess = this.fall_prevention_assess;

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int delete()
        {
            working_height wh = new working_height();
            ModelUtilization.Clone(this, wh);
            this.db.working_height.Remove(wh);
            int retVal = this.db.SaveChanges();
            return retVal;
        }

        public void generateNumber(string ptw_no)
        {
            string result = "WH-" + ptw_no;

            this.wh_no = result;
        }

        public int sendInspectorOrPrescreening(UserEntity user, string serverUrl)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                wh.status = (int)WHStatus.EDITANDSEND;

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                if (this.access == 4 && this.scaffolding == 2)
                {
                    retVal = 2;
                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Inspector Scaffolding Utilization Approval");
                }
                else
                {
                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Pre-Job Screening");
                }
            }
            return retVal;
        }

        public int inspectorSign(UserEntity user, string serverUrl)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                wh.inspector_signature = user.signature;
                wh.inspector_sign_date = DateTime.Now;

                wh.status = (int)WHStatus.INSPECTORSIGN;

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                email.Add("septujamasoka@gmail.com");
                //s.Add(gasTester.email);
                //s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=WH/edit/" + this.id;

                sendEmail.Send(email, message, "Working At Height Clearance Permit Requestor Pre-Job Screening");
            }

            return retVal;
        }

        public int savePreScreening(int who)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            if (wh != null)
            {
                if (who == 1)
                {
                    wh.pre_screening_req = this.pre_screening_req;
                }
                else if (who == 2)
                {
                    wh.pre_screening_fo = this.pre_screening_fo;
                }

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int completePreScreening(int who, string serverUrl, UserEntity user)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                if (who == 1)
                {
                    wh.status = (int)WHStatus.REQUESTORSCREENING;

                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Facility Owner Pre-Job Screening");
                }
                else if (who == 2)
                {
                    wh.status = (int)WHStatus.FOSCREENING;

                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Pre-Job Screening Completed");
                }

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int rejectPreScreening(int who, string serverUrl, UserEntity user, string comment)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                if (who == 2)
                {
                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Pre-Job Screening Rejected");
                }
            }

            return retVal;
        }

        internal int signPermitStart(UserEntity user, int who, string serverUrl, out string messages)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);

            List<string> email = new List<string>();
            string title = "";
            SendEmail sendEmail = new SendEmail();
            string message = "";
            messages = "";
            if (wh != null)
            {
                switch (who)
                {
                    case 1:
                        if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
                        {
                            if (user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id)
                            {
                                wh.requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate)
                            {
                                wh.requestor_delegate = user.id.ToString();
                                wh.requestor_signature = "d" + user.signature;
                            }
                        }
                        else
                        {
                            if (user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id)
                            {
                                wh.requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate)
                            {
                                wh.requestor_delegate = user.id.ToString();
                                wh.requestor_signature = "d" + user.signature;
                            }
                        }

                        wh.requestor_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.REQUESTORAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}

                        if (this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()] == null)
                        {
                            //email.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                            email.Add("septujamasoka@gmail.com");
                            title = "Working At Height Clearance Permit Supervisor Assignment Needed";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            messages = "Working At Height Clearance Permit is signed. Notification has been sent to Supervisor for clicking link in email.";
                        }
                        else
                        {
                            //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                            //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                            //{
                            //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                            //    email.Add(@delegate.email);
                            //}
                            email.Add("septujamasoka@gmail.com");
                            title = "Working At Height Clearance Permit Supervisor Approval";
                            message = serverUrl + "Home?p=WH/edit/" + this.id;
                            messages = "Working At Height Clearance Permit is signed. Notification has been sent to Supervisor for Signing.";
                        }
                        break;
                    case 2:
                        if (user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].id)
                        {
                            wh.supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].employee_delegate)
                        {
                            wh.supervisor_delegate = user.id.ToString();
                            wh.supervisor_signature = "d" + user.signature;
                        }

                        wh.supervisor_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.SPVAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}
                        email.Add("septujamasoka@gmail.com");
                        title = "Working At Height Clearance Permit Facility Owner Approval";
                        message = serverUrl + "Home?p=WH/edit/" + this.id;
                        messages = "Working At Height Clearance Permit is signed. Notification has been sent to Facility Owner for Signing.";
                        break;
                    case 3:
                        if (user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id)
                        {
                            wh.facility_owner_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate)
                        {
                            wh.facility_owner_delegate = user.id.ToString();
                            wh.facility_owner_signature = "d" + user.signature;
                        }

                        wh.facility_owner_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.FOAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        this.ptw = new PtwEntity(wh.id_ptw.Value, user);

                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString());

                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}
                        email.Add("septujamasoka@gmail.com");
                        title = "Working At Height Clearance Permit Signed";
                        message = serverUrl + "Home?p=WH/edit/" + this.id;
                        messages = "Working At Height Clearance Permit is signed.";
                        break;
                }


                sendEmail.Send(email, message, title);
            }

            return retVal;
        }

        internal int cancelPermit(UserEntity user, string serverUrl)
        {
            int retVal = 0;
            working_height rad = this.db.working_height.Find(this.id);

            List<string> email = new List<string>();
            string title = "";
            SendEmail sendEmail = new SendEmail();

            if (rad != null)
            {
                rad.status = (int)WHStatus.CLOSING;

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                email.Add("septujamasoka@gmail.com");
                title = "Working At Height Clearance Permit Requestor / Erector Cancellation Screening";
                string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                sendEmail.Send(email, message, title);
            }

            return retVal;
        }

        public int saveCancelScreening(int who)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            if (wh != null)
            {
                if (who == 1)
                {
                    wh.can_screening_req = this.can_screening_req;
                }
                else if (who == 2)
                {
                    wh.can_screening_fo = this.can_screening_fo;
                }

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int completeCancelScreening(int who, string serverUrl, UserEntity user)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                if (who == 1)
                {
                    wh.status = (int)WHStatus.CANREQUESTORSCREENING;

                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Facility Owner Cancellation Screening");
                }
                else if (who == 2)
                {
                    wh.status = (int)WHStatus.CANFOSCREENING;

                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Cancellation Screening Completed");
                }

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int rejectCancelScreening(int who, string serverUrl, UserEntity user, string comment)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            List<string> email = new List<string>();
            SendEmail sendEmail = new SendEmail();
            if (wh != null)
            {
                if (who == 2)
                {
                    // sending email
                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                    email.Add("septujamasoka@gmail.com");
                    //s.Add(gasTester.email);
                    //s.Add("septu.jamasoka@gmail.com");

                    string message = serverUrl + "Home?p=WH/edit/" + this.id;

                    sendEmail.Send(email, message, "Working At Height Clearance Permit Cancellation Screening Rejected");
                }
            }

            return retVal;
        }

        internal int signPermitCancel(UserEntity user, int who, string serverUrl, out string messages)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);

            List<string> email = new List<string>();
            string title = "";
            SendEmail sendEmail = new SendEmail();
            string message = "";
            messages = "";
            if (wh != null)
            {
                switch (who)
                {
                    case 1:
                        if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
                        {
                            if (user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id)
                            {
                                wh.can_requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate)
                            {
                                wh.can_requestor_delegate = user.id.ToString();
                                wh.can_requestor_signature = "d" + user.signature;
                            }
                        }
                        else
                        {
                            if (user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id)
                            {
                                wh.can_requestor_signature = "a" + user.signature;
                            }
                            else if (user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate)
                            {
                                wh.can_requestor_delegate = user.id.ToString();
                                wh.can_requestor_signature = "d" + user.signature;
                            }
                        }

                        wh.can_requestor_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.CANREQUESTORAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        
                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}
                        email.Add("septujamasoka@gmail.com");
                        title = "Working At Height Clearance Permit Supervisor Cancellation Sign";
                        message = serverUrl + "Home?p=WH/edit/" + this.id;
                        messages = "Working At Height Clearance Permit Cancellation is signed. Notification has been sent to Supervisor for Signing.";
                        break;
                    case 2:
                        if (user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].id)
                        {
                            wh.can_supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].employee_delegate)
                        {
                            wh.can_supervisor_delegate = user.id.ToString();
                            wh.can_supervisor_signature = "d" + user.signature;
                        }

                        wh.can_supervisor_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.CANSPVAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}
                        email.Add("septujamasoka@gmail.com");
                        title = "Working At Height Clearance Permit Facility Owner Cancellation Sign";
                        message = serverUrl + "Home?p=WH/edit/" + this.id;
                        messages = "Working At Height Clearance Permit Cancellation is signed. Notification has been sent to Facility Owner for Signing.";
                        break;
                    case 3:
                        if (user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id)
                        {
                            wh.can_facility_owner_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate)
                        {
                            wh.can_facility_owner_delegate = user.id.ToString();
                            wh.can_facility_owner_signature = "d" + user.signature;
                        }

                        wh.can_facility_owner_signature_date = DateTime.Now;
                        wh.status = (int)WHStatus.CANFOAPPROVE;

                        this.db.Entry(wh).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        this.ptw = new PtwEntity(wh.id_ptw.Value, user);

                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString());

                        //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        //{
                        //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                        //    email.Add(@delegate.email);
                        //}
                        email.Add("septujamasoka@gmail.com");
                        title = "Working At Height Clearance Permit Cancellation Signed";
                        message = serverUrl + "Home?p=WH/edit/" + this.id;
                        messages = "Working At Height Clearance Permit cancellation is signed.";
                        break;
                }


                sendEmail.Send(email, message, title);
            }

            return retVal;
        }

        #region internal function

        private void generateUserInWorkingHeight(working_height wh, UserEntity user)
        {
            ListUser listUser = new ListUser(user.token, user.id);
            int userId = 0;

            Int32.TryParse(this.requestor, out userId);
            this.userInWorkingHeight.Add(UserInRadiography.REQUESTOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            if (this.inspectorUser != null)
            {
                this.userInWorkingHeight.Add(UserInRadiography.INSPECTOR.ToString(), this.inspectorUser.user);
            }

            if (this.erectorUser != null)
            {
                this.userInWorkingHeight.Add(UserInRadiography.ERECTOR.ToString(), this.erectorUser.user);
            }

            Int32.TryParse(this.supervisor, out userId);
            this.userInWorkingHeight.Add(UserInRadiography.SUPERVISOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.facility_owner, out userId);
            this.userInWorkingHeight.Add(UserInRadiography.FACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.requestor_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.REQUESTORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.supervisor_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.SUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.facility_owner_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.FACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_requestor_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.CANREQUESTORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_supervisor_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.CANSUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_facility_owner_delegate, out userId);
            if (userId != 0)
            {
                this.userInWorkingHeight.Add(UserInRadiography.CANFACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
        }

        #endregion

        #region assigment user

        internal int assignSupervisor(UserEntity user)
        {
            int retVal = 0;
            working_height wh = this.db.working_height.Find(this.id);
            if (wh != null)
            {
                wh.supervisor = user.id.ToString();

                this.db.Entry(wh).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        #endregion

        #region is can edit user

        public bool isCanEditFormRequestor(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status <= (int)WHStatus.REQUESTORSCREENING)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanSignInspector(UserEntity user)
        {
            if (this.userInWorkingHeight.Keys.ToList().Exists(p => p == UserInRadiography.INSPECTOR.ToString()) && this.access == 4 && this.scaffolding == 2)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.INSPECTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.INSPECTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.EDITANDSEND)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanPreScreeningRequestor(UserEntity user)
        {
            if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
            {
                if (this.userInWorkingHeight.Keys.ToList().Exists(p => p == UserInRadiography.ERECTOR.ToString()))
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.EDITANDSEND)
                    {
                        return true;
                    }
                }
            }
            else if (this.access == 4 && this.scaffolding == 2)
            {
                if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.INSPECTORSIGN)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.EDITANDSEND)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanPreScreeningFO(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)WHStatus.REQUESTORSCREENING)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanSignRequestor(UserEntity user)
        {
            if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
            {
                if (this.userInWorkingHeight.Keys.ToList().Exists(p => p == UserInRadiography.ERECTOR.ToString()))
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.FOSCREENING)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.FOSCREENING)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanSignSpv(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)WHStatus.REQUESTORAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanSignFO(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)WHStatus.SPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanCancel(UserEntity user)
        {
            this.ptw = new PtwEntity(this.id_ptw.Value, user);
            if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status <= (int)WHStatus.FOAPPROVE && this.ptw.status == (int)PtwEntity.statusPtw.ACCFO)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanCancelScreeningRequestor(UserEntity user)
        {
            if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
            {
                if (this.userInWorkingHeight.Keys.ToList().Exists(p => p == UserInRadiography.ERECTOR.ToString()))
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.CLOSING)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.CLOSING)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanCancelScreeningFO(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)WHStatus.CANREQUESTORSCREENING)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanSignRequestorCancel(UserEntity user)
        {
            if (this.access == 4 && (this.scaffolding == 1 || this.scaffolding == 3))
            {
                if (this.userInWorkingHeight.Keys.ToList().Exists(p => p == UserInRadiography.ERECTOR.ToString()))
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.ERECTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.CANFOSCREENING)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)WHStatus.CANFOSCREENING)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanSignSpvCancel(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)WHStatus.CANREQUESTORAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanSignFOCancel(UserEntity user)
        {
            if (this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                if ((user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInWorkingHeight[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)WHStatus.CANSPVAPPROVE)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        internal bool isUserInWH(UserEntity user)
        {
            foreach (UserEntity us in this.userInWorkingHeight.Values)
            {
                if (us != null)
                {
                    if (us.id == user.id || us.employee_delegate == user.id)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}