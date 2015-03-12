using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class WorkflowNodeServiceModel
    {
        private WWUserService.UserServiceClient serviceClient;

        public enum NodeStatus {
            NEW,
            ONGOING,
            APPROVED,
            REJECTED
        }

        public enum CSEPNodeName {
            REQUESTOR_INPUT,
            SUPERVISOR_SCREENING,
            FACILITY_OWNER_SCREENING,
            GAS_TESTING,
            REQUESTOR_APPROVE,
            SUPERVISOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_INITIATOR,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum HotWorkNodeName
        {
            REQUESTOR_INPUT,
            SUPERVISOR_SCREENING,
            FACILITY_OWNER_SCREENING,
            GAS_TESTING,
            REQUESTOR_APPROVE,
            SUPERVISOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_INITIATOR,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum GeneralPermitNodeName
        {
            REQUESTOR_APPROVE,
            SUPERVISOR_APPROVE,
            CHOOSING_ASSESSOR,
            ASSESSOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_REQUESTOR,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_ASSESSOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum FireImpairmentNodeName
        {
            REQUESTOR_APPROVE,
            FIREWATCH_APPROVE,
            SUPERVISOR_SCREENING,
            CHOOSING_SO_DEPT_HEAD_FO,
            SAFETY_OFFICER_APPROVE,
            FACILITY_OWNER_APPROVE,
            DEPT_HEAD_FO_APPROVE,
            CANCELLATION_REQUESTOR,
            CANCELLATION_FIREWATCH,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_SAFETY_OFFICER,
            CANCELLATION_FACILITY_OWNER,
            CANCELLATION_DEPT_HEAD_FO
        }

        public enum ExcavationNodeName
        {
            REQUESTOR_INPUT,
            SUPERVISOR_APPROVE,
            EANDI_APPROVE,
            CIVIL_APPROVE,
            REQUESTOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_INPUT,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_EANDI,
            CANCELLATION_CIVIL,
            CANCELLATION_REQUESTOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum WorkingAtHeightNodeName
        {
            REQUESTOR_INPUT,
            INSPECTOR_APPROVE,
            ERECTOR_REQUESTOR_APPROVE,
            SUPERVISOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_INPUT,
            CANCELLATION_ERECTOR_REQUESTOR,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum RadiographicNodeName
        {
            REQUESTOR_INPUT,
            OPERATOR_1_APPROVE,
            OPERATOR_2_APPROVE,
            SAFETY_OFFICER_APPROVE,
            CHOOSING_SAFETY_OFFICER,
            SUPERVISOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_INPUT,
            CANCELLATION_OPERATOR_1,
            CANCELLATION_OPERATOR_2,
            CANCELLATION_SAFETY_OFFICER,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum LotoNodeName
        {
            SUPERVISOR_INPUT,
            FACILITY_OWNER_APPLICATION,
            SUPERVISOR_INSPECT,
            SUPERVISOR_APPROVE,
            FACILITY_OWNER_APPROVE,
            CANCELLATION_SUPERVISOR,
            CANCELLATION_FACILITY_OWNER
        }

        public enum DocumentType
        {
            GENERALPERMIT,
            CSEP,
            HOTWORK,
            FIREIMPAIRMENT,
            EXCAVATION,
            WORKINGATHEIGHT,
            RADIOGRAPHIC,
            LOTO
        }

        public bool CreateNode(int permitId, string permitName, string nodeName, byte nodeStatus)
        {
            serviceClient = new WWUserService.UserServiceClient();

            bool status = serviceClient.WorkflowNodeCreate(permitId, permitName, nodeName, nodeStatus);
            serviceClient.Close();

            return status;
        }

        public List<workflow_node> GetWorkflow(int permitId, string permitName)
        {
            serviceClient = new WWUserService.UserServiceClient();

            string serviceData = serviceClient.GetWorkflowNode(permitId, permitName);
            List<workflow_node> serviceDataParsed = JsonConvert.DeserializeObject<List<workflow_node>>(serviceData);
            serviceClient.Close();

            return serviceDataParsed;
        }
    }
}