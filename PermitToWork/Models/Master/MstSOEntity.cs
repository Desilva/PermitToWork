using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstSOEntity : mst_safety_officer
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstSOEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstSOEntity(int id, UserEntity user) : this()
        {
            mst_safety_officer mst_so = this.db.mst_safety_officer.Find(id);

            ModelUtilization.Clone(mst_so, this);
            this.user = new UserEntity(this.id_so.Value, user.token, user);
        }

        public MstSOEntity(mst_safety_officer mst_so, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(mst_so, this);
            this.user = new UserEntity(this.id_so.Value, user.token, user);
        }

        public MstSOEntity(mst_safety_officer mst_so, UserEntity user, ListUser listUser)
            : this()
        {
            ModelUtilization.Clone(mst_so, this);
            this.user = listUser.listUser.Find(p => p.id == this.id_so);
        }

        public List<MstSOEntity> getListMstSO(UserEntity user)
        {
            var list = this.db.mst_safety_officer.ToList();
            List<MstSOEntity> ret = new List<MstSOEntity>();
            foreach (mst_safety_officer i in list)
            {
                ret.Add(new MstSOEntity(i, user));
            }

            return ret;
        }

        public List<MstSOEntity> getListMstSO(UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_safety_officer.ToList();
            List<MstSOEntity> ret = new List<MstSOEntity>();
            foreach (mst_safety_officer i in list)
            {
                ret.Add(new MstSOEntity(i, user, listUser));
            }

            return ret;
        }

        public int addSO()
        {
            mst_safety_officer mst_so = new mst_safety_officer();
            ModelUtilization.Clone(this, mst_so);

            this.db.mst_safety_officer.Add(mst_so);
            int retVal = this.db.SaveChanges();

            this.id = mst_so.id;

            return retVal;
        }

        public int editSO()
        {
            mst_safety_officer mst_so = this.db.mst_safety_officer.Find(this.id);
            mst_so.id_so = this.id_so;

            this.db.Entry(mst_so).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteSO()
        {
            mst_safety_officer mst_so = this.db.mst_safety_officer.Find(this.id);
            if (mst_so == null)
            {
                return 404;
            }
            this.db.mst_safety_officer.Remove(mst_so);

            return this.db.SaveChanges();
        }
    }
}