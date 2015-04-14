using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.KPI
{
    public class KPIMaster
    {
        public enum AppState
        {
            EMPTY, START
        };

        public const string RequestorApprove = "Approved by Permit To Work Requestor";
        public const string SupervisorReject = "Rejected by Supervisor";
        public const string SupervisorApprove = "Approved by Supervisor";
        public const string SupervisorClosingApprove = "Cancellation approved by Supervisor";
        public const string AssessorApprove = "Approved by Assessor";
        public const string AssessorReject = "Rejected by Assessor";
        public const string FOReject = "Rejected by Facility Owner";
        public const string FOClosingApprove = "Cancellation approved by Facility Owner";
        public const string FOClosingReject = "Cancellation rejected by Facility Owner";
    }
}