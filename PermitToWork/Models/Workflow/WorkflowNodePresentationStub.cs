using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Workflow
{
    public class WorkflowNodePresentationStub
    {
        public enum NodeStatus
        {
            EMPTY, ONGOING, FINISH
        }

        public string NodeName { get; set; }
        public NodeStatus Status { get; set; }
        public string StatusString
        {
            get
            {
                return Status.ToString();
            }
        }
    }
}