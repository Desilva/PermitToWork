using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoSuspensionHolderEntity : loto_suspension_holder
    {
        public override loto_suspension loto_suspension { get { return null; } set { } }

        public UserEntity userEntity { get; set; }
        public UserEntity userApprovalDelegate { get; set; }
        public UserEntity userCancellationDelegate { get; set; }
 
        private star_energy_ptwEntities db;

        public LotoSuspensionHolderEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public LotoSuspensionHolderEntity(int id, UserEntity user) : this()
        {
            loto_suspension_holder lotoSuspensionHolder = this.db.loto_suspension_holder.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(lotoSuspensionHolder, this);

            if (this.holder_spv != null)
            {
                this.userEntity = new UserEntity(Int32.Parse(this.holder_spv), user.token, user);
            }

            if (this.holder_sign_delegate_approval != null)
            {
                this.userApprovalDelegate = new UserEntity(Int32.Parse(this.holder_sign_delegate_approval), user.token, user);
            }

            if (this.holder_sign_delegate_cancellation != null)
            {
                this.userCancellationDelegate = new UserEntity(Int32.Parse(this.holder_sign_delegate_cancellation), user.token, user);
            }
        }

        public LotoSuspensionHolderEntity(int id_suspension, int holder_no, string user_spv)
            : this()
        {
            this.id_loto_suspension = id_suspension;
            this.holder_spv = user_spv;
            this.no_holder = holder_no;
        }

        public int create()
        {
            int retVal = 0;

            loto_suspension_holder suspensionHolder = new loto_suspension_holder();
            ModelUtilization.Clone(this, suspensionHolder);
            this.db.loto_suspension_holder.Add(suspensionHolder);
            retVal = this.db.SaveChanges();

            this.id = suspensionHolder.id;

            return retVal;
        }

        public int signApprove(UserEntity user)
        {
            int retVal = 0;
            loto_suspension_holder suspensionHolder = this.db.loto_suspension_holder.Find(this.id);
            if (suspensionHolder != null)
            {
                if (user.id == this.userEntity.id)
                {
                    suspensionHolder.holder_sign_approval = "a" + user.signature;
                    suspensionHolder.holder_date_approval = DateTime.Now;
                }
                else if (user.id == this.userEntity.employee_delegate)
                {
                    suspensionHolder.holder_sign_approval = "d" + user.signature;
                    suspensionHolder.holder_date_approval = DateTime.Now;
                    suspensionHolder.holder_sign_delegate_approval = user.id.ToString();
                }

                this.db.Entry(suspensionHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int signCancellation(UserEntity user)
        {
            int retVal = 0;
            loto_suspension_holder suspensionHolder = this.db.loto_suspension_holder.Find(this.id);
            if (suspensionHolder != null)
            {
                if (user.id == this.userEntity.id)
                {
                    suspensionHolder.holder_sign_cancellation = "a" + user.signature;
                    suspensionHolder.holder_date_cancellation = DateTime.Now;
                }
                else if (user.id == this.userEntity.employee_delegate)
                {
                    suspensionHolder.holder_sign_cancellation = "d" + user.signature;
                    suspensionHolder.holder_date_cancellation = DateTime.Now;
                    suspensionHolder.holder_sign_delegate_cancellation = user.id.ToString();
                }

                this.db.Entry(suspensionHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public List<LotoSuspensionHolderEntity> getList(UserEntity user, int id_suspension)
        {
            List<int> listId = this.db.loto_suspension_holder.Where(p => p.id_loto_suspension == id_suspension).Select(p => p.id).ToList();
            List<LotoSuspensionHolderEntity> result = new List<LotoSuspensionHolderEntity>();
            foreach (int id in listId)
            {
                result.Add(new LotoSuspensionHolderEntity(id, user));
            }
            return result.OrderBy(p => p.id).ToList();
        }

        public bool isApprove()
        {
            return this.holder_sign_approval != null;
        }

        public bool isApproveComplete()
        {
            return this.holder_sign_cancellation != null;
        }

        internal bool isAgree()
        {
            return this.is_agree == 1;
        }

        internal int agree()
        {
            int retVal = 0;
            loto_suspension_holder suspensionHolder = this.db.loto_suspension_holder.Find(this.id);
            if (suspensionHolder != null)
            {
                suspensionHolder.is_agree = 1;

                this.db.Entry(suspensionHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        internal int removeAgree()
        {
            int retVal = 0;
            loto_suspension_holder suspensionHolder = this.db.loto_suspension_holder.Find(this.id);
            if (suspensionHolder != null)
            {
                suspensionHolder.is_agree = null;

                this.db.Entry(suspensionHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }
    }
}