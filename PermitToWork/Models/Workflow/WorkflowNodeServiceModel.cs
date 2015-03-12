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
            FACILITY_OWNER_APPROVE
        }

        public enum DocumentType
        {
            CSEP
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