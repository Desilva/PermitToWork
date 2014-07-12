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

        public UserEntity user { get; set; }

        private star_energy_ptwEntities db;

        public LogEntity() {
            this.db = new star_energy_ptwEntities();
        }
        public LogEntity(int id, UserEntity user, star_energy_ptwEntities db = null)
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
            this.user = new UserEntity(this.user_id.Value, user.token, user);
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

        public string generateLog(UserEntity user, int id_permit, string controllerName, string actionName, string comment = null, int extension = 0, string assessor = "", int? who = null) {
            actionName = actionName.ToLower();
            if (controllerName.ToLower() == "ptw")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 0;
                switch (actionName)
                {
                    case "requestoracc":
                        this.status = "Approved by Permit To Work Requestor";
                        addLog();
                        break;
                    case "supervisoracc":
                        this.status = "Approved by Supervisor";
                        addLog();
                        break;
                    case "supervisoraccreject":
                        this.status = "Rejected by Supervisor";
                        addLog();
                        break;
                    case "assessoracc":
                        this.status = "Approved by Assessor";
                        addLog();
                        break;
                    case "assessoraccreject":
                        this.status = "Rejected by Assessor";
                        addLog();
                        break;
                    case "foacc":
                        this.status = "Approved by Facility Owner";
                        addLog();
                        break;
                    case "foaccreject":
                        this.status = "Rejected by Facility Owner";
                        addLog();
                        break;
                    case "cancelptw":
                        this.status = "Cancellation requested by Permit To Work Requestor";
                        addLog();
                        break;
                    case "requestorcan":
                        this.status = "Cancellation requested by Permit To Work Requestor";
                        addLog();
                        break;
                    case "supervisorcan":
                        this.status = "Cancellation approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorcanreject":
                        this.status = "Cancellation rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorcan":
                        this.status = "Cancellation approved by Assessor";
                        addLog();
                        break;
                    case "assessorcanreject":
                        this.status = "Cancellation rejected by Assessor";
                        addLog();
                        break;
                    case "focan":
                        this.status = "Cancellation approved by Facility Owner";
                        addLog();
                        break;
                    case "focanreject":
                        this.status = "Cancellation rejected by Facility Owner";
                        addLog();
                        break;
                    case "editptw":
                        if (assessor != "")
                        {
                            this.status = "Facility Owner has chosen Assessor";
                            addLog();
                        }
                        break;
                }
            }
            else if (controllerName.ToLower() == "hw")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 3;
                switch (actionName)
                {
                    case "gastesteracc":
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
                    case "requestoracc":
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
                    case "supervisoracc":
                        this.status = "Approved by Supervisor";
                        addLog();
                        break;
                    case "supervisoraccreject":
                        this.status = "Rejected by Supervisor";
                        addLog();
                        break;
                    case "firewatchacc":
                        this.status = "Approved by Fire Watch";
                        addLog();
                        break;
                    case "firewatchaccreject":
                        this.status = "Rejected by Fire Watch";
                        addLog();
                        break;
                    case "foacc":
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
                    case "foaccreject":
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
                    case "closehw":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "requestorcan":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "supervisorcan":
                        this.status = "Closing approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorcanreject":
                        this.status = "Closing rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorcan":
                        this.status = "Closing approved by Assessor";
                        addLog();
                        break;
                    case "assessorcanreject":
                        this.status = "Closing rejected by Assessor";
                        addLog();
                        break;
                    case "fOcan":
                        this.status = "Closing approved by Facility Owner";
                        addLog();
                        break;
                    case "fOcanreject":
                        this.status = "Closing rejected by Facility Owner";
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "csep")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 2;
                switch (actionName)
                {
                    case "gastesteracc":
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
                    case "requestoracc":
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
                    case "supervisoracc":
                        this.status = "Approved by Supervisor";
                        addLog();
                        break;
                    case "supervisoraccreject":
                        this.status = "Rejected by Supervisor";
                        addLog();
                        break;
                    case "firewatchacc":
                        this.status = "Approved by Fire Watch";
                        addLog();
                        break;
                    case "firewatchaccreject":
                        this.status = "Rejected by Fire Watch";
                        addLog();
                        break;
                    case "foacc":
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
                    case "foaccreject":
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
                    case "closehw":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "requestorcan":
                        this.status = "Closing requested by Work Leader";
                        addLog();
                        break;
                    case "supervisorcan":
                        this.status = "Closing approved by Supervisor";
                        addLog();
                        break;
                    case "supervisorcanreject":
                        this.status = "Closing rejected by Supervisor";
                        addLog();
                        break;
                    case "assessorcan":
                        this.status = "Closing approved by Assessor";
                        addLog();
                        break;
                    case "assessorcanreject":
                        this.status = "Closing rejected by Assessor";
                        addLog();
                        break;
                    case "focan":
                        this.status = "Closing approved by Facility Owner";
                        addLog();
                        break;
                    case "focanreject":
                        this.status = "Closing rejected by Facility Owner";
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "fi")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 4;
                switch (actionName)
                {
                    case "savecomplete":
                        switch (who)
                        {
                            case 1:
                                this.status = "Approved by Requestor";
                                break;
                            case 2:
                                this.status = "Approved by Fire Watch";
                                break;
                            case 3:
                                this.status = "Pre-job Screening by Supervisor";
                                break;
                            case 4:
                                this.status = "Approved by Safety Officer";
                                break;
                            case 5:
                                this.status = "Approved by Facility Owner";
                                break;
                            case 6:
                                this.status = "Approved by Dept. Head Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectfipermit":
                        switch (who)
                        {
                            case 1:
                                this.status = "Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Rejected by Fire Watch";
                                break;
                            case 3:
                                this.status = "Rejected by Supervisor";
                                break;
                            case 4:
                                this.status = "Rejected by Safety Officer";
                                break;
                            case 5:
                                this.status = "Rejected by Facility Owner";
                                break;
                            case 6:
                                this.status = "Rejected by Dept. Head Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "saveassignso":
                        this.status = "Assigned Safety Officer and Dept. Head Facility Owner by Facility Owner";
                        addLog();
                        break;
                    case "cancelfipermit":
                        this.status = "Cancellation FI Permit by Requestor";
                        addLog();
                        break;
                    case "savecompletecancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Approved by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Approved by Fire Watch";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Pre-job Screening by Supervisor";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Approved by Safety Officer";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Approved by Facility Owner";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Approved by Dept. Head Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectfipermitcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Rejected by Fire Watch";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Rejected by Supervisor";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Rejected by Safety Officer";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Rejected by Facility Owner";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Rejected by Dept. Head Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "excavation")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 5;
                switch (actionName)
                {
                    case "savecomplete":
                        switch (who)
                        {
                            case 1:
                                this.status = "Sent for Screening and Approval by Requestor";
                                break;
                            case 2:
                                this.status = "Approved by Supervisor";
                                break;
                            case 3:
                                this.status = "Approved by SHE";
                                break;
                            case 4:
                                this.status = "Approved by Civil";
                                break;
                            case 5:
                                this.status = "Approved by E&I";
                                break;
                            case 6:
                                this.status = "Approved by Requestor";
                                break;
                            case 7:
                                this.status = "Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermit":
                        switch (who)
                        {
                            case 1:
                                this.status = "Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Rejected by Supervisor";
                                break;
                            case 3:
                                this.status = "Rejected by SHE";
                                break;
                            case 4:
                                this.status = "Rejected by Civil";
                                break;
                            case 5:
                                this.status = "Rejected by E&I";
                                break;
                            case 6:
                                this.status = "Rejected by Requestor";
                                break;
                            case 7:
                                this.status = "Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "saveassignso":
                        this.status = "Assigned Safety Officer and Dept. Head Facility Owner by Facility Owner";
                        addLog();
                        break;
                    case "cancelpermit":
                        this.status = "Cancellation Excavation Permit by Requestor";
                        addLog();
                        break;
                    case "savecompletecancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Approved by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Approved by Supervisor";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Approved by Civil";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Approved by E&I";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Approved by SHE";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermitcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Rejected by Supervisor";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Rejected by Civil";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Rejected by E&I";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Rejected by SHE";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "workingheight")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 6;
                switch (actionName)
                {
                    case "saveandsend":
                        switch (who)
                        {
                            case 1:
                                this.status = "Sent for Screening and Approval by Requestor";
                                break;
                            case 2:
                                this.status = "Signed by Inspector";
                                break;
                            case 3:
                                this.status = "Approved by Requestor / Erector";
                                break;
                            case 4:
                                this.status = "Approved by Supervisor";
                                break;
                            case 5:
                                this.status = "Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermit":
                        switch (who)
                        {
                            case 1:
                                this.status = "Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Rejected by Inspector";
                                break;
                            case 3:
                                this.status = "Rejected by Erector";
                                break;
                            case 4:
                                this.status = "Rejected by Supervisor";
                                break;
                            case 5:
                                this.status = "Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "inspectorsign":
                        this.status = "Signed by Inspector";
                        addLog();
                        break;
                    case "cancelwhpermit":
                        this.status = "Cancellation Working at Height Permit by Requestor";
                        addLog();
                        break;
                    case "saveandsendcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Approved by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Approved by Requestor / Erector";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Approved by Supervisor";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermitcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Rejected by Requestor / Erector";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Rejected by Supervisor";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "radiography")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 7;
                switch (actionName)
                {
                    case "saveandsend":
                        switch (who)
                        {
                            case 1:
                                this.status = "Sent for Screening and Approval by Requestor";
                                break;
                            case 2:
                                this.status = "Approved by Radiographer Level 1 (Operator)";
                                break;
                            case 3:
                                this.status = "Approved by Radiographer Level 2";
                                break;
                            case 4:
                                this.status = "Approved by Supervisor";
                                break;
                            case 5:
                                this.status = "Approved by SHE Officer";
                                break;
                            case 6:
                                this.status = "Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermit":
                        switch (who)
                        {
                            case 1:
                                this.status = "Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Rejected by Radiographer Level 1 (Operator)";
                                break;
                            case 3:
                                this.status = "Rejected by Radiographer Level 2";
                                break;
                            case 4:
                                this.status = "Rejected by Supervisor";
                                break;
                            case 5:
                                this.status = "Rejected by SHE Officer";
                                break;
                            case 6:
                                this.status = "Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "assignsafetyofficer":
                        this.status = "Safety Officer Assigned by Facility Owner";
                        addLog();
                        break;
                    case "cancelradpermit":
                        this.status = "Cancellation Radiography Permit by Requestor";
                        addLog();
                        break;
                    case "saveandsendcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Approved by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Approved by Radiographer Level 1 (Operator)";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Approved by Radiographer Level 2";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Approved by Supervisor";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Approved by SHE Officer";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Approved by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                    case "rejectpermitcancel":
                        switch (who)
                        {
                            case 1:
                                this.status = "Cancellation Permit Rejected by Requestor";
                                break;
                            case 2:
                                this.status = "Cancellation Permit Rejected by Radiographer Level 1 (Operator)";
                                break;
                            case 3:
                                this.status = "Cancellation Permit Rejected by Radiographer Level 2";
                                break;
                            case 4:
                                this.status = "Cancellation Permit Rejected by Supervisor";
                                break;
                            case 5:
                                this.status = "Cancellation Permit Rejected by SHE Officer";
                                break;
                            case 6:
                                this.status = "Cancellation Permit Rejected by Facility Owner";
                                break;
                        }
                        addLog();
                        break;
                }
            }
            else if (controllerName.ToLower() == "loto")
            {
                this.id_permit = id_permit;
                this.user_id = user.id;
                this.datetime = DateTime.Now;
                this.comment = comment;
                this.permit_type = 1;
                switch (actionName)
                {
                    case "createnewloto":
                        this.status = "Created new LOTO.";
                        addLog();
                        break;
                    case "frompreviousloto":
                        this.status = "Created LOTO from Previous LOTO.";
                        addLog();
                        break;
                    case "savecomplete":
                        this.status = "LOTO is completed by Requestor, sent to FO for set agreeing LOTO Point and applying it.";
                        addLog();
                        break;
                    case "saveandinspect":
                        this.status = "LOTO is sent to inspecting by LOTO Holder";
                        addLog();
                        break;
                    case "saveandsendapprove":
                        this.status = "LOTO is sent to Supervisor for approval.";
                        addLog();
                        break;
                    case "saveapprove":
                        switch (who)
                        {
                            case 1:
                                this.status = "LOTO is approved by Supervisor.";
                                break;
                            case 2:
                                this.status = "LOTO is approved by Facility Owner.";
                                break;
                        }
                        addLog();
                        break;
                    case "savecomingholderapprove":
                        this.status = "LOTO is approved by Coming Holder.";
                        addLog();
                        break;
                    case "requestsuspension":
                        this.status = "Coming Holder request LOTO Suspension.";
                        addLog();
                        break;
                    case "sendapprovalsuspension":
                        this.status = "LOTO Suspension is sent to other coming holder for agreeing.";
                        addLog();
                        break;
                    case "agreesuspension":
                        this.status = "Other holder agrees LOTO Suspension.";
                        addLog();
                        break;
                    case "rejectsuspension":
                        this.status = "Other holder rejects LOTO Suspension.";
                        addLog();
                        break;
                    case "saveapprovalfosuspension":
                        this.status = "Facility Owner set agreed, applied, or removed LOTO Point prior to Suspension.";
                        addLog();
                        break;
                    case "approvesuspension":
                        this.status = "LOTO Point is inspected by all holder, and LOTO Suspension is approved.";
                        addLog();
                        break;
                    case "approvefosuspension":
                        this.status = "LOTO Suspension is approved by Facility Owner .";
                        addLog();
                        break;
                    case "completesuspension":
                        this.status = "LOTO Suspension is requested to cancel.";
                        addLog();
                        break;
                    case "sendcompletesuspension":
                        this.status = "LOTO Suspension Cancellation is sent to other coming holder for agreeing.";
                        addLog();
                        break;
                    case "agreecompletesuspensions":
                        this.status = "LOTO Suspension Cancellation is agreed by other coming holder.";
                        addLog();
                        break;
                    case "sendinspectcompletesuspension":
                        this.status = "LOTO Suspension Cancellation Point has been set agreed and applied by Facility Owner, and sent to Holder for inspection.";
                        addLog();
                        break;
                    case "inspectcompletesuspension":
                        this.status = "LOTO Suspension Cancellation Point has been inspected by holder.";
                        addLog();
                        break;
                }
            }

            return "200";
        }

        public List<LogEntity> getLogsById(int id_permit, int permit_type, string token, UserEntity user)
        {
            var list = from log in this.db.permit_log
                       where log.id_permit == id_permit && log.permit_type == permit_type
                       select new LogEntity
                       {
                           datetime = log.datetime,
                           user_id = log.user_id,
                           status = log.status,
                           comment = log.comment,
                       };
            var listLogs = list.ToList();
            foreach (var a in listLogs)
            {
                a.user = new UserEntity(a.user_id.Value, token, user);
            }
            return listLogs;
        }
    }
}