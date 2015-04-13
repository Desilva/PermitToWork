using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterDisposalLocationController : Controller
    {
        //
        // GET: /MasterLockBox/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstDisposalLocationEntity().getListDisposalLocation();
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstDisposalLocationEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstDisposalLocationEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.edit();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstDisposalLocationEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.delete();
            return Json(true);
        }

    }
}