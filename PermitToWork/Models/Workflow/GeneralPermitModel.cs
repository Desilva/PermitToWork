using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class GeneralPermitModel
    {
        //urutan tidak boleh diganti
        private List<string> NodeNameList = new List<string> { "REQUESTOR_APPROVE", "SUPERVISOR_APPROVE", "CHOOSING_ASSESSOR", "ASSESSOR_APPROVE", "FACILITY_OWNER_APPROVE" };
        private List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_REQUESTOR", "CANCELLATION_SUPERVISOR", "CANCELLATION_ASSESSOR", "CANCELLATION_FACILITY_OWNER" };
        
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes"></param>
        public GeneralPermitModel(List<workflow_node> signals)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
            NodeList = ProcessGPNodes(signals.Where(m => NodeNameList.Contains(m.node_name)).ToList(), NodeNameList);

            cancellationSignals = signals.Where(m => CancellationNodeNameList.Contains(m.node_name)).ToList();
            if (cancellationSignals.Count() > 0)
            {
                cancellationNodes = ProcessGPCancellationNodes(cancellationSignals, CancellationNodeNameList);
                NodeList = NodeList.Concat(cancellationNodes).ToList();
            }
        }

        /// <summary>
        /// memproses list of signals
        /// kasus
        ///     assessor reject -> choosing assessor finish, supervisor screening ongoing
        ///     facility owner reject -> assessor approval empty, supervisor screening ongoing
        /// </summary>
        /// <param name="signals">signals terkait workflow general permit saja, TIDAK TERMASUK signal cancellation</param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessGPNodes(List<workflow_node> signals, List<string> nodeNames)
        {
            //kamus lokal
            List<string> approvedNodeNames = new List<string>(); //stack of node names
            List<string> nodeNamesToBeDeleted;
            string lastApproved;
            int nodeNameIndex;
            List<WorkflowNodePresentationStub> nodeList;
            bool existOngoing = false;

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
                    nodeNamesToBeDeleted = new List<string>();
                    lastApproved = approvedNodeNames.LastOrDefault();
                    if (lastApproved != null)
                    {
                        //kasus khusus reject assessor
                        if (node.node_name == WorkflowNodeServiceModel.GeneralPermitNodeName.ASSESSOR_APPROVE.ToString())
                        {
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString());
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.GeneralPermitNodeName.FACILITY_OWNER_APPROVE.ToString())
                        {
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.GeneralPermitNodeName.ASSESSOR_APPROVE.ToString());
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString());
                        }
                        else
                        {
                            nodeNameIndex = nodeNames.FindIndex(a => a == node.node_name);
                            nodeNamesToBeDeleted.Add(nodeNames[nodeNameIndex - 1]);
                        }

                        foreach (string nodeName in nodeNamesToBeDeleted)
                        {
                            if (approvedNodeNames.Remove(nodeName) == false)
                            {
                                throw new Exception("Kesalahan dalam fungsi pemrosesan signal.");
                            }
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
                    if (!existOngoing)
                    {
                        node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        existOngoing = true;
                    }

                    if (node.NodeName == WorkflowNodeServiceModel.GeneralPermitNodeName.REQUESTOR_APPROVE.ToString() || 
                        node.NodeName == WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString() ||
                        node.NodeName == WorkflowNodeServiceModel.GeneralPermitNodeName.CHOOSING_ASSESSOR.ToString())
                    {
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return nodeList;
        }

        /// <summary>
        /// memproses list of cancellation signals
        /// kasus
        ///     assessor reject -> choosing assessor finish, supervisor screening ongoing
        ///     facility owner reject -> assessor approval empty, supervisor screening ongoing
        /// </summary>
        /// <param name="signals">signals terkait workflow cancellation general permit saja</param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessGPCancellationNodes(List<workflow_node> signals, List<string> nodeNames)
        {
            //kamus lokal
            List<string> approvedNodeNames = new List<string>(); //stack of node names
            List<string> nodeNamesToBeDeleted;
            string lastApproved;
            int nodeNameIndex;
            List<WorkflowNodePresentationStub> nodeList;
            bool existOngoing = false;

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
                    nodeNamesToBeDeleted = new List<string>();
                    lastApproved = approvedNodeNames.LastOrDefault();
                    if (lastApproved != null)
                    {
                        //kasus khusus reject assessor
                        if (node.node_name == WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_FACILITY_OWNER.ToString())
                        {
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_ASSESSOR.ToString());
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_SUPERVISOR.ToString());
                        }
                        else
                        {
                            nodeNameIndex = nodeNames.FindIndex(a => a == node.node_name);
                            nodeNamesToBeDeleted.Add(nodeNames[nodeNameIndex - 1]);
                        }

                        foreach (string nodeName in nodeNamesToBeDeleted)
                        {
                            if (approvedNodeNames.Remove(nodeName) == false)
                            {
                                throw new Exception("Kesalahan dalam fungsi pemrosesan signal.");
                            }
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
                    node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                    break;
                }
            }

            return nodeList;
        }
    }
}