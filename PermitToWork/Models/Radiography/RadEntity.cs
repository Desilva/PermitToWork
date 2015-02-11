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

namespace PermitToWork.Models.Radiography
{
    public class RadEntity : radiography, IClearancePermitEntity
    {
        public override permit_to_work permit_to_work { get { return null; } set { } }

        // ptw entity for PTW reference
        private PtwEntity ptw { get; set; }

        public string[] screening_spv_arr { get; set; }
        public string[] screening_rad_arr { get; set; }
        public string[] screening_fo_arr { get; set; }

        public string[] can_screening_spv_arr { get; set; }
        public string[] can_screening_rad_arr { get; set; }
        public string[] can_screening_fo_arr { get; set; }

        public Dictionary<string, UserEntity> userInRadiography { get; set; }

        public MstRadiographerEntity radiographer2 { get; set; }
        public MstRadiographerEntity radiographer1 { get; set; }
        public MstRadiationPOEntity radiationPO { get; set; }

        public Dictionary<string, List<string>> listDocumentUploaded { get; set; }

        public int ids { get; set; }
        public string statusText { get; set; }

        public bool is_guest { get; set; }
        public bool isUser { get; set; }

        // HIRA Document related
        // public List<HiraEntity> hira_document { get; set; }
        // public string hira_no { get; set; }

        public string rad_status { get; set; }

        private star_energy_ptwEntities db;

        public enum RadStatus
        {
            CREATE,
            EDITANDSEND,
            OPERATORAPPROVE,
            RADAPPROVE,
            SPVAPPROVE,
            SOAPPROVE,
            FOAPPROVE,
            CLOSING,
            CANOPERATORAPPROVE,
            CANRADAPPROVE,
            CANSPVAPPROVE,
            CANSOAPPROVE,
            CANFOAPPROVE
        };

        public enum UserInRadiography
        {
            REQUESTOR,
            RADIOGRAPHER1,
            RADIOGRAPHER2,
            SUPERVISOR,
            SAFETYOFFICER,
            FACILITYOWNER,
            RADIOGRAPHER1DELEGATE,
            RADIOGRAPHER2DELEGATE,
            SUPERVISORDELEGATE,
            SAFETYOFFICERDELEGATE,
            FACILITYOWNERDELEGATE,
            CANRADIOGRAPHER1DELEGATE,
            CANRADIOGRAPHER2DELEGATE,
            CANSUPERVISORDELEGATE,
            CANSAFETYOFFICERDELEGATE,
            CANFACILITYOWNERDELEGATE,
        }

        public enum DocumentUploaded
        {
            RADIOGRAPHER1LICENSENUMBER,
            RADIOGRAPHER2LICENSENUMBER,
            RADIATIONPOLICENSENUMBER,
            ATTACHMENT
        }

        // parameterless constructor for declaring db
        public RadEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
            this.userInRadiography = new Dictionary<string, UserEntity>();
            this.listDocumentUploaded = new Dictionary<string, List<string>>();
            //this.screening_fo_arr = new string[];
        }

