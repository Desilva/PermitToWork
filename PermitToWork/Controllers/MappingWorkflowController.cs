using PermitToWork.Models.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MappingWorkflowController : Controller
    {
        private MappingWorkflowModel mappingWorkflow;
        public bool GeneralPermit() {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingGeneralPermitWorkflow();
            return result;
        }

        public bool CSEP()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingCSEPWorkflow();
            return result;
        }

        public bool HotWork()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingHotWorkWorkflow();
            return result;
        }

        public bool FireImpairment()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingFireImpairmentWorkflow();
            return result;
        }
    }
}