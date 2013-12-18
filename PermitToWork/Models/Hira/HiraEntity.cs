using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Hira
{
    public class HiraEntity
    {
        public int id { get; set; }
        public string location { get; set; }
        public string filename { get; set; }
        public Nullable<int> id_ptw { get; set; }

        private star_energy_ptwEntities db;

        public HiraEntity() {
            this.db = new star_energy_ptwEntities();
        }

        public HiraEntity(string location, string filename, int? id_ptw = null)
        {
            this.db = new star_energy_ptwEntities();
            this.location = location;
            this.filename = filename;
            this.id_ptw = id_ptw;
        }

        public HiraEntity(int id, star_energy_ptwEntities db)
        {
            this.db = db;
            hira_document hira = db.hira_document.Find(id);
            this.id = hira.id;
            this.location = hira.location;
            this.filename = hira.filename;
            this.id_ptw = hira.id_ptw;
        }

        public HiraEntity(string fileName)
        {
            this.db = new star_energy_ptwEntities();
            hira_document hira = db.hira_document.Where(p => p.filename == fileName).FirstOrDefault();
            this.id = hira.id;
            this.location = hira.location;
            this.filename = hira.filename;
            this.id_ptw = hira.id_ptw;
        }

        public int addHiraDocument()
        {
            hira_document hira = new hira_document
            {
                location = location,
                filename = filename,
                id_ptw = id_ptw
            };

            db.hira_document.Add(hira);
            return db.SaveChanges();
        }

        public int editHiraDocument()
        {
            hira_document hira = this.db.hira_document.Find(this.id);
            hira.location = location;
            hira.filename = filename;
            hira.id_ptw = id_ptw;

            this.db.Entry(hira).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deleteHiraDocument()
        {
            hira_document hira = this.db.hira_document.Find(this.id);
            this.db.hira_document.Remove(hira);
            return this.db.SaveChanges();
        }

        public int changeIdPtw(int id_ptw)
        {
            this.id_ptw = id_ptw;
            hira_document hira = this.db.hira_document.Find(this.id);
            hira.id_ptw = this.id_ptw;
            this.db.Entry(hira).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }
    }
}