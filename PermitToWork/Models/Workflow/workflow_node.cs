using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class workflow_node
    {
        public long id { get; set; }
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
            //if (permitName == WorkflowNodeServiceModel.DocumentType.CSEP.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "CSEP", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "CSEP", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "CSEP", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "GAS_TESTING", report_type = "CSEP", status = 3 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "CSEP", status = 3 },
            //        new workflow_node { id = 6, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "CSEP", status = 2 },
            //        new workflow_node { id = 7, id_report = 105, node_name = "CANCELLATION_INITIATOR", report_type = "CSEP", status = 2 },
            //    };
            //}

            //dummy data HOT WORK
            //if (permitName == WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "HOTWORK", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "HOTWORK", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "HOTWORK", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "GAS_TESTING", report_type = "HOTWORK", status = 3 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "FACILITY_OWNER_SCREENING", report_type = "HOTWORK", status = 3 },
            //        new workflow_node { id = 6, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "HOTWORK", status = 2 },
            //        new workflow_node { id = 7, id_report = 105, node_name = "CANCELLATION_INITIATOR", report_type = "HOTWORK", status = 2 },
            //    };
            //}

            //dummy data FIRE IMPAIRMENT
            //if (permitName == WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_APPROVE", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "FIREWATCH_APPROVE", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "CHOOSING_SO_DEPT_HEAD_FO", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "SAFETY_OFFICER_APPROVE", report_type = "FIREIMPAIRMENT", status = 3 },
            //        new workflow_node { id = 6, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "FIREIMPAIRMENT", status = 3 },
            //        new workflow_node { id = 7, id_report = 105, node_name = "FIREWATCH_APPROVE", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 8, id_report = 105, node_name = "SUPERVISOR_SCREENING", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 9, id_report = 105, node_name = "CANCELLATION_REQUESTOR", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_FIREWATCH", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_SAFETY_OFFICER", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "FIREIMPAIRMENT", status = 2 },
            //        new workflow_node { id = 14, id_report = 105, node_name = "CANCELLATION_DEPT_HEAD_FO", report_type = "FIREIMPAIRMENT", status = 2 }
            //    };
            //}

            //dummy data GENERAL PERMIT
            //if (permitName == WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_APPROVE", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "CHOOSING_ASSESSOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "ASSESSOR_APPROVE", report_type = "GENERALPERMIT", status = 3 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "GENERALPERMIT", status = 3 },
            //        new workflow_node { id = 6, id_report = 105, node_name = "REQUESTOR_APPROVE", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 7, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 8, id_report = 105, node_name = "ASSESSOR_APPROVE", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 9, id_report = 105, node_name = "CANCELLATION_REQUESTOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_ASSESSOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "GENERALPERMIT", status = 2 },
            //    };
            //}

            WorkflowNodeServiceModel workflowNodeServiceModel = new WorkflowNodeServiceModel();
            result = workflowNodeServiceModel.GetWorkflow(permitId, permitName);

            return result;
        }
    }
}