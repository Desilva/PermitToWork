using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstFOEntity
    {
        public int id { get; set; }
        public Nullable<int> id_employee { get; set; }
        public string fo_code { get; set; }
        public string fo_name { get; set; }
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstFOEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstFOEntity(UserEntity user) : this()
        {
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Where(p => p.id_employee == user.id).FirstOrDefault();

            if (fac_owner != null)
            {
                this.id = fac_owner.id;
                this.id_employee = fac_owner.id_employee;
                this.fo_code = fac_owner.fo_code;
                this.fo_name = fac_owner.fo_name;
                this.user = new UserEntity(this.id_employee.Value, user.token, user);
            }
        }

        public MstFOEntity(int id, UserEntity user)
        {
            this.db = new star_energy_ptwEntities();
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Find(id);

            this.id = fac_owner.id;
            this.id_employee = fac_owner.id_employee;
            this.fo_code = fac_owner.fo_code;
            this.fo_name = fac_owner.fo_name;
            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstFOEntity(int id, UserEntity user, ListUser listUser)
        {
            this.db = new star_energy_ptwEntities();
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Find(id);

            this.id = fac_owner.id;
            this.id_employee = fac_owner.id_employee;
            this.fo_code = fac_owner.fo_code;
            this.fo_name = fac_owner.fo_name;
            this.user = listUser.listUser.Find(p => p.id == this.id_employee.Value);
        }

        public MstFOEntity(string fo_code, UserEntity user, ListUser listUser = null)
        {
            this.db = new star_energy_ptwEntities();
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Where(p => p.fo_code == fo_code).FirstOrDefault();

            if (fac_owner != null)
            {
                this.id = fac_owner.id;
                this.id_employee = fac_owner.id_employee;
                this.fo_code = fac_owner.fo_code;
                this.fo_name = fac_owner.fo_name;
                this.user = listUser == null ? new UserEntity(this.id_employee.Value, user.token, user) : listUser.listUser.Find(p => p.id == this.id_employee);
            }
        }

        public MstFOEntity(mst_facility_owner fac_owner)
        {
            this.db = new star_energy_ptwEntities();

            this.id = fac_owner.id;
            this.id_employee = fac_owner.id_employee;
            this.fo_code = fac_owner.fo_code;
            this.fo_name = fac_owner.fo_name;
        }

        public List<MstFOEntity> getListMstFO()
        {
            var list = from a in db.mst_facility_owner
                       select new MstFOEntity
                       {
                           fo_code = a.fo_code,
                            fo_name = a.fo_name
                       };

            return list.Distinct().ToList();
        }

        public List<MstFOEntity> getListMstFO(UserEntity user)
        {
            List<int> listId = this.db.mst_facility_owner.Select(p => p.id).ToList();
            var result = new List<MstFOEntity>();
            foreach (int i in listId)
            {
                result.Add(new MstFOEntity(i, user));
            }

            return result;
        }

        public List<MstFOEntity> getListMstFOByDept(string fo_code, UserEntity user)
        {
            List<int> listId = this.db.mst_facility_owner.Where(p => p.fo_code == fo_code).Select(p => p.id).ToList();
            var result = new List<MstFOEntity>();
            foreach (int i in listId)
            {
                result.Add(new MstFOEntity(i, user));
            }

            return result;
        }

        public int addFO()
        {
            mst_facility_owner fac_owner = new mst_facility_owner
            {
                id_employee = this.id_employee,
                fo_code = this.fo_code,
                fo_name = this.fo_name
            };

            this.db.mst_facility_owner.Add(fac_owner);
            int retVal = this.db.SaveChanges();

            this.id = fac_owner.id;

            return retVal;
        }

        public int editFO()
        {
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Find(id);
            fac_owner.id_employee = this.id_employee;
            fac_owner.fo_code = this.fo_code;
            fac_owner.fo_name = this.fo_name;

            this.db.Entry(fac_owner).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteFO()
        {
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Find(id);
            if (fac_owner == null)
            {
                return 404;
            }
            this.db.mst_facility_owner.Remove(fac_owner);

            return this.db.SaveChanges();
        }
    }
}