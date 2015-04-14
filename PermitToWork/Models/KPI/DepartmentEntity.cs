using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.KPI
{
    public class DepartmentEntity
    {
        public string DepartmentName { get; set; }
        public int TotalClosing { get; set; }
        public double AverageClosingTime { get; set; } //in hours
    }
}