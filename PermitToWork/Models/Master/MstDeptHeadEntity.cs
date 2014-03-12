using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstDeptHeadEntity : mst_dept_head_fo
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstDeptHeadEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstDeptHeadEntity(int id, UserEntity user) : this()
        {
            mst_dept_head_fo mst_dept_head = this.db.mst_dept_head_fo.Find(id);

            ModelUtilization.Clone(mst_dept_head, this);
            this.user = new UserEntity(this.id_dept_head_fo.Value, user.token, user);
        }

        public MstDeptHeadEntity(mst_dept_head_fo mst_dept_head, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(mst_dept_head, this);
            this.user = new UserEntity(this.id_dept_head_fo.Value, user.token, user);
        }

        public List<MstDeptHeadEntity> getListMstDeptHead(UserEntity user)
        {
            var list = this.db.mst_dept_head_fo.ToList();
            List<MstDeptHeadEntity> ret = new List<MstDeptHeadEntity>();
            foreach (mst_dept_head_fo i in list)
            {
                ret.Add(new MstDeptHeadEntity(i, user));
            }

            return ret;
        }

        public int addDeptHead()
        {
            mst_dept_head_fo mst_dept_head = new mst_dept_head_fo();
            ModelUtilization.Clone(this, mst_dept_head);

            this.db.mst_dept_head_fo.Add(mst_dept_head);
            int retVal = this.db.SaveChanges();

            this.id = mst_dept_head.id;

            return retVal;
        }

        public int editDeptHead()
        {
            mst_dept_head_fo mst_dept_head = this.db.mst_dept_head_fo.Find(this.id);
            mst_dept_head.id_dept_head_fo = this.id_dept_head_fo;

            this.db.Entry(mst_dept_head).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteDeptHead()
        {
            mst_dept_head_fo mst_dept_head = this.db.mst_dept_head_fo.Find(this.id);
            if (mst_dept_head == null)
            {
                return 404;
            }
            this.db.mst_dept_head_fo.Remove(mst_dept_head);

            return this.db.SaveChanges();
        }
    }
}