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
        public int RequestorImpromptuPermit { get; set; }
        public double SupervisorAverageResponseTime { get; set; } //in hours
        public double AssessorAverageResponseTime { get; set; } //in hours
        public int FOClosingApprove { get; set; }
        public double FOAverageClosingTime { get; set; } //in hours

        public string status { get; set; }
    }
}