using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterRadiationPOController : Controller
    {
        //
        // GET: /MasterRadiationPO/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstRadiationPOEntity().getListRadiationPO(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstRadiationPOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addRadiationPO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstRadiationPOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editRadiationPO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstRadiationPOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteRadiationPO();
            return Json(true);
        }
    }
}
