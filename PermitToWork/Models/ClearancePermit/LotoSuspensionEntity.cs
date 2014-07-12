using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoSuspensionEntity : loto_suspension
    {
        public override loto_permit loto_permit { get { return null; } set {} }
        public override ICollection<loto_suspension_holder> loto_suspension_holder { get { return null; } set { } }

        public UserEntity requestorUser { get; set; }
        public UserEntity foUser { get; set; }
        public UserEntity foDelegateUser { get; set; }
        public List<string> otherHolder { get; set; }

        public List<LotoSuspensionHolderEntity> suspensionHolder { get; set; }

        private star_energy_ptwEntities db;

        public enum SuspensionStatus
        {
            SUSPENDED,
            EDITANDSEND,
            SUSPENSIONAPPROVE,
            SUSPENSIONFOAPPROVE,
            SUSPENSIONINSPECTION,
            SUSPENSIONAPPROVED,
            SUSPENDCOMPLETE,
            SUSPENSIONCOMPLETESEND,
            SUSPENSIONCOMPLETEAGREED,
            SUSPENSIONCOMPLETEINSPECTED,
            SUSPENSIONCOMPLETED
        }

        public LotoSuspensionEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public LotoSuspensionEntity(int id, UserEntity user) : this()
        {
            loto_suspension lotoSuspension = this.db.loto_suspension.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(lotoSuspension, this);

            if (this.requestor != null)
            {
                this.requestorUser = new UserEntity(Int32.Parse(this.requestor), user.token, user);
            }

            if (this.facility_owner != null)
            {
                this.foUser = new UserEntity(Int32.Parse(this.facility_owner), user.token, user);
            }

            if (this.fo_delegate != null)
            {
                this.foUser = new UserEntity(Int32.Parse(this.fo_delegate), user.token, user);
            }

            this.suspensionHolder = new LotoSuspensionHolderEntity().getList(user, this.id);
        }

        public LotoSuspensionEntity(int id_loto, string requestor, int suspension_no, List<string> otherHolder, string fo_id)
            : this()
        {
            this.id_loto = id_loto;
            this.requestor = requestor;
            this.otherHolder = otherHolder;
            this.facility_owner = fo_id;
            this.status = (int)SuspensionStatus.SUSPENDED;
        }

        public int create()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = new loto_suspension();
            ModelUtilization.Clone(this, lotoSuspension);
            this.db.loto_suspension.Add(lotoSuspension);
            retVal = this.db.SaveChanges();

            this.id = lotoSuspension.id;

            // to-do: add loto suspension other holder
            int i = 2;
            foreach (string s in this.otherHolder)
            {
                LotoSuspensionHolderEntity suspensionHolder = new LotoSuspensionHolderEntity(this.id, i, s);
                i++;
                suspensionHolder.create();
            }

            return retVal;
        }

        public List<LotoSuspensionEntity> getList(UserEntity user, int id_loto)
        {
            List<int> listId = this.db.loto_suspension.Where(p => p.id_loto == id_loto).Select(p => p.id).ToList();
            List<LotoSuspensionEntity> result = new List<LotoSuspensionEntity>();
            foreach (int id in listId)
            {
                result.Add(new LotoSuspensionEntity(id, user));
            }
            return result.OrderBy(p => p.suspend_no).ToList();
        }



        internal int sendApprove()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.notes = this.notes;
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int foToHolderInspection()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.notes = this.notes;
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        internal int approveFO(UserEntity user)
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.notes = this.notes;
                lotoSuspension.status = this.status;
                lotoSuspension.fo_signature = this.fo_signature;
                if (user.id != foUser.id)
                {
                    lotoSuspension.fo_delegate = user.id.ToString();
                }

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int approveInspection()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.notes = this.notes;
                lotoSuspension.status = this.status;
                lotoSuspension.requestor_signature = this.requestor_signature;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        internal int agreeSuspension(UserEntity user)
        {
            int retVal = 0;
            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                bool complete = true;
                foreach (LotoSuspensionHolderEntity suspensionHolder in this.suspensionHolder)
                {
                    if (user.id == suspensionHolder.userEntity.id)
                    {
                        retVal = suspensionHolder.agree();
                    }
                    else
                    {
                        complete = complete && suspensionHolder.isAgree();
                    }
                }

                if (complete)
                {
                    lotoSuspension.status = (int)SuspensionStatus.SUSPENSIONAPPROVE;

                    this.db.Entry(lotoSuspension).State = EntityState.Modified;
                    retVal = this.db.SaveChanges();
                }
            }
            return retVal;
        }

        internal int completeSuspension()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                foreach (LotoSuspensionHolderEntity suspensionHolder in this.suspensionHolder)
                {
                    retVal = suspensionHolder.removeAgree();
                }
            }
            return retVal;
        }

        internal int sendCompleteAgreement()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                foreach (LotoSuspensionHolderEntity suspensionHolder in this.suspensionHolder)
                {
                    retVal = suspensionHolder.removeAgree();
                }
            }
            return retVal;
        }

        internal int completedSuspension()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();

                foreach (LotoSuspensionHolderEntity suspensionHolder in this.suspensionHolder)
                {
                    retVal = suspensionHolder.removeAgree();
                }
            }
            return retVal;
        }

        internal int agreeCompleteSuspension(UserEntity user, int id_loto)
        {
            int retVal = 0;
            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                bool complete = true;
                foreach (LotoSuspensionHolderEntity suspensionHolder in this.suspensionHolder)
                {
                    if (user.id == suspensionHolder.userEntity.id)
                    {
                        retVal = suspensionHolder.agree();
                    }
                    else
                    {
                        complete = complete && suspensionHolder.isAgree();
                    }
                }

                if (complete)
                {
                    LotoPointEntity loto = new LotoPointEntity();
                    loto.id_loto = id_loto;
                    loto.is_set_empty = 1;
                    loto.add();

                    lotoSuspension.status = (int)SuspensionStatus.SUSPENSIONCOMPLETEAGREED;

                    this.db.Entry(lotoSuspension).State = EntityState.Modified;
                    retVal = this.db.SaveChanges();
                }
            }
            return retVal;
        }

        internal int foToHolderCompleteInspection()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.status = this.status;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        internal int approveInspectionComplete()
        {
            int retVal = 0;

            loto_suspension lotoSuspension = this.db.loto_suspension.Find(this.id);
            if (lotoSuspension != null)
            {
                lotoSuspension.status = this.status;
                lotoSuspension.can_requestor_signature = this.requestor_signature;

                this.db.Entry(lotoSuspension).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }
    }
}