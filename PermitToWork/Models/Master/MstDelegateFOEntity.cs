using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PermitToWork.Models.User;
using PermitToWork.Utilities;

namespace PermitToWork.Models.Master
{
    public class MstDelegateFOEntity : mst_delegate_fo
    {
        public override mst_facility_owner mst_facility_owner { get { return null; } set { } }
        private star_energy_ptwEntities db;
        public MstFOEntity mst_fo { get; set; }
        public UserEntity user { get; set; }

        public MstDelegateFOEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstDelegateFOEntity(int id, UserEntity user) : this()
        {
            mst_delegate_fo delegateFo = this.db.mst_delegate_fo.Find(id);
            mst_fo = new MstFOEntity(delegateFo.id_mst_fo.Value, user);
            this.user = new UserEntity(delegateFo.user_delegate_id.Value, user.token, user);
            ModelUtilization.Clone(delegateFo, this);
        }

        public MstDelegateFOEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            mst_delegate_fo delegateFo = this.db.mst_delegate_fo.Find(id);
            mst_fo = new MstFOEntity(delegateFo.id_mst_fo.Value, user, listUser);
            this.user =  listUser.listUser.Find(p => p.id == delegateFo.user_delegate_id.Value);
            ModelUtilization.Clone(delegateFo, this);
        }

        public MstDelegateFOEntity(int id, UserEntity user, ListUser listUser, MstFOEntity mstFo)
            : this()
        {
            mst_delegate_fo delegateFo = this.db.mst_delegate_fo.Find(id);
            mst_fo = mstFo;
            this.user = listUser.listUser.Find(p => p.id == delegateFo.user_delegate_id.Value);
            ModelUtilization.Clone(delegateFo, this);
        }

        //public MstDelegateFOEntity(string areaCode)
        //    : this()
        //{
        //    mst_area_code area_code = this.db.mst_area_code.Where(p => p.area_code == areaCode).FirstOrDefault();
        //    ModelUtilization.Clone(area_code, this);
        //}

        public MstDelegateFOEntity(mst_delegate_fo delegateFo, UserEntity user)
            : this()
        {
            mst_fo = new MstFOEntity(delegateFo.id_mst_fo.Value, user);
            this.user = new UserEntity(delegateFo.user_delegate_id.Value, user.token, user);
            ModelUtilization.Clone(delegateFo, this);
        }

        public MstDelegateFOEntity(mst_delegate_fo delegateFo, UserEntity user, ListUser listUser)
            : this()
        {
            mst_fo = new MstFOEntity(delegateFo.id_mst_fo.Value, user, listUser);
            this.user = listUser.listUser.Find(p => p.id == delegateFo.user_delegate_id.Value);
            ModelUtilization.Clone(delegateFo, this);
        }

        public MstDelegateFOEntity(mst_delegate_fo delegateFo, UserEntity user, ListUser listUser, MstFOEntity mstFo)
            : this()
        {
            mst_fo = new MstFOEntity(delegateFo.id_mst_fo.Value, user, listUser);
            this.user = listUser.listUser.Find(p => p.id == delegateFo.user_delegate_id.Value);
            ModelUtilization.Clone(delegateFo, this);
        }

        public List<MstDelegateFOEntity> getList(UserEntity user)
        {
            var list = this.db.mst_delegate_fo.ToList();
            List<MstDelegateFOEntity> ret = new List<MstDelegateFOEntity>();
            foreach (mst_delegate_fo i in list)
            {
                ret.Add(new MstDelegateFOEntity(i, user));
            }

            return ret;
        }

        public List<MstDelegateFOEntity> getListByFO(int fo_id, UserEntity user)
        {
            var list = this.db.mst_delegate_fo.ToList();
            List<MstDelegateFOEntity> ret = new List<MstDelegateFOEntity>();
            foreach (mst_delegate_fo i in list)
            {
                MstDelegateFOEntity mstDelFo = new MstDelegateFOEntity(i, user);
                if (mstDelFo.mst_fo.id_employee == fo_id)
                {
                    ret.Add(mstDelFo);
                }
            }

            return ret;
        }

        public List<MstDelegateFOEntity> getListByFO(int fo_id, UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_delegate_fo.Where(p => p.mst_facility_owner.id_employee == fo_id).ToList();
            List<MstDelegateFOEntity> ret = new List<MstDelegateFOEntity>();
            foreach (mst_delegate_fo i in list)
            {
                MstDelegateFOEntity mstDelFo = listUser == null ? new MstDelegateFOEntity(i, user) : new MstDelegateFOEntity(i, user, listUser);
                ret.Add(mstDelFo);
            }

            return ret;
        }

        public List<MstDelegateFOEntity> getListByFO(MstFOEntity fo, UserEntity user, ListUser listUser = null)
        {
            var list = this.db.mst_delegate_fo.ToList();
            List<MstDelegateFOEntity> ret = new List<MstDelegateFOEntity>();
            foreach (mst_delegate_fo i in list)
            {
                MstDelegateFOEntity mstDelFo = new MstDelegateFOEntity(i, user, listUser, fo);
                if (mstDelFo.mst_fo.id_employee == fo.id_employee)
                {
                    ret.Add(mstDelFo);
                }
            }

            return ret;
        }

        public int add()
        {
            mst_delegate_fo delegateFO = new mst_delegate_fo();
            ModelUtilization.Clone(this, delegateFO);

            this.db.mst_delegate_fo.Add(delegateFO);
            int retVal = this.db.SaveChanges();

            this.id = delegateFO.id;

            return retVal;
        }

        public int edit()
        {
            mst_delegate_fo delegateFO = this.db.mst_delegate_fo.Find(id);
            ModelUtilization.Clone(this, delegateFO);

            this.db.Entry(delegateFO).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int delete()
        {
            mst_delegate_fo delegateFO = this.db.mst_delegate_fo.Find(id);
            if (delegateFO == null)
            {
                return 404;
            }
            this.db.mst_delegate_fo.Remove(delegateFO);

            return this.db.SaveChanges();
        }
    }
}