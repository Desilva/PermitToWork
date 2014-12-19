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
using System.Web.UI.WebControls;
using System.IO;

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

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstPtwHolderNoEntity().getListMstPtwHolderNo(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AddPtwHolder(MstPtwHolderNoEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addPtwHolderNo();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditPtwHolder(MstPtwHolderNoEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editPtwHolderNo();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeletePtwHolder(MstPtwHolderNoEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deletePtwHolderNo();
            return Json(true);
        }

        public JsonResult ImportExcel()
        {
            HttpPostedFileBase file = Request.Files[0];
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string err = "";
            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            ExcelReader excel = new ExcelReader();
            List<string> errs = excel.LoadHolderNo(path);

            
            return Json(true);
        }

        public ActionResult GetTemplate()
        {
            UserEntity user = Session["user"] as UserEntity;
            List<UserEntity> users = new ListUser(user.token, user.id).listUser.ToList();
            List<MstPtwHolderNoEntity> ptwHolders = new MstPtwHolderNoEntity().getListMstPtwHolderNo(user);
            List<PtwHolderExcelModel> result = new List<PtwHolderExcelModel>();
            MstPtwHolderNoEntity ptwHolder = new MstPtwHolderNoEntity();
            foreach (UserEntity u in users)
            {
                if ((ptwHolder = ptwHolders.Where(p => p.id_employee.Value == u.id).FirstOrDefault()) != null)
                {
                    result.Add(new PtwHolderExcelModel(ptwHolder, u));
                }
                else
                {
                    result.Add(new PtwHolderExcelModel(null, u));
                }
            }
            GridView gv = new GridView();
            gv.Caption = "PTW Holder No Template";
            gv.DataSource = result;
            if (result.Count == 0)
            {
                return new JavaScriptResult();
            }
            gv.DataBind();
            gv.HeaderRow.Cells[0].Text = "id";
            gv.HeaderRow.Cells[1].Text = "Name";
            gv.HeaderRow.Cells[2].Text = "Department";
            gv.HeaderRow.Cells[3].Text = "PTW Holder No";
            gv.HeaderRow.Cells[4].Text = "Active Until";
            if (gv != null)
            {
                return new DownloadFileActionResult(gv, "ptw_holder_no_template.xls");
            }
            else
            {
                return new JavaScriptResult();
            }
        }

    }
}
