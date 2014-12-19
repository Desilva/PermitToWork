using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstNoInspectionEntity : mst_no_inspection
    {
        private star_energy_ptwEntities db;
        public override scaffolding_inspection scaffolding_inspection { get { return null; } set { } }

        public ScaffoldingInspectionModel s { get; set; }

        public UserEntity user { get; set; }

        public MstNoInspectionEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstNoInspectionEntity(int id, UserEntity user) : this()
        {
            mst_no_inspection mst_no_inspection = this.db.mst_no_inspection.Find(id);

            ModelUtilization.Clone(mst_no_inspection, this);
            s = new ScaffoldingInspectionModel(mst_no_inspection.id_inspection.Value);
            this.user = new UserEntity(Int32.Parse(this.inspector_id), user.token, user);
        }

        public MstNoInspectionEntity(mst_no_inspection mst_no_inspection, UserEntity user)
            : this()
        {
            ModelUtilization.Clone(mst_no_inspection, this);
            if (mst_no_inspection.id_inspection != null && mst_no_inspection.id_inspection != 0)
            {
                s = new ScaffoldingInspectionModel(mst_no_inspection.id_inspection.Value);
            }
            this.user = new UserEntity(Int32.Parse(this.inspector_id), user.token, user);
        }

        public List<MstNoInspectionEntity> getListMstNoInspection(UserEntity user)
        {
            var userId = user.id.ToString();
            var list = this.db.mst_no_inspection;
            List<MstNoInspectionEntity> ret = new List<MstNoInspectionEntity>();
            foreach (mst_no_inspection i in list)
            {
                ret.Add(new MstNoInspectionEntity(i, user));
            }

            return ret;
        }

        public List<MstNoInspectionEntity> getListMstNoInspectionByUser(UserEntity user)
        {
            var userId = user.id.ToString();
            var list = this.db.mst_no_inspection.Where(p => p.inspector_id == userId).ToList();
            List<MstNoInspectionEntity> ret = new List<MstNoInspectionEntity>();
            foreach (mst_no_inspection i in list)
            {
                ret.Add(new MstNoInspectionEntity(i, user));
            }

            return ret;
        }

        public string getLastNumberByUser(UserEntity user) {
            var userId = user.id.ToString();
            var list = this.db.mst_no_inspection.Where(p => p.inspector_id == userId).ToList().OrderByDescending(p => p.no_inspection != null ? p.no_inspection.Split('/').ElementAt(0) : "").ToList();
            string retVal = "000";
            if (list.Count != 0) {
                retVal = list.FirstOrDefault().no_inspection.Split('/')[0];
                int i = 0;
                if (!Int32.TryParse(retVal, out i))
                {
                    retVal = "000";
                }
            }

            return retVal;
        }

        public int add()
        {
            mst_no_inspection mst_no_inspection = new mst_no_inspection();
            ModelUtilization.Clone(this, mst_no_inspection);

            this.db.mst_no_inspection.Add(mst_no_inspection);
            int retVal = this.db.SaveChanges();

            this.id = mst_no_inspection.id;

            return retVal;
        }

        public int edit()
        {
            mst_no_inspection mst_no_inspection = this.db.mst_no_inspection.Find(this.id);
            mst_no_inspection.inspector_id = this.inspector_id;
            mst_no_inspection.no_inspection = this.no_inspection;
            mst_no_inspection.valid_date = this.valid_date;

            this.db.Entry(mst_no_inspection).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int delete()
        {
            mst_no_inspection mst_no_inspection = this.db.mst_no_inspection.Find(this.id);
            if (mst_no_inspection == null)
            {
                return 404;
            }
            this.db.mst_no_inspection.Remove(mst_no_inspection);

            return this.db.SaveChanges();
        }
    }
}