using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoComingHolderEntity : loto_coming_holder
    {
        public override loto_permit loto_permit { get { return null; } set { } }
        public UserEntity userEntity { get; set; }
        public UserEntity userApprovalDelegate { get; set; }
        public UserEntity userCancellationDelegate { get; set; }

        private star_energy_ptwEntities db;

        public LotoComingHolderEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public LotoComingHolderEntity(int id, UserEntity user) : this()
        {
            loto_coming_holder LotoComingHolder = this.db.loto_coming_holder.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(LotoComingHolder, this);

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

        public LotoComingHolderEntity(int id_loto, string user, int holder_no, string user_spv)
            : this()
        {
            this.id_loto = id_loto;
            this.holder_spv = user_spv;
            this.no_holder = holder_no;
            this.holder = user;
        }

        public int create()
        {
            int retVal = 0;

            loto_coming_holder comingHolder = new loto_coming_holder();
            ModelUtilization.Clone(this, comingHolder);
            this.db.loto_coming_holder.Add(comingHolder);
            retVal = this.db.SaveChanges();

            this.id = comingHolder.id;

            return retVal;
        }

        public int signApprove(UserEntity user)
        {
            int retVal = 0;
            loto_coming_holder comingHolder = this.db.loto_coming_holder.Find(this.id);
            if (comingHolder != null)
            {
                if (user.id == this.userEntity.id)
                {
                    comingHolder.holder_sign_approval = "a" + user.signature;
                    comingHolder.holder_date_approval = DateTime.Now;
                }
                else if (user.id == this.userEntity.employee_delegate)
                {
                    comingHolder.holder_sign_approval = "d" + user.signature;
                    comingHolder.holder_date_approval = DateTime.Now;
                    comingHolder.holder_sign_delegate_approval = user.id.ToString();
                }

                this.db.Entry(comingHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int signCancellation(UserEntity user)
        {
            int retVal = 0;
            loto_coming_holder comingHolder = this.db.loto_coming_holder.Find(this.id);
            if (comingHolder != null)
            {
                if (user.id == this.userEntity.id)
                {
                    comingHolder.holder_sign_cancellation = "a" + user.signature;
                    comingHolder.holder_date_cancellation = DateTime.Now;
                }
                else if (user.id == this.userEntity.employee_delegate)
                {
                    comingHolder.holder_sign_cancellation = "d" + user.signature;
                    comingHolder.holder_date_cancellation = DateTime.Now;
                    comingHolder.holder_sign_delegate_cancellation = user.id.ToString();
                }

                this.db.Entry(comingHolder).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public List<LotoComingHolderEntity> getList(UserEntity user, int id_loto)
        {
            List<int> listId = this.db.loto_coming_holder.Where(p => p.id_loto == id_loto).Select(p => p.id).ToList();
            List<LotoComingHolderEntity> result = new List<LotoComingHolderEntity>();
            foreach (int id in listId)
            {
                result.Add(new LotoComingHolderEntity(id, user));
            }
            return result.OrderBy(p => p.no_holder).ToList();
        }

        public bool isApprove()
        {
            return this.holder_sign_approval != null;
        }

        internal bool isCancel()
        {
            return this.holder_sign_cancellation != null;
        }
    }
}