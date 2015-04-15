using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class ExcavationModel
    {
        //urutan tidak boleh diganti
        private List<string> NodeNameList = new List<string> { "REQUESTOR_INPUT", "SUPERVISOR_APPROVE", "EANDI_APPROVE", "CIVIL_APPROVE", "ENVIRO_APPROVE", "REQUESTOR_APPROVE", "FACILITY_OWNER_APPROVE", "DEPT_HEAD_FO_APPROVE" };
        private List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_REQUESTOR", "CANCELLATION_SUPERVISOR", "CANCELLATION_EANDI", "CANCELLATION_CIVIL", "CANCELLATION_ENVIRO", "CANCELLATION_FACILITY_OWNER" };
        
        public List<WorkflowNodePresentationStub> NodeList { get; set; }
        private bool IsDisposalMoved { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes"></param>
        public ExcavationModel(List<workflow_node> signals, bool isDisposalMoved = false)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
            IsDisposalMoved = isDisposalMoved;
            if (!IsDisposalMoved)
            {
                NodeNameList.Remove(WorkflowNodeServiceModel.ExcavationNodeName.ENVIRO_APPROVE.ToString());
                CancellationNodeNameList.Remove(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_ENVIRO.ToString());
            }

            NodeList = ProcessExcavationNodes(signals.Where(m => NodeNameList.Contains(m.node_name)).ToList(), NodeNameList);

            cancellationSignals = signals.Where(m => CancellationNodeNameList.Contains(m.node_name)).ToList();
            if (cancellationSignals.Count() > 0)
            {
                cancellationNodes = ProcessExcavationCancellationNodes(cancellationSignals, CancellationNodeNameList);
                NodeList = NodeList.Concat(cancellationNodes).ToList();
            }
        }

        /// <summary>
        /// memproses list of signals
        /// kasus (!IsDisposalMoved)
        ///     supervisor approve ->  EI & civil ongoing
        ///     EI & civil approve -> requestor approve ongoing
        ///     EI reject / civil reject -> supervisor approve ongoing
        ///     requestor reject -> EI & civil ongoing
        /// kasus (IsEnviro)
        ///     supervisor approve ->  EI & civil ongoing
        ///     EI & civil approve          -> enviro approve ongoing
        ///     EI reject / civil reject    -> supervisor approve ongoing
        ///     enviro approve              -> requestor approve ongoing
        ///     enviro reject -> EI & civil ongoing
        /// </summary>
        /// <param name="signals"></param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessExcavationNodes(List<workflow_node> signals, List<string> nodeNames)
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
                        if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString())
                        {
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString()))
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString());
                            }
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.SUPERVISOR_APPROVE.ToString());
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString())
                        {
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString()))
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString());
                            }
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.SUPERVISOR_APPROVE.ToString());
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_APPROVE.ToString())
                        {
                            if (!IsDisposalMoved)
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString());
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString());
                            }
                            else
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.ENVIRO_APPROVE.ToString());
                            }
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.ENVIRO_APPROVE.ToString())
                        {
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString());
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString());
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
                    if (node.NodeName == WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString() ||
                        node.NodeName == WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString())
                    {
                        node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        existOngoing = true;
                    }
                    else
                    {
                        if (!existOngoing)
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        break;
                    }
                }
            }

            return nodeList;
        }

        /// <summary>
        /// memproses list of signals kasus cancellation
        /// kasus (!IsDisposalMoved)
        ///     supervisor approve ->  EI & civil ongoing
        ///     EI & civil approve -> requestor approve ongoing
        ///     EI reject / civil reject -> supervisor approve ongoing
        ///     requestor reject -> EI & civil ongoing
        /// kasus (IsDisposalMoved)
        ///     supervisor approve          ->  EI & civil ongoing
        ///     EI & civil approve          -> enviro approve ongoing
        ///     EI reject / civil reject    -> supervisor approve ongoing
        ///     enviro approve              -> fo approve ongoing
        ///     enviro reject               -> EI & civil ongoing
        /// </summary>
        /// <param name="signals"></param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessExcavationCancellationNodes(List<workflow_node> signals, List<string> nodeNames)
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
                        if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString())
                        {
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString()))
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString());
                            }
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_SUPERVISOR.ToString());
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString())
                        {
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString()))
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString());
                            }
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_SUPERVISOR.ToString());
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_FACILITY_OWNER.ToString())
                        {
                            if (!IsDisposalMoved)
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString());
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString());
                            }
                            else
                            {
                                nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_ENVIRO.ToString());
                            }
                        }
                        else if (node.node_name == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_ENVIRO.ToString())
                        {
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString());
                            nodeNamesToBeDeleted.Add(WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString());
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
                    if (node.NodeName == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString() ||
                        node.NodeName == WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString())
                    {
                        node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        existOngoing = true;
                    }
                    else
                    {
                        if (!existOngoing)
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        break;
                    }
                }
            }

            return nodeList;
        }
    }
}