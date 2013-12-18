using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.User
{
    public class UserEntity
    {
        public int id { get; set; }
        public string alpha_name { get; set; }
        public string username { get; set; }
        public bool isSuccessLogin { get; set; }
        public int? employee_boss { get; set; }
        public int? employee_delegate { get; set; }
        public string signature { get; set; }
        public string position { get; set; }
        public string email { get; set; }

        public List<user_per_role> roles { get; set; }

        public star_energi_geoEntities db = new star_energi_geoEntities();

        public enum role
        {
            ADMIN,
            FRACAS,
            RCA,
            RCAVIEW,
            FULLPIR,
            PIRINITIATOR,
            PIRPROCESS,
            AUDITOR,
            IIR,
            INITIATORIR,
            ADMINSHEOBSERVATION,
            ADMINMASTERSHE,
            MEDIC,
            DAILYLOG,
            DAILYLOGWEEKLYTARGET,
            DAILYLOGLEADER,
            DAILYLOGSUPERVISOR,
            SHEOBSERVATION,
            ENVIRONMENTAL
        };

        public UserEntity() { }

        public UserEntity(int id)
        {
            employee emp = db.employees.Find(id);
            if (emp != null)
            {
                this.id = emp.id;
                this.alpha_name = emp.alpha_name;
                this.employee_boss = emp.employee_boss;
                this.employee_delegate = emp.employee_delegate;
                this.signature = ConfigurationManager.AppSettings["fracas"] + emp.signature;
                this.position = emp.position;
                this.email = emp.email;
                this.username = this.db.users.Where(p => p.employee_id == this.id).FirstOrDefault().username;

                this.roles = this.db.user_per_role.Where(p => p.username == this.username).ToList();
            }
        }

        public UserEntity(string username, string password)
        {
            user userCheck = db.users.Find(username);
            if (userCheck == null || userCheck.password != password)
            {
                this.isSuccessLogin = false;
            }
            else
            {
                this.isSuccessLogin = true;
                employee emp = db.employees.Find(userCheck.employee_id);
                this.id = emp.id;
                this.alpha_name = emp.alpha_name;
                this.username = username;
                this.employee_boss = emp.employee_boss;
                this.employee_delegate = emp.employee_delegate;
                this.signature = ConfigurationManager.AppSettings["fracas"] + emp.signature;
                this.position = emp.position;
                this.email = emp.email;
                this.roles = this.db.user_per_role.Where(p => p.username == this.username).ToList();
            }
        }
    }
}