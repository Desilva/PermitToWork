using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models
{
    public partial class working_height
    {
        public bool isNeedInspector()
        {
            if (this.access.Contains('4') && this.scaffolding == 2)
            {
                return true;
            }
            return false;
        }
    }
}