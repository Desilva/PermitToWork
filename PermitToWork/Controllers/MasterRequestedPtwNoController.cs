using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;

namespace PermitToWork.Controllers
{
    public class MasterRequestedPtwNoController : Controller
    {
        //
        // GET: /MasterRequestedPtwNo/

        public ActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        public JsonResult Binding()
        {
            ListPtw listPtw = new ListPtw();
            var result = listPtw.ListPtwRequestedNo();
            return Json(result);
        }

        [HttpPost]
        public JsonResult SetValue(PtwEntity ptw)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            ptw.setGuestHolderNo();
            ptw.sendEmailRequestNoSet(userLogin);
            return Json(true);
        }

    }
}
