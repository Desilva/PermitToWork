using PermitToWork.Models.KPI;
using PermitToWork.Models.Log;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class KPIController : Controller
    {
        public PartialViewResult Index()
        {
            KPIFormStub model = new KPIFormStub();
            model.SetYearOptions();

            return PartialView(model);
        }

        public JsonResult AjaxCalculateKPI(KPIFormStub model)
        {
            UserEntity user = (UserEntity)Session["user"];
            int userId = user.id;
            KPIModel kpi = new KPIModel(userId, model.Year);

            KPIResult result = new KPIResult 
            { 
                RequestorOntimeClosing = kpi.CalculateRequestorOntimeClosing(), 
                RequestorOverdueClosing = kpi.CalculateRequestorOverdueClosing(),
                status = "200" 
            };

            return Json(result);
        }
    }
}