        // constructor with id to get object from database
        public RadEntity(int id, UserEntity user)
            : this()
        {
            radiography rad = this.db.radiographies.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(rad, this);

            this.screening_fo_arr = this.pre_screening_fo.Split('#');
            this.screening_spv_arr = this.pre_screening_spv.Split('#');
            this.screening_rad_arr = this.pre_screening_rad.Split('#');

            this.can_screening_fo_arr = this.can_screening_fo.Split('#');
            this.can_screening_spv_arr = this.can_screening_spv.Split('#');
            this.can_screening_rad_arr = this.can_screening_rad.Split('#');

            this.is_guest = rad.permit_to_work.is_guest == 1;

            int radiographerId = 0;

            if (Int32.TryParse(this.radiographer_1, out radiographerId))
            {
                this.radiographer1 = new MstRadiographerEntity(radiographerId, user);
            }

            if (Int32.TryParse(this.radiographer_2, out radiographerId))
            {
                this.radiographer2 = new MstRadiographerEntity(radiographerId, user);
            }

            if (Int32.TryParse(this.radiation_protection_officer, out radiographerId))
            {
                this.radiationPO = new MstRadiationPOEntity(radiographerId, user);
            }

            string path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumber1");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIOGRAPHER1LICENSENUMBER.ToString(), Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumber2");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIOGRAPHER2LICENSENUMBER.ToString(), Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumberRPO");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIATIONPOLICENSENUMBER.ToString(), Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/Attachment");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.ATTACHMENT.ToString(), Files.Select(p => p.Name).ToList());

            this.statusText = getStatus();

            generateUserInRadiography(rad, user, null);

            // this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        public RadEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            radiography rad = this.db.radiographies.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(rad, this);

            this.screening_fo_arr = this.pre_screening_fo.Split('#');
            this.screening_spv_arr = this.pre_screening_spv.Split('#');
            this.screening_rad_arr = this.pre_screening_rad.Split('#');

            this.can_screening_fo_arr = this.can_screening_fo.Split('#');
            this.can_screening_spv_arr = this.can_screening_spv.Split('#');
            this.can_screening_rad_arr = this.can_screening_rad.Split('#');

            this.is_guest = rad.permit_to_work.is_guest == 1;

            int radiographerId = 0;

            if (Int32.TryParse(this.radiographer_1, out radiographerId)) {
                this.radiographer1 = new MstRadiographerEntity(radiographerId, user, listUser);
            }

            if (Int32.TryParse(this.radiographer_2, out radiographerId))
            {
                this.radiographer2 = new MstRadiographerEntity(radiographerId, user, listUser);
            }

            if (Int32.TryParse(this.radiation_protection_officer, out radiographerId))
            {
                this.radiationPO = new MstRadiationPOEntity(radiographerId, user, listUser);
            }

            string path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumber1");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIOGRAPHER1LICENSENUMBER.ToString(),Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumber2");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIOGRAPHER2LICENSENUMBER.ToString(), Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumberRPO");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.RADIATIONPOLICENSENUMBER.ToString(), Files.Select(p => p.Name).ToList());

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/Attachment");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            d = new DirectoryInfo(path);//Assuming Test is your Folder
            Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add(DocumentUploaded.ATTACHMENT.ToString(), Files.Select(p => p.Name).ToList());

            this.statusText = getStatus();

            generateUserInRadiography(rad, user, listUser);

            // this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        public RadEntity(radiography rad, ListUser listUser, UserEntity user)
            : this()
        {
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(rad, this);

            this.is_guest = rad.permit_to_work.is_guest == 1;
            this.isUser = false;

            int radiographerId = 0;

            if (Int32.TryParse(this.radiographer_1, out radiographerId))
            {
                this.radiographer1 = new MstRadiographerEntity(radiographerId, user, listUser);
            }

            if (Int32.TryParse(this.radiographer_2, out radiographerId))
            {
                this.radiographer2 = new MstRadiographerEntity(radiographerId, user, listUser);
            }

            if (Int32.TryParse(this.radiation_protection_officer, out radiographerId))
            {
                this.radiationPO = new MstRadiationPOEntity(radiographerId, user, listUser);
            }
            this.statusText = getStatus();

            generateUserInRadiography(rad, user, listUser);

            // this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        private string getStatus()
        {
            string retVal = "";
            switch (this.status)
            {
                case (int)RadStatus.CREATE: retVal = "Radiography Permit is still edited by Requestor"; break;
                case (int)RadStatus.EDITANDSEND: retVal = "Waiting for Radiographer Level 1 (Operator) Approval"; break;
                //case (int)FIStatus.SPVSCREENING: retVal = "Waiting for SO Pre-job Screening"; break;
                //case (int)FIStatus.SOSCREENING: retVal = "Waiting for FO Pre-job Screening"; break;
                //case (int)FIStatus.FOSCREENING: retVal = "Waiting for Approval by Requestor"; break;
                case (int)RadStatus.OPERATORAPPROVE: retVal = "Waiting for Radiographer Level 2 Screening and Approval"; break;
                case (int)RadStatus.RADAPPROVE: retVal = "Waiting for Supervisor Screening and Approval"; break;
                case (int)RadStatus.SPVAPPROVE: retVal = "Waiting for Safety Officer Approval"; break;
                case (int)RadStatus.SOAPPROVE: retVal = "Waiting for Facility Owner Screening and Approval"; break;
                case (int)RadStatus.FOAPPROVE: retVal = "Completed. Radiography Permit has been approved by Facility Owner"; break;
                case (int)RadStatus.CLOSING: retVal = "Radiography Permit is cancelled by Requestor. Waiting for Radiographer Level 1 (Operator) Cancellation Approval"; break;
                case (int)RadStatus.CANOPERATORAPPROVE: retVal = "Waiting for Radiographer Level 2 Cancellation Screening and Approval"; break;
                case (int)RadStatus.CANRADAPPROVE: retVal = "Waiting for Supervisor Cancellation Screening and Approval"; break;
                case (int)RadStatus.CANSPVAPPROVE: retVal = "Waiting for Safety Officer Cancellation Approval"; break;
                case (int)RadStatus.CANSOAPPROVE: retVal = "Waiting for Facility Owner CancellationScreening and Approval"; break;
                case (int)RadStatus.CANFOAPPROVE: retVal = "Cancelled. Radiography Permit has been cancelled"; break;
            };

            return retVal;
        }

        public RadEntity(int ptw_id, string requestor, string purpose, string acc_fo, string acc_supervisor, DateTime? start_date, DateTime? end_date)
            : this()
        {
            // TODO: Complete member initialization
            this.purpose = purpose;
            this.id_ptw = ptw_id;
            this.@operator = requestor;
            this.facility_owner = acc_fo;
            this.supervisor = acc_supervisor;
            this.estimate_time_start = start_date;
            this.estimate_time_end = end_date;

            this.pre_screening_fo = "#############";
            this.pre_screening_spv = "#############";
            this.pre_screening_rad = "#############";
            this.can_screening_fo = "##";
            this.can_screening_spv = "##";
            this.can_screening_rad = "##";
        }

        // function insert data to database
        public int create()
        {
            radiography rad = new radiography();
            this.status = (int)RadStatus.CREATE;
            ModelUtilization.Clone(this, rad);
            this.db.radiographies.Add(rad);
            int retVal = this.db.SaveChanges();
            this.id = rad.id;

            string path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id  + "/LicenseNumber1");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumber2");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/LicenseNumberRPO");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            path = HttpContext.Current.Server.MapPath("~/Upload/Radiography/" + this.id + "/Attachment");
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
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                rad.purpose = this.purpose;
                rad.work_location = this.work_location;
                rad.radiographer_2 = this.radiographer_2;
                rad.radiographer_1 = this.radiographer_1;
                rad.radiographer_1_license_number = this.radiographer_1_license_number;
                rad.radiographer_2_license_number = this.radiographer_2_license_number;
                rad.radiation_protection_officer = this.radiation_protection_officer;
                rad.radiation_protection_officer_license_number = this.radiation_protection_officer_license_number;
                rad.potential_area_affected = this.potential_area_affected;
                rad.total_crew = this.total_crew;
                rad.radiographic_source = this.radiographic_source;
                rad.estimate_time_start = this.estimate_time_start;
                rad.estimate_time_end = this.estimate_time_end;

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int delete()
        {
            radiography rad = this.db.radiographies.Find(this.id);
            this.db.radiographies.Remove(rad);
            int retVal = this.db.SaveChanges();
            return retVal;
        }

        public void generateNumber(string ptw_no)
        {
            string result = "RT-" + ptw_no;

            this.rg_no = result;
        }

        public int saveAsDraft(int who)
        {
            int retVal = 0;
            switch (who)
            {
                case 1 /* Requestor */:
                    retVal = this.edit();
                    break;
                case 3 /* Level 2 */:
                    retVal = savePreScreening(2);
                    break;
                case 4 /* Supervisor */:
                    retVal = savePreScreening(1);
                    break;
                case 6 /* Facility Owner */:
                    retVal = savePreScreening(3);
                    break;
                default:
                    retVal = 1;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// function for signing clearance permit
        /// </summary>
        /// <param name="who">1 if requestor, 2 if radiographic operator, 3 if radiographic level 2, 4 if supervisor, 5 if safety officer, 6 if FO</param>
        /// <returns>1 if success, 0 if fail, -1 if user doesn't exist</returns>
        public int signClearance(int who, UserEntity user)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            UserEntity userRad = null;
            if (rad != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:
                        rad.status = (int)RadStatus.EDITANDSEND;

                        break;
                    case 2 /* Radiographic Operator (Level 1) */:
                        userRad = this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.operator_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.operator_signature = "d" + user.signature;
                            rad.operator_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.OPERATORAPPROVE;
                        break;
                    case 3 /* Radiographic Level 2 */:
                        userRad = this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.radiographer_2_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.radiographer_2_signature = "d" + user.signature;
                            rad.radiographer_2_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.RADAPPROVE;
                        break;
                    case 4 /* Supervisor */:
                        userRad = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.supervisor_signature = "d" + user.signature;
                            rad.supervisor_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.SPVAPPROVE;
                        break;
                    case 5 /* SHE Officer */:
                        userRad = this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.safety_officer_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.safety_officer_signature = "d" + user.signature;
                            rad.safety_officer_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.SOAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        userRad = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.facility_owner_signature = "a" + user.signature;
                        }
                        else
                        {
                            rad.facility_owner_signature = "d" + user.signature;
                            rad.facility_owner_delegate = user.id.ToString();
                        }

                        this.ptw = new PtwEntity(rad.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());

                        rad.status = (int)RadStatus.FOAPPROVE;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public int rejectClearance(int who)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:

                        rad.status = (int)RadStatus.EDITANDSEND;

                        break;
                    case 2 /* Radiographic Operator (Level 1) */:
                        rad.status = (int)RadStatus.CREATE;
                        break;
                    case 3 /* Radiographic Level 2 */:
                        rad.operator_signature = null;
                        rad.operator_delegate = null;
                        rad.status = (int)RadStatus.EDITANDSEND;
                        break;
                    case 4 /* Supervisor */:
                        rad.radiographer_2_signature = null;
                        rad.radiographer_2_delegate = null;
                        rad.status = (int)RadStatus.OPERATORAPPROVE;
                        break;
                    case 5 /* SHE Officer */:
                        rad.safety_officer_signature = null;
                        rad.safety_officer_delegate = null;
                        rad.status = (int)RadStatus.RADAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        rad.facility_owner_signature = null;
                        rad.facility_owner_delegate = null;
                        rad.status = (int)RadStatus.SPVAPPROVE;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        /// <summary>
        /// sending email to user, need the object instatiate first to get user
        /// </summary>
        /// <param name="who"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int sendToUser(int who, int stat, string serverUrl, UserEntity user, string comment = "")
        {
            int retVal = 0;
            int? userId = null;
            radiography rad = this.db.radiographies.Find(this.id);
            UserEntity userRad = null;
            List<string> listEmail = new List<string>();
            SendEmail sendEmail = new SendEmail();
            string message = "";
            string title = "";
            List<int> userIds = new List<int>();
            if (rad != null)
            {
#if DEBUG
                listEmail.Add("septujamasoka@gmail.com");
#endif
                switch (who)
                {
                    case 1 /* Requestor */:
#if !DEBUG
                        if (is_guest)
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.SUPERVISOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }
                        else
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.REQUESTOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Rejected from Radiographic Operator";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }

                        retVal = 1;
                        break;
                    case 2 /* Radiographic Level 1 */:
#if !DEBUG

                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.RADIOGRAPHER1.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Rejected from Radiographic Level 2";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 3 /* Radiographic Level 2 */:
#if !DEBUG

                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.RADIOGRAPHER2.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Rejected from Supervisor";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 4 /* Supervisor */:
                        if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
                        {
#if !DEBUG
                            listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
#endif

                            if (stat == 1)
                            {
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                            else if (stat == 2)
                            {
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Rejected from Safety Officer";
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                        }
                        else
                        {
                            // send email supervisor
                        }
                        retVal = 1;
                        break;
                    case 5 /* Safety Officer */:

                        if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()] != null)
                        {
#if !DEBUG
                            listEmail.Add(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
#endif
                            
                            if (stat == 1)
                            {
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                            else if (stat == 2)
                            {
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Rejected from Facility Owner";
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                        }
                        else
                        {
#if !DEBUG
                            if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                                List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                    userIds.Add(u.id);
                                }
                            }
#endif
                            title = "[URGENT] Radiography Clearance Permit (" + this.rg_no + ") Safety Officer hasn't been Chosen";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please choose Safety Officer for Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }

                        retVal = 1;
                        break;
                    case 6 /* Facility Owner */:
#if !DEBUG
                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.FACILITYOWNER.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                            List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user);
                            foreach (UserEntity u in listDel)
                            {
                                listEmail.Add(u.email);
                                userIds.Add(u.id);
                            }
                        }
#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;

                    case 7 /* Requestor */:
#if !DEBUG
                        if (is_guest)
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.SUPERVISOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }
                        else
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.REQUESTOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Completed and Approved";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Radiography Permit No. " + this.rg_no + " has been completed.", serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }

                        retVal = 1;
                        break;
                }

                sendEmail.Send(listEmail, message, title);
            }

            return retVal;
        }

        //public int requestorSendScreeningSpv(string serverUrl)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);
        //    if (rad != null)
        //    {
        //        if (this.status == (int)RadStatus.CREATE)
        //        {
        //            if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
        //            {
        //                rad.status = (int)RadStatus.EDITANDSEND;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();

        //                // sending email
        //                List<string> email = new List<string>();

        //                //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
        //                email.Add("septujamasoka@gmail.com");
        //                SendEmail sendEmail = new SendEmail();
        //                //s.Add(gasTester.email);
        //                //s.Add("septu.jamasoka@gmail.com");

        //                string message = serverUrl + "Home?p=Radiography/edit/" + this.id;

        //                sendEmail.Send(email, message, "Radiography Clearance Permit Supervisor Pre-Job Screening");
        //            }
        //            else
        //            {
        //                retVal = -1;
        //            }
        //        }
        //        else
        //        {
        //            List<string> email = new List<string>();
        //            SendEmail sendEmail = new SendEmail();
        //            string message = "";
        //            switch (this.status)
        //            {
        //                case (int)RadStatus.EDITANDSEND:
        //                    // sending email

        //                    //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, "Radiography Clearance Permit Supervisor Pre-Job Screening");
        //                    break;
        //                case (int)RadStatus.SPVSCREENING:
        //                    // sending email

        //                    //email.add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, "Radiography Clearance Permit Radiographer Level 2 Pre-Job Screening");
        //                    break;
        //                case (int)RadStatus.RADSCREENING:
        //                    // sending email

        //                    //email.add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, "Radiography Clearance Permit Facility Owner Pre-Job Screening");
        //                    break;
        //            }

        //            retVal = 1;
        //        }
        //    }

        //    return retVal;
        //}

        public int savePreScreening(int who /* 1 = spv, 2 = rad, 3 = fo */)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                rad.pre_remark = pre_remark;
                switch (who)
                {
                    case 1: // Supervisor
                        rad.pre_screening_spv = this.pre_screening_spv;
                        break;
                    case 2: // radiographer level 2
                        rad.pre_screening_rad = this.pre_screening_rad;
                        break;
                    case 3:
                        rad.pre_screening_fo = this.pre_screening_fo;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        //public int rejectPreScreening(int who /* 1 = spv, 2 = rad, 3 = fo */, string serverUrl, string comment)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);
        //    if (rad != null)
        //    {
        //        // sending email
        //        List<string> email = new List<string>();
        //        string title = "";
        //        switch (who)
        //        {
        //            case 1:
        //                title = "Radiography Clearance Permit Rejected by Supervisor on Pre-Job Screening";
        //                break;
        //            case 2:
        //                title = "Radiography Clearance Permit Rejected by Radiographer Level 2 on Pre-Job Screening";
        //                break;
        //            case 3:
        //                title = "Radiography Clearance Permit Rejected by Facility Owner on Pre-Job Screening";
        //                break;
        //        }
        //        //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
        //        email.Add("septujamasoka@gmail.com");
        //        SendEmail sendEmail = new SendEmail();
        //        string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //        sendEmail.Send(email, message, title);
        //    }

        //    return retVal;
        //}

        //public int completePreScreening(int who /* 1 = spv, 2 = rad, 3 = fo */, UserEntity user, string serverUrl)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);

        //    List<string> email = new List<string>();
        //    string title = "";
        //    SendEmail sendEmail = new SendEmail();
            
        //    if (rad != null)
        //    {
        //        switch (who)
        //        {
        //            case 1: // Supervisor
        //                if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.SPVSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Radiographer Level 2 Pre-Job Screening";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //            case 2: // radiographer level 2
        //                if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.RADSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Facility Owner Pre-Job Screening";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //            case 3:
        //                if (this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.FOSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Operator Approval";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //        }
        //    }

        //    return retVal;
        //}

        //internal int signPermitStart(UserEntity user, int who, string serverUrl, out string messages)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);

        //    List<string> email = new List<string>();
        //    string title = "";
        //    SendEmail sendEmail = new SendEmail();
        //    string message = "";
        //    messages = "";
        //    if (rad != null)
        //    {
        //        switch (who)
        //        {
        //            case 1:
        //                if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id)
        //                {
        //                    rad.operator_signature = "a" + user.signature;
        //                }
        //                else if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate)
        //                {
        //                    rad.operator_delegate = user.id.ToString();
        //                    rad.operator_signature = "d" + user.signature;
        //                }

        //                rad.status = (int)RadStatus.OPERATORAPPROVE;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();

        //                //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
        //                //{
        //                //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
        //                //    email.Add(@delegate.email);
        //                //}
        //                email.Add("septujamasoka@gmail.com");
        //                title = "Radiography Clearance Permit Radiographer Level 2 Approval";
        //                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                messages = "Radiography Clearance Permit is signed. Notification has been sent to Radiographer Level 2 for Signing.";
        //                break;
        //            case 2:
        //                if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id)
        //                {
        //                    rad.radiographer_2_signature = "a" + user.signature;
        //                }
        //                else if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate)
        //                {
        //                    rad.radiographer_2_delegate = user.id.ToString();
        //                    rad.radiographer_2_signature = "d" + user.signature;
        //                }

        //                rad.status = (int)RadStatus.RADAPPROVE;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();

        //                //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
        //                //{
        //                //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
        //                //    email.Add(@delegate.email);
        //                //}
        //                email.Add("septujamasoka@gmail.com");
        //                title = "Radiography Clearance Permit Supervisor Approval";
        //                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                messages = "Radiography Clearance Permit is signed. Notification has been sent to Supervisor for Signing.";
        //                break;
        //            case 3:
        //                if (user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id)
        //                {
        //                    rad.supervisor_signature = "a" + user.signature;
        //                }
        //                else if (user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate)
        //                {
        //                    rad.supervisor_delegate = user.id.ToString();
        //                    rad.supervisor_signature = "d" + user.signature;
        //                }

        //                rad.status = (int)RadStatus.SPVAPPROVE;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();

        //                if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()] == null)
        //                {
        //                    //email.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Safety Officer Assignment Needed";
        //                    message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    messages = "Radiography Clearance Permit is signed. Notification has been sent to Facility Owner for assigning Safety Officer.";
        //                }
        //                else
        //                {
        //                    //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                    //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
        //                    //{
        //                    //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
        //                    //    email.Add(@delegate.email);
        //                    //}
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Safety Officer Approval";
        //                    message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    messages = "Radiography Clearance Permit is signed. Notification has been sent to Safety Officer for Signing.";
        //                }
        //                break;
        //            case 4:
        //                if (user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id)
        //                {
        //                    rad.safety_officer_signature = "a" + user.signature;
        //                }
        //                else if (user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate)
        //                {
        //                    rad.safety_officer_delegate = user.id.ToString();
        //                    rad.safety_officer_signature = "d" + user.signature;
        //                }

        //                rad.status = (int)RadStatus.SOAPPROVE;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();
                        
        //                //email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                //if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
        //                //{
        //                //    UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
        //                //    email.Add(@delegate.email);
        //                //}
        //                email.Add("septujamasoka@gmail.com");
        //                title = "Radiography Clearance Permit Facility Owner Approval";
        //                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                messages = "Radiography Clearance Permit is signed. Notification has been sent to Facility Owner for Signing.";
        //                break;
        //            case 5:
        //                if (user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id)
        //                {
        //                    rad.facility_owner_signature = "a" + user.signature;
        //                }
        //                else if (user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate)
        //                {
        //                    rad.facility_owner_delegate = user.id.ToString();
        //                    rad.facility_owner_signature = "d" + user.signature;
        //                }

        //                rad.status = (int)RadStatus.FOAPPROVE;

        //                this.db.Entry(rad).State = EntityState.Modified;
        //                retVal = this.db.SaveChanges();

        //                this.ptw = new PtwEntity(rad.id_ptw.Value, user);

        //                this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());

        //                //email.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
        //                email.Add("septujamasoka@gmail.com");
        //                title = "Radiography Clearance Permit Approval Complete";
        //                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                messages = "Radiography Clearance Permit is signed. Radiography Clearance Permit is completed. Notification has been sent to Requestor.";
        //                break;
        //        }


        //        sendEmail.Send(email, message, title);
        //    }

        //    return retVal;
        //}

        internal int cancelPermit(UserEntity user, string serverUrl)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);

            List<string> email = new List<string>();
            string title = "";
            SendEmail sendEmail = new SendEmail();

            if (rad != null)
            {
                rad.status = (int)RadStatus.CLOSING;

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
#if (!DEBUG)
                email.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
#else
                email.Add("septu.jamasoka@gmail.com");
#endif
                title = "Radiography Clearance Permit Supervisor Cancellation Screening";
                string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                sendEmail.Send(email, message, title);
            }

            return retVal;
        }

        public int saveAsDraftCancel(int who)
        {
            int retVal = 0;
            switch (who)
            {
                case 3 /* Level 2 */:
                    retVal = saveCancelScreening(2);
                    break;
                case 4 /* Supervisor */:
                    retVal = saveCancelScreening(1);
                    break;
                case 6 /* Facility Owner */:
                    retVal = saveCancelScreening(3);
                    break;
                default:
                    retVal = 1;
                    break;
            }

            return retVal;
        }

        /// <summary>
        /// function for signing clearance permit
        /// </summary>
        /// <param name="who">1 if requestor, 2 if radiographic operator, 3 if radiographic level 2, 4 if supervisor, 5 if safety officer, 6 if FO</param>
        /// <returns>1 if success, 0 if fail, -1 if user doesn't exist</returns>
        public int signClearanceCancel(int who, UserEntity user)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            UserEntity userRad = null;
            if (rad != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:
                        rad.status = (int)RadStatus.CLOSING;

                        break;
                    case 2 /* Radiographic Operator (Level 1) */:
                        userRad = this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.can_operator_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.can_operator_signature = "d" + user.signature;
                            rad.can_operator_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.CANOPERATORAPPROVE;
                        break;
                    case 3 /* Radiographic Level 2 */:
                        userRad = this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.can_radiographer_2_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.can_radiographer_2_signature = "d" + user.signature;
                            rad.can_radiographer_2_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.CANRADAPPROVE;
                        break;
                    case 4 /* Supervisor */:
                        userRad = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.can_supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.can_supervisor_signature = "d" + user.signature;
                            rad.can_supervisor_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.CANSPVAPPROVE;

                        this.ptw = new PtwEntity(rad.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.REQUESTORCANCELLED, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());
                        break;
                    case 5 /* SHE Officer */:
                        userRad = this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.can_safety_officer_signature = "a" + user.signature;
                        }
                        else if (user.id == userRad.employee_delegate)
                        {
                            rad.can_safety_officer_signature = "d" + user.signature;
                            rad.can_safety_officer_delegate = user.id.ToString();
                        }

                        rad.status = (int)RadStatus.CANSOAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        userRad = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()];
                        if (user.id == userRad.id)
                        {
                            rad.can_fo_signature = "a" + user.signature;
                        }
                        else
                        {
                            rad.can_fo_signature = "d" + user.signature;
                            rad.can_fo_delegate = user.id.ToString();
                        }

                        this.ptw = new PtwEntity(rad.id_ptw.Value, user);
                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());

                        rad.status = (int)RadStatus.CANFOAPPROVE;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="who"></param>
        /// <returns></returns>
        public int rejectClearanceCancel(int who)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                switch (who)
                {
                    case 1 /* Requestor */:

                        rad.status = (int)RadStatus.CLOSING;

                        break;
                    case 2 /* Radiographic Operator (Level 1) */:
                        rad.status = (int)RadStatus.CLOSING;
                        break;
                    case 3 /* Radiographic Level 2 */:
                        rad.can_operator_signature = null;
                        rad.can_operator_delegate = null;
                        rad.status = (int)RadStatus.CLOSING;
                        break;
                    case 4 /* Supervisor */:
                        rad.can_radiographer_2_signature = null;
                        rad.can_radiographer_2_delegate = null;
                        rad.status = (int)RadStatus.CANOPERATORAPPROVE;
                        break;
                    case 5 /* SHE Officer */:
                        rad.can_safety_officer_signature = null;
                        rad.can_safety_officer_delegate = null;
                        rad.status = (int)RadStatus.CANRADAPPROVE;
                        break;
                    case 6 /* Facility Owner */:
                        rad.can_fo_signature = null;
                        rad.can_fo_delegate = null;
                        rad.status = (int)RadStatus.CANSPVAPPROVE;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        /// <summary>
        /// sending email to user, need the object instatiate first to get user
        /// </summary>
        /// <param name="who"></param>
        /// <param name="serverUrl"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public int sendToUserCancel(int who, int stat, string serverUrl, UserEntity user, string comment = "")
        {
            int retVal = 0;
            int? userId = null;
            radiography rad = this.db.radiographies.Find(this.id);
            UserEntity userRad = null;
            List<string> listEmail = new List<string>();
            SendEmail sendEmail = new SendEmail();
            string message = "";
            string title = "";
            List<int> userIds = new List<int>();
            if (rad != null)
            {
#if DEBUG
                listEmail.Add("septujamasoka@gmail.com");
#endif
                switch (who)
                {
                    case 1 /* Requestor */:
#if !DEBUG
                        if (is_guest)
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.SUPERVISOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }
                        else
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.REQUESTOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Rejected from Radiographic Operator";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }

                        retVal = 1;
                        break;
                    case 2 /* Radiographic Level 1 */:
#if !DEBUG

                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.RADIOGRAPHER1.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Rejected from Radiographic Level 2";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 3 /* Radiographic Level 2 */:
#if !DEBUG

                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.RADIOGRAPHER2.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        else if (stat == 2)
                        {
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Rejected from Supervisor";
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;
                    case 4 /* Supervisor */:
                        if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
                        {
#if !DEBUG
                            listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
#endif

                            if (stat == 1)
                            {
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                            else if (stat == 2)
                            {
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Rejected from Safety Officer";
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                        }
                        else
                        {
                            // send email supervisor
                        }
                        retVal = 1;
                        break;
                    case 5 /* Safety Officer */:

                        if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()] != null)
                        {
#if !DEBUG
                            listEmail.Add(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
#endif

                            if (stat == 1)
                            {
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                            else if (stat == 2)
                            {
                                message = serverUrl + "Home?p=Radiography/edit/" + this.id + "<br />Comment: " + comment;
                                title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Rejected from Facility Owner";
                                sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " is rejected with comment: " + comment, serverUrl + "Home?p=Radiography/edit/" + this.id);
                            }
                        }
                        else
                        {
#if !DEBUG
                            if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                                List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user);
                                foreach (UserEntity u in listDel)
                                {
                                    listEmail.Add(u.email);
                                    userIds.Add(u.id);
                                }
                            }
#endif
                            title = "[URGENT] Radiography Clearance Permit (" + this.rg_no + ") Cancellation Safety Officer hasn't been Chosen";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        }

                        retVal = 1;
                        break;
                    case 6 /* Facility Owner */:
#if !DEBUG
                        if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.FACILITYOWNER.ToString()))
                        {
                            listEmail.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                            userIds.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id);
                            if ((userId = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) != null)
                            {
                                userRad = new UserEntity(userId.Value, user.token, user);
                                listEmail.Add(userRad.email);
                                userIds.Add(userRad.id);
                            }
                            List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user);
                            foreach (UserEntity u in listDel)
                            {
                                listEmail.Add(u.email);
                                userIds.Add(u.id);
                            }
                        }
