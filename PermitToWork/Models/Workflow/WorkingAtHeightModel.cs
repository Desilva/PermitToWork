using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class WorkingAtHeightModel
    {
        public List<string> NodeNameList = new List<string> { "REQUESTOR_INPUT", "INSPECTOR_APPROVE", "ERECTOR_REQUESTOR_APPROVE", "SUPERVISOR_APPROVE", "FACILITY_OWNER_APPROVE" };
        public List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_INPUT", "CANCELLATION_ERECTOR_REQUESTOR", "CANCELLATION_SUPERVISOR", "CANCELLATION_FACILITY_OWNER" };
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        private bool IsInspectorApproveRequired = false;

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes">signal terurut dari paling lama ke paling baru</param>
        public WorkingAtHeightModel(List<workflow_node> signals, bool isInspectorApproveRequiredParam)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
            IsInspectorApproveRequired = isInspectorApproveRequiredParam;
            if (!IsInspectorApproveRequired)
            {
                NodeNameList.Remove("INSPECTOR_APPROVE");
            }

            NodeList = helper.ProcessSerialNodes(signals.Where(m => NodeNameList.Contains(m.node_name)).ToList(), NodeNameList);

            cancellationSignals = signals.Where(m => CancellationNodeNameList.Contains(m.node_name)).ToList();
            if (cancellationSignals.Count() > 0)
            {
                cancellationNodes = helper.ProcessSerialNodes(cancellationSignals, CancellationNodeNameList);
                NodeList = NodeList.Concat(cancellationNodes).ToList();
            }
        }
    }
}