﻿using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstErectorEntity : mst_erector
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstErectorEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstErectorEntity(int id, UserEntity user) : this()
        {
            mst_erector erector = this.db.mst_erector.Find(id);
            ModelUtilization.Clone(erector, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstErectorEntity(mst_erector erector, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(erector, this);

            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public List<MstErectorEntity> getListErector(UserEntity user)
        {
            var list = this.db.mst_erector.ToList();
            List<MstErectorEntity> ret = new List<MstErectorEntity>();
            foreach (mst_erector i in list)
            {
                if (i.valid_date.Value.CompareTo(DateTime.Now) >= 0)
                {
                    ret.Add(new MstErectorEntity(i, user));
                }
            }

            return ret;
        }

        public int addErector()
        {
            mst_erector erector = new mst_erector();
            ModelUtilization.Clone(this, erector);

            this.db.mst_erector.Add(erector);
            int retVal = this.db.SaveChanges();

            this.id = erector.id;

            return retVal;
        }

        public int editErector()
        {
            mst_erector erector = this.db.mst_erector.Find(id);
            ModelUtilization.Clone(this, erector);

            this.db.Entry(erector).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteErector()
        {
            mst_erector erector = this.db.mst_erector.Find(id);
            if (erector == null)
            {
                return 404;
            }
            this.db.mst_erector.Remove(erector);

            return this.db.SaveChanges();
        }
    }
}