#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancellation Need Review and Approval";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Please approve cancellation of Radiography Permit No. " + this.rg_no, serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }
                        retVal = 1;
                        break;

                    case 7 /* Requestor */:
#if !DEBUG
                        if (is_guest)
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.SUPERVISOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }
                        else
                        {
                            if (this.userInRadiography.Keys.ToList().Exists(p => p == UserInRadiography.REQUESTOR.ToString()))
                            {
                                listEmail.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
                                userIds.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id);
                                if ((userId = this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) != null)
                                {
                                    userRad = new UserEntity(userId.Value, user.token, user);
                                    listEmail.Add(userRad.email);
                                    userIds.Add(userRad.id);
                                }
                            }
                        }

#endif

                        if (stat == 1)
                        {
                            title = "Radiography Clearance Permit (" + this.rg_no + ") Cancelled and Approved";
                            message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                            sendEmail.SendToNotificationCenter(userIds, "Radiography Permit", "Cancellation of Radiography Permit No. " + this.rg_no + " has been completed.", serverUrl + "Home?p=Radiography/edit/" + this.id);
                        }

                        retVal = 1;
                        break;
                }

                sendEmail.Send(listEmail, message, title);
            }

            return retVal;
        }

        public int saveCancelScreening(int who /* 1 = spv, 2 = rad, 3 = fo */)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                rad.can_remark = can_remark;
                switch (who)
                {
                    case 1: // Supervisor
                        rad.can_screening_spv = this.can_screening_spv;
                        break;
                    case 2: // radiographer level 2
                        rad.can_screening_rad = this.can_screening_rad;
                        break;
                    case 3:
                        rad.can_screening_fo = this.can_screening_fo;
                        break;
                }

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        //public int rejectCancelScreening(int who /* 1 = spv, 2 = rad, 3 = fo */, string serverUrl, string comment)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);
        //    if (rad != null)
        //    {
        //        // sending email
        //        List<string> email = new List<string>();
        //        string title = "";
        //        switch (who)
        //        {
        //            case 1:
        //                title = "Radiography Clearance Permit Rejected by Supervisor on Cancellation Screening";
        //                break;
        //            case 2:
        //                title = "Radiography Clearance Permit Rejected by Radiographer Level 2 on Cancellation Screening";
        //                break;
        //            case 3:
        //                title = "Radiography Clearance Permit Rejected by Facility Owner on Cancellation Screening";
        //                break;
        //        }
        //        //email.add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
        //        email.Add("septujamasoka@gmail.com");
        //        SendEmail sendEmail = new SendEmail();
        //        string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //        sendEmail.Send(email, message, title);
        //    }

        //    return retVal;
        //}

        //public int completeCancelScreening(int who /* 1 = spv, 2 = rad, 3 = fo */, UserEntity user, string serverUrl)
        //{
        //    int retVal = 0;
        //    radiography rad = this.db.radiographies.Find(this.id);

        //    List<string> email = new List<string>();
        //    string title = "";
        //    SendEmail sendEmail = new SendEmail();

        //    if (rad != null)
        //    {
        //        switch (who)
        //        {
        //            case 1: // Supervisor
        //                if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.CANSPVSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Radiographer Level 2 Cancellation Screening";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //            case 2: // radiographer level 2
        //                if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.CANRADSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Facility Owner Cancellation Screening";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //            case 3:
        //                if (this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()] != null)
        //                {
        //                    rad.status = (int)RadStatus.CANFOSCREENING;

        //                    this.db.Entry(rad).State = EntityState.Modified;
        //                    retVal = this.db.SaveChanges();

        //                    //email.add(this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].email);
        //                    email.Add("septujamasoka@gmail.com");
        //                    title = "Radiography Clearance Permit Cancellation Operator Approval";
        //                    string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
        //                    sendEmail.Send(email, message, title);
        //                }
        //                else
        //                {
        //                    retVal = -1;
        //                }
        //                break;
        //        }
        //    }

        //    return retVal;
        //}

        internal int signPermitCancel(UserEntity user, int who, string serverUrl, out string messages)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);

            List<string> email = new List<string>();
            string title = "";
            SendEmail sendEmail = new SendEmail();
            string message = "";
            messages = "";
            if (rad != null)
            {
                switch (who)
                {
                    case 1:
                        if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id)
                        {
                            rad.can_operator_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate)
                        {
                            rad.can_operator_delegate = user.id.ToString();
                            rad.can_operator_signature = "d" + user.signature;
                        }

                        rad.status = (int)RadStatus.CANOPERATORAPPROVE;

                        this.db.Entry(rad).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

#if (!DEBUG)
                        email.Add(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].email);
                        if (this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate != null)
                        {
                            UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate.Value, user.token, user);
                            email.Add(@delegate.email);
                        }
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        title = "Radiography Clearance Permit Radiographer Level 2 Approval";
                        message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        messages = "Radiography Clearance Permit's cancellation is signed. Notification has been sent to Radiographer Level 2 for Signing.";
                        break;
                    case 2:
                        if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id)
                        {
                            rad.can_radiographer_2_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate)
                        {
                            rad.can_radiographer_2_delegate = user.id.ToString();
                            rad.can_radiographer_2_signature = "d" + user.signature;
                        }

                        rad.status = (int)RadStatus.CANRADAPPROVE;

                        this.db.Entry(rad).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();
