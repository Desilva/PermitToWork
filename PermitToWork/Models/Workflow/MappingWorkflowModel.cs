using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.WorkingHeight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace PermitToWork.Models.Workflow
{
    public class MappingWorkflowModel
    {
        private star_energy_ptwEntities db;
        private WorkflowNodeServiceModel workflowNodeService;

        public MappingWorkflowModel()
        {
            this.db = new star_energy_ptwEntities();
            this.workflowNodeService = new WorkflowNodeServiceModel();
        }

        public bool MappingGeneralPermitWorkflow()
        {
            bool result = true;
            List<permit_to_work> PTWs = db.permit_to_work.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (permit_to_work ptw in PTWs)
            {
                nodes.Clear();
                if (ptw.status != (int)PtwEntity.statusPtw.CANCELLED)
                {
                    if (ptw.status >= (int)PtwEntity.statusPtw.CLEARANCECOMPLETE)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.REQUESTOR_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (ptw.status >= (int)PtwEntity.statusPtw.ACCSPV)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.SUPERVISOR_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (ptw.status >= (int)PtwEntity.statusPtw.CHOOSEASS)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.CHOOSING_ASSESSOR.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (ptw.status >= (int)PtwEntity.statusPtw.ACCASS)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.ASSESSOR_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (ptw.status >= (int)PtwEntity.statusPtw.ACCFO)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.FACILITY_OWNER_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }


                    if (ptw.status >= (int)PtwEntity.statusPtw.CANREQ)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_REQUESTOR.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }


                    if (ptw.status >= (int)PtwEntity.statusPtw.CANSPV)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_SUPERVISOR.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }


                    if (ptw.status >= (int)PtwEntity.statusPtw.CANASS)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_ASSESSOR.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }


                    if (ptw.status >= (int)PtwEntity.statusPtw.CANFO)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = ptw.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.GENERALPERMIT.ToString(),
                            node_name = WorkflowNodeServiceModel.GeneralPermitNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }
                    if (nodes.Count > 0)
                    {
                        string data = new JavaScriptSerializer().Serialize(nodes);
                        result = result && workflowNodeService.CreateNodeBulk(data);
                        if (result == false)
                        {
                            string a = "0";
                            result = false;
                        }
                    }
                }
            }

            return result;
        }

        public bool MappingCSEPWorkflow()
        {
            bool result = true;
            List<confined_space> CSEPs = db.confined_space.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.CSEP.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (confined_space csep in CSEPs)
            {
                nodes.Clear();
                if (csep.status >= (int)CsepEntity.CsepStatus.CREATE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.REQUESTOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (csep.status >= (int)CsepEntity.CsepStatus.SPVSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.SUPERVISOR_SCREENING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (csep.status >= (int)CsepEntity.CsepStatus.FOSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.FACILITY_OWNER_SCREENING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (csep.status >= (int)CsepEntity.CsepStatus.GASTESTER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.GAS_TESTING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (csep.status >= (int)CsepEntity.CsepStatus.ACCWORKLEADER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.REQUESTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (csep.status >= (int)CsepEntity.CsepStatus.ACCSPV)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (csep.status >= (int)CsepEntity.CsepStatus.ACCFO)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (csep.status >= (int)CsepEntity.CsepStatus.CANWORKLEADER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.CANCELLATION_INITIATOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (csep.status >= (int)CsepEntity.CsepStatus.CANSPV)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (csep.status >= (int)CsepEntity.CsepStatus.CANFO)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = csep.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.CSEP.ToString(),
                        node_name = WorkflowNodeServiceModel.CSEPNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingHotWorkWorkflow()
        {
            bool result = true;
            List<hot_work> HotWorks = db.hot_work.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (hot_work hotWork in HotWorks)
            {
                nodes.Clear();
                if (hotWork.status >= (int)HwEntity.statusHW.CREATE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.REQUESTOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (hotWork.status >= (int)HwEntity.statusHW.SPVSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.SUPERVISOR_SCREENING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (hotWork.status >= (int)HwEntity.statusHW.FOSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.FACILITY_OWNER_SCREENING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (hotWork.status >= (int)HwEntity.statusHW.GASTESTER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.GAS_TESTING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (hotWork.status >= (int)HwEntity.statusHW.ACCWORKLEADER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.REQUESTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (hotWork.status >= (int)HwEntity.statusHW.ACCSPV)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (hotWork.status >= (int)HwEntity.statusHW.ACCFO)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (hotWork.status >= (int)HwEntity.statusHW.CANWORKLEADER)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.CANCELLATION_INITIATOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (hotWork.status >= (int)HwEntity.statusHW.CANSPV)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (hotWork.status >= (int)HwEntity.statusHW.CANFO)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = hotWork.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.HOTWORK.ToString(),
                        node_name = WorkflowNodeServiceModel.HotWorkNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingFireImpairmentWorkflow()
        {
            bool result = true;
            List<fire_impairment> FireImpairments = db.fire_impairment.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (fire_impairment fireImpairment in FireImpairments)
            {
                nodes.Clear();
                if (fireImpairment.status >= (int)FIEntity.FIStatus.REQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.REQUESTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.FIREWATCHAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.FIREWATCH_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.SPVSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.SUPERVISOR_SCREENING.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.SOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CHOOSING_SO_DEPT_HEAD_FO.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });

                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.SAFETY_OFFICER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.FOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (fireImpairment.status >= (int)FIEntity.FIStatus.DEPTFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.DEPT_HEAD_FO_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANREQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_REQUESTOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANFIREWATCHAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_FIREWATCH.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANSPVSCREENING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANSOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_SAFETY_OFFICER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (fireImpairment.status >= (int)FIEntity.FIStatus.CANDEPTFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = fireImpairment.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.FIREIMPAIRMENT.ToString(),
                        node_name = WorkflowNodeServiceModel.FireImpairmentNodeName.CANCELLATION_DEPT_HEAD_FO.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingExcavationWorkflow()
        {
            bool result = true;
            List<excavation> Excavations = db.excavations.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (excavation excavation in Excavations)
            {
                nodes.Clear();
                if (excavation.status >= (int)ExcavationEntity.ExStatus.EDITANDSEND)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (excavation.status >= (int)ExcavationEntity.ExStatus.SHEAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });

                    if (excavation.facilities_signature != null)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = excavation.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            node_name = WorkflowNodeServiceModel.ExcavationNodeName.CIVIL_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (excavation.ei_signature != null)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = excavation.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            node_name = WorkflowNodeServiceModel.ExcavationNodeName.EANDI_APPROVE.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }
                }

                if (excavation.status >= (int)ExcavationEntity.ExStatus.REQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.REQUESTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (excavation.status >= (int)ExcavationEntity.ExStatus.FOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (excavation.status >= (int)ExcavationEntity.ExStatus.CANREQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_REQUESTOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (excavation.status >= (int)ExcavationEntity.ExStatus.CANSPVAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });

                    if (excavation.can_facilities_signature != null)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = excavation.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            node_name = WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_CIVIL.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }

                    if (excavation.can_ei_signature != null)
                    {
                        nodes.Add(new workflow_node
                        {
                            id_report = excavation.id,
                            report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                            node_name = WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_EANDI.ToString(),
                            status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                        });
                    }
                }


                if (excavation.status >= (int)ExcavationEntity.ExStatus.CANFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = excavation.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.EXCAVATION.ToString(),
                        node_name = WorkflowNodeServiceModel.ExcavationNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingWorkingAtHeightWorkflow()
        {
            bool result = true;
            List<working_height> WorkingAtHeights = db.working_height.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (working_height workingHeight in WorkingAtHeights)
            {
                nodes.Clear();
                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.EDITANDSEND)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.REQUESTOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.INSPECTORSIGN)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.INSPECTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.REQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.ERECTOR_REQUESTOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.SPVAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.FOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.CLOSING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.CANCELLATION_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.CANREQUESTORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.CANCELLATION_ERECTOR_REQUESTOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.CANSPVAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (workingHeight.status >= (int)WorkingHeightEntity.WHStatus.CANFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = workingHeight.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.WORKINGATHEIGHT.ToString(),
                        node_name = WorkflowNodeServiceModel.WorkingAtHeightNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingRadiographicWorkflow()
        {
            bool result = true;
            List<radiography> Radiographics = db.radiographies.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (radiography radiographic in Radiographics)
            {
                nodes.Clear();
                if (radiographic.status >= (int)RadEntity.RadStatus.EDITANDSEND)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.REQUESTOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.OPERATORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.OPERATOR_1_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.RADAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.OPERATOR_2_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.SPVAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.SOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.SAFETY_OFFICER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.FOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.CLOSING)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }


                if (radiographic.status >= (int)RadEntity.RadStatus.CANOPERATORAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_OPERATOR_1.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.CANRADAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_OPERATOR_2.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.CANSPVAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.CANSOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_SAFETY_OFFICER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (radiographic.status >= (int)RadEntity.RadStatus.CANFOAPPROVE)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = radiographic.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.RADIOGRAPHIC.ToString(),
                        node_name = WorkflowNodeServiceModel.RadiographicNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }

        public bool MappingLOTOWorkflow()
        {
            bool result = true;
            List<loto_permit> LOTOs = db.loto_permit.ToList();

            workflowNodeService.DeleteNode(WorkflowNodeServiceModel.DocumentType.LOTO.ToString());

            List<workflow_node> nodes = new List<workflow_node>();

            foreach (loto_permit loto in LOTOs)
            {
                nodes.Clear();
                if (loto.status >= (int)LotoEntity.LOTOStatus.SENDTOFO)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.SUPERVISOR_INPUT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.FOAPPLIED)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.FACILITY_OWNER_APPLICATION.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.INSPECTION)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.SUPERVISOR_INSPECT.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.SPVSIGN)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.SUPERVISOR_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.FOSIGN)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.FACILITY_OWNER_APPROVE.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.CANCELSPV)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.CANCELLATION_SUPERVISOR.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (loto.status >= (int)LotoEntity.LOTOStatus.LOTOCANCELLED)
                {
                    nodes.Add(new workflow_node
                    {
                        id_report = loto.id,
                        report_type = WorkflowNodeServiceModel.DocumentType.LOTO.ToString(),
                        node_name = WorkflowNodeServiceModel.LotoNodeName.CANCELLATION_FACILITY_OWNER.ToString(),
                        status = (byte)WorkflowNodeServiceModel.NodeStatus.APPROVED
                    });
                }

                if (nodes.Count > 0)
                {
                    string data = new JavaScriptSerializer().Serialize(nodes);
                    result = result && workflowNodeService.CreateNodeBulk(data);
                    if (result == false)
                    {
                        string a = "0";
                        result = false;
                    }
                }
            }

            return result;
        }
    }
}