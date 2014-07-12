using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using PermitToWork.Utilities;
using PermitToWork.Models;

namespace PermitToWork.Controllers
{
    public class InspectionNoController : Controller
    {
        //
        // GET: /InspectionNo/

        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult InspectionSheet(int id)
        {
            scaffolding_inspection si = new star_energy_ptwEntities().scaffolding_inspection.Find(id);
            return PartialView(si);
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstNoInspectionEntity().getListMstNoInspection(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstNoInspectionEntity inspection)
        {
            UserEntity user = Session["user"] as UserEntity;
            inspection.inspector_id = user.id.ToString();
            inspection.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult AddNew(DateTime valid_until)
        {
            HttpPostedFileBase file = Request.Files[0];
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string err = "";
            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            ExcelReader excel = new ExcelReader();
            int i = excel.LoadInspection(path, null, null);

            UserEntity user = Session["user"] as UserEntity;
            MstInspectorEntity inspector = new MstInspectorEntity(user);
            MstNoInspectionEntity inspection = new MstNoInspectionEntity();

            string no = inspection.getLastNumberByUser(user);
            no = (Int32.Parse(no) + 1).ToString().PadLeft(3, '0');

            inspection.id_inspection = i;
            inspection.valid_date = valid_until;
            inspection.inspector_id = user.id.ToString();
            inspection.no_inspection = no + "/" + inspector.code_name;
            inspection.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(int id, DateTime valid_until)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstInspectorEntity inspector = new MstInspectorEntity(user);
            MstNoInspectionEntity inspection = new MstNoInspectionEntity(id, user);

            HttpPostedFileBase file = Request.Files[0];
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string err = "";
            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            ExcelReader excel = new ExcelReader();
            int i = excel.LoadInspection(path, inspection.id_inspection, null);

            inspection.valid_date = valid_until;
            inspection.edit();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Extension(DateTime valid_until, int idPrev)
        {
            HttpPostedFileBase file = Request.Files[0];
            // extract only the fielname
            var fileName = Path.GetFileName(file.FileName);
            string err = "";
            // store the file inside ~/App_Data/uploads folder
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
            file.SaveAs(path);
            ExcelReader excel = new ExcelReader();
            int i = excel.LoadInspection(path, null, idPrev);

            UserEntity user = Session["user"] as UserEntity;
            MstInspectorEntity inspector = new MstInspectorEntity(user);
            MstNoInspectionEntity inspection = new MstNoInspectionEntity();

            string no = inspection.getLastNumberByUser(user);
            no = (Int32.Parse(no) + 1).ToString().PadLeft(3, '0');

            inspection.id_inspection = i;
            inspection.valid_date = valid_until;
            inspection.inspector_id = user.id.ToString();
            inspection.no_inspection = no + "/" + inspector.code_name;
            inspection.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstNoInspectionEntity inspection)
        {
            UserEntity user = Session["user"] as UserEntity;
            inspection.delete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult GetScaffolding()
        {
            UserEntity user = Session["user"] as UserEntity;
            MstNoInspectionEntity inspection = new MstNoInspectionEntity();
            var result = inspection.getListMstNoInspection(user);
            List<ScaffoldingInspectionItem> listResult = new List<ScaffoldingInspectionItem>();
            foreach (MstNoInspectionEntity noInspection in result)
            {
                if (noInspection.s != null)
                {
                    listResult.Add(new ScaffoldingInspectionItem
                    {
                        id = noInspection.s.id,
                        text = noInspection.no_inspection + " - " + (noInspection.s != null ? noInspection.s.location : "")
                    });
                }
            }
            return Json(listResult);
        }

    }

    public class ScaffoldingInspectionItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }
}
