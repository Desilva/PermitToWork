using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermitToWork.Models.User;
using PermitToWork.Utilities;

namespace PermitToWork.Models.Master
{
    public class ScaffoldingInspectionModel : scaffolding_inspection
    {
        private star_energy_ptwEntities db;
        public override ICollection<mst_no_inspection> mst_no_inspection { get { return null; } set { } }

        public ScaffoldingInspectionModel() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        public ScaffoldingInspectionModel(int id)
            : this()
        {
            scaffolding_inspection scaffolding_inspection = this.db.scaffolding_inspection.Find(id);

            ModelUtilization.Clone(scaffolding_inspection, this);
        }
    }
}