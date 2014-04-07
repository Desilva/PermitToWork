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
    public class LotoGlarfEntity : loto_glarf
    {
        public override loto_permit loto_permit { get { return null; } set { } }
        public override ICollection<loto_glarf_user> loto_glarf_user { get { return null; } set { } }
        public override ICollection<permit_to_work> permit_to_work { get { return null; } set { } }

        public LotoEntity lotoPermit { get; set; }
        public List<LotoGlarfUserEntity> listGlarfUser { get; set; }

        public PtwEntity ptw { get; set; }
        public int id_ptw { get; set; }

        public UserEntity requestorUser { get; set; }
        public UserEntity supervisorUser { get; set; }

        private star_energy_ptwEntities db;

        public enum GlarfStatus
        {
            CREATE,
            SIGNING,
            SIGNCOMPLETE,
            REQUESTORSIGN,
            SPVSIGN,
            CANCELLATIONSIGN,
            CANCELLATIONSIGNCOMPLETE
        }

        public LotoGlarfEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public LotoGlarfEntity(int id, UserEntity user)
            : this()
        {
            loto_glarf glarf = this.db.loto_glarf.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(glarf, this);

            id_ptw = glarf.permit_to_work.ElementAt(0).id;

            listGlarfUser = new LotoGlarfUserEntity().listUserGlarf(this.id, user);

            if (this.requestor != null)
            {
                this.requestorUser = new UserEntity(Int32.Parse(this.requestor), user.token, user);
            }

            if (this.supervisor != null)
            {
                this.supervisorUser = new UserEntity(Int32.Parse(this.supervisor), user.token, user);
            }
        }

        public LotoGlarfEntity(string requestor, string supervisor) : this()
        {
            this.supervisor = supervisor;
            this.requestor = requestor;
            this.status = (int)GlarfStatus.CREATE;
        }

        public int create()
        {
            int retVal = 0;

            loto_glarf glarf = new loto_glarf();
            ModelUtilization.Clone(this, glarf);
            this.db.loto_glarf.Add(glarf);
            retVal = this.db.SaveChanges();

            this.id = glarf.id;

            LotoGlarfUserEntity glarfSpv = new LotoGlarfUserEntity(this.id, this.supervisor, 0);
            glarfSpv.create();
            glarfSpv = new LotoGlarfUserEntity(this.id, this.requestor, 0);
            glarfSpv.create();

            return retVal;
        }

        public int edit()
        {
            return 0;
        }

        public int delete()
        {
            return 0;
        }

        public int assignLotoForm(int id_loto, string loto_no)
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.id_loto_permit = id_loto;
                glarf.loto_no = loto_no;

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int sendToSign()
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.status = (int)GlarfStatus.SIGNING;

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int signComplete()
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                bool isComplete = true;
                foreach (LotoGlarfUserEntity lg in listGlarfUser)
                {
                    if (lg.user_application_signature == null)
                    {
                        isComplete = false;
                    }
                }
                if (isComplete)
                {
                    glarf.status = (int)GlarfStatus.SIGNCOMPLETE;
                }

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int cancellationSignComplete()
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                bool isComplete = true;
                foreach (LotoGlarfUserEntity lg in listGlarfUser)
                {
                    if (lg.user_cancellation_signature == null)
                    {
                        isComplete = false;
                    }
                }
                if (isComplete)
                {
                    glarf.status = (int)GlarfStatus.CANCELLATIONSIGNCOMPLETE;
                }

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int signRequestor(UserEntity user)
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.requestor_signature = user.signature;
                glarf.status = (int)GlarfStatus.REQUESTORSIGN;

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int signSupervisor(UserEntity user)
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.supervisor_signature = user.signature;
                glarf.status = (int)GlarfStatus.SPVSIGN;

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public bool isUserInLOTO(UserEntity user)
        {
            bool isUser = false;
            this.lotoPermit = new LotoEntity(this.id_loto_permit.Value, user);
            foreach (LotoGlarfUserEntity glarfUser in listGlarfUser)
            {
                if (glarfUser.user == user.id.ToString())
                {
                    isUser = true;
                }
            }
            return isUser || this.lotoPermit.isUserInLOTO(user);
        }

        #region is Can Edit

        public bool isCanEditForm(UserEntity user)
        {
            if (user.id.ToString() == this.requestor && this.status == (int)GlarfStatus.CREATE)
            {
                return true;
            }
            return false;
        }

        public bool isCanSign(UserEntity user)
        {
            if (this.listGlarfUser.Exists(p => p.user == user.id.ToString()) && this.status == (int)GlarfStatus.SIGNING)
            {
                return true;
            }
            return false;
        }

        public bool requestorCanSign(UserEntity user)
        {
            if (user.id.ToString() == this.requestor && this.status == (int)GlarfStatus.SIGNCOMPLETE)
            {
                return true;
            }
            return false;
        }

        public bool supervisorCanSign(UserEntity user)
        {
            if (user.id.ToString() == this.supervisor && this.status == (int)GlarfStatus.REQUESTORSIGN)
            {
                return true;
            }
            return false;
        }

        public bool isCanSignCancel(UserEntity user)
        {
            if (this.listGlarfUser.Exists(p => p.user == user.id.ToString()) && this.status == (int)GlarfStatus.CANCELLATIONSIGN)
            {
                return true;
            }
            return false;
        }

        #endregion

        internal void assignSupervisor(UserEntity user)
        {
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.supervisor = user.id.ToString();

                LotoGlarfUserEntity glarfUser = this.listGlarfUser.OrderBy(p => p.id).FirstOrDefault();
                glarfUser.user = user.id.ToString();
                glarfUser.edit();

                this.db.Entry(glarf).State = EntityState.Modified;
                this.db.SaveChanges();

                loto_permit loto = this.db.loto_permit.Find(this.id_loto_permit);

                if (loto.requestor.Split('#')[0] == this.requestor && loto.supervisor == null)
                {
                    loto.supervisor = user.id.ToString();

                    this.db.Entry(loto).State = EntityState.Modified;
                    this.db.SaveChanges();
                }
            }
        }

        public int setCancel()
        {
            int retVal = 0;
            loto_glarf glarf = this.db.loto_glarf.Find(this.id);
            if (glarf != null)
            {
                glarf.status = (int)GlarfStatus.CANCELLATIONSIGN;

                this.db.Entry(glarf).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }
            return retVal;
        }

        public List<LotoGlarfEntity> listLotoGlarfWithSameLotoPermit(int loto_permit, UserEntity user)
        {
            List<int> listId = this.db.loto_glarf.Where(p => p.id_loto_permit == loto_permit).Select(p => p.id).ToList();
            List<LotoGlarfEntity> result = new List<LotoGlarfEntity>();
            foreach (int i in listId)
            {
                result.Add(new LotoGlarfEntity(i, user));
            }

            return result;
        }

        public bool isCanCancel(UserEntity user)
        {
            this.ptw = new PtwEntity(this.id_ptw, user);
            if (user.id.ToString() == this.requestor && this.status == (int)GlarfStatus.SPVSIGN && this.ptw.status == (int)PtwEntity.statusPtw.ACCFO)
            {
                return true;
            }
            return false;
        }
    }
}