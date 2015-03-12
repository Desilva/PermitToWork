using PermitToWork.Models;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermitToWork.Utilities;

namespace PermitToWork.Controllers
{
    public class FiController : Controller
    {
        //
        // GET: /Fi/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult Edit(int id)
        {

            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            FIEntity entity = new FIEntity(id, user, listUser);
            entity.getPtw(user, listUser);
            entity.getHiraNo();
            bool[] isCanEdit = new bool[14];

            isCanEdit[0] = entity.isCanEditFormRequestor(user, listUser);
            isCanEdit[1] = entity.isCanEditAssign(user, listUser);
            isCanEdit[2] = entity.isCanEditApproveFireWatch(user, listUser);
            isCanEdit[3] = entity.isCanEditFormSPV(user, listUser);
            isCanEdit[4] = entity.isCanEditApproveSO(user, listUser);
            isCanEdit[5] = entity.isCanEditApproveFO(user, listUser);
            isCanEdit[6] = entity.isCanEditApproveDeptHead(user, listUser);
            isCanEdit[7] = entity.isCanEditCancel(user, listUser);
            isCanEdit[8] = entity.isCanEditApproveRequestorCancel(user, listUser);
            isCanEdit[9] = entity.isCanEditApproveFireWatchCancel(user, listUser);
            isCanEdit[10] = entity.isCanEditFormSPVCancel(user, listUser);
            isCanEdit[11] = entity.isCanEditApproveSOCancel(user, listUser);
            isCanEdit[12] = entity.isCanEditApproveFOCancel(user, listUser);
            isCanEdit[13] = entity.isCanEditApproveDeptHeadCancel(user, listUser);

            ViewBag.isCanEdit = isCanEdit;

            ViewBag.position = "Edit";
            ViewBag.listUser = listUser;

            var listSO = new List<SelectListItem>();
            if (isCanEdit[1]) {
                var listSOs = new MstSOEntity().getListMstSO(user, listUser);
                foreach (MstSOEntity sect in listSOs)
                {
                    listSO.Add(new SelectListItem
                    {
                        Text = sect.user.alpha_name,
                        Value = sect.user.id.ToString(),
                        Selected = false
                    });
                }
            }
            ViewBag.listSO = listSO;

            var listDeptHead = new List<SelectListItem>();
            if (isCanEdit[1])
            {
                var listDeptHeads = new MstDeptHeadEntity().getListMstDeptHead(user, listUser);
                foreach (MstDeptHeadEntity sect in listDeptHeads)
                {
                    listDeptHead.Add(new SelectListItem
                    {
                        Text = sect.user.alpha_name,
                        Value = sect.user.id.ToString(),
                        Selected = false
                    });
                }
            }
            ViewBag.listDeptHead = listDeptHead;
            ViewBag.ptwStatus = entity.ptw_status;
            return PartialView("create", entity);
        }

        [HttpPost]
        public JsonResult SaveAsDraft(FIEntity fi, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.saveAsDraft(who);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveComplete(FIEntity fi, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & fi.saveAsDraft(who);
            fi = new FIEntity(fi.id, user);
            retVal = retVal & fi.signClearance(who, user);
            retVal = retVal & fi.sendToUser(who + 1, 1, fullUrl(), user);

            if (who == 1 && !fi.isExistFO())
            {
                sendEmailFO(fi);
            }
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult rejectFIPermit(FIEntity fi, string comment, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & fi.saveAsDraft(who);
            fi = new FIEntity(fi.id, user);
            retVal = retVal & fi.rejectClearance(who);
            retVal = retVal & fi.sendToUser(who - 1, 2, fullUrl(), user, comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        //[HttpPost]
        //public JsonResult SaveAsDraftPreScreening(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = fi.SavePreScreening(type);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult SaveCompletePreScreening(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = fi.SavePreScreening(type);
        //    fi = new FIEntity(fi.id, user);
        //    fi.completePreScreening(type, user, fullUrl());
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult RejectPreScreening(FIEntity fi, int who, string comment)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = fi.SavePreScreening(who);
        //    fi = new FIEntity(fi.id, user);
        //    retVal = fi.rejectPreScreening(who, fullUrl(), comment);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        [HttpPost]
        public JsonResult SaveAssignSO(FIEntity fi)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;

            if (fi.acc_so != null)
            {
                UserEntity so = new UserEntity(Int32.Parse(fi.acc_so), user.token, user);
                int a = fi.assignSO(so, fullUrl());
                retVal = retVal & a;
            }

            if (fi.acc_dept_head != null)
            {
                UserEntity deptHead = new UserEntity(Int32.Parse(fi.acc_dept_head), user.token, user);
                retVal &= fi.assignDeptHead(deptHead, fullUrl());
            }
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        //[HttpPost]
        //public JsonResult ApproveFIPermit(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    ResponseModel response = fi.approvePermit(user, type, fullUrl());
        //    return Json(new { status = response.status, message = response.message });
        //}

        [HttpPost]
        public JsonResult CancelFIPermit(FIEntity fi)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            fi = new FIEntity(fi.id, user);
            retVal = retVal & fi.signClearanceCancel(1, user);
            retVal = retVal & fi.sendToUserCancel(2, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404", message = "Fire Impairment Permit Cancelled." });
        }

        [HttpPost]
        public JsonResult SaveAsDraftCancel(FIEntity fi, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.saveAsDraftCancel(who);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveCompleteCancel(FIEntity fi, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & fi.saveAsDraftCancel(who);
            fi = new FIEntity(fi.id, user);
            retVal = retVal & fi.signClearanceCancel(who, user);
            retVal = retVal & fi.sendToUserCancel(who + 1, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult rejectFIPermitCancel(FIEntity fi, string comment, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & fi.saveAsDraftCancel(who);
            fi = new FIEntity(fi.id, user);
            retVal = retVal & fi.rejectClearanceCancel(who);
            retVal = retVal & fi.sendToUserCancel(who - 1, 2, fullUrl(), user, comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        //[HttpPost]
        //public JsonResult SaveAsDraftCancelScreening(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = fi.SaveCancelScreening(type, user);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult SaveCompleteCancelScreening(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = fi.SaveCancelScreening(type, user, true, fullUrl());
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult ApproveFIPermitCancel(FIEntity fi, int type)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    ResponseModel response = fi.approvePermitCancel(user, type, fullUrl());
        //    return Json(new { status = response.status, message = response.message });
        //}

        #region set facility owner

        private string sendEmailFO(FIEntity fi)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            List<UserEntity> listHWFO = listUser.GetHotWorkFO();

            fi.sendEmailFO(listHWFO, fullUrl(), userLogin.token, userLogin);

            return "200";
        }

        // url to set who is the supervisor
        public ActionResult SetFacilityOwner(string a, string b, string c)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailfo";

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

                    FIEntity fi = new FIEntity(ptw_id,userLogin);
                    UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);

                    if (!fi.isExistFO())
                    {
                        fi.assignFO(user_id);
                        PtwEntity ptw = new PtwEntity(fi.id_ptw.Value, user);
                        if (ptw.acc_fo == null)
                        {
                            ptw.assignFO(user);
                        }

                        return RedirectToAction("Index", "Home", new { p = "FI/Edit/" + fi.id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { e = "402" });
                    }
                    // Session["user"] = user;


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

        #region utilities

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;
            return fullUrl;
        }

        #endregion

    }
}
