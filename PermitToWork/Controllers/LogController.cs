using PermitToWork.Models.Log;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class LogController : Controller
    {
        //
        // GET: /Log/

        public ActionResult Index(int id, int permitType, string backPath)
        {
            UserEntity user = Session["user"] as UserEntity;
            ViewBag.backPath = backPath;
            ViewBag.listLogs = new LogEntity().getLogsById(id, permitType, user.token, user);
            return PartialView();
        }

    }
}
