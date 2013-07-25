using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class WorkingHeightController : Controller
    {
        //
        // GET: /WorkingHeight/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult ScaffoldingDesign()
        {
            return PartialView();
        }

        public ActionResult ScaffoldingInspection()
        {
            return PartialView();
        }
    }
}
