using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class WorkflowHelper
    {
        /// <summary>
        /// memproses nodes dengan diagram berbentuk seial
        /// </summary>
        /// <param name="signals">signal pekerjaan PTW</param>
        /// <param name="nodeNames">nama node terurut sesuai diagram</param>
        /// <returns></returns>
        public List<WorkflowNodePresentationStub> ProcessSerialNodes(List<workflow_node> signals, List<string> nodeNames)
        {
            //kamus lokal
            Stack<string> nodeNameStack = new Stack<string>(); //stack of node names
            string topStack, prevNodeName;
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
                if (node.status == (int)workflow_node.WorkflowNodeStatus.APPROVE) //approve signal
                {
                    nodeNameStack.Push(node.node_name);
                }
                else if (node.status == (int)workflow_node.WorkflowNodeStatus.REJECT) //reject signal
                {
                    topStack = nodeNameStack.FirstOrDefault();
                    if (topStack != null)
                    {
                        nodeNameIndex = nodeNames.FindIndex(a => a == node.node_name);
                        prevNodeName = nodeNames[nodeNameIndex - 1];
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
            foreach (WorkflowNodePresentationStub node in nodeList)
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

            return nodeList;
        }
    }
}