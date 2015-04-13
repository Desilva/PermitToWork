using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstDisposalLocationEntity : mst_ex_disposal_location
    {
        public override ICollection<excavation_disposal_location> excavation_disposal_location { get { return null; } set { } }
        private star_energy_ptwEntities db;

        public MstDisposalLocationEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstDisposalLocationEntity(int id) : this()
        {
            mst_ex_disposal_location disposalLocation = this.db.mst_ex_disposal_location.Find(id);
            ModelUtilization.Clone(disposalLocation, this);
        }

        public MstDisposalLocationEntity(mst_ex_disposal_location disposalLocation)
            : this()
        {
            ModelUtilization.Clone(disposalLocation, this);
        }

        public List<MstDisposalLocationEntity> getListDisposalLocation()
        {
            var list = this.db.mst_ex_disposal_location.Where(p => p.is_delete == false).ToList();
            List<MstDisposalLocationEntity> ret = new List<MstDisposalLocationEntity>();
            foreach (mst_ex_disposal_location i in list)
            {
                ret.Add(new MstDisposalLocationEntity(i));
            }

            return ret;
        }

        public int add()
        {
            mst_ex_disposal_location disposalLocation = new mst_ex_disposal_location();
            ModelUtilization.Clone(this, disposalLocation);

            this.db.mst_ex_disposal_location.Add(disposalLocation);
            int retVal = this.db.SaveChanges();

            this.id = disposalLocation.id;

            return retVal;
        }

        public int edit()
        {
            mst_ex_disposal_location disposalLocation = this.db.mst_ex_disposal_location.Find(id);
            ModelUtilization.Clone(this, disposalLocation);

            this.db.Entry(disposalLocation).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int delete()
        {
            mst_ex_disposal_location disposalLocation = this.db.mst_ex_disposal_location.Find(id);
            disposalLocation.is_delete = true;

            this.db.Entry(disposalLocation).State = EntityState.Modified;
            return this.db.SaveChanges();
        }
    }
}