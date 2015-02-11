using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using PermitToWork.Models.Master;

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
        public string department { get; set; }
        public string employee_no { get; set; }
        public int? approval_level { get; set; }

        public string token { get; set; }

        public List<int> roles { get; set; }

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
            ENVIRONMENTAL,
            ADMINFSR,
            IIRVIEW,
            ADMINPTW,
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
                this.department = response.result.department;
                this.employee_no = response.result.employee_no;
                this.approval_level = response.result.approval_level;
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
                this.department = response.result.department;
                this.employee_no = response.result.employee_no;
                this.roles = response.result.role.ToList();
                this.approval_level = response.result.approval_level;

                this.token = response.message;
            }
            else
            {
                this.isSuccessLogin = false;
            }

            client.Close();
        }

        public UserEntity(int id, string token)
        {
            WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();

            WWUserService.ResponseModel response = client.getUser(token, id, id);

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
                this.department = response.result.department;
                this.employee_no = response.result.employee_no;
                this.token = token;
                this.approval_level = response.result.approval_level;
                // this.roles = response.result.;
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
            this.employee_delegate = cloningUser.employee_delegate;
            this.employee_boss = cloningUser.employee_boss;
            this.department = cloningUser.department;
            this.employee_no = cloningUser.employee_no;
            this.approval_level = cloningUser.approval_level;
            // this.approval_level = cloningUser.approval_level;
            // this.department = cloningUser.department;

            return this;
        }

        /// <summary>
        /// Get List of Delegation User By FO
        /// </summary>
        /// <param name="user">FO</param>
        /// <returns></returns>
        public List<UserEntity> GetDelegateFO(UserEntity user, ListUser listUser)
        {
            List<UserEntity> result = new List<UserEntity>();
            MstFOEntity fo = new MstFOEntity(this);
            if (fo.id_employee != null)
            {
                List<MstDelegateFOEntity> listDelegate = new MstDelegateFOEntity().getListByFO(this.id, user, listUser);
                result.AddRange(listDelegate.Select(p => p.user));
            }
            return result;
        }

        public List<UserEntity> GetDelegateFO(UserEntity user)
        {
            List<UserEntity> result = new List<UserEntity>();
            MstFOEntity fo = new MstFOEntity(this);
            if (fo.id_employee != null)
            {
                List<MstDelegateFOEntity> listDelegate = new MstDelegateFOEntity().getListByFO(this.id, user);
                result.AddRange(listDelegate.Select(p => p.user));
            }
            return result;
        }
    }
}