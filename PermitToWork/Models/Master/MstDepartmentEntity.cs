using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstDepartmentEntity
    {
        public int id { get; set; }
        public string department { get; set; }
        private star_energy_ptwEntities db;

        public MstDepartmentEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstDepartmentEntity(int id)
        {
            this.db = new star_energy_ptwEntities();
            mst_department dept = this.db.mst_department.Find(id);

            this.id = dept.id;
            this.department = dept.department;
        }

        public MstDepartmentEntity(mst_department dept)
        {
            this.db = new star_energy_ptwEntities();

            this.id = dept.id;
            this.department = dept.department;
        }

        public List<MstDepartmentEntity> getListMstDepartment()
        {
            var list = from a in db.mst_department
                       select new MstDepartmentEntity
                       {
                           id = a.id,
                           department = a.department
                       };

            return list.ToList();
        }

        public int addDepartment()
        {
            mst_department dept = new mst_department
            {
                department = this.department
            };

            this.db.mst_department.Add(dept);
            int retVal = this.db.SaveChanges();

            this.id = dept.id;

            return retVal;
        }

        public int editDepartment()
        {
            mst_department dept = this.db.mst_department.Find(id);
            dept.department = this.department;

            this.db.Entry(dept).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteDepartment()
        {
            mst_department dept = this.db.mst_department.Find(id);
            if (dept == null)
            {
                return 404;
            }
            this.db.mst_department.Remove(dept);

            return this.db.SaveChanges();
        }
    }
}