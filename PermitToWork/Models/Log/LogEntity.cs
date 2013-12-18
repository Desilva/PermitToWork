using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Log
{
    public class LogEntity
    {
        public int id { get; set; }
        public Nullable<int> id_permit { get; set; }
        public Nullable<System.DateTime> datetime { get; set; }
        public Nullable<int> user_id { get; set; }
        public string status { get; set; }
        public string comment { get; set; }
        public int? permit_type { get; set; }

        private star_energy_ptwEntities db;

        public LogEntity() {
            this.db = new star_energy_ptwEntities();
        }
        public LogEntity(int id, star_energy_ptwEntities db = null)
        {
            if (db != null)
            {
                this.db = db;
            }
            else
            {
                this.db = new star_energy_ptwEntities();
            }

            permit_log log = this.db.permit_log.Find(id);
            this.id = log.id;
            this.id_permit = log.id_permit;
            this.datetime = log.datetime;
            this.user_id = log.user_id;
            this.status = log.status;
            this.comment = log.comment;
            this.permit_type = log.permit_type;
        }

        public int addLog()
        {
            permit_log log = new permit_log
            {
                id_permit = this.id_permit,
                datetime = this.datetime,
                user_id = this.user_id,
                status = this.status,
                comment = this.comment,
                permit_type = this.permit_type
            };

            this.db.permit_log.Add(log);
            return this.db.SaveChanges();
        }

        public string generateLog(UserEntity user, int id_permit, string controllerName, string actionName, string comment = null, int extension = 0) {
            if (controllerName == "Ptw")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 0;
                switch (actionName)
                {
                    case "requestorAcc":
                        this.status = "Approved by Permit To Work Requestor";
                        addLog();
                        break;
                    case "supervisorAcc":
                        this.status = "Approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorAccReject":
                        this.status = "Rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorAcc":
                        this.status = "Approved by Assessor";
                        addLog();
                        break;
                    case "assessorAccReject":
                        this.status = "Rejected by Assessor";
                        addLog();
                        break;
                    case "fOAcc":
                        this.status = "Approved by Facility Owner";
                        addLog();
                        break;
                    case "fOAccReject":
                        this.status = "Rejected by Facility Owner";
                        addLog();
                        break;
                    case "cancelPtw":
                        this.status = "Cancellation requested by Permit To Work Requestor";
                        addLog();
                        break;
                    case "requestorCan":
                        this.status = "Cancellation requested by Permit To Work Requestor";
                        addLog();
                        break;
                    case "supervisorCan":
                        this.status = "Cancellation approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorCanReject":
                        this.status = "Cancellation rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorCan":
                        this.status = "Cancellation approved by Assessor";
                        addLog();
                        break;
                    case "assessorCanReject":
                        this.status = "Cancellation rejected by Assessor";
                        addLog();
                        break;
                    case "fOCan":
                        this.status = "Cancellation approved by Facility Owner";
                        addLog();
                        break;
                    case "fOCanReject":
                        this.status = "Cancellation rejected by Facility Owner";
                        addLog();
                        break;
                }
            }
            else if (controllerName == "Hw")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 3;
                switch (actionName)
                {
                    case "gasTesterAcc":
                        if (extension == 0)
                        {
                            this.status = "Gas Testing by Gas Tester";
                        }
                        else
                        {
                            this.status = "Extension " + extension + " Gas Testing by Gas Tester";
                        }
                        addLog();
                        break;
                    case "requestorAcc":
                        if (extension == 0)
                        {
                            this.status = "Approved by Work Leader";
                        }
                        else
                        {
                            this.status = "Extension " + extension + " Approved by Work Leader";
                        }
                        addLog();
                        break;
                    case "supervisorAcc":
                        this.status = "Approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorAccReject":
                        this.status = "Rejected by Supervisor";
                        addLog();
                        break;
                    case "fireWatchAcc":
                        this.status = "Approved by Fire Watch";
                        addLog();
                        break;
                    case "fireWatchAccReject":
                        this.status = "Rejected by Fire Watch";
                        addLog();
                        break;
                    case "fOAcc":
                        if (extension == 0)
                        {
                            this.status = "Approved by Facility Owner";
                        }
                        else
                        {
                            this.status = "Extension " + extension + " Approved by Facility Owner";
                        }
                        addLog();
                        break;
                    case "fOAccReject":
                        if (extension == 0)
                        {
                            this.status = "Rejected by Facility Owner";
                        }
                        else
                        {
                            this.status = "Extension " + extension + " Rejected by Facility Owner";
                        }
                        addLog();
                        break;
                    case "closeHw":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "requestorCan":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "supervisorCan":
                        this.status = "Closing approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorCanReject":
                        this.status = "Closing rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorCan":
                        this.status = "Closing approved by Assessor";
                        addLog();
                        break;
                    case "assessorCanReject":
                        this.status = "Closing rejected by Assessor";
                        addLog();
                        break;
                    case "fOCan":
                        this.status = "Closing approved by Facility Owner";
                        addLog();
                        break;
                    case "fOCanReject":
                        this.status = "Closing rejected by Facility Owner";
                        addLog();
                        break;
                }
            }

            return "200";
        }
    }
}