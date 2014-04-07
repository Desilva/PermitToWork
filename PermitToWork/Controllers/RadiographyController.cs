using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class RadiographyController : Controller
    {
        //
        // GET: /Radiography/

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
            RadEntity entity = new RadEntity(id, user);

            bool[] isCanEdit = new bool[19];

            isCanEdit[0] = entity.isCanEditGeneralInformation(user);
            isCanEdit[1] = entity.isCanEditFOChoosingSO(user);
            isCanEdit[2] = entity.isCanApproveOperator(user);
            isCanEdit[3] = entity.isCanApproveRadiographer2(user);
            isCanEdit[4] = entity.isCanApproveSpv(user);
            isCanEdit[5] = entity.isCanApproveSO(user);
            isCanEdit[6] = entity.isCanApproveFO(user);
            isCanEdit[7] = entity.isCanCancel(user);
            //isCanEdit[11] = entity.isCanEditSpvCancelScreening(user);
            //isCanEdit[12] = entity.isCanEditRadCancelScreening(user);
            //isCanEdit[13] = entity.isCanEditFOCancelScreening(user);
            isCanEdit[8] = entity.isCanApproveOperatorCancel(user);
            isCanEdit[9] = entity.isCanApproveRadiographer2Cancel(user);
            isCanEdit[10] = entity.isCanApproveSpvCancel(user);
            isCanEdit[11] = entity.isCanApproveSOCancel(user);
            isCanEdit[12] = entity.isCanApproveFOCancel(user);
            //isCanEdit[10] = entity.isCanEditFormSPVCancel(user);
            //isCanEdit[11] = entity.isCanEditFormSOCancel(user);
            //isCanEdit[12] = entity.isCanEditFormFOCancel(user);
            //isCanEdit[13] = entity.isCanEditApproveRequestorCancel(user);
            //isCanEdit[14] = entity.isCanEditApproveFireWatchCancel(user);
            //isCanEdit[15] = entity.isCanEditApproveSOCancel(user);
            //isCanEdit[16] = entity.isCanEditApproveFOCancel(user);
            //isCanEdit[17] = entity.isCanEditApproveDeptHeadCancel(user);

            ViewBag.isCanEdit = isCanEdit;

            ViewBag.position = "Edit";
            ViewBag.listUser = new ListUser(user.token, user.id);

            var listRadiographer2 = new List<SelectListItem>();
            var listRadiographer1 = new List<SelectListItem>();
            var listRadiographers = new MstRadiographerEntity().getListRadiographer(user);
            listRadiographer2.Add(new SelectListItem
            {
                Text = "",
                Value = "",
            });
            listRadiographer1.Add(new SelectListItem
            {
                Text = "",
                Value = "",
            });
            foreach (MstRadiographerEntity rad in listRadiographers)
            {
                if (rad.level == 2)
                {
                    listRadiographer2.Add(new SelectListItem
                    {
                        Text = rad.user.alpha_name,
                        Value = rad.id.ToString(),
                        Selected = entity.radiographer_2 == rad.id.ToString() ? true : false,
                    });
                }
                else if (rad.level == 1)
                {
                    listRadiographer1.Add(new SelectListItem
                    {
                        Text = rad.user.alpha_name,
                        Value = rad.id.ToString(),
                        Selected = entity.radiographer_1 == rad.id.ToString() ? true : false
                    });
                }
            }
            ViewBag.listRadiographer2 = listRadiographer2;
            ViewBag.listRadiographer1 = listRadiographer1;


            var listRPO = new List<SelectListItem>();
            var listRPOs = new MstRadiationPOEntity().getListRadiationPO(user);
            listRPO.Add(new SelectListItem
            {
                Text = "",
                Value = "",
            });
            foreach (MstRadiationPOEntity rad in listRPOs)
            {
                listRPO.Add(new SelectListItem
                {
                    Text = rad.user.alpha_name,
                    Value = rad.id.ToString(),
                    Selected = entity.radiation_protection_officer == rad.id.ToString() ? true : false
                });
            }
            ViewBag.listRPO = listRPO;

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

            var listSafetyOfficer = new List<SelectListItem>();
            var listMstSO = new MstSOEntity().getListMstSO(user);
            foreach (MstSOEntity so in listMstSO)
            {
                listSafetyOfficer.Add(new SelectListItem
                {
                    Text = so.user.alpha_name,
                    Value = so.user.id.ToString(),
                    Selected = entity.safety_officer == so.user.id.ToString() ? true : false
                });
            }
            ViewBag.listSafetyOfficer = listSafetyOfficer;

            ViewBag.ptwStatus = new PtwEntity(entity.id_ptw.Value, user).status;
            return PartialView("create", entity);
        }

        [HttpPost]
        public JsonResult saveAsDraft(RadEntity rad, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraft(who);
            return Json(new { status = retVal == 1 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAndSend(RadEntity rad, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraft(who);
            rad = new RadEntity(rad.id, user);
            retVal = retVal & rad.signClearance(who, user);
            retVal = retVal & rad.sendToUser(who + 1, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : (retVal == 0 ? "400" : "404") });
        }

        [HttpPost]
        public JsonResult rejectPermit(RadEntity rad, int who, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraft(who);
            rad = new RadEntity(rad.id, user);
            retVal = retVal & rad.rejectClearance(who);
            retVal = retVal & rad.sendToUser(who - 1, 2, fullUrl(), user, comment);
            return Json(new { status = retVal > 0 ? "200" : (retVal == 0 ? "400" : "404") });
        }

        [HttpPost]
        public JsonResult assignSafetyOfficer(RadEntity rad)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = rad.assignSafetyOfficer(user, fullUrl());
            return Json(new { status = retVal > 0 ? "200" : (retVal == 0 ? "400" : "404") });
        }

        //[HttpPost]
        //public JsonResult saveAsDraftPreScreening(RadEntity rad, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    rad.savePreScreening(who);
        //    rad.assignSafetyOfficer(user, fullUrl());
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult saveCompletePreScreening(RadEntity rad, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    rad.savePreScreening(who);
        //    rad.assignSafetyOfficer(user, fullUrl());
        //    rad = new RadEntity(rad.id, user);
        //    rad.completePreScreening(who, user, fullUrl());
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult rejectPreScreening(RadEntity rad, int who, string comment)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    rad.savePreScreening(who);

        //    rad = new RadEntity(rad.id, user);
        //    rad.rejectPreScreening(who, fullUrl(), comment);
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult signStartPermit(int id, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    RadEntity rad = new RadEntity(id, user);
        //    string message = "";
        //    int retVal = rad.signPermitStart(user, who, fullUrl(), out message);

        //    return Json(new { status = retVal == 0 ? "400" : "200", message = message});
        //}

        [HttpPost]
        public JsonResult cancelRadPermit(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            RadEntity rad = new RadEntity(id, user);
            int retVal = rad.signClearanceCancel(1, user);
            retVal = retVal & rad.sendToUserCancel(1, 1, fullUrl(), user);

            return Json(new { status = retVal == 0 ? "400" : "200" });
        }

        [HttpPost]
        public JsonResult saveAsDraftCancel(RadEntity rad, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraftCancel(who);
            return Json(new { status = retVal == 1 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveAndSendCancel(RadEntity rad, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraftCancel(who);
            rad = new RadEntity(rad.id, user);
            retVal = retVal & rad.signClearanceCancel(who, user);
            retVal = retVal & rad.sendToUserCancel(who + 1, 1, fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : (retVal == 0 ? "400" : "404") });
        }

        [HttpPost]
        public JsonResult rejectPermitCancel(RadEntity rad, int who, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = 1;
            retVal = retVal & rad.saveAsDraftCancel(who);
            rad = new RadEntity(rad.id, user);
            retVal = retVal & rad.rejectClearanceCancel(who);
            retVal = retVal & rad.sendToUserCancel(who - 1, 2, fullUrl(), user, comment);
            return Json(new { status = retVal > 0 ? "200" : (retVal == 0 ? "400" : "404") });
        }

        [HttpPost]
        public JsonResult saveAsDraftCancelScreening(RadEntity rad, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            rad.saveCancelScreening(who);
            rad.assignSafetyOfficer(user, fullUrl());
            return Json(true);
        }

        //[HttpPost]
        //public JsonResult saveCompleteCancelScreening(RadEntity rad, int who)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    rad.saveCancelScreening(who);
        //    rad = new RadEntity(rad.id, user);
        //    rad.completeCancelScreening(who, user, fullUrl());
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult rejectCancelScreening(RadEntity rad, int who, string comment)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    rad.saveCancelScreening(who);

        //    rad = new RadEntity(rad.id, user);
        //    rad.rejectCancelScreening(who, fullUrl(), comment);
        //    return Json(true);
        //}

        [HttpPost]
        public JsonResult signCancelPermit(int id, int who)
        {
            UserEntity user = Session["user"] as UserEntity;
            RadEntity rad = new RadEntity(id, user);
            string message = "";
            int retVal = rad.signPermitCancel(user, who, fullUrl(), out message);

            return Json(new { status = retVal == 0 ? "400" : "200", message = message });
        }

        [HttpPost]
        public JsonResult getLicenseNumber(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstRadiographerEntity radio = new MstRadiographerEntity(id, user);

            return Json(new { status = "200", data = radio.license_number });
        }

        [HttpPost]
        public JsonResult getLicenseNumberRPO(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstRadiationPOEntity radio = new MstRadiationPOEntity(id, user);

            return Json(new { status = "200", data = radio.license_number });
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

        public ActionResult saveLicenseNumber(IEnumerable<HttpPostedFileBase> files, int? id, int type)
        {
            var dPath = "";
            var pPath = "";
            // The Name of the Upload component is "attachments" 
            switch (type)
            {
                case 1:
                    dPath = "\\Upload\\Radiography\\" + id + "\\LicenseNumber1";
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumber1";
                    break;
                case 2:
                    dPath = "\\Upload\\Radiography\\" + id + "\\LicenseNumber2";
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumber2";
                    break;
                case 3:
                    dPath = "\\Upload\\Radiography\\" + id + "\\LicenseNumberRPO";
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumberRPO";
                    break;
                case 4:
                    dPath = "\\Upload\\Radiography\\" + id + "\\Attachment";
                    pPath = "~/Upload/Radiography/" + id + "/Attachment";
                    break;
            }

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

        public ActionResult removeLicenseNumber(string[] fileNames, int id, int type)
        {
            var pPath = "";
            // The Name of the Upload component is "attachments" 
            switch (type)
            {
                case 1:
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumber1";
                    break;
                case 2:
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumber2";
                    break;
                case 3:
                    pPath = "~/Upload/Radiography/" + id + "/LicenseNumberRPO";
                    break;
                case 4:
                    pPath = "~/Upload/Radiography/" + id + "/Attachment";
                    break;
            }

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

    }
}
