using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using PermitToWork.Models.Ptw;

namespace PermitToWork.Models.SafetyBriefing
{
    public class SafetyBriefingEntity : safety_briefing
    {
        public override permit_to_work permit_to_work { get { return null; } set { } }
        public override ICollection<safety_briefing_user> safety_briefing_user { get { return null; } set { } }

        public List<SafetyBriefingUserEntity> listUser { get; set; }

        public UserEntity requestorUser { get; set; }
        public List<string> listDocumentUploaded { get; set; }
        public string supervisor { get; set; }

        private star_energy_ptwEntities db;

        public enum SafetyBriefingStatus
        {
            CREATE,
            PRINT,
            CANCELLATIONSIGN,
            CANCELLATIONSIGNCOMPLETE
        }

        public SafetyBriefingEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
            this.listDocumentUploaded = new List<string>();
        }

        public SafetyBriefingEntity(int id, UserEntity user)
            : this()
        {
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(safetyBriefing, this);


            this.listUser = new SafetyBriefingUserEntity().listUser(this.id, user);

            PtwEntity ptw = new PtwEntity(this.id_ptw.Value, user);

            if (ptw.is_guest != 1 && this.requestor != null)
            {
                this.requestorUser = new UserEntity(Int32.Parse(this.requestor), user.token, user);
            }
            else
            {
                this.requestorUser = new UserEntity(Int32.Parse(ptw.acc_supervisor), user.token, user);
            }

            string path = HttpContext.Current.Server.MapPath("~/Upload/SafetyBriefing/" + this.id);

            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded = Files.Select(p => p.Name).ToList();
        }

        public SafetyBriefingEntity(string requestor, string section, string area, DateTime date, string topic, int id_ptw, string supervisor)
            : this()
        {
            this.supervisor = supervisor;
            this.requestor = requestor;
            this.section = section;
            this.work_area = area == "1" ? "Power Station" : (area == "2" ? "Steam Field" : area);
            this.date = date;
            this.topic = topic;
            this.id_ptw = id_ptw;
            this.status = (int)SafetyBriefingStatus.CREATE;
        }

        public int create()
        {
            int retVal = 0;

            safety_briefing safetyBriefing = new safety_briefing();
            ModelUtilization.Clone(this, safetyBriefing);
            this.db.safety_briefing.Add(safetyBriefing);
            this.db.SaveChanges();

            this.id = safetyBriefing.id;
            retVal = this.id;

            SafetyBriefingUserEntity glarfSpv = new SafetyBriefingUserEntity(this.id, this.supervisor, "");
            glarfSpv.create();
            glarfSpv = new SafetyBriefingUserEntity(this.id, this.requestor, "");
            glarfSpv.create();

            string path = HttpContext.Current.Server.MapPath("~/Upload/SafetyBriefing/" + this.id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return retVal;
        }

        public int edit()
        {
            int retVal = 0;
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(this.id);
            if (safetyBriefing != null)
            {
                safetyBriefing.section = this.section;
                safetyBriefing.work_area = this.work_area;
                safetyBriefing.date = this.date;
                safetyBriefing.topic = this.topic;
                safetyBriefing.hazard = this.hazard;
                safetyBriefing.control_method = this.control_method;

                this.db.Entry(safetyBriefing).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int delete()
        {
            return 0;
        }

        #region is Can Edit

        public bool isCanEditForm(UserEntity user)
        {
            if (this.requestorUser != null)
            {
                if ((user.id == this.requestorUser.id || user.id == this.requestorUser.employee_delegate) && (this.status == (int)SafetyBriefingStatus.CREATE || this.status == (int)SafetyBriefingStatus.PRINT))
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanCancel(UserEntity user)
        {
            if (this.requestorUser != null)
            {
                if ((user.id == this.requestorUser.id || user.id == this.requestorUser.employee_delegate) && this.status == (int)SafetyBriefingStatus.CANCELLATIONSIGN)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanPrint(UserEntity user)
        {
            if (this.requestorUser != null)
            {
                if ((user.id == this.requestorUser.id || user.id == this.requestorUser.employee_delegate) && (this.status == (int)SafetyBriefingStatus.PRINT))
                {
                    return true;
                }
            }
            return false;
        }

        #endregion

        internal void assignSupervisor(UserEntity user)
        {
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(this.id);
            if (safetyBriefing != null)
            {
                SafetyBriefingUserEntity glarfUser = this.listUser.OrderBy(p => p.id).FirstOrDefault();
                glarfUser.user = user.id.ToString();
                glarfUser.edit();
            }
        }

        public int setCancel()
        {
            int retVal = 0;
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(this.id);
            if (safetyBriefing != null)
            {
                safetyBriefing.status = (int)SafetyBriefingStatus.CANCELLATIONSIGN;

                this.db.Entry(safetyBriefing).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int saveForPrint()
        {
            int retVal = 0;
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(this.id);
            if (safetyBriefing != null)
            {
                safetyBriefing.status = (int)SafetyBriefingStatus.PRINT;

                this.db.Entry(safetyBriefing).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int saveCancellation(UserEntity user)
        {
            int retVal = 0;
            safety_briefing safetyBriefing = this.db.safety_briefing.Find(this.id);
            if (safetyBriefing != null)
            {
                if (this.listDocumentUploaded.Count == 0)
                {
                    return -1;
                }
                else
                {
                    safetyBriefing.status = (int)SafetyBriefingStatus.CANCELLATIONSIGNCOMPLETE;

                    this.db.Entry(safetyBriefing).State = EntityState.Modified;
                    retVal = this.db.SaveChanges();
                }
            }
            return retVal;
        }
    }
}