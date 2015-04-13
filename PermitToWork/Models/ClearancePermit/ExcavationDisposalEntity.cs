using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class ExcavationDisposalEntity : excavation_disposal_location
    {
        public override excavation excavation { get { return null; } set { } }
        public override mst_ex_disposal_location mst_ex_disposal_location { get { return null; } set { } }

        public string DisposalLocation { get; set; }

        private star_energy_ptwEntities db;

        public ExcavationDisposalEntity()
            : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public ExcavationDisposalEntity(excavation_disposal_location excavationDisposal, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(excavationDisposal, this);
            DisposalLocation = excavationDisposal.mst_ex_disposal_location.name;
        }

        //public ExcavationDisposalEntity(LotoPointEntity lotoPoint, int id_loto)
        //    : this()
        //{
        //    ModelUtilization.Clone(lotoPoint, this);
        //    this.id = 0;
        //    this.id_loto = id_loto;
        //}

        public int create()
        {
            excavation_disposal_location excavationDisposal = new excavation_disposal_location();
            ModelUtilization.Clone(this, excavationDisposal);
            this.db.excavation_disposal_location.Add(excavationDisposal);
            int retVal = this.db.SaveChanges();
            return retVal;
        }

        public int add()
        {
            excavation_disposal_location excavationDisposal = new excavation_disposal_location
            {
                id_permit = this.id_permit,
                id_disposal_location = this.id_disposal_location,
                volume = this.volume
            };

            this.db.excavation_disposal_location.Add(excavationDisposal);
            int retVal = this.db.SaveChanges();
            return retVal;
        }

        public int edit()
        {
            int retVal = 0;
            excavation_disposal_location excavationDisposal = this.db.excavation_disposal_location.Find(this.id_permit, this.id_disposal_location);
            if (excavationDisposal != null)
            {
                excavationDisposal.volume = this.volume;

                this.db.Entry(excavationDisposal).State = EntityState.Modified;
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public int delete()
        {
            int retVal = 0;
            excavation_disposal_location excavationDisposal = this.db.excavation_disposal_location.Find(this.id_permit, this.id_disposal_location);
            if (excavationDisposal != null)
            {
                this.db.excavation_disposal_location.Remove(excavationDisposal);
                retVal = this.db.SaveChanges();
            }

            return retVal;
        }

        public List<ExcavationDisposalEntity> getList(UserEntity user, int id_permit)
        {
            List<excavation_disposal_location> listDisposalLocation = this.db.excavation_disposal_location.Where(p => p.id_permit == id_permit).ToList();
            List<ExcavationDisposalEntity> result = new List<ExcavationDisposalEntity>();
            foreach (excavation_disposal_location i in listDisposalLocation)
            {
                ExcavationDisposalEntity excavationDisposal = new ExcavationDisposalEntity(i, user);
                result.Add(excavationDisposal);
            }

            return result;
        }
    }
}