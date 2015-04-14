using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstEnviroOfficerEntity : mst_enviro_officer
    {
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstEnviroOfficerEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstEnviroOfficerEntity(int id, UserEntity user) : this()
        {
            mst_enviro_officer enviroOfficer = this.db.mst_enviro_officer.Find(id);
            ModelUtilization.Clone(enviroOfficer, this);

            this.user = new UserEntity(this.employee_id.Value, user.token, user);
        }

        public MstEnviroOfficerEntity(int id, UserEntity user, ListUser listUser)
            : this()
        {
            mst_enviro_officer enviroOfficer = this.db.mst_enviro_officer.Find(id);
            ModelUtilization.Clone(enviroOfficer, this);

            this.user = listUser.listUser.Find(p => p.id == this.employee_id.Value);
        }

        public MstEnviroOfficerEntity(mst_enviro_officer enviroOfficer, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(enviroOfficer, this);

            this.user = new UserEntity(this.employee_id.Value, user.token, user);
        }

        public MstEnviroOfficerEntity(mst_enviro_officer enviroOfficer, UserEntity user, ListUser listUser)
            : this()
        {
            ModelUtilization.Clone(enviroOfficer, this);

            this.user = listUser.listUser.Find(p => p.id == this.employee_id.Value);
        }

        public List<MstEnviroOfficerEntity> getListEnviroOfficer(UserEntity user, ListUser listUser)
        {
            var list = this.db.mst_enviro_officer.ToList();
            List<MstEnviroOfficerEntity> ret = new List<MstEnviroOfficerEntity>();
            foreach (mst_enviro_officer i in list)
            {
                ret.Add(new MstEnviroOfficerEntity(i, user, listUser));
            }

            return ret;
        }

        public List<MstEnviroOfficerEntity> getListEnviroOfficer(UserEntity user)
        {
            var list = this.db.mst_enviro_officer.ToList();
            List<MstEnviroOfficerEntity> ret = new List<MstEnviroOfficerEntity>();
            foreach (mst_enviro_officer i in list)
            {
                ret.Add(new MstEnviroOfficerEntity(i, user));
            }

            return ret;
        }

        public int add()
        {
            mst_enviro_officer enviroOfficer = new mst_enviro_officer();
            ModelUtilization.Clone(this, enviroOfficer);

            this.db.mst_enviro_officer.Add(enviroOfficer);
            int retVal = this.db.SaveChanges();

            this.id = enviroOfficer.id;

            return retVal;
        }

        public int edit()
        {
            mst_enviro_officer enviroOfficer = this.db.mst_enviro_officer.Find(id);
            ModelUtilization.Clone(this, enviroOfficer);

            this.db.Entry(enviroOfficer).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int delete()
        {
            mst_enviro_officer enviroOfficer = this.db.mst_enviro_officer.Find(id);
            if (enviroOfficer == null)
            {
                return 404;
            }
            this.db.mst_enviro_officer.Remove(enviroOfficer);

            return this.db.SaveChanges();
        }
    }
}