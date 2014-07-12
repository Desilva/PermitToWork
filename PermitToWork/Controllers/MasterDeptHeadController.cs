using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterDeptHeadController : Controller
    {
        //
        // GET: /MasterDeptHead/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstDeptHeadEntity().getListMstDeptHead(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstDeptHeadEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addDeptHead();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstDeptHeadEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editDeptHead();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstDeptHeadEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteDeptHead();
            return Json(true);
        }

    }
}
