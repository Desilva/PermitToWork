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
        private star_energy_ptwEntities db;

        public MstFOEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstFOEntity(int id)
        {
            this.db = new star_energy_ptwEntities();
            mst_facility_owner fac_owner = this.db.mst_facility_owner.Find(id);

            this.id = fac_owner.id;
            this.id_employee = fac_owner.id_employee;
            this.fo_code = fac_owner.fo_code;
        }

        public MstFOEntity(mst_facility_owner fac_owner)
        {
            this.db = new star_energy_ptwEntities();

            this.id = fac_owner.id;
            this.id_employee = fac_owner.id_employee;
            this.fo_code = fac_owner.fo_code;
        }

        public List<MstFOEntity> getListMstFO()
        {
            var list = from a in db.mst_facility_owner
                       select new MstFOEntity
                       {
                           id = a.id,
                           id_employee = a.id_employee,
                           fo_code = a.fo_code
                       };

            return list.ToList();
        }

        public int addFO()
        {
            mst_facility_owner fac_owner = new mst_facility_owner
            {
                id_employee = this.id_employee,
                fo_code = this.fo_code
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