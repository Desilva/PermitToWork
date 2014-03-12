using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index(string p, string e)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            if (p != null)
            {
                ViewBag.p = p;
            }

            if (e != null)
            {
                switch (e)
                {
                    case "401":
                        ViewBag.m = "Another user already become supervisor for that Permit.";
                        break;
                    case "402":
                        ViewBag.m = "Another user already become facility owner for that Permit.";
                        break;
                    case "404":
                        ViewBag.m = "Something wrong with your link. Maybe you have try something?";
                        break;
                };
            }
            return View();
        }

        public ActionResult PartialIndex()
        {
            return PartialView("Index");
        }

        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity user = Session["user"] as UserEntity;
            ListPtw listPtw = new ListPtw(user);
            var result = listPtw.listPtwByUser(user);
            return Json(result);
        }
    }
}
