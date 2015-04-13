using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIResult
    {
        public int RequestorOntimeClosing { get; set; }
        public int RequestorOverdueClosing { get; set; }

        public string status { get; set; }
    }
}