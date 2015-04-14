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
            KPIFilterFormStub model = new KPIFilterFormStub();
            model.SetYearOptions();

            return PartialView(model);
        }

        public JsonResult AjaxCalculateKPI(KPIFilterFormStub model)
        {
            UserEntity user = (UserEntity)Session["user"];
            int userId = user.id;
            KPIUserModel kpi = new KPIUserModel(userId, model.Year);

            KPIResult result = new KPIResult 
            { 
                RequestorOntimeClosing = kpi.CalculateRequestorOntimeClosing(), 
                RequestorOverdueClosing = kpi.CalculateRequestorOverdueClosing(),
                SupervisorAverageResponseTime = kpi.CalculateSupervisorAverageResponseTimeInHours(),
                AssessorAverageResponseTime = kpi.CalculateAssessorAverageResponseTimeInHours(),
                FOClosingApprove = kpi.CalculateFOClosing(),
                FOAverageClosingTime = kpi.CalculateFOAverageClosingTimeInHours(),
                status = "200" 
            };

            return Json(result);
        }

        public ActionResult DownloadReport(KPIFilterFormStub form)
        {
            byte[] excel = new KPIReport().GenerateExcelReport(form.Year);
            string filename = string.Format("Star Energy Wayang Windu - PTW KPI Report{0}.xlsx", (form.Year == null ? "" : " " + form.Year.Value.ToString()));

            return File(excel, "application/vns.ms-excel", filename);
        }

        //public int Test()
        //{
        //    KPIDepartmentModel kpi = new KPIDepartmentModel(null);
        //    kpi.CalculateDepartmentClosing();
        //    int i = 1;

        //    return i;
        //}
    }
}
