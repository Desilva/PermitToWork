using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterErectorController : Controller
    {
        //
        // GET: /MasterErector/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstErectorEntity().getListErector(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstErectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addErector();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstErectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editErector();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstErectorEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteErector();
            return Json(true);
        }

    }
}
