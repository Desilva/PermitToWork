using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoEntity : loto_permit, IClearancePermitEntity
    {
        public override ICollection<loto_point> loto_point { get { return null; } set { } }
        public override ICollection<loto_coming_holder> loto_coming_holder { get { return null; } set { } }
        public override loto_glarf loto_glarf { get { return null; } set { } }
        public override ICollection<loto_suspension> loto_suspension { get { return null; } set { } }
        public override ICollection<permit_to_work> permit_to_work { get { return null; } set { } }

        public int ids { get; set; }
        public string statusText { get; set; }

        public Dictionary<string, UserEntity> listUserInLOTO { get; set; }
        public string work_description { get; set; }
        public UserEntity supervisorUser { get; set; }

        public bool isUser { get; set; }

        public List<LotoPointEntity> lotoPoint { get; set; }
        public List<LotoComingHolderEntity> lotoComingHolder { get; set; }
        public List<LotoSuspensionEntity> lotoSuspension { get; set; }
        public Dictionary<string, List<string>> listDocumentUploaded { get; set; }

        public enum userInLOTO
        {
            REQUESTOR,
            SUPERVISOR,
            APPROVALFACILITYOWNER,
            APPROVALFACILITYOWNERDELEGATE,
            ONCOMINGHOLDER1,
            ONCOMINGHOLDER2,
            ONCOMINGHOLDER3,
            ONCOMINGHOLDER4,
            ONCOMINGHOLDER5,
            ONCOMINGHOLDER6,
            ONCOMINGHOLDER7,
            NEWCOMINGHOLDER,
            CANCELLATIONFACILITYOWNER,
            CANCELLATIONFACILITYOWNERDELEGATE,
        }

        public enum LOTOStatus
        {
            CREATE, //0
            SENDTOFO,
            FOAGREED,
            FOAPPLIED,
            INSPECTION,
            SPVSIGN, //5
            FOSIGN,
            LOTOCHANGE,
            CHGSENDTOHOLDER,
            CHGSENDTOFO,
            CHGFOAGREED, // 10
            CHGFOAPPLIED,
            CHGINSPECTION,
            CHGSPVSIGN,
            CHGFOSIGN,
            COMINGHOLDERSIGN, // 15
            LOTOSUSPENSION,
            CANCELSPV, // 17
            LOTOCANCELLED,
        }

        private star_energy_ptwEntities db;

        public LotoEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
            this.listUserInLOTO = new Dictionary<string, UserEntity>();
            this.listDocumentUploaded = new Dictionary<string, List<string>>();
        }

        // constructor with id to get object from database
        public LotoEntity(int id, UserEntity user)
            : this()
        {
            loto_permit loto = this.db.loto_permit.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(loto, this);
            this.lotoPoint = new LotoPointEntity().getList(user, this.id);
            this.lotoComingHolder = new LotoComingHolderEntity().getList(user, this.id);
            this.work_description = "";
            foreach (permit_to_work permit in loto.permit_to_work)
            {
                this.work_description += permit.work_description + ", ";
            }
            if (this.work_description.Length != 0)
            {
                this.work_description = this.work_description.Substring(0, this.work_description.Length - 2);
            }
            if (this.supervisor != null)
            {
                this.supervisorUser = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            }

            this.lotoSuspension = new LotoSuspensionEntity().getList(user, this.id);
            getUserInLOTO(user);

            string path = HttpContext.Current.Server.MapPath("~/Upload/Loto/Attachment/" + this.id + "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add("ATTACHMENT", Files.Select(p => p.Name).ToList());
        }

        public LotoEntity(int id, UserEntity user, string ptw_requestor_id)
            : this()
        {
            loto_permit loto = this.db.loto_permit.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(loto, this);
            this.lotoPoint = new LotoPointEntity().getList(user, this.id);
            this.lotoComingHolder = new LotoComingHolderEntity().getList(user, this.id);
            this.work_description = "";
            foreach (permit_to_work permit in loto.permit_to_work)
            {
                this.work_description += permit.work_description + ", ";
            }
            if (this.work_description.Length != 0)
            {
                this.work_description = this.work_description.Substring(0, this.work_description.Length - 2);
            }
            if (this.supervisor != null)
            {
                this.supervisorUser = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            }

            this.lotoSuspension = new LotoSuspensionEntity().getList(user, this.id);
            getUserInLOTO(user);

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_2_glarf;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_3_glarf;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_4_glarf;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_5_glarf;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_6_glarf;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null && this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id.ToString() == ptw_requestor_id)
            {
                this.id_glarf = this.holder_7_glarf;
            }

            string path = HttpContext.Current.Server.MapPath("~/Upload/Loto/Attachment/" + this.id + "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add("ATTACHMENT", Files.Select(p => p.Name).ToList());
        }

        public LotoEntity(loto_permit loto, UserEntity user, string ptw_requestor_id, ListUser listUser)
            : this()
        {
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(loto, this);
            this.lotoComingHolder = new LotoComingHolderEntity().getList(user, this.id);
            this.work_description = "";
            if (this.supervisor != null)
            {
                this.supervisorUser = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            }
            getUserInLOTO(user, listUser);

            string path = HttpContext.Current.Server.MapPath("~/Upload/Loto/Attachment/" + this.id + "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add("ATTACHMENT", Files.Select(p => p.Name).ToList());
        }

        public LotoEntity(string requestor, string work_location, int id_glarf, string acc_spv = null, string acc_fo = null)
            : this()
        {
            this.requestor = requestor;
            this.work_location = work_location;
            this.supervisor = acc_spv;
            this.id_glarf = id_glarf;
            this.approval_facility_owner = acc_fo;
            this.cancellation_facility_owner = acc_fo;
            this.status = (int)LOTOStatus.CREATE;
        }

        public LotoEntity(LotoEntity loto, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(loto, this);
            this.id = 0;
            this.new_coming_holder = user.id.ToString();
        }

        public int create()
        {
            loto_permit loto = new loto_permit();
            ModelUtilization.Clone(this, loto);
            this.db.loto_permit.Add(loto);
            int retVal = this.db.SaveChanges();
            this.id = loto.id;
            string path = HttpContext.Current.Server.MapPath("~/Upload/Loto/Attachment/" + this.id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return retVal;
        }

        public int create(int glarf_id) {
            loto_permit loto = new loto_permit();
            ModelUtilization.Clone(this, loto);
            loto.holder_1_glarf = glarf_id;
            this.db.loto_permit.Add(loto);
            int retVal = this.db.SaveChanges();
            this.id = loto.id;

            return retVal;
        }

        public int addNewHolder(string oncomingHolder, string comingHolderSupervisor, int glarf_id)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                int holder_no = 2;
                LotoComingHolderEntity comingHolder = this.lotoComingHolder.LastOrDefault();
                if (comingHolder != null)
                {
                    holder_no = comingHolder.no_holder.Value + 1;
                }

                switch (holder_no)
                {
                    case 2:
                        loto.oncoming_holder_2 = comingHolderSupervisor;
                        loto.holder_2_glarf = glarf_id;
                        break;
                    case 3:
                        loto.oncoming_holder_3 = comingHolderSupervisor;
                        loto.holder_3_glarf = glarf_id;
                        break;
                    case 4:
                        loto.oncoming_holder_4 = comingHolderSupervisor;
                        loto.holder_4_glarf = glarf_id;
                        break;
                    case 5:
                        loto.oncoming_holder_5 = comingHolderSupervisor;
                        loto.holder_5_glarf = glarf_id;
                        break;
                    case 6:
                        loto.oncoming_holder_6 = comingHolderSupervisor;
                        loto.holder_6_glarf = glarf_id;
                        break;
                    case 7:
                        loto.oncoming_holder_7 = comingHolderSupervisor;
                        loto.holder_7_glarf = glarf_id;
                        break;
                }

                this.db.Entry(loto).State = EntityState.Modified;
                this.db.SaveChanges();

                LotoComingHolderEntity lo = new LotoComingHolderEntity(this.id, oncomingHolder, holder_no, comingHolderSupervisor);
                retVal = lo.create();

                //LotoGlarfUserEntity glarfUser = new LotoGlarfUserEntity(this.id_glarf.Value, comingHolderSupervisor, 0);
                //glarfUser.create();
                //glarfUser = new LotoGlarfUserEntity(this.id_glarf.Value, oncomingHolder, 0);
                //glarfUser.create();
            }

            return retVal;
        }

        public int edit() {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.work_location = this.work_location;
                loto.lock_box_no = this.lock_box_no;
                loto.initial_pad_lock = this.initial_pad_lock;
                loto.qty_pad_lock_usage = this.qty_pad_lock_usage;
                loto.balance = this.balance;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int delete()
        {
            throw new NotImplementedException("method not implemented yet.");
        }

        public int delete(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id.ToString() == loto.requestor)
                {
                    this.db.loto_permit.Remove(loto);
                    retVal = this.db.SaveChanges();
                }
                else
                {
                    if (user.id.ToString() == loto.oncoming_holder_1)
                    {
                        loto.oncoming_holder_1 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_2)
                    {
                        loto.oncoming_holder_2 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_3)
                    {
                        loto.oncoming_holder_3 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_4)
                    {
                        loto.oncoming_holder_4 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_5)
                    {
                        loto.oncoming_holder_5 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_6)
                    {
                        loto.oncoming_holder_6 = null;
                    }
                    else if (user.id.ToString() == loto.oncoming_holder_7)
                    {
                        loto.oncoming_holder_7 = null;
                    }

                    this.db.Entry(loto).State = EntityState.Modified;
                    retVal = this.db.SaveChanges();
                }
            }
            return retVal;
        }

        public void generateNumber(string ptw_no)
        {
            string result = "LT-" + ptw_no;

            this.loto_no = result;
        }

        public void generateLotoReviewNumber(string loto_no)
        {
            string[] no = loto_no.Split('-');
            if (no.Length == 3)
            {
                this.loto_no = loto_no + "-1";
            }
            else
            {
                int a = Int32.Parse(no[3]);
                a += 1;
                this.loto_no = no[0] + "-" + no[1] + "-" + no[2] + "-" + a;
            }
        }

        public int approvalSupervisor(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id)
                {
                    loto.approval_supervisor_signature = "a" + user.signature;
                }
                else if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate)
                {
                    loto.approval_supervisor_signature = "d" + user.signature;
                    loto.approval_supervisor_signature_delegate = user.id.ToString();
                }

                loto.approval_supervisor_signature_date = DateTime.Now;
                loto.status = (int)LOTOStatus.SPVSIGN;
                loto.approval_notes = this.approval_notes;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int approvalFacilityOwner(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id)
                {
                    loto.approval_fo_signature = "a" + user.signature;
                }
                else
                {
                    loto.approval_fo_signature = "d" + user.signature;
                    loto.approval_fo_signature_delegate = user.id.ToString();
                }

                loto.approval_fo_signature_date = DateTime.Now;
                loto.approval_date = loto.approval_fo_signature_date;
                loto.approval_notes = this.approval_notes;
                loto.status = (int)LOTOStatus.FOSIGN;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                foreach (permit_to_work ptw in loto.permit_to_work)
                {
                    bool isComplete = true;
                    PtwEntity ptwE = new PtwEntity(ptw.id, user);
                    if (ptw.acc_ptw_requestor == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id.ToString())
                    {
                        isComplete = isComplete && loto.approval_fo_signature != null;
                        LotoGlarfEntity glarf = new LotoGlarfEntity(loto.id_glarf.Value, user);
                        isComplete = isComplete && glarf.status == (int)LotoGlarfEntity.GlarfStatus.SIGNING;
                    }

                    foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                    {
                        if (ptw.acc_supervisor == comingHolder.holder_spv)
                        {
                            isComplete = isComplete && comingHolder.isApprove();
                            LotoGlarfEntity glarf = null;
                            switch (comingHolder.no_holder)
                            {
                                case 2:
                                    glarf = new LotoGlarfEntity(loto.holder_2_glarf.Value, user);
                                    break;
                                case 3:
                                    glarf = new LotoGlarfEntity(loto.holder_3_glarf.Value, user);
                                    break;
                                case 4:
                                    glarf = new LotoGlarfEntity(loto.holder_4_glarf.Value, user);
                                    break;
                                case 5:
                                    glarf = new LotoGlarfEntity(loto.holder_5_glarf.Value, user);
                                    break;
                                case 6:
                                    glarf = new LotoGlarfEntity(loto.holder_6_glarf.Value, user);
                                    break;
                                case 7:
                                    glarf = new LotoGlarfEntity(loto.holder_7_glarf.Value, user);
                                    break;
                            }

                            if (glarf != null)
                            {
                                isComplete = isComplete && glarf.status == (int)LotoGlarfEntity.GlarfStatus.SIGNING;
                            }
                        }
                    }

                    if (isComplete)
                    {
                        ptwE.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        retVal = 2;
                    }
                }
            }
            return retVal;
        }

        public int approvalOncomingHolder(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                {
                    if (user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate)
                    {
                        comingHolder.signApprove(user);
                    }
                }

                //if (loto.new_coming_holder.Split('#')[0] == user.id.ToString())
                //{
                //    loto.new_coming_holder = null;
                //}

                loto.approval_notes = this.approval_notes;
                //if ((loto.oncoming_holder_2 == null || (loto.oncoming_holder_2 != null && loto.approval_holder_2_signature != null)) &&
                //    (loto.oncoming_holder_3 == null || (loto.oncoming_holder_3 != null && loto.approval_holder_3_signature != null)) &&
                //    (loto.oncoming_holder_4 == null || (loto.oncoming_holder_4 != null && loto.approval_holder_4_signature != null)) &&
                //    (loto.oncoming_holder_5 == null || (loto.oncoming_holder_5 != null && loto.approval_holder_5_signature != null)) &&
                //    (loto.oncoming_holder_6 == null || (loto.oncoming_holder_6 != null && loto.approval_holder_6_signature != null)) &&
                //    (loto.oncoming_holder_7 == null || (loto.oncoming_holder_7 != null && loto.approval_holder_7_signature != null)))
                //{
                //    loto.status = (int)LOTOStatus.COMINGHOLDERSIGN;
                //}

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                foreach (permit_to_work ptw in loto.permit_to_work)
                {
                    if (ptw.acc_supervisor == user.id.ToString())
                    {
                        bool isComplete = true;
                        PtwEntity ptwE = new PtwEntity(ptw.id, user);
                        foreach (LotoEntity lotos in ptwE.lotoPermit)
                        {
                            if (ptw.acc_ptw_requestor == lotos.requestor)
                            {
                                isComplete = isComplete && lotos.approval_fo_signature != null;
                                LotoGlarfEntity glarf = new LotoGlarfEntity(loto.id_glarf.Value, user);
                                isComplete = isComplete && glarf.status == (int)LotoGlarfEntity.GlarfStatus.SIGNING;
                            }

                            foreach (LotoComingHolderEntity comingHolder in lotos.lotoComingHolder)
                            {
                                if (ptw.acc_supervisor == comingHolder.holder_spv)
                                {
                                    isComplete = isComplete && comingHolder.isApprove();

                                    LotoGlarfEntity glarf = null;
                                    switch (comingHolder.no_holder)
                                    {
                                        case 2:
                                            glarf = new LotoGlarfEntity(loto.holder_2_glarf.Value, user);
                                            break;
                                        case 3:
                                            glarf = new LotoGlarfEntity(loto.holder_3_glarf.Value, user);
                                            break;
                                        case 4:
                                            glarf = new LotoGlarfEntity(loto.holder_4_glarf.Value, user);
                                            break;
                                        case 5:
                                            glarf = new LotoGlarfEntity(loto.holder_5_glarf.Value, user);
                                            break;
                                        case 6:
                                            glarf = new LotoGlarfEntity(loto.holder_6_glarf.Value, user);
                                            break;
                                        case 7:
                                            glarf = new LotoGlarfEntity(loto.holder_7_glarf.Value, user);
                                            break;
                                    }

                                    if (glarf != null)
                                    {
                                        isComplete = isComplete && glarf.status == (int)LotoGlarfEntity.GlarfStatus.SIGNING;
                                    }
                                }
                            }
                        }

                        if (isComplete)
                        {
                            ptwE.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }
            }
            return retVal;
        }

        public int sendToFO()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.status = (int)LOTOStatus.SENDTOFO;

                LotoPointEntity lotoPoint = new LotoPointEntity();
                lotoPoint.id_loto = this.id;
                lotoPoint.is_set_empty = 1;
                lotoPoint.create();

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int rejectToSupervisor()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.status = (int)LOTOStatus.CREATE;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToInspect()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.status = (int)LOTOStatus.FOAPPLIED;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToApprove()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.status = (int)LOTOStatus.INSPECTION;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int requestChange()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.approval_supervisor_signature = null;
                loto.approval_supervisor_signature_date = null;
                loto.approval_supervisor_signature_delegate = null;
                loto.approval_fo_signature = null;
                loto.approval_fo_signature_date = null;
                loto.approval_fo_signature_delegate = null;
                loto.approval_holder_2_signature = null;
                loto.approval_holder_2_datetime = null;
                loto.approval_holder_3_signature = null;
                loto.approval_holder_3_datetime = null;
                loto.approval_holder_4_signature = null;
                loto.approval_holder_4_datetime = null;
                loto.approval_holder_5_signature = null;
                loto.approval_holder_5_datetime = null;
                loto.approval_holder_6_signature = null;
                loto.approval_holder_6_datetime = null;
                loto.approval_holder_7_signature = null;
                loto.approval_holder_7_datetime = null;
                loto.requestor_ok = null;
                loto.holder_2_ok = null;
                loto.holder_3_ok = null;
                loto.holder_4_ok = null;
                loto.holder_5_ok = null;
                loto.holder_6_ok = null;
                loto.holder_7_ok = null;

                loto.status = (int)LOTOStatus.LOTOCHANGE;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToApproveOtherHolder()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                string[] split = loto.new_coming_holder.Split('#');
                if (split[1] == "2")
                {
                    loto.holder_2_ok = 1;
                }
                else if (split[1] == "3")
                {
                    loto.holder_3_ok = 1;
                }
                else if (split[1] == "4")
                {
                    loto.holder_4_ok = 1;
                }
                else if (split[1] == "5")
                {
                    loto.holder_5_ok = 1;
                }
                else if (split[1] == "6")
                {
                    loto.holder_6_ok = 1;
                }
                else if (split[1] == "7")
                {
                    loto.holder_7_ok = 1;
                }
                loto.status = (int)LOTOStatus.CHGSENDTOHOLDER;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int otherHolderApprove(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id.ToString() == loto.requestor)
                {
                    loto.requestor_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_2)
                {
                    loto.holder_2_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_3)
                {
                    loto.holder_3_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_4)
                {
                    loto.holder_4_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_5)
                {
                    loto.holder_5_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_6)
                {
                    loto.holder_6_ok = 1;
                }
                
                if (user.id.ToString() == loto.oncoming_holder_7)
                {
                    loto.holder_7_ok = 1;
                }

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int otherHolderReject(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.requestor_ok = null;
                loto.holder_2_ok = null;
                loto.holder_3_ok = null;
                loto.holder_4_ok = null;
                loto.holder_5_ok = null;
                loto.holder_6_ok = null;
                loto.holder_7_ok = null;
                loto.status = (int)LOTOStatus.LOTOCHANGE;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public bool isAllOtherHolderApprove()
        {
            bool retVal = true;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                retVal = retVal && loto.requestor_ok == 1;
                if (loto.oncoming_holder_2 != null)
                {
                    retVal = retVal && loto.holder_2_ok == 1;
                }

                if (loto.oncoming_holder_3 != null)
                {
                    retVal = retVal && loto.holder_3_ok == 1;
                }

                if (loto.oncoming_holder_4 != null)
                {
                    retVal = retVal && loto.holder_4_ok == 1;
                }

                if (loto.oncoming_holder_5 != null)
                {
                    retVal = retVal && loto.holder_5_ok == 1;
                }

                if (loto.oncoming_holder_6 != null)
                {
                    retVal = retVal && loto.holder_6_ok == 1;
                }
                if (loto.oncoming_holder_7 != null)
                {
                    retVal = retVal && loto.holder_7_ok == 1;
                }

                if (retVal) loto.status = (int)LOTOStatus.CHGSENDTOFO;

                this.db.Entry(loto).State = EntityState.Modified;
                this.db.SaveChanges();
            }
            return retVal;
        }

        public int sendToInspectChange()
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.status = (int)LOTOStatus.CHGFOAPPLIED;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int isAllPointInspectedAndVerified(int isChange)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                bool[] holder = new bool[8];
                if (this.oncoming_holder_2 != null)
                {
                    holder[2] = true;
                }
                if (this.oncoming_holder_3 != null)
                {
                    holder[3] = true;
                }
                if (this.oncoming_holder_4 != null)
                {
                    holder[4] = true;
                }
                if (this.oncoming_holder_5 != null)
                {
                    holder[5] = true;
                }
                if (this.oncoming_holder_6 != null)
                {
                    holder[6] = true;
                }
                if (this.oncoming_holder_7 != null)
                {
                    holder[7] = true;
                }

                bool isComplete = true;
                foreach (LotoPointEntity lotoP in lotoPoint)
                {
                    if (lotoP.inspected_1 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[2] && lotoP.inspected_2 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[3] && lotoP.inspected_3 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[4] && lotoP.inspected_4 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[5] && lotoP.inspected_5 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[6] && lotoP.inspected_6 == null)
                    {
                        isComplete = false;
                    }
                    if (holder[7] && lotoP.inspected_7 == null)
                    {
                        isComplete = false;
                    }
                }

                if (isComplete)
                {
                    if (isChange == 1)
                    {
                        loto.status = (int)LOTOStatus.CHGINSPECTION;
                    }
                    else
                    {
                        loto.status = (int)LOTOStatus.INSPECTION;
                    }

                    this.db.Entry(loto).State = EntityState.Modified;
                    retVal = this.db.SaveChanges();
                }
            }
            return retVal;
        }

        public int approvalSupervisorChange(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id)
                {
                    loto.approval_supervisor_signature = "a" + user.signature;
                }
                else if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate)
                {
                    loto.approval_supervisor_signature = "d" + user.signature;
                    loto.approval_supervisor_signature_delegate = user.id.ToString();
                }

                loto.approval_supervisor_signature_date = DateTime.Now;
                loto.status = (int)LOTOStatus.CHGSPVSIGN;
                loto.approval_notes = this.approval_notes;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int approvalFacilityOwnerChange(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id)
                {
                    loto.approval_fo_signature = "a" + user.signature;
                }
                else
                {
                    loto.approval_fo_signature = "d" + user.signature;
                    loto.approval_fo_signature_delegate = user.id.ToString();
                }

                loto.approval_fo_signature_date = DateTime.Now;
                loto.approval_date = loto.approval_fo_signature_date;
                loto.approval_notes = this.approval_notes;
                loto.status = (int)LOTOStatus.CHGFOSIGN;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int suspendLoto(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id)
                {
                    List<string> otherHolder = new List<string>();
                    foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                    {
                        if (!comingHolder.isCancel())
                            otherHolder.Add(comingHolder.holder_spv);
                    }

                    LotoSuspensionEntity suspension = new LotoSuspensionEntity(this.id, this.supervisor, this.lotoSuspension.Count + 1, otherHolder, this.approval_facility_owner);
                    retVal = suspension.create();
                }
                else
                {
                    List<string> otherHolder = new List<string>();
                    if (this.cancellation_supervisor_signature == null)
                        otherHolder.Add(this.supervisor);
                    foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                    {
                        if (comingHolder.userEntity.id != user.id && !comingHolder.isCancel())
                        {
                            otherHolder.Add(comingHolder.holder_spv);
                        }
                    }

                    LotoSuspensionEntity suspension = new LotoSuspensionEntity(this.id, user.id.ToString(), this.lotoSuspension.Count + 1, otherHolder, this.approval_facility_owner);
                    retVal = suspension.create();
                }

                loto.status = (int)LOTOStatus.LOTOSUSPENSION;
                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int sendAgreePointSuspension(UserEntity user, string notes)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.EDITANDSEND;
                    suspension.notes = notes;

                    if (suspension.suspensionHolder.Count == 0)
                    {
                        suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE;
                    }

                    retVal = suspension.sendApprove();
                }
            }

            return retVal;
        }

        public int agreeSuspension(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    retVal = suspension.agreeSuspension(user);
                }
            }

            return retVal;
        }

        public int saveAppliedFOSuspension(UserEntity user, string notes)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONFOAPPROVE;
                    suspension.notes = notes;
                    // suspension.facility_owner = user.id.ToString();

                    retVal = suspension.foToHolderInspection();
                }
            }

            return retVal;
        }

        public int saveApprovedInspected(UserEntity user, string notes)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.notes = notes;
                    
                    bool isComplete = true;
                    if (user.id == suspension.requestorUser.id)
                    {
                        suspension.requestor_signature = user.signature;
                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (!suspensionHolder.isApprove())
                            {
                                isComplete = false;
                            }
                        }
                    }
                    else
                    {
                        isComplete = suspension.requestor_signature != null;
                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (user.id != suspensionHolder.userEntity.id && !suspensionHolder.isApprove())
                            {
                                isComplete = false;
                            }
                            else if(user.id == suspensionHolder.userEntity.id)
                            {
                                suspensionHolder.signApprove(user);
                            }
                        }
                    }

                    if (isComplete)
                    {
                        suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONINSPECTION;
                    }

                    retVal = suspension.approveInspection();
                }
            }

            return retVal;
        }

        public int foApproveSuspension(UserEntity user, string notes) {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVED;
                    suspension.notes = notes;
                    suspension.fo_signature = user.signature;

                    retVal = suspension.approveFO(user);
                }
            }

            return retVal;
        }

        public int suspensionCompletion(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENDCOMPLETE;

                    retVal = suspension.completeSuspension();
                }
            }
            return retVal;
        }

        public int suspensionCompletionSend(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                bool complete = true;
                foreach (LotoPointEntity lotoPoint in this.lotoPoint)
                {
                    if (lotoPoint.is_set_empty == null || lotoPoint.is_set_empty == 0)
                        complete = complete && lotoPoint.applied_by != null;
                }

                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    if (!complete)
                    {
                        suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETESEND;

                        retVal = suspension.sendCompleteAgreement();
                    }
                    else
                    {
                        suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETED;

                        retVal = suspension.completedSuspension();

                        loto.status = (int)LOTOStatus.FOSIGN;

                        this.db.Entry(loto).State = EntityState.Modified;
                        this.db.SaveChanges();

                        retVal = 100;
                    }
                }
            }
            return retVal;
        }

        public int agreeCompleteSuspension(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    retVal = suspension.agreeCompleteSuspension(user, loto.id);
                }
            }

            return retVal;
        }

        public int saveAppliedFOCompleteSuspension(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETEINSPECTED;

                    retVal = suspension.foToHolderCompleteInspection();
                }
            }

            return retVal;
        }

        public int saveApprovedInspectedCompleteSuspension(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    bool isComplete = true;
                    if (user.id == suspension.requestorUser.id)
                    {
                        suspension.can_requestor_signature = user.signature;
                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (!suspensionHolder.isApproveComplete())
                            {
                                isComplete = false;
                            }
                        }
                    }
                    else
                    {
                        isComplete = suspension.can_requestor_signature != null;
                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (user.id != suspensionHolder.userEntity.id && !suspensionHolder.isApproveComplete())
                            {
                                isComplete = false;
                            }
                            else if (user.id == suspensionHolder.userEntity.id)
                            {
                                suspensionHolder.signCancellation(user);
                            }
                        }
                    }

                    if (isComplete)
                    {
                        suspension.status = (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETED;

                        loto.status = (int)LOTOStatus.FOSIGN;

                        this.db.Entry(loto).State = EntityState.Modified;
                        this.db.SaveChanges();
                    }

                    retVal = suspension.approveInspectionComplete();
                }
            }

            return retVal;
        }

        public int lotoCancel(UserEntity user, out int loto_glarf)
        {
            int retVal = 0;
            loto_glarf = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate)
                {
                    loto_glarf = this.id_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.id_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_2_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_2_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_3_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_3_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_4_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_4_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_5_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_5_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_6_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_6_glarf.Value, user);
                    retVal = glarf.setCancel();
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null && (user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate))
                {
                    loto_glarf = this.holder_7_glarf.Value;
                    LotoGlarfEntity glarf = new LotoGlarfEntity(this.holder_7_glarf.Value, user);
                    retVal = glarf.setCancel();
                }
            }
            return retVal;
        }

        public int holderCancel(UserEntity user, int id_loto)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                bool complete = loto.cancellation_supervisor_signature != null;
                foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                {
                    if (user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate)
                    {
                        comingHolder.signCancellation(user);
                    }
                    else
                    {
                        complete = complete && comingHolder.isCancel();
                    }
                }
                loto.cancellation_notes = this.cancellation_notes;
                if (complete)
                {
                    this.status = (int)LOTOStatus.CANCELSPV;
                    loto.status = (int)LOTOStatus.CANCELSPV;
                }

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                if (!complete)
                {
                    PtwEntity ptw = new PtwEntity(id_loto, user);
                    ptw.checkLotoCancellationComplete(user);
                }
            }

            return retVal;
        }

        public int spvCancel(UserEntity user, int id_loto)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
                {
                    if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate))
                    {
                        loto.cancellation_supervisor_signature = user.signature;
                        loto.cancellation_supervisor_signature_date = DateTime.Now;
                        loto.cancellation_notes = this.cancellation_notes;

                        bool complete = true;
                        foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                        {
                            complete = complete && comingHolder.isCancel();
                        }
                        if (complete)
                        {
                            this.status = (int)LOTOStatus.CANCELSPV;
                            loto.status = (int)LOTOStatus.CANCELSPV;
                        }

                        this.db.Entry(loto).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();

                        if (!complete)
                        {
                            PtwEntity ptw = new PtwEntity(id_loto, user);
                            ptw.checkLotoCancellationComplete(user);
                        }
                    }
                }
            }
            return retVal;
        }

        public int FOCancel(UserEntity user, int id_loto)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (user.id.ToString() != this.cancellation_facility_owner)
                {
                    loto.cancellation_fo_signature_delegate = user.id.ToString();
                }
                loto.cancellation_fo_signature = user.signature;
                loto.cancellation_fo_signature_date = DateTime.Now;
                loto.cancellation_notes = this.cancellation_notes;
                loto.status = (int)LOTOStatus.LOTOCANCELLED;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                PtwEntity ptw = new PtwEntity(id_loto, user);
                ptw.checkLotoCancellationComplete(user);
            }
            return retVal;
        }


        #region internal function

        internal void getUserInLOTO(UserEntity user, ListUser listUsers = null)
        {
            ListUser listUser = listUsers != null ? listUsers : new ListUser(user.token, user.id);
            int userId = 0;

            if (this.requestor != null)
            {
                string[] requestors = this.requestor.Split('#');
                Int32.TryParse(requestors[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.REQUESTOR.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.REQUESTOR.ToString(), null);
            }

            userId = 0;
            Int32.TryParse(this.supervisor, out userId);
            this.listUserInLOTO.Add(userInLOTO.SUPERVISOR.ToString(), listUser.listUser.Find(p => p.id == userId));

            userId = 0;
            Int32.TryParse(this.approval_facility_owner, out userId);
            this.listUserInLOTO.Add(userInLOTO.APPROVALFACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));

            if (this.approval_fo_signature_delegate != null)
            {
                Int32.TryParse(this.approval_fo_signature_delegate, out userId);
                this.listUserInLOTO.Add(userInLOTO.APPROVALFACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }

            userId = 0;
            if (this.oncoming_holder_1 != null)
            {
                Int32.TryParse(this.oncoming_holder_1.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER1.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER1.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_2 != null)
            {
                Int32.TryParse(this.oncoming_holder_2.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER2.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER2.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_3 != null)
            {
                Int32.TryParse(this.oncoming_holder_3.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER3.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER3.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_4 != null)
            {
                Int32.TryParse(this.oncoming_holder_4.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER4.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER4.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_5 != null)
            {
                Int32.TryParse(this.oncoming_holder_5.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER5.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER5.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_6 != null)
            {
                Int32.TryParse(this.oncoming_holder_6.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER6.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER6.ToString(), null);
            }

            userId = 0;
            if (this.oncoming_holder_7 != null)
            {
                Int32.TryParse(this.oncoming_holder_7.Split('#')[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER7.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.ONCOMINGHOLDER7.ToString(), null);
            }

            userId = 0;
            if (this.new_coming_holder != null)
            {
                string[] newHolder = this.new_coming_holder.Split('#');
                Int32.TryParse(newHolder[0], out userId);
                this.listUserInLOTO.Add(userInLOTO.NEWCOMINGHOLDER.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.NEWCOMINGHOLDER.ToString(), null);
            }

            userId = 0;
            if (this.cancellation_facility_owner != null)
            {
                Int32.TryParse(this.cancellation_facility_owner, out userId);
                this.listUserInLOTO.Add(userInLOTO.CANCELLATIONFACILITYOWNER.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
            else
            {
                this.listUserInLOTO.Add(userInLOTO.CANCELLATIONFACILITYOWNER.ToString(), null);
            }

            if (this.cancellation_fo_signature_delegate != null)
            {
                Int32.TryParse(this.cancellation_fo_signature_delegate, out userId);
                this.listUserInLOTO.Add(userInLOTO.CANCELLATIONFACILITYOWNERDELEGATE.ToString(), listUser.listUser.Find(p => p.id == userId));
            }
        }
        #endregion

        public string sendEmailFO(List<UserEntity> listFO, string serverUrl, string token, UserEntity user, int stat)
        {
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailfo";
            SendEmail sendEmail = new SendEmail();
            foreach (UserEntity fo in listFO)
            {
                string timestamp = DateTime.UtcNow.Ticks.ToString();
                List<string> s = new List<string>();
                s.Add(fo.email);
                // s.Add("septu.jamasoka@gmail.com"); // email FO
                if (fo.employee_delegate != null)
                {
                    UserEntity del = new UserEntity(fo.employee_delegate.Value, token, user);
                    s.Add(del.email);
                    // s.Add("septu.jamasoka@gmail.com"); // email Delegasi FO
                }

                string encodedValue = stat + salt + fo.id + val + this.id;
                string encodedElement = Base64.Base64Encode(encodedValue);

                string seal = Base64.MD5Seal(timestamp + salt + val);

                string message = serverUrl + "Loto/SetFacilityOwner?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                sendEmail.Send(s, message, "LOTO Permit Facility Owner");
            }

            return "200";
        }

        public int setFacilityOwner(UserEntity user, int stat)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (stat == 0)
                {
                    loto.approval_facility_owner = user.id.ToString();
                }

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        #region is Can Edit

        public bool isCanEditFirstRequestor(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CREATE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanAgreedApplied(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.SENDTOFO)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)LOTOStatus.SENDTOFO)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanInspect(UserEntity user)
        {
            if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.FOAPPLIED)
            {
                return true;
            }
            return false;
        }

        public bool isCanApproveSpv(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.INSPECTION)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveFO(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.SPVSIGN)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)LOTOStatus.SPVSIGN)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveChangeComingHolder(UserEntity user)
        {
            loto_permit loto = this.db.loto_permit.Find(this.id);
            bool isApproveGlarf = false;
            foreach (LotoComingHolderEntity comingHolder in lotoComingHolder)
            {
                if ((user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate) && comingHolder.holder_sign_approval == null)
                {
                    return true;
                }
            }
            
            return false;
        }

        public bool isCanEditComingHolder(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.LOTOCHANGE)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveOtherHolder(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.requestor_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_2_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_3_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_4_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_5_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_6_ok == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSENDTOHOLDER && this.holder_7_ok == null)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isFOCanAgreedAndAppliedChange(UserEntity user)
        {

            ListUser listUser = new ListUser(user.token, user.id);
            List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);

            if ((this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id == user.id || listDel.Exists(p => p.id == user.id)) && this.status == (int)LOTOStatus.CHGSENDTOFO)
            {
                return true;
            }
            return false;
        }

        public bool isCanInspectChange(UserEntity user)
        {
            if ((user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
            {
                return true;
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOAPPLIED)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApprovingOtherHolder(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_2_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_3_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_4_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_5_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_6_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGFOSIGN && this.approval_holder_7_signature == null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanApproveSpvChange(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGINSPECTION)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanApproveFOChange(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()] != null)
            {
                List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSPVSIGN)
                {
                    return true;
                }
                else if (listDel.Exists(p => p.id == user.id) && this.status == (int)LOTOStatus.CHGSPVSIGN)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanSuspend(UserEntity user) {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                bool isCan = false;
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status != (int)LOTOStatus.FOSIGN)
                {
                    isCan = false;
                }
                else if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.FOSIGN && this.cancellation_supervisor_signature == null)
                {
                    isCan = true;
                }
                else if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
                {
                    isCan = false;
                }
                else
                {
                    foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                    {
                        if ((user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate) && comingHolder.isApprove() && !comingHolder.isCancel())
                        {
                            isCan = true;
                        }
                        else
                        {
                            isCan = false;
                        }
                    }
                }
                return isCan;
            }
            return false;
        }

        public bool isCanEditOnSuspension(UserEntity user) {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION) {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null) {
                    if (user.id == suspension.requestorUser.id && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENDED) {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanApproveChangeSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.EDITANDSEND)
                {
                    foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                    {
                        if ((user.id == suspensionHolder.userEntity.id || user.id == suspensionHolder.userEntity.employee_delegate) && !suspensionHolder.isAgree())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool isCanSetAgreedRemovedFO(UserEntity user) {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    ListUser listUser = new ListUser(user.token, user.id);
                    List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);

                    if ((this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id == user.id || listDel.Exists(p => p.id == user.id)) && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanInspectChangeHolder(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONFOAPPROVE)
                {
                    bool isCan = false;
                    if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate))
                    {
                        foreach (LotoPointEntity lotoPoint in this.lotoPoint)
                        {
                            if (lotoPoint.inspected_1 == null)
                            {
                                isCan = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                        {
                            if ((user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate))
                            {
                                var enumerator = this.lotoPoint.GetEnumerator();
                                while (enumerator.MoveNext() && !isCan)
                                {
                                    LotoPointEntity lotoPoint = enumerator.Current;

                                    switch (comingHolder.no_holder)
                                    {
                                        case 2:
                                            if (lotoPoint.inspected_2 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 3:
                                            if (lotoPoint.inspected_3 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 4:
                                            if (lotoPoint.inspected_4 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 5:
                                            if (lotoPoint.inspected_5 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 6:
                                            if (lotoPoint.inspected_6 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 7:
                                            if (lotoPoint.inspected_7 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    if (!isCan)
                    {
                        if (user.id == suspension.requestorUser.id)
                        {
                            if (suspension.requestor_signature == null)
                            {
                                isCan = true;
                            }
                        }

                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (user.id == suspensionHolder.userEntity.id && !suspensionHolder.isApprove())
                            {
                                isCan = true;
                            }
                        }
                    }
                    return isCan;
                }
            }
            return false;
        }

        public bool isCanApproveFOSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {

                    List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                    if (suspension.foUser != null && (suspension.foUser.id == user.id || listDel.Exists(p => p.id == user.id)) && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONINSPECTION)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanCompleteSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    if (user.id == suspension.requestorUser.id && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVED)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanAddPointOnCompleteSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null)
                {
                    if (user.id == suspension.requestorUser.id && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENDCOMPLETE)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanAgreedNewLotoPointCompleteSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETESEND)
                {
                    foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                    {
                        if ((user.id == suspensionHolder.userEntity.id || user.id == suspensionHolder.userEntity.employee_delegate) && !suspensionHolder.isAgree())
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool isCanSetAppliedCompleteSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null && suspension.foUser != null)
                {
                    List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);

                    if ((listDel.Exists(p => p.id == user.id) || suspension.foUser.id == user.id) && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETEAGREED)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool isCanInspectHolderCompleteSuspension(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.LOTOSUSPENSION)
            {
                LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
                if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONCOMPLETEINSPECTED)
                {
                    bool isCan = false;
                    if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate))
                    {
                        foreach (LotoPointEntity lotoPoint in this.lotoPoint)
                        {
                            if (lotoPoint.inspected_1 == null)
                            {
                                isCan = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (LotoComingHolderEntity comingHolder in this.lotoComingHolder)
                        {
                            if ((user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate))
                            {
                                var enumerator = this.lotoPoint.GetEnumerator();
                                while (enumerator.MoveNext() && !isCan)
                                {
                                    LotoPointEntity lotoPoint = enumerator.Current;

                                    switch (comingHolder.no_holder)
                                    {
                                        case 2:
                                            if (lotoPoint.inspected_2 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 3:
                                            if (lotoPoint.inspected_3 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 4:
                                            if (lotoPoint.inspected_4 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 5:
                                            if (lotoPoint.inspected_5 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 6:
                                            if (lotoPoint.inspected_6 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                        case 7:
                                            if (lotoPoint.inspected_7 == null)
                                            {
                                                isCan = true;
                                            }
                                            break;
                                    }
                                }
                            }
                        }
                    }

                    if (!isCan)
                    {
                        if (user.id == suspension.requestorUser.id)
                        {
                            if (suspension.can_requestor_signature == null)
                            {
                                isCan = true;
                            }
                        }

                        foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                        {
                            if (user.id == suspensionHolder.userEntity.id && !suspensionHolder.isApproveComplete())
                            {
                                isCan = true;
                            }
                        }
                    }
                    return isCan;
                }
            }
            return false;
        }

        public bool isCanCancel(UserEntity user)
        {
            if (this.status == (int)LOTOStatus.FOSIGN || this.status == (int)LOTOStatus.CANCELSPV)
            {
                var enume = this.lotoComingHolder.GetEnumerator();
                bool found = false;
                while (enume.MoveNext() && !found)
                {
                    LotoComingHolderEntity comingHolder = enume.Current;
                    if ((user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate) && !comingHolder.isCancel())
                    {
                        found = true;
                    }
                }
                return found;
            }

            return false;
        }

        public bool isCanCancelSpv(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(this.id_glarf.Value, user);
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_supervisor_signature == null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanCancelFO(UserEntity user)
        {
            List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].GetDelegateFO(user);

            if ((listDel.Exists(p => p.id == user.id) || this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id == user.id) && this.status == (int)LOTOStatus.CANCELSPV)
            {
                return true;
            }
            return false;
        }


        #endregion

        internal int assignSupervisor(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.supervisor = user.id.ToString();

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal bool isUserInLOTO(UserEntity user)
        {
            foreach (UserEntity us in listUserInLOTO.Values)
            {
                if (us != null)
                {
                    if (user.id == us.id || user.id == us.employee_delegate)
                    {
                        return true;
                    }
                }
            }

            foreach (LotoComingHolderEntity comingHolder in lotoComingHolder)
            {
                if (user.id == comingHolder.userEntity.id || user.id == comingHolder.userEntity.employee_delegate)
                {
                    return true;
                }
            }

            return false;
        }

        public List<LotoEntity> listLoto(UserEntity user)
        {
            List<int> listId = this.db.loto_permit.Where(p => p.status < (int)LOTOStatus.LOTOCANCELLED).Select(p => p.id).ToList();
            List<LotoEntity> result = new List<LotoEntity>();
            foreach (int i in listId)
            {
                result.Add(new LotoEntity(i, user));
            }

            return result;
        }

        #region send email LOTO

        public int sendEmailFOAgreed(string url, UserEntity user)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.approval_facility_owner), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
            List<UserEntity> listDel = userFo.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
                s.Add(u.email);
                userIds.Add(u.id);
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Facility Owner Set Agreed LOTO Point and Applied");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please set agreed LOTO Point of LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailRejectSpv(string url, UserEntity user, string comment)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id + "<br />Comment: " + comment;

            sendEmail.Send(s, message, "LOTO Permit Rejected by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "LOTO Permit No. " + this.loto_no + " is rejected by Facility Owner with comment: " + comment, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailInspected(string url, UserEntity user)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Inspection and Verification");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please inspect and verify LOTO Point of LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailSupervisorApprove(string url, UserEntity user)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Supervisor Approval");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailSupervisorComplete(int retVals, string url, UserEntity user)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();

#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;
            if (retVals == 1) {
                sendEmail.Send(s, message, "LOTO GLARF Completion");
                sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please complete LOTO GLARF for LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);
            } else if (retVals == 2) {
                sendEmail.Send(s, message, "LOTO Permit Completed");
                sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "LOTO Permit No. " + this.loto_no + " has been completed.", url + "Home?p=LOTO/edit/" + this.id);
            }

            return retVal;
        }

        public int sendEmailFOApprove(string url, UserEntity user)
        {
            int retVal = 0;

            UserEntity userFo = new UserEntity(Int32.Parse(this.approval_facility_owner), user.token, user);
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            s.Add(userFo.email);
            userIds.Add(userFo.id);
            List<UserEntity> listDel = userFo.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
                s.Add(u.email);
                userIds.Add(u.id);
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Facility Owner Approval");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailOnComingHolderApprove(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            //s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Onccoming Holder Approved");
            //sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailAgreeSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.EDITANDSEND)
            {
                foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                {
                    s.Add(suspensionHolder.userEntity.email);
                    userIds.Add(suspensionHolder.userEntity.id);
                }
            }
            else if (suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE)
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Need To Be Agreed");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "LOTO Permit No. " + this.loto_no + " is requested to be suspended. Please follow up the request.", url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailHolderAgreedSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status != (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE)
            {
                s.Add(suspension.requestorUser.email);
                userIds.Add(suspension.requestorUser.id);
            }
            else
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Agreed by " + user.alpha_name);

            return retVal;
        }

        public int sendEmailFoAppliedSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null)
            {
                s.Add(suspension.requestorUser.email);
                userIds.Add(suspension.requestorUser.id);
                foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                {
                    s.Add(suspensionHolder.userEntity.email);
                    userIds.Add(suspensionHolder.userEntity.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Point Agreed and Applied by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "LOTO Permit No. " + this.loto_no + " suspension point has been agreed and applied by Facility Owner.", url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailCompleteInspectedSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONINSPECTION)
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Inspected by LOTO Holder, Need To Be Approved by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit Suspension No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailFoApprovedSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null)
            {
                s.Add(suspension.requestorUser.email);
                userIds.Add(suspension.requestorUser.id);
                foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                {
                    s.Add(suspensionHolder.userEntity.email);
                    userIds.Add(suspensionHolder.userEntity.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Approved By Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "LOTO Permit Suspension No. " + this.loto_no + " has been approved by Facility Owner.", url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailAgreeCompletionSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.EDITANDSEND)
            {
                foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                {
                    s.Add(suspensionHolder.userEntity.email);
                    userIds.Add(suspensionHolder.userEntity.id);
                }
            }
            else if (suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE)
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Inspected by LOTO Holder, Need To Be Approved by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit Suspension No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailHolderAgreedCompletionSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status != (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONAPPROVE)
            {
                s.Add(suspension.requestorUser.email);
                userIds.Add(suspension.requestorUser.id);
            }
            else
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Agreed by " + user.alpha_name);

            return retVal;
        }

        public int sendEmailFoAppliedCompletionSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null)
            {
                s.Add(suspension.requestorUser.email);
                userIds.Add(suspension.requestorUser.id);
                foreach (LotoSuspensionHolderEntity suspensionHolder in suspension.suspensionHolder)
                {
                    s.Add(suspensionHolder.userEntity.email);
                    userIds.Add(suspensionHolder.userEntity.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Point Agreed and Applied by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit Suspension No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailCompleteInspectedCompletionSuspension(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            LotoSuspensionEntity suspension = this.lotoSuspension.LastOrDefault();
            if (suspension != null && suspension.status == (int)LotoSuspensionEntity.SuspensionStatus.SUSPENSIONINSPECTION)
            {
                s.Add(suspension.foUser.email);
                userIds.Add(suspension.foUser.id);
                List<UserEntity> listDel = suspension.foUser.GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Suspension Inspected by LOTO Holder, Need To Be Approved by Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve LOTO Permit Suspension No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailFOCancellation(string url, UserEntity user)
        {
            int retVal = 0;
            SendEmail sendEmail = new SendEmail();

            List<string> s = new List<string>();
            List<int> userIds = new List<int>();
#if (!DEBUG)
            if(this.status == (int)LOTOStatus.CANCELSPV)
            {

                s.Add(this.listUserInLOTO[userInLOTO.CANCELLATIONFACILITYOWNER.ToString()].email);
                userIds.Add(this.listUserInLOTO[userInLOTO.CANCELLATIONFACILITYOWNER.ToString()].id);
                List<UserEntity> listDel = this.listUserInLOTO[userInLOTO.CANCELLATIONFACILITYOWNER.ToString()].GetDelegateFO(user);
                foreach (UserEntity u in listDel)
                {
                    s.Add(u.email);
                    userIds.Add(u.id);
                }
            }
#else
            s.Add("septu.jamasoka@gmail.com"); // email FO
#endif
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Cancellation By Facility Owner");
            sendEmail.SendToNotificationCenter(userIds, "LOTO Permit", "Please approve cancellation of LOTO Permit No. " + this.loto_no, url + "Home?p=LOTO/edit/" + this.id);

            return retVal;
        }

        public int sendEmailLotoCancelByOncomingHolder(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            //s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Oncoming Holder Cancelled");

            return retVal;
        }

        #endregion

        internal List<LotoEntity> listLotoRemove(UserEntity userLogin)
        {
            List<int> listId = this.db.loto_permit.Select(p => p.id).ToList();
            List<LotoEntity> result = new List<LotoEntity>();
            foreach (int i in listId)
            {
                LotoEntity loto = new LotoEntity(i, userLogin);
                if (loto.status == (int)LOTOStatus.LOTOCANCELLED)
                {
                    result.Add(loto);
                }
                else
                {
                    var enumerator = loto.lotoPoint.GetEnumerator();
                    bool need = false;
                    while (enumerator.MoveNext() && !need)
                    {
                        LotoPointEntity lotoPoint = enumerator.Current;
                        if (lotoPoint.is_removed == 1 && lotoPoint.removed_by == null)
                        {
                            need = true;
                        }
                        if (need)
                        {
                            result.Add(loto);
                        }
                    }
                }
            }

            return result;
        }
    }
}