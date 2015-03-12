using PermitToWork.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace PermitToWork.Controllers
{
    public class WorkflowController : Controller
    {
        public string CSEP(int id)
        {
            CSEPModel model = new CSEPModel(new workflow_node().FindAllNode(id,WorkflowNodeServiceModel.DocumentType.CSEP.ToString()));

            return new JavaScriptSerializer().Serialize(model.NodeList);
        }
    }
}
