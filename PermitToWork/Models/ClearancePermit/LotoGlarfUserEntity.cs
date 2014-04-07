using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class LotoGlarfUserEntity : loto_glarf_user
    {
        public override loto_glarf loto_glarf { get { return null; } set { } }
        public UserEntity userEntity { get; set; }

        private star_energy_ptwEntities db;

        public LotoGlarfUserEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public LotoGlarfUserEntity(int id, UserEntity user) : this()
        {
            loto_glarf_user lotoGlarfUser = this.db.loto_glarf_user.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(lotoGlarfUser, this);

            if (this.user != null)
            {
                this.userEntity = new UserEntity(Int32.Parse(this.user), user.token, user);
            }
        }

        public LotoGlarfUserEntity(int id_glarf, string user, byte can_edit = 1)
            : this()
        {
            this.id_glarf = id_glarf;
            this.user = user;
            this.can_edit = can_edit;
        }

        public int create()
        {
            int retVal = 0;

            loto_glarf_user glarfUser = new loto_glarf_user();
            ModelUtilization.Clone(this, glarfUser);
            this.db.loto_glarf_user.Add(glarfUser);
            retVal = this.db.SaveChanges();

            this.id = glarfUser.id;

            return retVal;
        }

        public int edit()
        {
            int retVal = 0;
            loto_glarf_user glarfUser = this.db.loto_glarf_user.Find(this.id);
            if (glarfUser != null)
            {
                glarfUser.user = this.user;

                this.db.Entry(glarfUser).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int delete()
        {
            int retVal = 0;
            loto_glarf_user glarfUser = this.db.loto_glarf_user.Find(this.id);
            if (glarfUser != null)
            {
                this.db.loto_glarf_user.Remove(glarfUser);
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int applicationSignature(UserEntity user)
        {
            int retVal = 0;
            loto_glarf_user glarfUser = this.db.loto_glarf_user.Find(this.id);
            if (glarfUser != null)
            {
                glarfUser.user_application_signature = user.signature;
                glarfUser.user_application_signature_date = DateTime.Now;

                this.db.Entry(glarfUser).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public int cancellationSignature(UserEntity user)
        {
            int retVal = 0;
            loto_glarf_user glarfUser = this.db.loto_glarf_user.Find(this.id);
            if (glarfUser != null)
            {
                glarfUser.user_cancellation_signature = user.signature;
                glarfUser.user_cancellation_signature_date = DateTime.Now;

                this.db.Entry(glarfUser).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public List<LotoGlarfUserEntity> listUserGlarf(int id_glarf, UserEntity user)
        {
            List<int> listId = this.db.loto_glarf_user.Where(p => p.id_glarf == id_glarf).Select(p => p.id).ToList();
            List<LotoGlarfUserEntity> listResult = new List<LotoGlarfUserEntity>();
            foreach (int i in listId)
            {
                listResult.Add(new LotoGlarfUserEntity(i, user));
            }

            return listResult;
        }
    }
}