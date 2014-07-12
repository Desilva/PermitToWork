using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterInspectorController : Controller
    {
        //
        // GET: /MasterInspector/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstInspectorEntity().getListInspector(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstInspectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addInspector();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstInspectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editInspector();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstInspectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteInspector();
            return Json(true);
        }

    }
}
