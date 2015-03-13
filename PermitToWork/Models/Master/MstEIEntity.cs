using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstEIEntity : mst_ei
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstEIEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstEIEntity(int id, UserEntity user) : this()
        {
            mst_ei ei = this.db.mst_ei.Find(id);
            ModelUtilization.Clone(ei, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstEIEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            mst_ei ei = this.db.mst_ei.Find(id);
            ModelUtilization.Clone(ei, this);

            this.user = listUser.listUser.Find(p => p.id == this.id_employee.Value);
        }

        public MstEIEntity(mst_ei ei, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(ei, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstEIEntity(mst_ei ei, UserEntity user, ListUser listUser)
            : this()
        {
            ModelUtilization.Clone(ei, this);

            this.user = listUser.listUser.Find(p => p.id == this.id_employee.Value);
        }

        public List<MstEIEntity> getListFacilities(UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_ei.ToList();
            List<MstEIEntity> ret = new List<MstEIEntity>();
            foreach (mst_ei i in list)
            {
                ret.Add(new MstEIEntity(i, user, listUser));
            }

            return ret;
        }

        public List<MstEIEntity> getListFacilities(UserEntity user)
        {
            var list = this.db.mst_ei.ToList();
            List<MstEIEntity> ret = new List<MstEIEntity>();
            foreach (mst_ei i in list)
            {
                ret.Add(new MstEIEntity(i, user));
            }

            return ret;
        }

        public int addEI()
        {
            mst_ei ei = new mst_ei();
            ModelUtilization.Clone(this, ei);

            this.db.mst_ei.Add(ei);
            int retVal = this.db.SaveChanges();

            this.id = ei.id;

            return retVal;
        }

        public int editEI()
        {
            mst_ei ei = this.db.mst_ei.Find(id);
            ModelUtilization.Clone(this, ei);

            this.db.Entry(ei).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteEI()
        {
            mst_ei ei = this.db.mst_ei.Find(id);
            if (ei == null)
            {
                return 404;
            }
            this.db.mst_ei.Remove(ei);

            return this.db.SaveChanges();
        }
    }
}