using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class PtwController : Controller
    {
        //
        // GET: /Ptw/

        public ActionResult Index()
        {
            return PartialView();
        }

    }
}
