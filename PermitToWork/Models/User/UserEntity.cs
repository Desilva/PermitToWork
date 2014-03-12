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
        public int? employee_dept { get; set; }

        public string token { get; set; }

        // public List<user_per_role> roles { get; set; }

        // public star_energi_geoEntities db = new star_energi_geoEntities();

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

        public UserEntity(int id, string token, UserEntity user)
        {
            WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();

            WWUserService.ResponseModel response = client.getUser(token, user.id, id);

            if (response.status)
            {
                this.id = response.result.id;
                this.alpha_name = response.result.alpha_name;
                this.employee_boss = response.result.employee_boss;
                this.employee_delegate = response.result.employee_delegate;
                this.signature = ConfigurationManager.AppSettings["fracas"] + response.result.signature;
                this.position = response.result.position;
                this.email = response.result.email;
                this.employee_dept = response.result.employee_dept;
                // this.roles = response.result.;
            }

            client.Close();
        }

        public UserEntity(string username, string password)
        {
            WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();

            WWUserService.ResponseModel response = client.login(username, password, null);

            if (response.status)
            {
                this.isSuccessLogin = true;
                this.id = response.result.id;
                this.alpha_name = response.result.alpha_name;
                this.username = username;
                this.employee_boss = response.result.employee_boss;
                this.employee_delegate = response.result.employee_delegate;
                this.signature = ConfigurationManager.AppSettings["fracas"] + response.result.signature;
                this.position = response.result.position;
                this.email = response.result.email;
                this.employee_dept = response.result.employee_dept;
                // this.roles = response.result.;

                this.token = response.message;
            }
            else
            {
                this.isSuccessLogin = false;
            }

            client.Close();
        }

        public UserEntity clone(WWUserService.UserModel cloningUser)
        {
            this.id = cloningUser.id;
            this.alpha_name = cloningUser.alpha_name;
            this.signature = cloningUser.signature;
            this.position = cloningUser.position;
            this.email = cloningUser.email;
            this.employee_delegate = cloningUser.delagate;
            this.employee_boss = cloningUser.employee_boss;
            // this.approval_level = cloningUser.approval_level;
            // this.department = cloningUser.department;

            return this;
        }
    }
}