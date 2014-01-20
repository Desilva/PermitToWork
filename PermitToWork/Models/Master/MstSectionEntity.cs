using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstSectionEntity
    {
        public int id { get; set; }
        public string section { get; set; }
        private star_energy_ptwEntities db;

        public MstSectionEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstSectionEntity(int id)
        {
            this.db = new star_energy_ptwEntities();
            mst_section sect = this.db.mst_section.Find(id);

            this.id = sect.id;
            this.section = sect.section;
        }

        public MstSectionEntity(mst_section sect)
        {
            this.db = new star_energy_ptwEntities();

            this.id = sect.id;
            this.section = sect.section;
        }

        public List<MstSectionEntity> getListMstSection()
        {
            var list = from a in db.mst_section
                       select new MstSectionEntity
                       {
                           id = a.id,
                           section = a.section
                       };

            return list.ToList();
        }

        public int addSection()
        {
            mst_section sect = new mst_section
            {
                section = this.section
            };

            this.db.mst_section.Add(sect);
            int retVal = this.db.SaveChanges();

            this.id = sect.id;

            return retVal;
        }

        public int editSection()
        {
            mst_section sect = this.db.mst_section.Find(id);
            sect.section = this.section;

            this.db.Entry(sect).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteSection()
        {
            mst_section sect = this.db.mst_section.Find(id);
            if (sect == null)
            {
                return 404;
            }
            this.db.mst_section.Remove(sect);

            return this.db.SaveChanges();
        }
    }
}