using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstFacilitiesEntity : mst_facilities
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstFacilitiesEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstFacilitiesEntity(int id, UserEntity user) : this()
        {
            mst_facilities fac = this.db.mst_facilities.Find(id);
            ModelUtilization.Clone(fac, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstFacilitiesEntity(mst_facilities fac, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(fac, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public List<MstFacilitiesEntity> getListFacilities(UserEntity user)
        {
            var list = this.db.mst_facilities.ToList();
            List<MstFacilitiesEntity> ret = new List<MstFacilitiesEntity>();
            foreach (mst_facilities i in list)
            {
                ret.Add(new MstFacilitiesEntity(i, user));
            }

            return ret;
        }

        public int addFacilities()
        {
            mst_facilities fac = new mst_facilities();
            ModelUtilization.Clone(this, fac);

            this.db.mst_facilities.Add(fac);
            int retVal = this.db.SaveChanges();

            this.id = fac.id;

            return retVal;
        }

        public int editFacilities()
        {
            mst_facilities fac = this.db.mst_facilities.Find(id);
            ModelUtilization.Clone(this, fac);

            this.db.Entry(fac).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteFacilities()
        {
            mst_facilities fac = this.db.mst_facilities.Find(id);
            if (fac == null)
            {
                return 404;
            }
            this.db.mst_facilities.Remove(fac);

            return this.db.SaveChanges();
        }
    }
}