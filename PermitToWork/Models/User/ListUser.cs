using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.User
{
    public class ListUser
    {
        public List<UserEntity> listUser { get; set; }
        public star_energi_geoEntities db = new star_energi_geoEntities();

        public ListUser() { }
        public ListUser(string token, int curLoginId)
        {
            DateTime lastUpdate = DateTime.Now;
            if (HttpContext.Current.Application["listUserLastUpdate"] != null) {
                lastUpdate = (HttpContext.Current.Application["listUserLastUpdate"] as DateTime?).Value;
            }
            if (HttpContext.Current.Application["listUser"] == null || DateTime.Now.Subtract(lastUpdate).TotalHours > 1) {
                WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();
                int count = 0;
                List<UserEntity> listUser = new List<UserEntity>();
                WWUserService.ResponseModel response = client.listUser(token, curLoginId, 50, 1000);

                while (response.status && count < response.results.count + 50)
                {
                    foreach (WWUserService.UserModel us in response.results.listUserModel)
                    {
                        listUser.Add(new UserEntity().clone(us));
                    }

                    response = client.listUser(token, curLoginId, 50, count);
                    count += 50;
                }

                client.Close();
                //this.listUser = new HashSet<UserEntity>(listUser.OrderBy(p => p.alpha_name));
                this.listUser = listUser.OrderBy(p => p.alpha_name).ToList();
                HttpContext.Current.Application["listUser"] = listUser.OrderBy(p => p.alpha_name).ToList();
                HttpContext.Current.Application["listUserLastUpdate"] = DateTime.Now;
            } else {
                this.listUser = ((ListUser)HttpContext.Current.Application["listUser"]).listUser;
                HttpContext.Current.Application["listUserLastUpdate"] = lastUpdate;
            }
            
        }

        public List<UserEntity> GetSupervisor(UserEntity requestor)
        {
            List<UserEntity> listSpv = new List<UserEntity>();
            // check if exist one level up
            if (requestor.employee_boss == null)
            {
                listSpv.Add(requestor);
                return listSpv;
            }

            UserEntity spv = listUser.Where(p => p.id == requestor.employee_boss).FirstOrDefault();
            bool found = false;
            while (spv != null && !found)
            {
                if (spv.approval_level == 1)
                {
                    found = true;
                    listSpv.Add(spv);
                }
                else
                {
                    spv = listUser.Where(p => p.id == spv.employee_boss).FirstOrDefault();
                }
            }

            //// get one level up
            //UserEntity userLevelOne = listUser.Where(p => p.id == requestor.employee_boss).FirstOrDefault();

            //// check if exist one level up again
            //if (userLevelOne.employee_boss == null)
            //{
            //    employee_dept ed = db.employees.Find(userLevelOne.id).employee_dept1;
            //    foreach (employee e in ed.employees)
            //    {
            //        listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
            //    }
            //    return listSpv;
            //}

            //// get one level up
            //UserEntity userLevelTwo = listUser.Where(p => p.id == userLevelOne.employee_boss).FirstOrDefault();
            //if (userLevelTwo.employee_boss == null)
            //{
            //    employee_dept ed = db.employees.Find(userLevelTwo.id).employee_dept1;
            //    foreach (employee e in ed.employees)
            //    {
            //        listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
            //    }
            //}
            //else
            //{
            //    employee ed = db.employees.Find(userLevelTwo.id).employee2;
            //    foreach (employee e in ed.employee1)
            //    {
            //        listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
            //    }
            //}
            // return the user
            return listSpv;
        }

        public List<UserEntity> GetAdminSHE(string token, int curLoginId)
        {
            WWUserService.UserServiceClient client = new WWUserService.UserServiceClient();
            int count = 0;
            List<UserEntity> listUser = new List<UserEntity>();
            WWUserService.ResponseModel response = client.getListAdminSHE(token, curLoginId);

            foreach (WWUserService.UserModel us in response.results.listUserModel)
            {
                listUser.Add(new UserEntity().clone(us));
            }

            client.Close();
            // this.listUser = new HashSet<UserEntity>(listUser.OrderBy(p => p.alpha_name));
            this.listUser = listUser.OrderBy(p => p.alpha_name).ToList();
            return this.listUser;
        }

        public List<UserEntity> GetListEmployeeInDepartment(string department)
        {
            List<UserEntity> listEmployeeInDepartment = listUser.Where(p => p.department == department).ToList();
            return listEmployeeInDepartment;
        }

        public List<UserEntity> GetHotWorkFO()
        {
            UserEntity oprSpv = listUser.Where(p => p.position != null && p.position.Trim().ToLower() == "operation supervisor").FirstOrDefault();
            List<UserEntity> listHotWorkFO = new List<UserEntity>();
            if (oprSpv != null)
            {
                listHotWorkFO = listUser.Where(p => p.employee_boss == oprSpv.id).ToList();
            }

            return listHotWorkFO;
        }

        public List<UserEntity> GetHotWorkGasTester()
        {
            List<UserEntity> listHotWorkFO = GetHotWorkFO();
            List<UserEntity> listHotWorkGasTester = new List<UserEntity>();
            foreach (UserEntity u in listHotWorkFO)
            {
                listHotWorkGasTester.AddRange(listUser.Where(p => p.employee_boss == u.id).ToList());
            }

            return listHotWorkGasTester;
        }
    }
}