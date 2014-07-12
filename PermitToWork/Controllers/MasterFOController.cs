using PermitToWork.Models.Master;
using PermitToWork.Models.User;
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
    public class MasterFOController : Controller
    {
        //
        // GET: /MasterFO/List

        [HttpGet]
        public JsonResult List(int? id, string timestamp, string seal)
        {
            MstFOEntity fo = new MstFOEntity();
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterFO").ToLower();
            if (seal_test != seal.ToLower() || id != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" }, JsonRequestBehavior.AllowGet);
            }

            List<MstFOEntity> list = fo.getListMstFO();
            return Json(new { status = HttpStatusCode.OK, message = "Listing Facility Owner", fos = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MasterFO/Add

        [HttpPost]
        public JsonResult Add(string timestamp, string seal, MstFOEntity fo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterFO").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (fo.addFO() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Add Facility Owner", fo = fo });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

        }

        //
        // POST: /MasterFO/Edit/id

        [HttpPost]
        public JsonResult Edit(string timestamp, string seal, MstFOEntity fo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterFO").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (fo.editFO() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Edit Facility Owner", fo = fo });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        //
        // POST: /MasterFO/Delete/id

        [HttpPost]
        public JsonResult Delete(string timestamp, string seal, MstFOEntity fo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterFO").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            int status = 0;
            if ((status = fo.deleteFO()) == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Delete Facility Owner" });
            }
            else if (status == 404)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { status = HttpStatusCode.NotFound, message = "Facility Owner does not exist" });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstFOEntity().getListMstFO(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AddFO(MstFOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addFO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditFO(MstFOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editFO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteFO(MstFOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteFO();
            return Json(true);
        }

    }
}
