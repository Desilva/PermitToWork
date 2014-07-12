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
    public class MasterDepartmentController : Controller
    {
        public ActionResult Index()
        {
            return PartialView();
        }

        //
        // GET: /MasterDepartment/List

        [HttpGet]
        public JsonResult List(int? id, string timestamp, string seal)
        {
            MstDepartmentEntity dept = new MstDepartmentEntity();
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterDepartment").ToLower();
            if (seal_test != seal.ToLower() || id != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json( new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" }, JsonRequestBehavior.AllowGet);
            }

            List<MstDepartmentEntity> list = dept.getListMstDepartment();
            return Json(new { status = HttpStatusCode.OK, message = "Listing Department", departments = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MasterDepartment/Add

        [HttpPost]
        public JsonResult Add(string timestamp, string seal, MstDepartmentEntity dept)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterDepartment").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (dept.addDepartment() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Add Department", department = dept });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            
        }

        //
        // POST: /MasterDepartment/Edit/id

        [HttpPost]
        public JsonResult Edit(string timestamp, string seal, MstDepartmentEntity dept)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterDepartment").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (dept.editDepartment() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Edit Department", department = dept });
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        //
        // POST: /MasterDepartment/Delete/id

        [HttpPost]
        public JsonResult Delete(string timestamp, string seal, MstDepartmentEntity dept)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterDepartment").ToLower();
            if (seal_test != seal.ToLower())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            int status = 0;
            if ((status = dept.deleteDepartment()) == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Delete Department" });
            }
            else if (status == 404)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { status = HttpStatusCode.NotFound, message = "Department does not exist" });
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
            var result = new MstDepartmentEntity().getListMstDepartment();
            return Json(result);
        }

        [HttpPost]
        public JsonResult AddDepartment(MstDepartmentEntity a)
        {
            a.addDepartment();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditDepartment(MstDepartmentEntity a)
        {
            a.editDepartment();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteDepartment(MstDepartmentEntity a)
        {
            a.deleteDepartment();
            return Json(true);
        }

    }
}
