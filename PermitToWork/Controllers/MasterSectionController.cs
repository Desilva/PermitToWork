using PermitToWork.Models.Master;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterSectionController : Controller
    {
        //
        // GET: /MasterSection/List

        [HttpGet]
        public JsonResult List(int? id, string timestamp, string seal)
        {
            MstSectionEntity sect = new MstSectionEntity();
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterSection").ToLower();
            if (seal_test != seal.ToLower() || id != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" }, JsonRequestBehavior.AllowGet);
            }

            List<MstSectionEntity> list = sect.getListMstSection();
            return Json(new { status = HttpStatusCode.OK, message = "Listing Section", sections = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MasterSection/Add

        [HttpPost]
        public JsonResult Add(string timestamp, string seal, MstSectionEntity sect)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterSection").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (sect.addSection() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Add Section", section = sect });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

        }

        //
        // POST: /MasterSection/Edit/id

        [HttpPost]
        public JsonResult Edit(string timestamp, string seal, MstSectionEntity sect)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterSection").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (sect.editSection() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Edit Section", section = sect });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        //
        // POST: /MasterSection/Delete/id

        [HttpPost]
        public JsonResult Delete(string timestamp, string seal, MstSectionEntity sect)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterSection").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            int status = 0;
            if ((status = sect.deleteSection()) == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Delete Section" });
            }
            else if (status == 404)
            {
                return Json(new { status = HttpStatusCode.NotFound, message = "Section does not exist" });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

    }
}