#if (!DEBUG)
                        email.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                        if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate != null)
                        {
                            UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate.Value, user.token, user);
                            email.Add(@delegate.email);
                        }
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        title = "Radiography Clearance Permit Supervisor Approval";
                        message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        messages = "Radiography Clearance Permit's cancellation is signed. Notification has been sent to Supervisor for Signing.";
                        break;
                    case 3:
                        if (user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id)
                        {
                            rad.can_supervisor_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate)
                        {
                            rad.can_supervisor_delegate = user.id.ToString();
                            rad.can_supervisor_signature = "d" + user.signature;
                        }

                        rad.status = (int)RadStatus.CANSPVAPPROVE;

                        this.db.Entry(rad).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

#if (!DEBUG)
                        email.Add(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].email);
                        if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate != null)
                        {
                            UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate.Value, user.token, user);
                            email.Add(@delegate.email);
                        }
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        title = "Radiography Clearance Permit Safety Officer Approval";
                        message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        messages = "Radiography Clearance Permit's cancellation is signed. Notification has been sent to Safety Officer for Signing.";
                        break;
                    case 4:
                        if (user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id)
                        {
                            rad.can_safety_officer_signature = "a" + user.signature;
                        }
                        else if (user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate)
                        {
                            rad.can_safety_officer_delegate = user.id.ToString();
                            rad.can_safety_officer_signature = "d" + user.signature;
                        }

                        rad.status = (int)RadStatus.CANSOAPPROVE;

                        this.db.Entry(rad).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();
