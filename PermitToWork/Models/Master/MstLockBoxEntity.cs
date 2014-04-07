using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstLockBoxEntity : mst_lock_box
    {
        private star_energy_ptwEntities db;

        public MstLockBoxEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstLockBoxEntity(int id) : this()
        {
            mst_lock_box lockBox = this.db.mst_lock_box.Find(id);
            ModelUtilization.Clone(lockBox, this);
        }

        public MstLockBoxEntity(mst_lock_box lockBox)
            : this()
        {
            ModelUtilization.Clone(lockBox, this);
        }

        public List<MstLockBoxEntity> getListLockBox()
        {
            var list = this.db.mst_lock_box.ToList();
            List<MstLockBoxEntity> ret = new List<MstLockBoxEntity>();
            foreach (mst_lock_box i in list)
            {
                ret.Add(new MstLockBoxEntity(i));
            }

            return ret;
        }

        public int add()
        {
            mst_lock_box lockBox = new mst_lock_box();
            ModelUtilization.Clone(this, lockBox);

            this.db.mst_lock_box.Add(lockBox);
            int retVal = this.db.SaveChanges();

            this.id = lockBox.id;

            return retVal;
        }

        public int edit()
        {
            mst_lock_box lockBox = this.db.mst_lock_box.Find(id);
            ModelUtilization.Clone(this, lockBox);

            this.db.Entry(lockBox).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int delete()
        {
            mst_lock_box lockBox = this.db.mst_lock_box.Find(id);
            if (lockBox == null)
            {
                return 404;
            }
            this.db.mst_lock_box.Remove(lockBox);

            return this.db.SaveChanges();
        }
    }
}