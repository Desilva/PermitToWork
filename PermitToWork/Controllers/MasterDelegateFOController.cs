using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermitToWork.Models;
using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using PermitToWork.Utilities;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class MasterDelegateFOController : Controller
    {
        //
        // GET: /MasterDelegateFO/

        public ActionResult Index()
        {
            UserEntity user = Session["user"] as UserEntity;
            MstFOEntity fo = new MstFOEntity(user);
            if (fo.id_employee != null)
            {
                ViewBag.Department = fo.fo_name;
            }
            else
            {
                Response.StatusCode = 302;

                return Content(Url.Action("Index", "Home"));
            }
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            List<MstDelegateFOEntity> delegateFoList = new MstDelegateFOEntity().getListByFO(user.id, user);
            return Json(delegateFoList);
        }

        [HttpPost]
        public JsonResult Add(MstDelegateFOEntity delegateFo)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstFOEntity fo = new MstFOEntity(user);
            delegateFo.id_mst_fo = fo.id;
            delegateFo.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Edit(MstDelegateFOEntity delegateFo)
        {
            UserEntity user = Session["user"] as UserEntity;
            MstFOEntity fo = new MstFOEntity(user);
            delegateFo.id_mst_fo = fo.id;
            delegateFo.edit();
            return Json(true);
        }

        [HttpPost]
        public JsonResult Delete(MstDelegateFOEntity delegateFo)
        {
            UserEntity user = Session["user"] as UserEntity;
            delegateFo.delete();
            return Json(true);
        }
    }
}
