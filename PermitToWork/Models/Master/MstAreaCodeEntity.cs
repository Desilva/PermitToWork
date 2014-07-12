using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using PermitToWork.Utilities;

namespace PermitToWork.Models.Master
{
    public class MstAreaCodeEntity : mst_area_code
    {
        private star_energy_ptwEntities db;

        public MstAreaCodeEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstAreaCodeEntity(int id) : this()
        {
            mst_area_code area_code = this.db.mst_area_code.Find(id);
            ModelUtilization.Clone(area_code, this);
        }

        public MstAreaCodeEntity(string areaCode)
            : this()
        {
            mst_area_code area_code = this.db.mst_area_code.Where(p => p.area_code == areaCode).FirstOrDefault();
            ModelUtilization.Clone(area_code, this);
        }

        public MstAreaCodeEntity(mst_area_code area_code)
            : this()
        {
            ModelUtilization.Clone(area_code, this);
        }

        public List<MstAreaCodeEntity> getListAreaCode()
        {
            var list = this.db.mst_area_code.ToList();
            List<MstAreaCodeEntity> ret = new List<MstAreaCodeEntity>();
            foreach (mst_area_code i in list)
            {
                ret.Add(new MstAreaCodeEntity(i));
            }

            return ret;
        }

        public int addAreaCode()
        {
            mst_area_code areaCode = new mst_area_code();
            ModelUtilization.Clone(this, areaCode);

            this.db.mst_area_code.Add(areaCode);
            int retVal = this.db.SaveChanges();

            this.id = areaCode.id;

            return retVal;
        }

        public int editAreaCode()
        {
            mst_area_code areaCode = this.db.mst_area_code.Find(id);
            ModelUtilization.Clone(this, areaCode);

            this.db.Entry(areaCode).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteAreaCode()
        {
            mst_area_code areaCode = this.db.mst_area_code.Find(id);
            if (areaCode == null)
            {
                return 404;
            }
            this.db.mst_area_code.Remove(areaCode);

            return this.db.SaveChanges();
        }
    }
}