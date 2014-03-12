using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstRadiographerEntity : mst_radiographer
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstRadiographerEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstRadiographerEntity(int id, UserEntity user) : this()
        {
            mst_radiographer radiographer = this.db.mst_radiographer.Find(id);
            ModelUtilization.Clone(radiographer, this);

            this.user = new UserEntity(Int32.Parse(this.employee), user.token, user);
        }

        public MstRadiographerEntity(mst_radiographer radiographer, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(radiographer, this);

            this.user = new UserEntity(Int32.Parse(this.employee), user.token, user);
        }

        public List<MstRadiographerEntity> getListRadiographer(UserEntity user)
        {
            var list = this.db.mst_radiographer.ToList();
            List<MstRadiographerEntity> ret = new List<MstRadiographerEntity>();
            foreach (mst_radiographer i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstRadiographerEntity(i, user));
                }
            }

            return ret;
        }

        public int addRadiographer()
        {
            mst_radiographer radiographer = new mst_radiographer();
            ModelUtilization.Clone(this, radiographer);

            this.db.mst_radiographer.Add(radiographer);
            int retVal = this.db.SaveChanges();

            this.id = radiographer.id;

            return retVal;
        }

        public int editRadiographer()
        {
            mst_radiographer radiographer = this.db.mst_radiographer.Find(id);
            ModelUtilization.Clone(this, radiographer);

            this.db.Entry(radiographer).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteRadiographer()
        {
            mst_radiographer radiographer = this.db.mst_radiographer.Find(id);
            if (radiographer == null)
            {
                return 404;
            }
            this.db.mst_radiographer.Remove(radiographer);

            return this.db.SaveChanges();
        }
    }
}