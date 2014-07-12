using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterSOController : Controller
    {
        //
        // GET: /MasterSO/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstSOEntity().getListMstSO(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstSOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addSO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstSOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editSO();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstSOEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteSO();
            return Json(true);
        }

    }
}
