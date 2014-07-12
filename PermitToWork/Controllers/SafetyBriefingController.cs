using PermitToWork.Models.SafetyBriefing;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class SafetyBriefingController : Controller
    {
        //
        // GET: /SafetyBriefing/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            SafetyBriefingEntity safetyBriefing = new SafetyBriefingEntity(id, user);

            bool[] isCanEdit = new bool[3];

            isCanEdit[0] = safetyBriefing.isCanEditForm(user);
            isCanEdit[1] = safetyBriefing.isCanCancel(user);
            isCanEdit[2] = safetyBriefing.isCanPrint(user);

            ViewBag.isCanEdit = isCanEdit;

            return PartialView(safetyBriefing);
        }

        [HttpPost]
        public JsonResult SaveAsDraft(int id, SafetyBriefingEntity sb) {
            UserEntity user = Session["user"] as UserEntity;
            SafetyBriefingEntity safetyBriefing = new SafetyBriefingEntity(id, user);
            safetyBriefing.hazard = sb.hazard;
            safetyBriefing.control_method = sb.control_method;
            safetyBriefing.section = sb.section;
            safetyBriefing.work_area = sb.work_area;
            safetyBriefing.date = sb.date;
            safetyBriefing.topic = sb.topic;
            int retVal = safetyBriefing.edit();
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveForPrint(int id, SafetyBriefingEntity sb)
        {
            UserEntity user = Session["user"] as UserEntity;
            SafetyBriefingEntity safetyBriefing = new SafetyBriefingEntity(id, user);
            safetyBriefing.hazard = sb.hazard;
            safetyBriefing.control_method = sb.control_method;
            safetyBriefing.section = sb.section;
            safetyBriefing.work_area = sb.work_area;
            safetyBriefing.date = sb.date;
            safetyBriefing.topic = sb.topic;
            int retVal = safetyBriefing.edit();
            retVal &= safetyBriefing.saveForPrint();
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult Cancellation(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            SafetyBriefingEntity sb = new SafetyBriefingEntity(id, user);
            int retVal = sb.saveCancellation(user);
            return Json(new { status = retVal == -1 ? "403" : (retVal == 0 ? "404" : "200") });
        }

        #region user

        [HttpPost]
        public JsonResult BindingUser(int id_safety)
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new SafetyBriefingUserEntity().listUser(id_safety, user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult DeleteUser(SafetyBriefingUserEntity safetyUser, int id_safety)
        {
            UserEntity user = Session["user"] as UserEntity;
            safetyUser.delete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveUser(SafetyBriefingUserEntity safetyUser, int id_safety)
        {
            UserEntity user = Session["user"] as UserEntity;
            if (safetyUser.id == 0)
            {
                safetyUser.id_safety_briefing = id_safety;
                safetyUser.create();
            }
            else
                safetyUser.edit();
            return Json(true);
        }

        public JsonResult ListingEmployee()
        {
            UserEntity user = Session["user"] as UserEntity;
            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public ActionResult saveAttachment(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            var dPath = "\\Upload\\SafetyBriefing\\" + id;
            var pPath = "~/Upload/SafetyBriefing/" + id;

            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var dummyPath = Path.Combine(dPath, fileName);
                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // save file
                file.SaveAs(physicalPath);
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult removeAttachment(string[] fileNames, int id)
        {
            var pPath = "~/Upload/SafetyBriefing/" + id;
            // The Name of the Upload component is "attachments" 
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    //remove file
                    System.IO.File.Delete(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

    }
}
