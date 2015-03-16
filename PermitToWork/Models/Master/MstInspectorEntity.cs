using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstInspectorEntity : mst_inspector
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstInspectorEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstInspectorEntity(int id, UserEntity user) : this()
        {
            mst_inspector inspector = this.db.mst_inspector.Find(id);
            ModelUtilization.Clone(inspector, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstInspectorEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            mst_inspector inspector = this.db.mst_inspector.Find(id);
            ModelUtilization.Clone(inspector, this);

            this.user = listUser.listUser.Find(p => p.id == this.id_employee.Value);
        }

        public MstInspectorEntity(UserEntity user)
            : this()
        {
            mst_inspector inspector = this.db.mst_inspector.Where(p => p.id_employee == user.id).FirstOrDefault();
            ModelUtilization.Clone(inspector, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstInspectorEntity(mst_inspector inspector, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(inspector, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstInspectorEntity(mst_inspector inspector, UserEntity user, ListUser listUser)
            : this()
        {
            ModelUtilization.Clone(inspector, this);

            this.user = listUser.listUser.Find(p => p.id == this.id_employee.Value);
        }

        public List<MstInspectorEntity> getListInspector(UserEntity user)
        {
            var list = this.db.mst_inspector.ToList();
            List<MstInspectorEntity> ret = new List<MstInspectorEntity>();
            foreach (mst_inspector i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstInspectorEntity(i, user));
                }
            }

            return ret;
        }

        public List<MstInspectorEntity> getListInspector(UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_inspector.ToList();
            List<MstInspectorEntity> ret = new List<MstInspectorEntity>();
            foreach (mst_inspector i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstInspectorEntity(i, user, listUser));
                }
            }

            return ret;
        }

        public int addInspector()
        {
            mst_inspector inspector = new mst_inspector();
            ModelUtilization.Clone(this, inspector);

            this.db.mst_inspector.Add(inspector);
            int retVal = this.db.SaveChanges();

            this.id = inspector.id;

            return retVal;
        }

        public int editInspector()
        {
            mst_inspector inspector = this.db.mst_inspector.Find(id);
            ModelUtilization.Clone(this, inspector);

            this.db.Entry(inspector).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteInspector()
        {
            mst_inspector inspector = this.db.mst_inspector.Find(id);
            if (inspector == null)
            {
                return 404;
            }
            this.db.mst_inspector.Remove(inspector);

            return this.db.SaveChanges();
        }
    }
}