using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstRadiationPOEntity : mst_radiation_po
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstRadiationPOEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstRadiationPOEntity(int id, UserEntity user) : this()
        {
            mst_radiation_po radiationPO = this.db.mst_radiation_po.Find(id);
            ModelUtilization.Clone(radiationPO, this);

            this.user = new UserEntity(Int32.Parse(this.employee), user.token, user);
        }

        public MstRadiationPOEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            mst_radiation_po radiationPO = this.db.mst_radiation_po.Find(id);
            ModelUtilization.Clone(radiationPO, this);

            this.user = listUser.listUser.Find(p => p.id == Int32.Parse(this.employee));
        }

        public MstRadiationPOEntity(mst_radiation_po radiationPO, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(radiationPO, this);

            this.user = new UserEntity(Int32.Parse(this.employee), user.token, user);
        }

        public MstRadiationPOEntity(mst_radiation_po radiationPO, UserEntity user, ListUser listUser)
            : this()
        {
            ModelUtilization.Clone(radiationPO, this);

            this.user = listUser.listUser.Find(p => p.id == Int32.Parse(this.employee));
        }

        public List<MstRadiationPOEntity> getListRadiationPO(UserEntity user)
        {
            var list = this.db.mst_radiation_po.ToList();
            List<MstRadiationPOEntity> ret = new List<MstRadiationPOEntity>();
            foreach (mst_radiation_po i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstRadiationPOEntity(i, user));
                }
            }

            return ret;
        }

        public List<MstRadiationPOEntity> getListRadiationPO(UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_radiation_po.ToList();
            List<MstRadiationPOEntity> ret = new List<MstRadiationPOEntity>();
            foreach (mst_radiation_po i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstRadiationPOEntity(i, user, listUser));
                }
            }

            return ret;
        }

        public int addRadiationPO()
        {
            mst_radiation_po radiationPO = new mst_radiation_po();
            ModelUtilization.Clone(this, radiationPO);

            this.db.mst_radiation_po.Add(radiationPO);
            int retVal = this.db.SaveChanges();

            this.id = radiationPO.id;

            return retVal;
        }

        public int editRadiationPO()
        {
            mst_radiation_po radiationPO = this.db.mst_radiation_po.Find(id);
            ModelUtilization.Clone(this, radiationPO);

            this.db.Entry(radiationPO).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteRadiationPO()
        {
            mst_radiation_po radiationPO = this.db.mst_radiation_po.Find(id);
            if (radiationPO == null)
            {
                return 404;
            }
            this.db.mst_radiation_po.Remove(radiationPO);

            return this.db.SaveChanges();
        }
    }
}