using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermitToWork.Models.User;

namespace PermitToWork.Models.Master
{
    public class PtwHolderExcelModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string department { get; set; }
        public string holderNo { get; set; }
        public DateTime? valid_until { get; set; }

        public PtwHolderExcelModel() { }

        public PtwHolderExcelModel(MstPtwHolderNoEntity ptwHolder, UserEntity user) {
            if (ptwHolder != null)
            {
                this.id = ptwHolder.id_employee.Value;
                this.name = ptwHolder.user.alpha_name;
                this.department = ptwHolder.user.department;
                this.holderNo = ptwHolder.ptw_holder_no;
                this.valid_until = ptwHolder.activated_date_until;
            }
            else
            {
                this.id = user.id;
                this.name = user.alpha_name;
                this.department = user.department;
            }
        }
    }
}