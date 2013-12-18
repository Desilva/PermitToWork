using PermitToWork.Models;
using PermitToWork.Models.Hira;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class PtwController : Controller
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
            entity.generatePtwNumber(listPtw.getLastPtw().ptw_no);
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
        public JsonResult Add(PtwEntity ptw, int hw_need, IList<string> hiras)
        {
            ListPtw listPtw = new ListPtw();
            ptw.generatePtwNumber(listPtw.getLastPtw().ptw_no);
            int ret = ptw.addPtw();

            ListHira listHira = new ListHira();
            listHira.changeIdPtw(hiras.ToList(),ptw.id);

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
            if (assessor_id != null)
            {
                UserEntity assesor = new UserEntity(assessor_id.Value);
                ptw.assignAssessor(assesor);
                ptw.sendEmailAssessor(fullUrl(), 0);
            }
            string retVal = ptw.supervisorAccApproval(user);
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
        public JsonResult assessorAcc(int id, int user_id, int? fo_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            if (fo_id != null)
            {
                UserEntity fo = new UserEntity(fo_id.Value);
                ptw.assignFO(fo);
            }
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorAccApproval(user);
            ptw.sendEmailFo(fullUrl(), 0);
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
            ptw.sendEmailAssessor(fullUrl(), 0, 1, comment);
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
        public JsonResult assessorCan(int id, int user_id)
        {
            UserEntity user = new UserEntity(user_id);
            PtwEntity ptw = new PtwEntity(id);
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorCanApproval(user);
            ptw.sendEmailFo(fullUrl(), 1);
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
            ptw.sendEmailAssessor(fullUrl(), 1, 1, comment);
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
            hw.generateHwNumber(hwLast.hw_no);
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
