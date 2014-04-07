using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoEntity : loto_permit, IClearancePermitEntity
    {
        public override ICollection<loto_point> loto_point { get { return null; } set { } }
        public override ICollection<loto_glarf> loto_glarf { get { return null; } set { } } 

        public int ids { get; set; }
        public string statusText { get; set; }

        public Dictionary<string, UserEntity> listUserInLOTO { get; set; }

        public List<LotoPointEntity> lotoPoint { get; set; }

        public enum userInLOTO
        {
            REQUESTOR,
            SUPERVISOR,
            APPROVALFACILITYOWNER,
            ONCOMINGHOLDER1,
            ONCOMINGHOLDER2,
            ONCOMINGHOLDER3,
            ONCOMINGHOLDER4,
            ONCOMINGHOLDER5,
            ONCOMINGHOLDER6,
            ONCOMINGHOLDER7,
            NEWCOMINGHOLDER,
            CANCELLATIONFACILITYOWNER,
        }

        public enum LOTOStatus
        {
            CREATE,
            SENDTOFO,
            FOAGREED,
            FOAPPLIED,
            INSPECTION,
            SPVSIGN,
            FOSIGN,
            LOTOCHANGE,
            CHGSENDTOHOLDER,
            CHGSENDTOFO,
            CHGFOAGREED,
            CHGFOAPPLIED,
            CHGINSPECTION,
            CHGSPVSIGN,
            CHGFOSIGN,
            COMINGHOLDERSIGN,
            LOTOSUSPENSION,
            CANCELSPV,
            LOTOCANCELLED,
        }

        private star_energy_ptwEntities db;

        public LotoEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
            this.listUserInLOTO = new Dictionary<string, UserEntity>();
        }

        // constructor with id to get object from database
        public LotoEntity(int id, UserEntity user)
            : this()
        {
            loto_permit loto = this.db.loto_permit.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(loto, this);
            this.lotoPoint = new LotoPointEntity().getList(user, this.id);
            getUserInLOTO(user);
        }

        public LotoEntity(string requestor, string work_location, int id_glarf, string acc_spv = null)
            : this()
        {
            this.requestor = requestor + '#' + id_glarf;
            this.work_location = work_location;
            this.supervisor = acc_spv;
            this.status = (int)LOTOStatus.CREATE;
        }

        public LotoEntity(LotoEntity loto, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(loto, this);
            this.id = 0;
            this.new_coming_holder = user.id.ToString();
        }

        public int create() {
            loto_permit loto = new loto_permit();
            ModelUtilization.Clone(this, loto);
            this.db.loto_permit.Add(loto);
            int retVal = this.db.SaveChanges();
            this.id = loto.id;

            return retVal;
        }

        public int addNewHolder(string oncomingHolder, int glarf_id)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (loto.oncoming_holder_2 == null)
                {
                    loto.oncoming_holder_2 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 2;
                }
                else if (loto.oncoming_holder_3 == null)
                {
                    loto.oncoming_holder_3 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 3;
                }
                else if (loto.oncoming_holder_4 == null)
                {
                    loto.oncoming_holder_4 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 4;
                }
                else if (loto.oncoming_holder_5 == null)
                {
                    loto.oncoming_holder_5 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 5;
                }
                else if (loto.oncoming_holder_6 == null)
                {
                    loto.oncoming_holder_6 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 6;
                }
                else if (loto.oncoming_holder_7 == null)
                {
                    loto.oncoming_holder_7 = oncomingHolder + '#' + glarf_id;
                    loto.new_coming_holder = loto.new_coming_holder + "#" + 7;
                }
                else
                {
                    return -1;
                }

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
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
                else if (user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate)
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

                foreach (loto_glarf glarf in loto.loto_glarf)
                {
                    if (glarf.requestor == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id.ToString())
                    {
                        PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
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
                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id)
                {
                    loto.approval_holder_2_signature = "a" + user.signature;
                    loto.approval_holder_2_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id)
                {
                    loto.approval_holder_3_signature = "a" + user.signature;
                    loto.approval_holder_3_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id)
                {
                    loto.approval_holder_4_signature = "a" + user.signature;
                    loto.approval_holder_4_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id)
                {
                    loto.approval_holder_5_signature = "a" + user.signature;
                    loto.approval_holder_5_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id)
                {
                    loto.approval_holder_6_signature = "a" + user.signature;
                    loto.approval_holder_6_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }
                
                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null && user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id)
                {
                    loto.approval_holder_7_signature = "a" + user.signature;
                    loto.approval_holder_7_datetime = DateTime.Now;

                    foreach (loto_glarf glarf in loto.loto_glarf)
                    {
                        if (glarf.requestor == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id.ToString())
                        {
                            PtwEntity ptw = new PtwEntity(glarf.permit_to_work.ElementAt(0).id, user);
                            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                        }
                    }
                }

                if (loto.new_coming_holder.Split('#')[0] == user.id.ToString())
                {
                    loto.new_coming_holder = null;
                }

                loto.approval_notes = this.approval_notes;
                if ((loto.oncoming_holder_2 == null || (loto.oncoming_holder_2 != null && loto.approval_holder_2_signature != null)) &&
                    (loto.oncoming_holder_3 == null || (loto.oncoming_holder_3 != null && loto.approval_holder_3_signature != null)) &&
                    (loto.oncoming_holder_4 == null || (loto.oncoming_holder_4 != null && loto.approval_holder_4_signature != null)) &&
                    (loto.oncoming_holder_5 == null || (loto.oncoming_holder_5 != null && loto.approval_holder_5_signature != null)) &&
                    (loto.oncoming_holder_6 == null || (loto.oncoming_holder_6 != null && loto.approval_holder_6_signature != null)) &&
                    (loto.oncoming_holder_7 == null || (loto.oncoming_holder_7 != null && loto.approval_holder_7_signature != null)))
                {
                    loto.status = (int)LOTOStatus.COMINGHOLDERSIGN;
                }

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
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
                else if (user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate)
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

        public int holderCancel(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_2.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_2_signature = user.signature;
                        loto.cancellation_holder_2_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_3.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_3_signature = user.signature;
                        loto.cancellation_holder_3_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_4.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_4_signature = user.signature;
                        loto.cancellation_holder_4_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_5.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_5_signature = user.signature;
                        loto.cancellation_holder_5_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_6.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_6_signature = user.signature;
                        loto.cancellation_holder_6_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }

                if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null)
                {
                    LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_7.Split('#')[1]), user);
                    if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE)
                    {
                        loto.cancellation_holder_7_signature = user.signature;
                        loto.cancellation_holder_7_signature_date = DateTime.Now;

                        PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                        ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
                    }
                }
                loto.cancellation_notes = this.cancellation_notes;

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int spvCancel(UserEntity user)
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
                        loto.status = (int)LOTOStatus.CANCELSPV;
                        loto.cancellation_notes = this.cancellation_notes;

                        this.db.Entry(loto).State = EntityState.Modified;
                        retVal = this.db.SaveChanges();
                    }
                }
            }
            return retVal;
        }

        public int FOCancel(UserEntity user)
        {
            int retVal = 0;
            loto_permit loto = this.db.loto_permit.Find(this.id);
            if (loto != null)
            {
                loto.cancellation_facility_owner = user.id.ToString();
                loto.cancellation_fo_signature = user.signature;
                loto.cancellation_fo_signature_date = DateTime.Now;
                loto.cancellation_notes = this.cancellation_notes;
                loto.status = (int)LOTOStatus.LOTOCANCELLED;

                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.requestor.Split('#')[1]), user);

                PtwEntity ptw = new PtwEntity(lotoGlarf.id_ptw, user);
                ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());

                this.db.Entry(loto).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }


        #region internal function

        internal void getUserInLOTO(UserEntity user)
        {
            ListUser listUser = new ListUser(user.token, user.id);
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
                //s.Add(fo.email);
                s.Add("septu.jamasoka@gmail.com"); // email FO
                if (fo.employee_delegate != null)
                {
                    UserEntity del = new UserEntity(fo.employee_delegate.Value, token, user);
                    //s.Add(del.email);
                    s.Add("septu.jamasoka@gmail.com"); // email Delegasi FO
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
            if ((user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CREATE)
            {
                return true;
            }
            return false;
        }

        public bool isCanAgreedApplied(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()] != null)
            {
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.SENDTOFO)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanInspect(UserEntity user)
        {
            if ((user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.REQUESTOR.ToString()].employee_delegate) && this.status == (int)LOTOStatus.FOAPPLIED)
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
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.SPVSIGN)
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
            if (this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()] != null)
            {
                //foreach (loto_glarf glarf in loto.loto_glarf)
                //{
                //    if (glarf.requestor == this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()].id.ToString())
                //    {
                //        isApproveGlarf = glarf.status == (int)PermitToWork.Models.ClearancePermit.LotoGlarfEntity.GlarfStatus.SPVSIGN;
                //    }
                //}

                if ((user.id == this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.NEWCOMINGHOLDER.ToString()].employee_delegate) && (this.status == (int)LOTOStatus.FOSIGN || this.status == (int)LOTOStatus.COMINGHOLDERSIGN))
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
            List<UserEntity> listHWFO = listUser.GetHotWorkFO();

            if (listHWFO.Exists(p => p.id == user.id) && this.status == (int)LOTOStatus.CHGSENDTOFO)
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
                if ((user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.APPROVALFACILITYOWNER.ToString()].employee_delegate) && this.status == (int)LOTOStatus.CHGSPVSIGN)
                {
                    return true;
                }
            }
            return false;
        }

        public bool isCanCancel(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_2.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER2.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_2_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_3.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER3.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_3_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_4.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER4.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_4_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_5.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER5.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_5_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_6.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER6.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_6_signature == null)
                {
                    return true;
                }
            }

            if (this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.oncoming_holder_7.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.ONCOMINGHOLDER7.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.cancellation_holder_7_signature == null)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanCancelSpv(UserEntity user)
        {
            if (this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()] != null)
            {
                LotoGlarfEntity lotoGlarf = new LotoGlarfEntity(Int32.Parse(this.requestor.Split('#')[1]), user);
                if ((user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].id || user.id == this.listUserInLOTO[userInLOTO.SUPERVISOR.ToString()].employee_delegate) && lotoGlarf.status == (int)LotoGlarfEntity.GlarfStatus.CANCELLATIONSIGNCOMPLETE && this.status < (int)LOTOStatus.CANCELSPV)
                {
                    return true;
                }
            }

            return false;
        }

        public bool isCanCancelFO(UserEntity user)
        {
            ListUser listUser = new ListUser(user.token, user.id);
            List<UserEntity> listHWFO = listUser.GetHotWorkFO();

            if (listHWFO.Exists(p => p.id == user.id) && this.status == (int)LOTOStatus.CANCELSPV)
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

            return false;
        }

        public List<LotoEntity> listLoto(UserEntity user)
        {
            List<int> listId = this.db.loto_permit.Where(p => (p.status == (int)LOTOStatus.FOSIGN || p.status == (int)LOTOStatus.COMINGHOLDERSIGN) && p.loto_glarf.Count > 0).Select(p => p.id).ToList();
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

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Facility Owner Set Agreed LOTO Point and Applied");

            return retVal;
        }

        public int sendEmailInspected(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Inspection and Verification");

            return retVal;
        }

        public int sendEmailSupervisorApprove(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Supervisor Approval");

            return retVal;
        }

        public int sendEmailFOApprove(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Facility Owner Approval");

            return retVal;
        }

        public int sendEmailOnComingHolderApprove(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Onccoming Holder Approved");

            return retVal;
        }

        public int sendEmailComingHolderRequestChange(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Oncoming Holder Request Change");

            return retVal;
        }

        public int sendEmailOtherHolderApproveChange(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Others Holder Approval");

            return retVal;
        }

        public int sendEmailFOAgreedChange(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Others Holder Approval");

            return retVal;
        }

        public int sendEmailInspectionChange(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Holders Approval");

            return retVal;
        }

        public int sendEmailSpvChangeApproval(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Supervisor Approval");

            return retVal;
        }

        public int sendEmailFOChangeApproval(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Facility Owner Approval");

            return retVal;
        }

        public int sendEmailOncomingHolderChangeApproval(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Oncoming Holder Change Approval");

            return retVal;
        }

        public int sendEmailLotoCancelByOncomingHolder(string url, UserEntity user)
        {
            int retVal = 0;

            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fo.email);
            s.Add("septu.jamasoka@gmail.com"); // email FO
            string message = url + "Home?p=Loto/edit/" + this.id;

            sendEmail.Send(s, message, "LOTO Permit Oncoming Holder Cancelled");

            return retVal;
        }

        #endregion
    }
}