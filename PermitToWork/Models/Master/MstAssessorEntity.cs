using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstAssessorEntity
    {
        public int id { get; set; }
        public Nullable<int> id_employee { get; set; }
        public string assessor_code { get; set; }
        private star_energy_ptwEntities db;

        public MstAssessorEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstAssessorEntity(int id)
        {
            this.db = new star_energy_ptwEntities();
            mst_assessor assessor = this.db.mst_assessor.Find(id);

            this.id = assessor.id;
            this.id_employee = assessor.id_employee;
            this.assessor_code = assessor.assessor_code;
        }

        public MstAssessorEntity(mst_assessor assessor)
        {
            this.db = new star_energy_ptwEntities();

            this.id = assessor.id;
            this.id_employee = assessor.id_employee;
            this.assessor_code = assessor.assessor_code;
        }

        public List<MstAssessorEntity> getListAssessor()
        {
            var list = from a in db.mst_assessor
                       select new MstAssessorEntity
                       {
                           id = a.id,
                           id_employee = a.id_employee,
                           assessor_code = a.assessor_code
                       };

            return list.ToList();
        }

        public int addAssessor()
        {
            mst_assessor assessor = new mst_assessor
            {
                id_employee = this.id_employee,
                assessor_code = this.assessor_code
            };

            this.db.mst_assessor.Add(assessor);
            int retVal = this.db.SaveChanges();

            this.id = assessor.id;

            return retVal;
        }

        public int editAssessor()
        {
            mst_assessor assessor = this.db.mst_assessor.Find(id);
            assessor.id_employee = this.id_employee;
            assessor.assessor_code = this.assessor_code;

            this.db.Entry(assessor).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteAssessor()
        {
            mst_assessor assessor = this.db.mst_assessor.Find(id);
            if (assessor == null)
            {
                return 404;
            }
            this.db.mst_assessor.Remove(assessor);

            return this.db.SaveChanges();
        }
    }
}