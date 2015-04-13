using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class RadiographyModel
    {
        //urutan tidak boleh diganti
        private List<string> NodeNameList = new List<string> { "REQUESTOR_INPUT", "OPERATOR_1_APPROVE", "OPERATOR_2_APPROVE", "SUPERVISOR_APPROVE", "CHOOSING_SAFETY_OFFICER", "SAFETY_OFFICER_APPROVE", "FACILITY_OWNER_APPROVE" };
        private List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_INPUT", "CANCELLATION_OPERATOR_1", "CANCELLATION_OPERATOR_2", "CANCELLATION_SUPERVISOR", "CANCELLATION_SAFETY_OFFICER", "CANCELLATION_FACILITY_OWNER" };
        
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes"></param>
        public RadiographyModel(List<workflow_node> signals)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
            NodeList = ProcessRadiographyNodes(signals.Where(m => NodeNameList.Contains(m.node_name)).ToList(), NodeNameList);

            cancellationSignals = signals.Where(m => CancellationNodeNameList.Contains(m.node_name)).ToList();
            if (cancellationSignals.Count() > 0)
            {
                cancellationNodes = helper.ProcessSerialNodes(cancellationSignals, CancellationNodeNameList);
                NodeList = NodeList.Concat(cancellationNodes).ToList();
            }
        }

        /// <summary>
        /// memproses list of signals
        /// kasus
        ///     requestor input -> op level 1 ongoing, choosing safety officer ongoing
        ///     safety officer reject -> choosing safety officer finish, supervisor approve ongoing
        /// </summary>
        /// <param name="signals">signals terkait workflow radiography saja, TIDAK TERMASUK signal cancellation</param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessRadiographyNodes(List<workflow_node> signals, List<string> nodeNames)
        {
            //kamus lokal
            List<string> approvedNodeNames = new List<string>(); //stack of node names
            string lastApproved, nodeNameToBeDeleted;
            int nodeNameIndex;
            List<WorkflowNodePresentationStub> nodeList;
            bool existOngong = false;

            //algoritma
            //inisiasi NodeList
            nodeList = new List<WorkflowNodePresentationStub>();
            foreach (string name in nodeNames)
                nodeList.Add(new WorkflowNodePresentationStub { NodeName = name, Status = WorkflowNodePresentationStub.NodeStatus.EMPTY });

            //mengisi nodes status ke stack
            signals = signals.OrderBy(m => m.id).ToList(); //diurutkan dari paling lama ke paling baru
            foreach (workflow_node node in signals)
            {
                if (node.status == (int)WorkflowNodeServiceModel.NodeStatus.APPROVED) //approve signal
                {
                    approvedNodeNames.Add(node.node_name);
                }
                else if (node.status == (int)WorkflowNodeServiceModel.NodeStatus.REJECTED) //reject signal
                {
                    lastApproved = approvedNodeNames.LastOrDefault();
                    if (lastApproved != null)
                    {
                        if (node.node_name == WorkflowNodeServiceModel.RadiographicNodeName.SAFETY_OFFICER_APPROVE.ToString())
                        {
                            nodeNameToBeDeleted = WorkflowNodeServiceModel.RadiographicNodeName.SUPERVISOR_APPROVE.ToString();
                        }
                        else
                        {
                            nodeNameIndex = nodeNames.FindIndex(a => a == node.node_name);
                            nodeNameToBeDeleted = nodeNames[nodeNameIndex - 1];
                        }

                        if (approvedNodeNames.Remove(nodeNameToBeDeleted) == false)
                        {
                            throw new Exception("Kesalahan dalam fungsi pemrosesan signal.");
                        }
                    }
                }
            }

            //set status setiap item di NodeList
            foreach (WorkflowNodePresentationStub node in nodeList)
            {
                if (approvedNodeNames.Contains(node.NodeName))
                {
                    node.Status = WorkflowNodePresentationStub.NodeStatus.FINISH;
                }
                else
                {
                    if (node.NodeName == WorkflowNodeServiceModel.RadiographicNodeName.CHOOSING_SAFETY_OFFICER.ToString())
                    {
                        if (approvedNodeNames.Contains(WorkflowNodeServiceModel.RadiographicNodeName.REQUESTOR_INPUT.ToString())){
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.RadiographicNodeName.SUPERVISOR_APPROVE.ToString()))
                                existOngong = true;
                        }
                    }
                    else
                    {
                        if (!existOngong)
                        {
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                            existOngong = true;
                        }
                    }
                }
            }

            return nodeList;
        }
    }
}