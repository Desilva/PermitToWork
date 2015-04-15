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
            //        new workflow_node { id = 8, id_report = 105, node_name = "ASSESSOR_APPROVE", report_type = "GENERALPERMIT", status = 3 },
            //        new workflow_node { id = 9, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "GENERALPERMIT", status = 3 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_REQUESTOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_ASSESSOR", report_type = "GENERALPERMIT", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "GENERALPERMIT", status = 3 },
            //    };
            //}

            //dummy data EXCAVATION
            //if (permitName == WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "EANDI_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "CIVIL_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "ENVIRO_APPROVE", report_type = "EXCAVATION", status = 3 },
            //        //new workflow_node { id = 6, id_report = 105, node_name = "CIVIL_APPROVE", report_type = "EXCAVATION", status = 3 },
            //        //new workflow_node { id = 7, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 8, id_report = 105, node_name = "EANDI_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 9, id_report = 105, node_name = "CIVIL_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        //new workflow_node { id = 10, id_report = 105, node_name = "ENVIRO_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        //new workflow_node { id = 11, id_report = 105, node_name = "REQUESTOR_APPROVE", report_type = "EXCAVATION", status = 2 },
            //        //new workflow_node { id = 8, id_report = 105, node_name = "ASSESSOR_APPROVE", report_type = "EXCAVATION", status = 3 },
            //        //new workflow_node { id = 9, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "EXCAVATION", status = 3 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_REQUESTOR", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_EANDI", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_CIVIL", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 14, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "EXCAVATION", status = 3 },
            //        new workflow_node { id = 15, id_report = 105, node_name = "CANCELLATION_EANDI", report_type = "EXCAVATION", status = 2 },
            //        new workflow_node { id = 16, id_report = 105, node_name = "CANCELLATION_CIVIL", report_type = "EXCAVATION", status = 2 },
            //        //new workflow_node { id = 15, id_report = 105, node_name = "CANCELLATION_EANDI", report_type = "EXCAVATION", status = 3 },
            //    };
            //}

            //dummy data RADIOGRAPHY
            //if (permitName == WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "CHOOSING_SAFETY_OFFICER", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "OPERATOR_1_APPROVE", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "OPERATOR_2_APPROVE", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 6, id_report = 105, node_name = "SAFETY_OFFICER_APPROVE", report_type = "RADIOGRAPHIC", status = 3 },
            //        new workflow_node { id = 7, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 8, id_report = 105, node_name = "ASSESSOR_APPROVE", report_type = "RADIOGRAPHIC", status = 3 },
            //        new workflow_node { id = 9, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "RADIOGRAPHIC", status = 3 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_INPUT", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_OPERATOR_1", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_OPERATOR_2", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 14, id_report = 105, node_name = "CANCELLATION_SAFETY_OFFICER", report_type = "RADIOGRAPHIC", status = 2 },
            //        new workflow_node { id = 15, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "RADIOGRAPHIC", status = 3 },
            //        new workflow_node { id = 16, id_report = 105, node_name = "CANCELLATION_SAFETY_OFFICER", report_type = "RADIOGRAPHIC", status = 3 },
            //        new workflow_node { id = 17, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "RADIOGRAPHIC", status = 3 },
            //    };
            //}

            //dummy data LOTO
            //if (permitName == WorkflowNodeServiceModel.DocumentType.LOTO.ToString())
            //{
            //    result = new List<workflow_node> 
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "SUPERVISOR_INPUT", report_type = "LOTO", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "FACILITY_OWNER_APPLICATION", report_type = "LOTO", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "LOTO", status = 2 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "LOTO", status = 3 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "LOTO", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "LOTO", status = 3 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_OPERATOR_2", report_type = "LOTO", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "LOTO", status = 2 },
            //    };
            //}

            //dummy data Working at Height
            //if (permitName == WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString())
            //{
            //    result = new List<workflow_node>
            //    {
            //        new workflow_node { id = 1, id_report = 105, node_name = "REQUESTOR_INPUT", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 2, id_report = 105, node_name = "INSPECTOR_APPROVE", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 3, id_report = 105, node_name = "ERECTOR_REQUESTOR_APPROVE", report_type = "WORKINGATHEIGHT", status = 3 },
            //        new workflow_node { id = 4, id_report = 105, node_name = "SUPERVISOR_APPROVE", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 5, id_report = 105, node_name = "FACILITY_OWNER_APPROVE", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 10, id_report = 105, node_name = "CANCELLATION_INPUT", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 11, id_report = 105, node_name = "CANCELLATION_ERECTOR_REQUESTOR", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 12, id_report = 105, node_name = "CANCELLATION_SUPERVISOR", report_type = "WORKINGATHEIGHT", status = 2 },
            //        new workflow_node { id = 13, id_report = 105, node_name = "CANCELLATION_FACILITY_OWNER", report_type = "WORKINGATHEIGHT", status = 2 },
            //    };
            //}

            WorkflowNodeServiceModel workflowNodeServiceModel = new WorkflowNodeServiceModel();
            result = workflowNodeServiceModel.GetWorkflow(permitId, permitName);

            return result;
        }
    }
}