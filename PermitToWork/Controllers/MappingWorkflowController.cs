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
        public bool Excavation()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingExcavationWorkflow();
            return result;
        }

        public bool WorkingAtHeight()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingWorkingAtHeightWorkflow();
            return result;
        }

        public bool Radiographic()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingRadiographicWorkflow();
            return result;
        }

        public bool LOTO()
        {
            mappingWorkflow = new MappingWorkflowModel();
            bool result = mappingWorkflow.MappingLOTOWorkflow();
            return result;
        }
    }
}