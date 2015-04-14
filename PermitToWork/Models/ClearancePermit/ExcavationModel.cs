using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models
{
    public partial class excavation
    {
        public bool IsDisposalMoved()
        {
            string[] pre_screening_spv_arr = this.pre_screening_spv.Split('#');
            if (pre_screening_spv_arr[8] == "1")
            {
                return true;
            }
            return false;
        }
    }
}