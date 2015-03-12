using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class CSEPModel
    {
        public List<string> NodeNameList = new List<string> { "REQUESTOR_INPUT", "SUPERVISOR_SCREENING", "FACILITY_OWNER_SCREENING", "GAS_TESTING", "REQUESTOR_APPROVE", "SUPERVISOR_APPROVE", "FACILITY_OWNER_APPROVE" };
        public List<WorkflowNodePresentationStub> NodeList { get; set; }

        /// <summary>
        /// parsing nodes dari service ke dalam NodeList
        /// </summary>
        /// <param name="nodes">signal terurut dari paling lama ke paling baru</param>
        public CSEPModel(List<workflow_node> nodes)
        {
            //kamus lokal
            Stack<string> nodeNameStack = new Stack<string>(); //stack of node names
            string topStack, prevNodeName;
            int nodeNameIndex;

            //algoritma
            //inisiasi NodeList
            NodeList = new List<WorkflowNodePresentationStub>();
            foreach (string name in NodeNameList)
                NodeList.Add(new WorkflowNodePresentationStub { NodeName = name, Status = WorkflowNodePresentationStub.NodeStatus.EMPTY });

            //mengisi nodes status ke stack
            nodes = nodes.OrderBy(m => m.id).ToList();
            foreach (workflow_node node in nodes)
            {
                if (node.status == (int)WorkflowNodeServiceModel.NodeStatus.APPROVED) //approve signal
                {
                    nodeNameStack.Push(node.node_name);
                }
                else if (node.status == (int)WorkflowNodeServiceModel.NodeStatus.REJECTED) //reject signal
                {
                    topStack = nodeNameStack.FirstOrDefault();
                    if (topStack != null)
                    {
                        nodeNameIndex = NodeNameList.FindIndex(a => a == node.node_name);
                        prevNodeName = NodeNameList[nodeNameIndex - 1];
                        if (topStack == prevNodeName)
                        {
                            nodeNameStack.Pop();
                        }
                        else
                        {
                            throw new Exception("Kesalahan dalam fungsi stack.");
                        }
                    }
                }
            }

            //set status setiap item di NodeList
            foreach (WorkflowNodePresentationStub node in NodeList)
            {
                if (nodeNameStack.Contains(node.NodeName))
                {
                    node.Status = WorkflowNodePresentationStub.NodeStatus.FINISH;
                }
                else
                {
                    node.Status = WorkflowNodePresentationStub.NodeStatus.ONGOING;
                    break;
                }
            }
        }
    }
}