using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class workflow_node
    {
        public int id { get; set; }
        public int id_report { get; set; }
        public string node_name { get; set; }
        public int status { get; set; }
        public string report_type { get; set; }
        
        /// <summary>
        /// memanggil service
        /// </summary>
        /// <returns></returns>
        public List<workflow_node> FindAllNode(int permitId, string permitName)
        {
            List<workflow_node> result = null;

            //dummy data CSEP
            result = new List<workflow_node> 
            {
                new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "CSEP", status = 2 },
                new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "CSEP", status = 2 },
                new workflow_node { id = 3, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "CSEP", status = 2 },
                new workflow_node { id = 4, id_report = 105, node_name = "GAS_TESTING", report_type = "CSEP", status = 3 },
                new workflow_node { id = 5, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "CSEP", status = 3 },
                new workflow_node { id = 6, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "CSEP", status = 2 },
                new workflow_node { id = 7, id_report = 105, node_name = "CANCELLATION_INITIATOR", report_type = "CSEP", status = 2 },
            };

            //dummy data HOT WORK
            result = new List<workflow_node> 
            {
                new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "HOTWORK", status = 2 },
                new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "HOTWORK", status = 2 },
                new workflow_node { id = 3, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "HOTWORK", status = 2 },
                new workflow_node { id = 4, id_report = 105, node_name = "GAS_TESTING", report_type = "HOTWORK", status = 3 },
                new workflow_node { id = 5, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "HOTWORK", status = 3 },
                new workflow_node { id = 6, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "HOTWORK", status = 2 },
                new workflow_node { id = 7, id_report = 105, node_name = "CANCELLATION_INITIATOR", report_type = "HOTWORK", status = 2 },
            };

            //WorkflowNodeServiceModel workflowNodeServiceModel = new WorkflowNodeServiceModel();
            //result = workflowNodeServiceModel.GetWorkflow(permitId, permitName);

            return result;
        }
    }
}