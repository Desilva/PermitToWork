using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterEIController : Controller
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
            var result = new MstEIEntity().getListFacilities(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstEIEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addEI();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstEIEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editEI();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstEIEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteEI();
            return Json(true);
        }

    }
}
