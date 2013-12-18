using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Hira
{
    public class ListHira
    {
        public List<HiraEntity> listHira { get; set; }
        public star_energy_ptwEntities db;
        
        public ListHira(star_energy_ptwEntities db = null)
        {
            if (db != null)
            {
                this.db = db;
            }
            else
            {
                this.db = new star_energy_ptwEntities();
            }
            var result = this.db.hira_document.Select(p => p.id).ToList();
            this.listHira = new List<HiraEntity>();
            foreach (int i in result)
            {
                this.listHira.Add(new HiraEntity(i,this.db));
            }
        }


        public ListHira(int id_ptw, star_energy_ptwEntities db = null)
        {
            if (db != null)
            {
                this.db = db;
            }
            else
            {
                this.db = new star_energy_ptwEntities();
            }
            var result = this.db.hira_document.Where(p => p.id_ptw == id_ptw).Select(p => p.id).ToList();
            this.listHira = new List<HiraEntity>();
            foreach (int i in result)
            {
                this.listHira.Add(new HiraEntity(i,this.db));
            }
        }

        public string changeIdPtw(List<string> filename, int id_ptw)
        {
            foreach (HiraEntity hira in listHira)
            {
                if (filename.Contains(hira.filename))
                {
                    hira.changeIdPtw(id_ptw);
                }
            }

            return "200";
        }
    }
}