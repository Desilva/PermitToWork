using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class FireImpairmentModel
    {
        //urutan tidak boleh diganti
        private List<string> NodeNameList = new List<string> { "REQUESTOR_APPROVE", "FIREWATCH_APPROVE", "SUPERVISOR_SCREENING", "CHOOSING_SO_DEPT_HEAD_FO", "SAFETY_OFFICER_APPROVE", "FACILITY_OWNER_APPROVE", "DEPT_HEAD_FO_APPROVE" };
        private List<string> CancellationNodeNameList = new List<string> { "CANCELLATION_REQUESTOR", "CANCELLATION_FIREWATCH", "CANCELLATION_SUPERVISOR", "CANCELLATION_SAFETY_OFFICER", "CANCELLATION_FACILITY_OWNER", "CANCELLATION_DEPT_HEAD_FO" };
        
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes"></param>
        public FireImpairmentModel(List<workflow_node> signals)
        {
            //kamus lokal
            List<WorkflowNodePresentationStub> cancellationNodes;
            List<workflow_node> cancellationSignals;
            WorkflowHelper helper = new WorkflowHelper();

            //algoritma
            NodeList = ProcessFINodes(signals.Where(m => NodeNameList.Contains(m.node_name)).ToList(), NodeNameList);

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
        ///     supervisor screening ok -> choosing safety officer ongoing, safety officer empty
        ///     safety officer reject -> choosing safety officer finish, supervisor screening ongoing
        /// </summary>
        /// <param name="signals">signals terkait workflow fire impairment saja, TIDAK TERMASUK signal cancellation</param>
        /// <param name="nodeNames"></param>
        /// <returns></returns>
        private List<WorkflowNodePresentationStub> ProcessFINodes(List<workflow_node> signals, List<string> nodeNames)
        {
            //kamus lokal
            List<string> approvedNodeNames = new List<string>(); //stack of node names
            string lastApproved, prevNodeName;
            int nodeNameIndex;
            List<WorkflowNodePresentationStub> nodeList;

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
                        //kasus khusus reject safety officer approve
                        if (node.node_name == WorkflowNodeServiceModel.FireImpairmentNodeName.SAFETY_OFFICER_APPROVE.ToString())
                        {
                            prevNodeName = WorkflowNodeServiceModel.FireImpairmentNodeName.SUPERVISOR_SCREENING.ToString();
                        }
                        else
                        {
                            nodeNameIndex = nodeNames.FindIndex(a => a == node.node_name);
                            prevNodeName = nodeNames[nodeNameIndex - 1];
                        }

                        if (approvedNodeNames.Remove(prevNodeName) == false)
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
                    if (node.NodeName == WorkflowNodeServiceModel.FireImpairmentNodeName.FIREWATCH_APPROVE.ToString() ||
                        node.NodeName == WorkflowNodeServiceModel.FireImpairmentNodeName.CHOOSING_SO_DEPT_HEAD_FO.ToString())
                    {
                        node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                    }
                    else if (node.NodeName == WorkflowNodeServiceModel.FireImpairmentNodeName.SUPERVISOR_SCREENING.ToString())
                    {
                        if (approvedNodeNames.Contains(WorkflowNodeServiceModel.FireImpairmentNodeName.FIREWATCH_APPROVE.ToString()))
                        {
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        }
                    }
                    else
                    {
                        if (node.NodeName == WorkflowNodeServiceModel.FireImpairmentNodeName.SAFETY_OFFICER_APPROVE.ToString())
                        {
                            if (approvedNodeNames.Contains(WorkflowNodeServiceModel.FireImpairmentNodeName.SUPERVISOR_SCREENING.ToString()) && approvedNodeNames.Contains(WorkflowNodeServiceModel.FireImpairmentNodeName.CHOOSING_SO_DEPT_HEAD_FO.ToString()))
                            {
                                node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                            }
                        }
                        else
                        {
                            node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                        }
                        break;
                    }
                }
            }

            return nodeList;
        }
    }
}