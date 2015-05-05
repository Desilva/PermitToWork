using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Models.WorkingHeight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class WorkingHeightController : Controller
    {
        //
        // GET: /WorkingHeight/

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
            WorkingHeightEntity entity = new WorkingHeightEntity(id, user, listUser);
            entity.getPtw(user, listUser);
            bool[] isCanEdit = new bool[13];

            isCanEdit[0] = entity.isCanEditFormRequestor(user);
            isCanEdit[1] = entity.isCanSignInspector(user);
            isCanEdit[2] = entity.isCanSignRequestor(user);
            isCanEdit[3] = entity.isCanSignSpv(user);
            isCanEdit[4] = entity.isCanSignFO(user);
            isCanEdit[5] = entity.isCanCancel(user);
            isCanEdit[6] = entity.isCanSignRequestorCancel(user);
            isCanEdit[7] = entity.isCanSignSpvCancel(user);
            isCanEdit[8] = entity.isCanSignFOCancel(user);

            ViewBag.isCanEdit = isCanEdit;

            ViewBag.position = "Edit";
            ViewBag.listUser = listUser;

            var listErector = new List<SelectListItem>();
            var listErectors = new MstErectorEntity().getListErector(user, listUser);
            listErector.Add(new SelectListItem
            {
                Text = "",
                Value = "",
            });

            foreach (MstErectorEntity erector in listErectors)
            {
                listErector.Add(new SelectListItem
                {
                    Text = erector.user.alpha_name,
                    Value = erector.id.ToString(),
                    Selected = entity.erector == erector.id ? true : false
                });
            }
            ViewBag.listErector = listErector;


            var listInspector = new List<SelectListItem>();
            var listInspectors = new MstInspectorEntity().getListInspector(user, listUser);
            listInspector.Add(new SelectListItem
            {
                Text = "",
                Value = "",
            });
            foreach (MstInspectorEntity rad in listInspectors)
            {
                listInspector.Add(new SelectListItem
                {
                    Text = rad.user.alpha_name,
                    Value = rad.id.ToString(),
                    Selected = entity.inspector == rad.id ? true : false
                });
            }
            ViewBag.listInspector = listInspector;

            var listTotalCrew = new List<SelectListItem>();
            for (int i = 1; i <= 100; i++)
            {
                listTotalCrew.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = entity.total_crew == i ? true : false
                });
            }
            ViewBag.listTotalCrew = listTotalCrew;

            var listNoInspection = new List<SelectListItem>();
            var listNoInspections = new MstNoInspectionEntity().getListMstNoInspectionByUser(user, listUser);
            listNoInspection.Add(new SelectListItem
            {
                Text = "",
                Value = "0",
            });
            foreach (MstNoInspectionEntity rad in listNoInspections)
            {
                listNoInspection.Add(new SelectListItem
                {
                    Text = rad.no_inspection,
                    Value = rad.id.ToString(),
                    Selected = entity.no_inspection == rad.id.ToString() ? true : false
                });
            }
            ViewBag.listNoInspection = listNoInspection;
            if (Session["ListPtwType"] as int? == 1)
            {
                Session["ListPtwDate"] = DateTime.Now.AddMinutes(-30);
            }
            //ViewBag.ptwStatus = new PtwEntity(entity.id_ptw.Value, user).status;
            return PartialView("create", entity);
        }

        public ActionResult ScaffoldingDesign()
        {
            return PartialView();
        }

        public ActionResult ScaffoldingInspection()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult saveAsDraft(WorkingHeightEntity wh, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & wh.saveAsDraft(who);
            return Json(new { status = retVal == 1 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAndSend(WorkingHeightEntity wh, int who, int? c)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            int retVal = 1;
            retVal = retVal & wh.saveAsDraft(who);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            retVal = retVal & wh.signClearance(who, user, c != null ? c.Value : 0);
            retVal = retVal & wh.sendToUser(who, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult rejectPermit(WorkingHeightEntity wh, int who, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            int retVal = 1;
            retVal = retVal & wh.saveAsDraft(who);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            retVal = retVal & wh.rejectClearance(who);
            retVal = retVal & wh.sendToUser(who, 2, fullUrl(), user, 0, comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult inspectorSign(WorkingHeightEntity wh)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            int retVal = 1;
            wh.saveInspector();
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            retVal = retVal & wh.signClearance(2, user);
            retVal = retVal & wh.sendToUser(2, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAsDraftCancel(WorkingHeightEntity wh, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & wh.saveAsDraftCancel(who);
            return Json(new { status = retVal == 1 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAndSendCancel(WorkingHeightEntity wh, int who, int? c)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            int retVal = 1;
            retVal = retVal & wh.saveAsDraftCancel(who);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            retVal = retVal & wh.signClearanceCancel(who, user);
            retVal = retVal & wh.sendToUserCancel(who, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult rejectPermitCancel(WorkingHeightEntity wh, int who, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            int retVal = 1;
            retVal = retVal & wh.saveAsDraftCancel(who);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            retVal = retVal & wh.rejectClearanceCancel(who);
            retVal = retVal & wh.sendToUserCancel(who, 2, fullUrl(), user, 0, comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAsDraftPreScreening(WorkingHeightEntity wh, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int status = 0;
            if (who == 1)
            {
                status = wh.edit();
            }
            status = wh.savePreScreening(who);
            return Json(new { status = status == 1 ? "200" : "404" });
        }

        //[HttpPost]
        //public JsonResult saveAndSendPreScreening(WorkingHeightEntity wh, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int status = 0;
        //    if (who == 1)
        //    {
        //        status = wh.edit();
        //    }
        //    status = wh.savePreScreening(who);
        //    wh = new WorkingHeightEntity(wh.id, user);
        //    status = wh.completePreScreening(who, fullUrl(), user);
        //    return Json(new { status = status == 1 ? "200" : (status == 2 ? "201" : "404") });
        //}

        //[HttpPost]
        //public JsonResult rejectPreScreening(WorkingHeightEntity wh, string comment)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int status = 0;
        //    status = wh.savePreScreening(2);
        //    wh = new WorkingHeightEntity(wh.id, user);
        //    status = wh.rejectPreScreening(2, fullUrl(), user, comment);
        //    return Json(new { status = status == 1 ? "200" : (status == 2 ? "201" : "404") });
        //}

        //[HttpPost]
        //public JsonResult signStartPermit(int id, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    WorkingHeightEntity wh = new WorkingHeightEntity(id, user);
        //    string message = "";
        //    int retVal = wh.signPermitStart(user, who, fullUrl(), out message);

        //    return Json(new { status = retVal == 0 ? "400" : "200", message = message });
        //}

        [HttpPost]
        public JsonResult cancelWHPermit(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            WorkingHeightEntity wh = new WorkingHeightEntity(id, user, listUser);
            int retVal = wh.signClearanceCancel(1, user);
            retVal = retVal & wh.sendToUserCancel(1, 1, fullUrl(), user);

            return Json(new { status = retVal == 0 ? "400" : "200" });
        }

        [HttpPost]
        public JsonResult getInspector(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstInspectorEntity inspector = new MstInspectorEntity(id, user);
            return Json(new { status = "200", inspectorCertifyNo = inspector.certificate_no });
        }

        [HttpPost]
        public JsonResult getErector(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstErectorEntity erector = new MstErectorEntity(id, user);
            return Json(new { status = "200", certificateNo = erector.certificate_no, validDate = erector.valid_date.Value.ToString("MM/dd/yyyy") });
        }

        [HttpPost]
        public JsonResult saveAsDraftCancelScreening(WorkingHeightEntity wh, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int status = 0;
            if (who == 1)
            {
                status = wh.edit();
            }
            status = wh.saveCancelScreening(who);
            return Json(new { status = status == 1 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAndSendCancelScreening(WorkingHeightEntity wh, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int status = 0;
            status = wh.saveCancelScreening(who);
            ListUser listUser = new ListUser(user.token, user.id);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            status = wh.completeCancelScreening(who, fullUrl(), user);
            return Json(new { status = status == 1 ? "200" : (status == 2 ? "201" : "404") });
        }

        [HttpPost]
        public JsonResult rejectCancelScreening(WorkingHeightEntity wh, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            int status = 0;
            status = wh.saveCancelScreening(2);
            ListUser listUser = new ListUser(user.token, user.id);
            wh = new WorkingHeightEntity(wh.id, user, listUser);
            status = wh.rejectCancelScreening(2, fullUrl(), user, comment);
            return Json(new { status = status == 1 ? "200" : (status == 2 ? "201" : "404") });
        }

        [HttpPost]
        public JsonResult signCancelPermit(int id, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            WorkingHeightEntity wh = new WorkingHeightEntity(id, user, listUser);
            string message = "";
            int retVal = wh.signPermitCancel(user, who, fullUrl(), out message);

            return Json(new { status = retVal == 0 ? "400" : "200", message = message });
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

        #region document attachment 

        public ActionResult saveAttachment(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            var dPath = "\\Upload\\WorkingHeight\\" + id + "\\Attachment";
            var pPath = "~/Upload/WorkingHeight/" + id + "/Attachment";

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
            var pPath = "~/Upload/WorkingHeight/" + id + "/Attachment";
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

        #endregion

        [HttpPost]
        public JsonResult InspectionNo(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstNoInspectionEntity noInspection = new MstNoInspectionEntity(id, user);
            return Json(noInspection.valid_date != null ? noInspection.valid_date.Value.ToString("MM/dd/yyyy") : "");
        }

        [HttpPost]
        public JsonResult CheckAttachment(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(user.token, user.id);
            WorkingHeightEntity wh = new WorkingHeightEntity(id, user, listUser);
            return Json(new { status = wh.listDocumentUploaded[WorkingHeightEntity.DocumentUploaded.ATTACHMENT.ToString()].Count > 0 ? "200" : "404" });
        }
    }
}
