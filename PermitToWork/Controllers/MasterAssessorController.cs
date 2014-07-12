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
    public class MasterAssessorController : Controller
    {
        //
        // GET: /MasterAssessor

        public ActionResult Index()
        {
            return PartialView();
        }
        
        //
        // GET: /MasterAssessor/List

        [HttpGet]
        public JsonResult List(int? id, string timestamp, string seal)
        {
            MstAssessorEntity assessor = new MstAssessorEntity();
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterAssessor").ToLower();
            if (seal_test != seal.ToLower() || id != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" }, JsonRequestBehavior.AllowGet);
            }

            List<MstAssessorEntity> list = assessor.getListAssessor();
            return Json(new { status = HttpStatusCode.OK, message = "Listing Assessor", assessors = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MasterAssessor/Add

        [HttpPost]
        public JsonResult Add(string timestamp, string seal, MstAssessorEntity assessor)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterAssessor").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (assessor.addAssessor() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Add Assessor", assessor = assessor });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

        }

        //
        // POST: /MasterAssessor/Edit/id

        [HttpPost]
        public JsonResult Edit(string timestamp, string seal, MstAssessorEntity assessor)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterAssessor").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (assessor.editAssessor() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Edit Assessor", assessor = assessor });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        //
        // POST: /MasterAssessor/Delete/id

        [HttpPost]
        public JsonResult Delete(string timestamp, string seal, MstAssessorEntity assessor)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterAssessor").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            int status = 0;
            if ((status = assessor.deleteAssessor()) == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Delete Assessor" });
            }
            else if (status == 404)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { status = HttpStatusCode.NotFound, message = "Assessor does not exist" });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstAssessorEntity().getListAssessor(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AddAssessor(MstAssessorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addAssessor();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditAssessor(MstAssessorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editAssessor();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteAssessor(MstAssessorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteAssessor();
            return Json(true);
        }

    }
}