#if (!DEBUG)
                        email.Add(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].email);
                        if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate != null)
                        {
                            UserEntity @delegate = new UserEntity(this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate.Value, user.token, user);
                            email.Add(@delegate.email);
                        }
                        List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user);
                        foreach (UserEntity u in listDel)
                        {
                            email.Add(u.email);
                        }
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        title = "Radiography Clearance Permit Facility Owner Approval";
                        message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        messages = "Radiography Clearance Permit's cancellation is signed. Notification has been sent to Facility Owner for Signing.";
                        break;
                    case 5:
                        if (user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id)
                        {
                            rad.can_fo_signature = "a" + user.signature;
                        }
                        else
                        {
                            rad.can_fo_delegate = user.id.ToString();
                            rad.can_fo_signature = "d" + user.signature;
                        }

                        rad.status = (int)RadStatus.CANFOAPPROVE;

                        this.db.Entry(rad).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        this.ptw = new PtwEntity(rad.id_ptw.Value, user);

                        this.ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());

#if (!DEBUG)
                        if (is_guest)
                        {
                            email.Add(this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].email);
                        }
                        else
                        {
                            email.Add(this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].email);
                        }
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        title = "Radiography Clearance Permit Cancellation Approval Complete";
                        message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        messages = "Radiography Clearance Permit's cancellation is signed. Radiography Clearance Permit is closed. Notification has been sent to Requestor.";
                        break;
                }


                sendEmail.Send(email, message, title);
            }

            return retVal;
        }

        #region is can edit

        public bool isCanEditGeneralInformation(UserEntity user)
        {
            if (this.ptw.is_guest == 1)
            {
                if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
                {
                    if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.CREATE)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInRadiography[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)RadStatus.CREATE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //public bool isCanEditSpvScreening(UserEntity user)
        //{
        //    if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.EDITANDSEND)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public bool isCanEditRadScreening(UserEntity user)
        //{
        //    if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER2.ToString()).Count() != 0)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) && this.status == (int)RadStatus.SPVSCREENING)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public bool isCanEditFOScreening(UserEntity user)
        //{
        //    if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)RadStatus.RADSCREENING)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public bool isCanEditFOChoosingSO(UserEntity user, ListUser listUser)
        {
            if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user, listUser);
                if ((user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status <= (int)RadStatus.SPVAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status <= (int)RadStatus.SPVAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveOperator(UserEntity user)
        {
            if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER1.ToString()).Count() != 0)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate) && this.status == (int)RadStatus.EDITANDSEND)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveRadiographer2(UserEntity user)
        {
            if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER2.ToString()).Count() != 0)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) && this.status == (int)RadStatus.OPERATORAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveSpv(UserEntity user)
        {
            if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.RADAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveSO(UserEntity user)
        {
            if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()] != null)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate) && this.status == (int)RadStatus.SPVAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveFO(UserEntity user, ListUser listUser)
        {
            if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user, listUser);
                if ((user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)RadStatus.SOAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)RadStatus.SOAPPROVE)
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
                if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
                {
                    if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.FOAPPROVE)
                    {
                        return true;
                    }
                }
            }
            else
            {
                if (this.userInRadiography[UserInRadiography.REQUESTOR.ToString()] != null)
                {
                    if ((user.id == this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.REQUESTOR.ToString()].employee_delegate) && this.status == (int)RadStatus.FOAPPROVE)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        //public bool isCanEditSpvCancelScreening(UserEntity user)
        //{
        //    if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.CLOSING)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public bool isCanEditRadCancelScreening(UserEntity user)
        //{
        //    if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER2.ToString()).Count() != 0)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) && this.status == (int)RadStatus.CANSPVSCREENING)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        //public bool isCanEditFOCancelScreening(UserEntity user)
        //{
        //    if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
        //    {
        //        if ((user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)RadStatus.CANRADSCREENING)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

        public bool isCanApproveOperatorCancel(UserEntity user)
        {
            if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER1.ToString()).Count() != 0)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER1.ToString()].employee_delegate) && this.status == (int)RadStatus.CLOSING)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveRadiographer2Cancel(UserEntity user)
        {
            if (this.userInRadiography.Keys.Where(p => p == UserInRadiography.RADIOGRAPHER2.ToString()).Count() != 0)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].id || user.id == this.userInRadiography[UserInRadiography.RADIOGRAPHER2.ToString()].employee_delegate) && this.status == (int)RadStatus.CANOPERATORAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveSpvCancel(UserEntity user)
        {
            if (this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)RadStatus.CANRADAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveSOCancel(UserEntity user)
        {
            if (this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()] != null)
            {
                if ((user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.SAFETYOFFICER.ToString()].employee_delegate) && this.status == (int)RadStatus.CANSPVAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveFOCancel(UserEntity user, ListUser listUser)
        {
            if (this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].GetDelegateFO(user, listUser);
                if ((user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].id || user.id == this.userInRadiography[UserInRadiography.FACILITYOWNER.ToString()].employee_delegate) && this.status == (int)RadStatus.CANSOAPPROVE)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)RadStatus.CANSOAPPROVE)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion

        #region internal function

        private void generateUserInRadiography(radiography rad, UserEntity user, ListUser listUsers)
        {
            ListUser listUser = listUsers != null ? listUsers : new ListUser(user.token, user.id);
            int userId = 0;

            Int32.TryParse(this.@operator, out userId);
            this.isUser = this.isUser || user.id == userId;
            this.userInRadiography.Add(UserInRadiography.REQUESTOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            if (this.radiographer1 != null)
            {
                this.isUser = this.isUser || user.id == this.radiographer1.user.id;
                this.userInRadiography.Add(UserInRadiography.RADIOGRAPHER1.ToString(), this.radiographer1.user);
            }

            if (this.radiographer2 != null)
            {
                this.isUser = this.isUser || user.id == this.radiographer2.user.id;
                this.userInRadiography.Add(UserInRadiography.RADIOGRAPHER2.ToString(), this.radiographer2.user);
            }

            Int32.TryParse(this.supervisor, out userId);
            this.isUser = this.isUser || user.id == userId;
            this.userInRadiography.Add(UserInRadiography.SUPERVISOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.safety_officer, out userId);
            this.isUser = this.isUser || user.id == userId;
            this.userInRadiography.Add(UserInRadiography.SAFETYOFFICER.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.facility_owner, out userId);
            this.isUser = this.isUser || user.id == userId;
            this.userInRadiography.Add(UserInRadiography.FACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.operator_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.RADIOGRAPHER1DELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.radiographer_2_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.RADIOGRAPHER2DELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.supervisor_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.SUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.safety_officer_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.SAFETYOFFICERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.facility_owner_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.FACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_operator_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.CANRADIOGRAPHER1DELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_radiographer_2_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.CANRADIOGRAPHER2DELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_supervisor_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.CANSUPERVISORDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_safety_officer_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.CANSAFETYOFFICERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            Int32.TryParse(this.can_fo_delegate, out userId);
            if (userId != 0)
            {
                this.isUser = this.isUser || user.id == userId;
                this.userInRadiography.Add(UserInRadiography.CANFACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
        }

        public void GetPtw(UserEntity user)
        {
            this.ptw = new PtwEntity(this.id_ptw.Value, user);
        }

        #endregion

        #region assigment user

        internal int assignSupervisor(UserEntity user)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            if (rad != null)
            {
                rad.supervisor = user.id.ToString();

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        internal int assignSafetyOfficer(UserEntity user, string serverUrl)
        {
            int retVal = 0;
            radiography rad = this.db.radiographies.Find(this.id);
            string prevSO = null;
            if (rad != null)
            {
                prevSO = rad.safety_officer;
                rad.safety_officer = this.safety_officer;

                this.db.Entry(rad).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                // sending email
                List<string> email = new List<string>();
                string title = "";
                if (rad.status == (int)RadStatus.SPVAPPROVE)
                {
                    title = "Assigned as Safety Officer of Radiography Clearance Permit and Approval Needed";
                }
                else
                {
                    title = "Assigned as Safety Officer of Radiography Clearance Permit";
                }
                int userId = 0;

                if (Int32.TryParse(this.safety_officer, out userId)) {
                    if (prevSO != this.safety_officer)
                    {
                        UserEntity safetyOfficer = new UserEntity(userId, user.token, user);
#if (!DEBUG)
                        email.Add(safetyOfficer.email);
#else
                        email.Add("septujamasoka@gmail.com");
#endif
                        SendEmail sendEmail = new SendEmail();
                        string message = serverUrl + "Home?p=Radiography/edit/" + this.id;
                        sendEmail.Send(email, message, title);
                    }
                }
            }

            return retVal;
        }

        #endregion

        internal bool isUserInRad(UserEntity user)
        {
            foreach (KeyValuePair<string, UserEntity> entry in userInRadiography)
            {
                UserEntity us = entry.Value;
                if (entry.Key == UserInRadiography.FACILITYOWNER.ToString() || entry.Key == UserInRadiography.REQUESTOR.ToString() || entry.Key == UserInRadiography.SUPERVISOR.ToString())
                {  
                } else if (us != null && (user.id == us.id || user.id == us.employee_delegate))
                {
                    return true;
                }
            }

            return false;
        }
    }
}