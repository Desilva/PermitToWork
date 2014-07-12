using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterRadiographerController : Controller
    {
        //
        // GET: /MasterRadiographer/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstRadiographerEntity().getListRadiographer(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstRadiographerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addRadiographer();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstRadiographerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editRadiographer();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstRadiographerEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteRadiographer();
            return Json(true);
        }

    }
}
