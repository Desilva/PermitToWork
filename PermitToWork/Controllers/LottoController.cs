using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class LottoController : Controller
    {
        //
        // GET: /Lotto/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult Create1()
        {
            return PartialView();
        }

        public ActionResult Create2()
        {
            return PartialView();
        }

    }
}
