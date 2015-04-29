using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

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
                return RedirectToAction("Index", "Login", new { p = p });
            }

            if (p != null)
            {
                ViewBag.p = p;
            }

            UserEntity user = Session["user"] as UserEntity;

            HttpContext.Application.Lock();
            HttpContext.Application["ListUser"] = new ListUser(user.token, user.id);
            HttpContext.Application.UnLock();

            ViewBag.inspector = new MstInspectorEntity().getListInspector(user).Exists(d => d.user.id == user.id);
            ViewBag.isFo = new MstFOEntity().getListMstFO(user).Exists(c => c.id_employee == user.id);
            ViewBag.isProd = user.department == "Production";

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

        public ActionResult Page(string p)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login", new { p = p });
            }
            ViewBag.p = p;
            return View();
        }

        public ActionResult PartialIndex()
        {
            return PartialView("Index");
        }

        [HttpPost]
        public string Binding(int type = 0)
        {
            GridRequestParameters param = GridRequestParameters.Current;
            int total = 0;
            UserEntity user = Session["user"] as UserEntity;
            var result = new List<PtwEntity>();
            if (user.id != 1)
            {
                if (Session["ListPtwByUser"] != null)
                {
                    int? types = Session["ListPtwType"] as int?;
                    if (types == type)
                    {
                        DateTime? LastListUserDate = Session["ListPtwDate"] as DateTime?;
                        if (DateTime.Now.Subtract(LastListUserDate.Value).TotalMinutes <= 30)
                        {
                            result = Session["ListPtwByUser"] as List<PtwEntity>;
                            total = result.Count;
                            result = result.Skip(param.Skip.Value).Take(param.Take.Value).ToList();
                        }
                        else
                        {
                            ListPtw listPtw = new ListPtw();
                            result = listPtw.listPtwByUser(user, type, HttpContext.Application["ListUser"] as ListUser);
                            total = result.Count;
                            result = result.Skip(param.Skip.Value).Take(param.Take.Value).ToList();
                        }
                    }
                    else
                    {
                        ListPtw listPtw = new ListPtw();
                        result = listPtw.listPtwByUser(user, type, HttpContext.Application["ListUser"] as ListUser);
                        total = result.Count;
                        result = result.Skip(param.Skip.Value).Take(param.Take.Value).ToList();
                    }
                }
                else
                {
                    ListPtw listPtw = new ListPtw();
                    result = listPtw.listPtwByUser(user, type, HttpContext.Application["ListUser"] as ListUser);
                    total = result.Count;
                    result = result.Skip(param.Skip.Value).Take(param.Take.Value).ToList();
                }
            }
            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = Int32.MaxValue;
            return serializer.Serialize(new { total = total, data = result });
        }

        [HttpPost]
        public JsonResult BindingOwnRequest()
        {
            UserEntity user = Session["user"] as UserEntity;
            var result = new List<PtwEntity>();
            if (user.id != 1)
            {
                ListPtw listPtw = new ListPtw();
                result = listPtw.listPtwOwn(user);
            }
            return Json(result);
        }


        public ActionResult TestExternalUrl()
        {
            // In some cases you might want to pull completely different URL that is not related to your application.
            // You can do that by specifying full URL.

            return new UrlAsPdf("http://www.github.com")
            {
                FileName = "TestExternalUrl.pdf",
                PageMargins = new Margins(0, 0, 0, 0)
            };
        }

        public ActionResult ExportExcel()
        {
            UserEntity user = Session["user"] as UserEntity;
            ListPtw listPtw = new ListPtw();
            List<PtwExcelEntity> result = listPtw.listPtwExcel(user);

            GridView gv = new GridView();
            gv.Caption = "Permit To Work";
            gv.DataSource = result;
            if (result.Count == 0)
            {
                return new JavaScriptResult();
            }
            gv.DataBind();
            gv.HeaderRow.Cells[0].Text = "PTW No.";
            gv.HeaderRow.Cells[1].Text = "Proposed Period Start";
            gv.HeaderRow.Cells[2].Text = "Proposed Period End";
            gv.HeaderRow.Cells[3].Text = "Department Of Requestor";
            gv.HeaderRow.Cells[4].Text = "Section";
            gv.HeaderRow.Cells[5].Text = "Total Crew";
            gv.HeaderRow.Cells[6].Text = "Requestor's PTW Holder No.";
            gv.HeaderRow.Cells[7].Text = "Area";
            gv.HeaderRow.Cells[8].Text = "Work Location";
            gv.HeaderRow.Cells[9].Text = "Area Code";
            gv.HeaderRow.Cells[10].Text = "Work Order No.";
            gv.HeaderRow.Cells[11].Text = "Work Description";
            gv.HeaderRow.Cells[12].Text = "HIRA";
            gv.HeaderRow.Cells[13].Text = "LOTO?";
            gv.HeaderRow.Cells[14].Text = "CSEP?";
            gv.HeaderRow.Cells[15].Text = "Hot Work?";
            gv.HeaderRow.Cells[16].Text = "Fire Impairment?";
            gv.HeaderRow.Cells[17].Text = "Excavation?";
            gv.HeaderRow.Cells[18].Text = "Working At Height?";
            gv.HeaderRow.Cells[19].Text = "Radiographic?";
            gv.HeaderRow.Cells[20].Text = "Pre Job Screening Q1 (Spv)";
            gv.HeaderRow.Cells[21].Text = "Pre Job Screening Q2 (Spv)";
            gv.HeaderRow.Cells[22].Text = "Pre Job Screening Q3 (Spv)";
            gv.HeaderRow.Cells[23].Text = "Pre Job Screening Q4 (Spv)";
            gv.HeaderRow.Cells[24].Text = "Pre Job Screening Q5 (Spv)";
            gv.HeaderRow.Cells[25].Text = "Pre Job Screening Q6 (Spv)";
            gv.HeaderRow.Cells[26].Text = "Pre Job Screening Q7 (Spv)";
            gv.HeaderRow.Cells[27].Text = "Pre Job Screening Q1 (FO)";
            gv.HeaderRow.Cells[28].Text = "Pre Job Screening Q2 (FO)";
            gv.HeaderRow.Cells[29].Text = "Pre Job Screening Q3 (FO)";
            gv.HeaderRow.Cells[30].Text = "Pre Job Screening Q4 (FO)";
            gv.HeaderRow.Cells[31].Text = "Pre Job Screening Q5 (FO)";
            gv.HeaderRow.Cells[32].Text = "Pre Job Screening Q6 (FO)";
            gv.HeaderRow.Cells[33].Text = "Pre Job Screening Q7 (FO)";
            gv.HeaderRow.Cells[34].Text = "Pre Job Screening Notes";
            gv.HeaderRow.Cells[35].Text = "Notes FO To Assessor";
            gv.HeaderRow.Cells[36].Text = "Notes Assessot To FO";
            gv.HeaderRow.Cells[37].Text = "Validity Period Start";
            gv.HeaderRow.Cells[38].Text = "Validity Period End";
            gv.HeaderRow.Cells[39].Text = "PTW Requestor";
            gv.HeaderRow.Cells[40].Text = "Supervisor";
            gv.HeaderRow.Cells[41].Text = "Delegation of Supervisor";
            gv.HeaderRow.Cells[42].Text = "Assessor";
            gv.HeaderRow.Cells[43].Text = "Delegation of Assessor";
            gv.HeaderRow.Cells[44].Text = "Facility Owner";
            gv.HeaderRow.Cells[45].Text = "Delegation of Facility Owner";
            gv.HeaderRow.Cells[46].Text = "Cancellation Screening Q1 (Spv)";
            gv.HeaderRow.Cells[47].Text = "Cancellation Screening Q2 (Spv)";
            gv.HeaderRow.Cells[48].Text = "Cancellation Screening Q3 (Spv)";
            gv.HeaderRow.Cells[49].Text = "Cancellation Screening Q4 (Spv)";
            gv.HeaderRow.Cells[50].Text = "Cancellation Screening Q5 (Spv)";
            gv.HeaderRow.Cells[51].Text = "Cancellation Screening Q6 (Spv)";
            gv.HeaderRow.Cells[52].Text = "Cancellation Screening Q7 (Spv)";
            gv.HeaderRow.Cells[53].Text = "Cancellation Screening Q1 (FO)";
            gv.HeaderRow.Cells[54].Text = "Cancellation Screening Q2 (FO)";
            gv.HeaderRow.Cells[55].Text = "Cancellation Screening Q3 (FO)";
            gv.HeaderRow.Cells[56].Text = "Cancellation Screening Q4 (FO)";
            gv.HeaderRow.Cells[57].Text = "Cancellation Screening Q5 (FO)";
            gv.HeaderRow.Cells[58].Text = "Cancellation Screening Q6 (FO)";
            gv.HeaderRow.Cells[59].Text = "Cancellation Screening Q7 (FO)";
            gv.HeaderRow.Cells[60].Text = "Cancellation Screening Notes";
            gv.HeaderRow.Cells[61].Text = "Cancellation Notes Assessor to FO";
            gv.HeaderRow.Cells[62].Text = "Cancellation Date";
            gv.HeaderRow.Cells[63].Text = "Cancellation Requestor";
            gv.HeaderRow.Cells[64].Text = "Cancellation Supervisor";
            gv.HeaderRow.Cells[65].Text = "Delegation of Cancellation Supervisor";
            gv.HeaderRow.Cells[66].Text = "Cancellation Assessor";
            gv.HeaderRow.Cells[67].Text = "Delegation of Cancellation Assessor";
            gv.HeaderRow.Cells[68].Text = "Cancellation Facility Owner";
            gv.HeaderRow.Cells[69].Text = "Delegation of Cancellation Facility Owner";
            gv.HeaderRow.Cells[70].Text = "Status";

            if (gv != null)
            {
                return new DownloadFileActionResult(gv, "PTW (" + DateTime.Now.ToString("yyyyMMdd") + ").xls");
            }
            else
            {
                return new JavaScriptResult();
            }
        }
    }
}
