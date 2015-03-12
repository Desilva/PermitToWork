using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class HotWorkModel
    {
        public List<string> NodeNameList = new List<string> { "REQUESTOR_INPUT", "SUPERVISOR_SCREENING", "FACILITY_OWNER_SCREENING", "GAS_TESTING", "REQUESTOR_APPROVE", "SUPERVISOR_APPROVE", "FACILITY_OWNER_APPROVE" };
        public List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_INITIATOR", "CANCELLATION_SUPERVISOR", "CANCELLATION_FACILITY_OWNER" };
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes"></param>
        public HotWorkModel(List<workflow_node> signals)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
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