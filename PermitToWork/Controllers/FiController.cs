using PermitToWork.Models;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            FIEntity entity = new FIEntity(id, user);

            bool[] isCanEdit = new bool[18];

            isCanEdit[0] = entity.isCanEditFormRequestor(user);
            isCanEdit[1] = entity.isCanEditAssign(user);
            isCanEdit[2] = entity.isCanEditFormSPV(user);
            isCanEdit[3] = entity.isCanEditFormSO(user);
            isCanEdit[4] = entity.isCanEditFormFO(user);
            isCanEdit[5] = entity.isCanEditApproveRequestor(user);
            isCanEdit[6] = entity.isCanEditApproveFireWatch(user);
            isCanEdit[7] = entity.isCanEditApproveSO(user);
            isCanEdit[8] = entity.isCanEditApproveFO(user);
            isCanEdit[9] = entity.isCanEditApproveDeptHead(user);
            isCanEdit[10] = entity.isCanEditFormSPVCancel(user);
            isCanEdit[11] = entity.isCanEditFormSOCancel(user);
            isCanEdit[12] = entity.isCanEditFormFOCancel(user);
            isCanEdit[13] = entity.isCanEditApproveRequestorCancel(user);
            isCanEdit[14] = entity.isCanEditApproveFireWatchCancel(user);
            isCanEdit[15] = entity.isCanEditApproveSOCancel(user);
            isCanEdit[16] = entity.isCanEditApproveFOCancel(user);
            isCanEdit[17] = entity.isCanEditApproveDeptHeadCancel(user);

            ViewBag.isCanEdit = isCanEdit;

            ViewBag.position = "Edit";
            ViewBag.listUser = new ListUser(user.token, user.id);

            var listSO = new List<SelectListItem>();
            var listSOs = new MstSOEntity().getListMstSO(user);
            foreach (MstSOEntity sect in listSOs)
            {
                listSO.Add(new SelectListItem
                {
                    Text = sect.user.alpha_name,
                    Value = sect.user.id.ToString(),
                    Selected = false
                });
            }
            ViewBag.listSO = listSO;

            var listDeptHead = new List<SelectListItem>();
            var listDeptHeads = new MstDeptHeadEntity().getListMstDeptHead(user);
            foreach (MstDeptHeadEntity sect in listDeptHeads)
            {
                listDeptHead.Add(new SelectListItem
                {
                    Text = sect.user.alpha_name,
                    Value = sect.user.id.ToString(),
                    Selected = false
                });
            }
            ViewBag.listDeptHead = listDeptHead;
            ViewBag.ptwStatus = new PtwEntity(entity.id_ptw.Value, user).status;
            return PartialView("create", entity);
        }

        [HttpPost]
        public JsonResult SaveAsDraft(FIEntity fi)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.edit();
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveComplete(FIEntity fi)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.edit();
            fi.sendToSPV(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveAsDraftPreScreening(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.SavePreScreening(type);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveCompletePreScreening(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.SavePreScreening(type);
            fi = new FIEntity(fi.id, user);
            fi.completePreScreening(type, user, fullUrl());
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult RejectPreScreening(FIEntity fi, int who, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.SavePreScreening(who);
            fi = new FIEntity(fi.id, user);
            retVal = fi.rejectPreScreening(who, fullUrl(), comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

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

        [HttpPost]
        public JsonResult ApproveFIPermit(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            ResponseModel response = fi.approvePermit(user, type, fullUrl());
            return Json(new { status = response.status, message = response.message });
        }

        [HttpPost]
        public JsonResult rejectFIPermit(FIEntity fi, int type, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            ResponseModel response = fi.rejectPermit(user, type, fullUrl(), comment);
            return Json(new { status = response.status, message = response.message });
        }

        [HttpPost]
        public JsonResult CancelFIPermit(FIEntity fi)
        {
            UserEntity user = Session["user"] as UserEntity;
            string retVal = fi.cancelFIPermit(user, fullUrl());
            return Json(new { status = retVal, message = "Fire Impairment Permit Cancelled." });
        }

        [HttpPost]
        public JsonResult SaveAsDraftCancelScreening(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.SaveCancelScreening(type, user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveCompleteCancelScreening(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = fi.SaveCancelScreening(type, user, true, fullUrl());
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult ApproveFIPermitCancel(FIEntity fi, int type)
        {
            UserEntity user = Session["user"] as UserEntity;
            ResponseModel response = fi.approvePermitCancel(user, type, fullUrl());
            return Json(new { status = response.status, message = response.message });
        }

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
