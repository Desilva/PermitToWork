using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermitToWork.Models.Master;
using PermitToWork.Models.User;

namespace PermitToWork.Controllers
{
    public class MasterAreaCodeController : Controller
    {
        //
        // GET: /MasterAreaCode/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstAreaCodeEntity().getListAreaCode();
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstAreaCodeEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addAreaCode();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstAreaCodeEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editAreaCode();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstAreaCodeEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteAreaCode();
            return Json(true);
        }
    }
}
