using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class MasterFacilitiesController : Controller
    {
        //
        // GET: /MasterFacilities/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new MstFacilitiesEntity().getListFacilities(user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult Add(MstFacilitiesEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.addFacilities();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstFacilitiesEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.editFacilities();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstFacilitiesEntity a)
        {
            UserEntity user = Session["user"] as UserEntity;
            a.deleteFacilities();
            return Json(true);
        }

    }
}
