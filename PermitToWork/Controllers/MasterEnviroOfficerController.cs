using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterEnviroOfficerController : Controller
    {
        //
        // GET: /MasterEI/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstEnviroOfficerEntity().getListEnviroOfficer(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstEnviroOfficerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstEnviroOfficerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.edit();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstEnviroOfficerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.delete();
            return Json(true);
        }

    }
}