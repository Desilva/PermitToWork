using PermitToWork.Models;
using PermitToWork.Models.Hira;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class PtwController : PdfViewController
    {
        //
        // GET: /Ptw/
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Create() {
            PtwEntity entity = new PtwEntity();
            ViewBag.position = "Create";
            ViewBag.listUser = new ListUser();
            ListPtw listPtw = new ListPtw();
            ViewBag.listFO = new MstFOEntity().getListMstFO();
            ViewBag.listAssessor = new MstAssessorEntity().getListAssessor();

            var listDepartment = new List<SelectListItem>();
            var listDept = new MstDepartmentEntity().getListMstDepartment();
            foreach (MstDepartmentEntity dept in listDept) {
                listDepartment.Add(new SelectListItem
                {
                    Text = dept.department,
                    Value = dept.id.ToString(),
                });
            }
            ViewBag.listDepartment = listDepartment;

            var listSection = new List<SelectListItem>();
            var listSect = new MstSectionEntity().getListMstSection();
            foreach (MstSectionEntity sect in listSect)
            {
                listSection.Add(new SelectListItem
                {
                    Text = sect.section,
                    Value = sect.id.ToString(),
                });
            }
            ViewBag.listSection = listSection;

            var listTotalCrew = new List<SelectListItem>();
            for (int i = 1; i <= 100; i++)
            {
                listTotalCrew.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                });
            }
            ViewBag.listTotalCrew = listTotalCrew;

            UserEntity user = Session["user"] as UserEntity;

            entity.ptw_holder_no = new MstPtwHolderNoEntity(user.id, 1);
            entity.requestor_ptw_holder_no = entity.ptw_holder_no.id;

            entity.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "");
            return PartialView(entity);
        }

        public ActionResult Edit(int id)
        {
            PtwEntity entity = new PtwEntity(id);
            UserEntity user = Session["user"] as UserEntity;
            ViewBag.isRequestor = entity.isRequestor(user);
            if (entity.status < (int)PtwEntity.statusPtw.CANCEL)
            {
                ViewBag.isAccSupervisor = entity.isAccSupervisor(user);
                ViewBag.isAccAssessor = entity.isAccAssessor(user);
                ViewBag.isAccFO = entity.isAccFO(user);
            }
            else
            {
                ViewBag.isCancel = true;
                ViewBag.isCanSupervisor = entity.isCanSupervisor(user);
                ViewBag.isCanAssessor = entity.isCanAssessor(user);
                ViewBag.isCanFO = entity.isCanFO(user);
            }
            ViewBag.isCanEdit = entity.isCanEdit(user);
            ViewBag.isClearenceComplete = entity.isAllClearanceComplete();
            ViewBag.isClearenceClose = entity.isAllClearanceClose();
            ViewBag.position = "Edit";
            ViewBag.listUser = new ListUser();
            ViewBag.listFO = new MstFOEntity().getListMstFO();
            ViewBag.listAssessor = new MstAssessorEntity().getListAssessor();

            var listDepartment = new List<SelectListItem>();
            var listDept = new MstDepartmentEntity().getListMstDepartment();
            foreach (MstDepartmentEntity dept in listDept)
            {
                listDepartment.Add(new SelectListItem
                {
                    Text = dept.department,
                    Value = dept.id.ToString(),
                    Selected = entity.dept_requestor == dept.id ? true : false
                });
            }
            ViewBag.listDepartment = listDepartment;

            var listSection = new List<SelectListItem>();
            var listSect = new MstSectionEntity().getListMstSection();
            foreach (MstSectionEntity sect in listSect)
            {
                listSection.Add(new SelectListItem
                {
                    Text = sect.section,
                    Value = sect.id.ToString(),
                    Selected = entity.section == sect.id ? true : false
                });
            }
            ViewBag.listSection = listSection;
            var listTotalCrew = new List<SelectListItem>();
            for (int i = 1; i <= 100; i++)
            {
                listTotalCrew.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = entity.total_crew == i.ToString() ? true : false
                });
            }
            ViewBag.listTotalCrew = listTotalCrew;
            return PartialView("create", entity);
        }

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;

            return fullUrl;
        }

        [HttpPost]
        public JsonResult GenerateNewNumber(int user_id)
        {
            List<MstFOEntity> listFo = new MstFOEntity().getListMstFO();
            MstFOEntity fo = listFo.Find(p => p.id_employee == user_id);
            ListPtw listPtw = new ListPtw();
            
            PtwEntity entity = new PtwEntity();
            entity.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "", fo == null ? null : fo.fo_code);

            return Json(new { status = "200", message = "", ptw_number = entity.ptw_no });
        }

        [HttpPost]
        public JsonResult Add(PtwEntity ptw, int hw_need, IList<string> hiras)
        {
            List<MstFOEntity> listFo = new MstFOEntity().getListMstFO();
            int fo_id = Int32.Parse(ptw.acc_fo);
            MstFOEntity fo = listFo.Find(p => p.id_employee == fo_id);
            ListPtw listPtw = new ListPtw();
            ptw.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "", fo == null ? null : fo.fo_code);
            int ret = ptw.addPtw();

            if (ptw.acc_fo != null)
            {
                UserEntity fos = new UserEntity(Int32.Parse(ptw.acc_fo));
                ptw.assignFO(fos);
            }

            ListHira listHira = new ListHira();
            if (hiras != null)
            {
                listHira.changeIdPtw(hiras.ToList(), ptw.id);
            }

            if (hw_need == 1)
            {
                HwEntity hw = addHotWork(ptw.id);
                ptw.setHw(hw.id, (int)PtwEntity.statusClearance.NOTCOMPLETE);
            }



            ListUser listUser = new ListUser();
            List<UserEntity> listSpv = listUser.GetSupervisor(Session["user"] as UserEntity);

            

            // send email to each supervisor
            ptw.sendEmailSpv(listSpv, fullUrl());

            if (ret == 1)
            {
                return Json(new { status = "200", message = "", id = ptw.id });
            }
            else
            {
                return Json(new { status = "400", message = "There is error when saving data to database. Please check again your data." });
            }
        }

        [HttpPost]
        public JsonResult EditPtw(PtwEntity ptw)
        {
            if (ptw.acc_assessor != null)
            {
                UserEntity assesor = new UserEntity(Int32.Parse(ptw.acc_assessor));
                ptw.assignAssessor(assesor);
                ptw.setStatus((int)PtwEntity.statusPtw.CHOOSEASS);
                ptw.sendEmailAssessor(fullUrl(), 0);
            }
            int ret = ptw.editPtw();
            PtwEntity ptw_new = new PtwEntity(ptw.id);
            if (ptw.hw_need == 1 && ptw_new.hw_id == null)
            {
                HwEntity hw = addHotWork(ptw.id);
                ptw.setHw(hw.id, (int)PtwEntity.statusClearance.NOTCOMPLETE);
            }
            else if (ptw.hw_need == 0 && ptw_new.hw_id != null)
            {
                deleteHotWork(ptw.id, ptw_new.hw_id.Value);
                ptw.setHw(null, null);
            }

            if (ret == 1)
            {
                return Json(new { status = "200", message = "" });
            }
            else
            {
                return Json(new { status = "400", message = "There is error when saving data to database. Please check again your data." });
            }
        }

        public ActionResult Print(int id)
        {
            PtwEntity ptw = new PtwEntity(id);

            List<UserEntity> listUser = new ListUser().listUser;

            int a = Int32.Parse(ptw.acc_ptw_requestor);
            ptw.acc_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_supervisor);
            ptw.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_supervisor_delegate != "" && ptw.acc_supervisor_delegate != null ? ptw.acc_supervisor_delegate : "0");
            ptw.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.acc_assessor);
            ptw.acc_assessor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_assessor_delegate != "" && ptw.acc_assessor_delegate != null ? ptw.acc_assessor_delegate : "0");
            ptw.acc_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.acc_fo);
            ptw.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_fo_delegate != "" && ptw.acc_fo_delegate != null ? ptw.acc_fo_delegate : "0");
            ptw.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_ptw_requestor);
            ptw.can_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_supervisor);
            ptw.can_supervisor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_supervisor_delegate != "" && ptw.can_supervisor_delegate != null ? ptw.can_supervisor_delegate : "0");
            ptw.can_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_assessor);
            ptw.can_assessor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_assessor_delegate != "" && ptw.can_assessor_delegate != null ? ptw.can_assessor_delegate : "0");
            ptw.can_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_fo);
            ptw.can_fo = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_fo_delegate != "" && ptw.can_fo_delegate != null ? ptw.can_fo_delegate : "0");
            ptw.can_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";

            return this.ViewPdf("", "Print", ptw);
        }

        #region approve and reject PTW and Cancellation PTW

        [HttpPost]
        public JsonResult requestorAcc(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.requestorAccApproval(user);
            ptw.sendEmailSupervisor(fullUrl(), 0);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAcc(int id, int user_id, int? assessor_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            //if (assessor_id != null)
            //{
            //    UserEntity assesor = new UserEntity(assessor_id.Value);
            //    ptw.assignAssessor(assesor);
                
            //}
            string retVal = ptw.supervisorAccApproval(user);
            if (ptw.acc_assessor != null)
            {
                ptw.sendEmailAssessor(fullUrl(), 0);
            }
            else
            {
                ptw.sendEmailFo(fullUrl(), 0);
            }
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAccReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.supervisorAccReject(user, comment);
            ptw.sendEmailRequestor(fullUrl(), 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorAcc(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            //if (fo_id != null)
            //{
            //    UserEntity fo = new UserEntity(fo_id.Value);
            //    ptw.assignFO(fo);
            //}
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorAccApproval(user);
            ptw.sendEmailFo(fullUrl(), 0, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorAccReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.assessorAccReject(user,comment);
            ptw.sendEmailSupervisor(fullUrl(), 0, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAcc(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.fOAccApproval(user);
            ptw.sendEmailRequestorPermitCompleted(fullUrl(), 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAccReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.fOAccReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), 0, 1, comment);
            return Json(new { status = retVal });
        }

        public JsonResult cancelPtw(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.cancelPtw(user);
            ptw.sendEmailSupervisor(fullUrl(), 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorCan(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.requestorCanApproval(user);
            ptw.sendEmailSupervisor(fullUrl(), 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCan(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.supervisorCanApproval(user);
            ptw.sendEmailAssessor(fullUrl(), 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCanReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.supervisorCanReject(user, comment);
            ptw.sendEmailRequestor(fullUrl(), 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorCan(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorCanApproval(user);
            ptw.sendEmailFo(fullUrl(), 1, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorCanReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.assessorCanReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), 1, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCan(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            
            string retVal = ptw.fOCanApproval(user);
            ptw.sendEmailRequestorPermitCompleted(fullUrl(), 2);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCanReject(int id, int user_id, string comment)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            string retVal = ptw.fOCanReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), 1, 1, comment);
            return Json(new { status = retVal });
        }

        #endregion


        [HttpPost]
        public JsonResult Binding()
        {
            ListPtw listPtw = new ListPtw();
            var result = listPtw.listPtw;
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        #region extends PTW

        [HttpPost]
        public JsonResult Extends(int id)
        {
            // time
            // status PTW
            // is requestor

            return Json(new { status = "200" });
        }

        public ActionResult Extend(int id)
        {
            PtwEntity entity = new PtwEntity(id);
            PtwEntity ptw = new PtwEntity();
            ViewBag.listUser = new ListUser();
            ptw.extendPtw(entity);

            //UserEntity user = Session["user"] as UserEntity;
            //ViewBag.isRequestor = ptw.isRequestor(user);
            //if (ptw.status < (int)PtwEntity.statusPtw.CANCEL)
            //{
            //    ViewBag.isAccSupervisor = ptw.isAccSupervisor(user);
            //    ViewBag.isAccAssessor = ptw.isAccAssessor(user);
            //    ViewBag.isAccFO = ptw.isAccFO(user);
            //}
            //else
            //{
            //    ViewBag.isCancel = true;
            //    ViewBag.isCanSupervisor = ptw.isCanSupervisor(user);
            //    ViewBag.isCanAssessor = ptw.isCanAssessor(user);
            //    ViewBag.isCanFO = ptw.isCanFO(user);
            //}
            //ViewBag.isCanEdit = ptw.isCanEdit(user);
            //ViewBag.isClearenceComplete = ptw.isAllClearanceComplete();
            //ViewBag.position = "Edit";
            
            return Edit(ptw.id);
        }

        #endregion

        #region setSupervisor

        // url to set who is the supervisor
        public ActionResult SetSupervisor(string a, string b, string c)
        {
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailsupervisor";

            string countSeal = Base64.MD5Seal(a + salt + val);

            if (countSeal == b)
            {
                string decodeElement = Base64.Base64Decode(c);

                if (decodeElement.Contains(salt) && decodeElement.Contains(val))
                {
                    decodeElement = decodeElement.Replace(salt, "#");
                    decodeElement = decodeElement.Replace(val, "#");

                    string[] s = decodeElement.Split('#');

                    int user_id = Int32.Parse(s[1]);
                    int ptw_id = Int32.Parse(s[2]);

                    PtwEntity ptw = new PtwEntity(ptw_id);
                    UserEntity user = new UserEntity(user_id);

                    if (ptw.acc_supervisor == null)
                    {
                        ptw.assignSupervisor(user);
                        if (ptw.hw_id != null)
                        {
                            HwEntity hw = new HwEntity(ptw.hw_id.Value);
                            if (hw.acc_supervisor == null)
                                hw.assignSupervisor(user);
                        }
                        return RedirectToAction("Index", "Home", new { p = "Ptw/Edit/" + ptw.id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { e = "401" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { e = "404" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { e = "404" });
            }
        }

        #endregion

        #region addClearancePermit

        private HwEntity addHotWork(int id)
        {
            PtwEntity ptw = new PtwEntity(id);
            HwEntity hw = new HwEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description);
            HwEntity hwLast = new HwEntity("0");
            hw.generateHwNumber(hwLast.hw_no,ptw.ptw_no);
            hw.addHotWork();

            return hw;
        }

        public string deleteHotWork(int id, int hw_id)
        {
            PtwEntity ptw = new PtwEntity(id);
            HwEntity hw = new HwEntity(hw_id);
            hw.deleteHotWork();

            return "200";
        }

        #endregion
    }
}
