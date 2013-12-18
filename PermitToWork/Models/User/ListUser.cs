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

        public ListUser() {
            var result = from emp in db.employees
                         orderby emp.alpha_name
                         select new UserEntity
                         {
                             id = emp.id,
                             alpha_name = emp.alpha_name,
                             employee_boss = emp.employee_boss,
                             employee_delegate = emp.employee_delegate,
                             signature = emp.signature,
                             position = emp.position,
                             email = emp.email,
                         };

            this.listUser = result.ToList();
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

            // get one level up
            UserEntity userLevelOne = listUser.Where(p => p.id == requestor.employee_boss).FirstOrDefault();

            // check if exist one level up again
            if (userLevelOne.employee_boss == null)
            {
                employee_dept ed = db.employees.Find(userLevelOne.id).employee_dept1;
                foreach (employee e in ed.employees)
                {
                    listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
                }
                return listSpv;
            }

            // get one level up
            UserEntity userLevelTwo = listUser.Where(p => p.id == userLevelOne.employee_boss).FirstOrDefault();
            if (userLevelTwo.employee_boss == null)
            {
                employee_dept ed = db.employees.Find(userLevelTwo.id).employee_dept1;
                foreach (employee e in ed.employees)
                {
                    listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
                }
            }
            else
            {
                employee ed = db.employees.Find(userLevelTwo.id).employee2;
                foreach (employee e in ed.employee1)
                {
                    listSpv.Add(listUser.Where(p => p.id == e.id).FirstOrDefault());
                }
            }
            // return the user
            return listSpv;
        }

        public List<UserEntity> GetHotWorkFO()
        {
            UserEntity oprSpv = listUser.Where(p => p.position.ToLower() == "operation supervisor").FirstOrDefault();
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