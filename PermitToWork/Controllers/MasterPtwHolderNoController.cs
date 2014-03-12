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
    public class MasterPtwHolderNoController : Controller
    {
        //
        // GET: /MasterSection/List

        [HttpGet]
        public JsonResult List(int? id, string timestamp, string seal)
        {
            MstPtwHolderNoEntity holderNo = new MstPtwHolderNoEntity();
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterPtwHolderNo").ToLower();
            if (seal_test != seal.ToLower() || id != null)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" }, JsonRequestBehavior.AllowGet);
            }

            List<MstPtwHolderNoEntity> list = holderNo.getListMstPtwHolderNo();
            return Json(new { status = HttpStatusCode.OK, message = "Listing PTW Holder No", ptw_nos = list, total = list.Count }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /MasterSection/Add

        [HttpPost]
        public JsonResult Add(string timestamp, string seal, MstPtwHolderNoEntity holderNo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterPtwHolderNo").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (holderNo.addPtwHolderNo() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Add PTW Holder No", ptw_no = holderNo });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

        }

        //
        // POST: /MasterSection/Edit/id

        [HttpPost]
        public JsonResult Edit(string timestamp, string seal, MstPtwHolderNoEntity holderNo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterPtwHolderNo").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }

            if (holderNo.editPtwHolderNo() == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Edit PTW Holder No", ptw_no = holderNo });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

        //
        // POST: /MasterSection/Delete/id

        [HttpPost]
        public JsonResult Delete(string timestamp, string seal, MstPtwHolderNoEntity holderNo)
        {
            string salt = ConfigurationManager.AppSettings["salt"].ToString();
            string seal_test = Base64.MD5Seal(timestamp + salt + "MasterPtwHolderNo").ToLower();
            if (seal_test != seal.ToLower())
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
            int status = 0;
            if ((status = holderNo.deletePtwHolderNo()) == 1)
            {
                return Json(new { status = HttpStatusCode.OK, message = "Delete PTW Holder No" });
            }
            else if (status == 404)
            {
                return Json(new { status = HttpStatusCode.NotFound, message = "PTW Holder No does not exist" });
            }
            else
            {
                return Json(new { status = HttpStatusCode.InternalServerError, message = "Something's wrong" });
            }
        }

    }
}
