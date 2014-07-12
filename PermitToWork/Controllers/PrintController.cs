using PermitToWork.Models;
using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.SafetyBriefing;
using PermitToWork.Models.User;
using PermitToWork.Models.WorkingHeight;
using Rotativa;
using Rotativa.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class PrintController : Controller
    {
        //
        // GET: /Print/

        public ActionResult Index(int id, int type, string no)
        {
            // Now I realize that this isn't very expressive example of why this can be useful.
            // However imagine that you have your own UrlHelper extensions like UrlHelper.User(...)
            // where you create correct URL according to passed conditions, prepare some complex model, etc.
            UserEntity user = Session["user"] as UserEntity;
            var urlHelper = new UrlHelper(Request.RequestContext);
            string url = "";
            string fileName = "";
            Size s = Size.A4;
            Orientation o = Orientation.Portrait;
            switch (type)
            {
                case 0 /* PTW */:
                    url = urlHelper.Action("PtwPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "PTW (" + no + ")";
                    break;
                case 1 /* Safety Briefing */:
                    url = urlHelper.Action("SafetyBriefingPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "Safety Briefing";
                    break;
                case 2 /* LOTO */:
                    url = urlHelper.Action("LotoPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "LOTO (" + no + ")";
                    s = Size.A3;
                    o = Orientation.Landscape;
                    break;
                case 3 /* GLARF */:
                    url = urlHelper.Action("GlarfPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "GLARF (" + no + ")";
                    break;
                case 4 /* CSEP */:
                    url = urlHelper.Action("CsepPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "CSEP (" + no + ")";
                    break;
                case 5 /* Hot Work */:
                    url = urlHelper.Action("HwPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "HW (" + no + ")";
                    break;
                case 6 /* Excavation */:
                    url = urlHelper.Action("ExPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "Ex (" + no + ")";
                    break;
                case 7 /* Fire Impairment */:
                    url = urlHelper.Action("FiPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "FI (" + no + ")";
                    break;
                case 8 /* Working at Height */:
                    url = urlHelper.Action("WhPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "WH (" + no + ")";
                    break;
                case 9 /* Radiography */:
                    url = urlHelper.Action("RadPrint", new { id = id, user_id = user.id, user_token = user.token });
                    fileName = "Rad (" + no + ")";
                    break;

            }

            return new UrlAsPdf(url) { FileName = fileName,
                PageMargins = new Margins(5, 5, 5, 5),
                PageSize = s,
                PageOrientation = o,
            };
        }

        public ActionResult PtwPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            PtwEntity ptw = new PtwEntity(id, user);

            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;

            int a = 0;
            if (Int32.TryParse(ptw.acc_ptw_requestor, out a))
            {
                ptw.acc_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            }
            if (Int32.TryParse(ptw.acc_supervisor, out a))
            {
                ptw.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.acc_supervisor_delegate != "" && ptw.acc_supervisor_delegate != null ? ptw.acc_supervisor_delegate : "0");
            ptw.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(ptw.acc_assessor, out a))
            {
                ptw.acc_assessor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.acc_assessor_delegate != "" && ptw.acc_assessor_delegate != null ? ptw.acc_assessor_delegate : "0");
            ptw.acc_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(ptw.acc_fo, out a))
            {
                ptw.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.acc_fo_delegate != "" && ptw.acc_fo_delegate != null ? ptw.acc_fo_delegate : "0");
            ptw.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(ptw.can_ptw_requestor, out a))
            {
                ptw.can_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            }
            if (Int32.TryParse(ptw.can_supervisor, out a))
            {
                ptw.can_supervisor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.can_supervisor_delegate != "" && ptw.can_supervisor_delegate != null ? ptw.can_supervisor_delegate : "0");
            ptw.can_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(ptw.can_assessor, out a))
            {
                ptw.can_assessor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.can_assessor_delegate != "" && ptw.can_assessor_delegate != null ? ptw.can_assessor_delegate : "0");
            ptw.can_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(ptw.can_fo, out a))
            {
                ptw.can_fo = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(ptw.can_fo_delegate != "" && ptw.can_fo_delegate != null ? ptw.can_fo_delegate : "0");
            ptw.can_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";

            //return this.ViewPdf("", "Print", ptw);

            return View(ptw);
        }

        public ActionResult FiPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            FIEntity fi = new FIEntity(id, user);
            fi.getPtw(user);
            fi.getHiraNo();

            //return this.ViewPdf("", "Print", ptw);

            return View(fi);
        }

        public ActionResult HwPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            HwEntity hw = new HwEntity(id);
            hw.GetPtw(user);
            hw.getHiraNo();

            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;
            ViewBag.listUser = listUser;

            int a = 0;
            if (Int32.TryParse(hw.fire_watch, out a))
            {
                hw.fire_watch = listUser.Find(p => p.id == a).alpha_name;
            }
            if (Int32.TryParse(hw.work_leader, out a))
            {
                hw.work_leader = listUser.Find(p => p.id == a).alpha_name;
            }
            if (Int32.TryParse(hw.acc_supervisor, out a))
            {
                hw.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(hw.acc_supervisor_delegate != "" && hw.acc_supervisor_delegate != null ? hw.acc_supervisor_delegate : "0");
            hw.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(hw.acc_fo, out a))
            {
                hw.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(hw.acc_fo_delegate != "" && hw.acc_fo_delegate != null ? hw.acc_fo_delegate : "0");
            hw.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(hw.acc_gas_tester, out a))
            {
                hw.acc_gas_tester = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(hw.ext_gas_tester_1 != "" && hw.ext_gas_tester_1 != null ? hw.ext_gas_tester_1 : "0");
            hw.ext_gas_tester_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_2 != "" && hw.ext_gas_tester_2 != null ? hw.ext_gas_tester_2 : "0");
            hw.ext_gas_tester_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_3 != "" && hw.ext_gas_tester_3 != null ? hw.ext_gas_tester_3 : "0");
            hw.ext_gas_tester_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_4 != "" && hw.ext_gas_tester_4 != null ? hw.ext_gas_tester_4 : "0");
            hw.ext_gas_tester_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_5 != "" && hw.ext_gas_tester_5 != null ? hw.ext_gas_tester_5 : "0");
            hw.ext_gas_tester_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_6 != "" && hw.ext_gas_tester_6 != null ? hw.ext_gas_tester_6 : "0");
            hw.ext_gas_tester_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_gas_tester_7 != "" && hw.ext_gas_tester_7 != null ? hw.ext_gas_tester_7 : "0");
            hw.ext_gas_tester_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_1 != "" && hw.ext_fo_1 != null ? hw.ext_fo_1 : "0");
            hw.ext_fo_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_1 != "" && hw.ext_fo_delegate_1 != null ? hw.ext_fo_delegate_1 : "0");
            hw.ext_fo_delegate_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_2 != "" && hw.ext_fo_2 != null ? hw.ext_fo_2 : "0");
            hw.ext_fo_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_2 != "" && hw.ext_fo_delegate_2 != null ? hw.ext_fo_delegate_2 : "0");
            hw.ext_fo_delegate_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_3 != "" && hw.ext_fo_3 != null ? hw.ext_fo_3 : "0");
            hw.ext_fo_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_3 != "" && hw.ext_fo_delegate_3 != null ? hw.ext_fo_delegate_3 : "0");
            hw.ext_fo_delegate_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_4 != "" && hw.ext_fo_4 != null ? hw.ext_fo_4 : "0");
            hw.ext_fo_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_4 != "" && hw.ext_fo_delegate_4 != null ? hw.ext_fo_delegate_4 : "0");
            hw.ext_fo_delegate_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_5 != "" && hw.ext_fo_5 != null ? hw.ext_fo_5 : "0");
            hw.ext_fo_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_5 != "" && hw.ext_fo_delegate_5 != null ? hw.ext_fo_delegate_5 : "0");
            hw.ext_fo_delegate_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_6 != "" && hw.ext_fo_6 != null ? hw.ext_fo_6 : "0");
            hw.ext_fo_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_6 != "" && hw.ext_fo_delegate_6 != null ? hw.ext_fo_delegate_6 : "0");
            hw.ext_fo_delegate_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_7 != "" && hw.ext_fo_7 != null ? hw.ext_fo_7 : "0");
            hw.ext_fo_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.ext_fo_delegate_7 != "" && hw.ext_fo_delegate_7 != null ? hw.ext_fo_delegate_7 : "0");
            hw.ext_fo_delegate_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.can_supervisor != "" && hw.can_supervisor != null ? hw.can_supervisor : "0");
            hw.can_supervisor = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.can_supervisor_delegate != "" && hw.can_supervisor_delegate != null ? hw.can_supervisor_delegate : "0");
            hw.can_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.can_fo != "" && hw.can_fo != null ? hw.can_fo : "0");
            hw.can_fo = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.can_fo_delegate != "" && hw.can_fo_delegate != null ? hw.can_fo_delegate : "0");
            hw.can_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";

            return View(hw);
        }

        public ActionResult CsepPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            CsepEntity csep = new CsepEntity(id, user);
            csep.GetPtw(user);
            csep.getHiraNo();

            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;
            ViewBag.listUser = listUser;

            int a = 0;
            if (Int32.TryParse(csep.work_leader, out a))
            {
                csep.work_leader = listUser.Find(p => p.id == a).alpha_name;
            }
            if (Int32.TryParse(csep.acc_supervisor, out a))
            {
                csep.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(csep.acc_supervisor_delegate != "" && csep.acc_supervisor_delegate != null ? csep.acc_supervisor_delegate : "0");
            csep.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(csep.acc_fo, out a))
            {
                csep.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(csep.acc_fo_delegate != "" && csep.acc_fo_delegate != null ? csep.acc_fo_delegate : "0");
            csep.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            if (Int32.TryParse(csep.acc_gas_tester, out a))
            {
                csep.acc_gas_tester = listUser.Find(p => p.id == a).alpha_name;
            }
            a = Int32.Parse(csep.ext_gas_tester_1 != "" && csep.ext_gas_tester_1 != null ? csep.ext_gas_tester_1 : "0");
            csep.ext_gas_tester_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_2 != "" && csep.ext_gas_tester_2 != null ? csep.ext_gas_tester_2 : "0");
            csep.ext_gas_tester_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_3 != "" && csep.ext_gas_tester_3 != null ? csep.ext_gas_tester_3 : "0");
            csep.ext_gas_tester_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_4 != "" && csep.ext_gas_tester_4 != null ? csep.ext_gas_tester_4 : "0");
            csep.ext_gas_tester_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_5 != "" && csep.ext_gas_tester_5 != null ? csep.ext_gas_tester_5 : "0");
            csep.ext_gas_tester_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_6 != "" && csep.ext_gas_tester_6 != null ? csep.ext_gas_tester_6 : "0");
            csep.ext_gas_tester_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_gas_tester_7 != "" && csep.ext_gas_tester_7 != null ? csep.ext_gas_tester_7 : "0");
            csep.ext_gas_tester_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_1 != "" && csep.ext_fo_1 != null ? csep.ext_fo_1 : "0");
            csep.ext_fo_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_1 != "" && csep.ext_fo_delegate_1 != null ? csep.ext_fo_delegate_1 : "0");
            csep.ext_fo_delegate_1 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_2 != "" && csep.ext_fo_2 != null ? csep.ext_fo_2 : "0");
            csep.ext_fo_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_2 != "" && csep.ext_fo_delegate_2 != null ? csep.ext_fo_delegate_2 : "0");
            csep.ext_fo_delegate_2 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_3 != "" && csep.ext_fo_3 != null ? csep.ext_fo_3 : "0");
            csep.ext_fo_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_3 != "" && csep.ext_fo_delegate_3 != null ? csep.ext_fo_delegate_3 : "0");
            csep.ext_fo_delegate_3 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_4 != "" && csep.ext_fo_4 != null ? csep.ext_fo_4 : "0");
            csep.ext_fo_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_4 != "" && csep.ext_fo_delegate_4 != null ? csep.ext_fo_delegate_4 : "0");
            csep.ext_fo_delegate_4 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_5 != "" && csep.ext_fo_5 != null ? csep.ext_fo_5 : "0");
            csep.ext_fo_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_5 != "" && csep.ext_fo_delegate_5 != null ? csep.ext_fo_delegate_5 : "0");
            csep.ext_fo_delegate_5 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_6 != "" && csep.ext_fo_6 != null ? csep.ext_fo_6 : "0");
            csep.ext_fo_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_6 != "" && csep.ext_fo_delegate_6 != null ? csep.ext_fo_delegate_6 : "0");
            csep.ext_fo_delegate_6 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_7 != "" && csep.ext_fo_7 != null ? csep.ext_fo_7 : "0");
            csep.ext_fo_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.ext_fo_delegate_7 != "" && csep.ext_fo_delegate_7 != null ? csep.ext_fo_delegate_7 : "0");
            csep.ext_fo_delegate_7 = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.can_supervisor != "" && csep.can_supervisor != null ? csep.can_supervisor : "0");
            csep.can_supervisor = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.can_supervisor_delegate != "" && csep.can_supervisor_delegate != null ? csep.can_supervisor_delegate : "0");
            csep.can_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.can_fo != "" && csep.can_fo != null ? csep.can_fo : "0");
            csep.can_fo = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(csep.can_fo_delegate != "" && csep.can_fo_delegate != null ? csep.can_fo_delegate : "0");
            csep.can_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";

            return View(csep);
        }

        public ActionResult WhPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            WorkingHeightEntity wh = new WorkingHeightEntity(id, user);

            //return this.ViewPdf("", "Print", ptw);

            return View(wh);
        }

        public ActionResult ExPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            ExcavationEntity ex = new ExcavationEntity(id, user);
            ex.getPtw(user);
            ex.getHiraNo();

            //return this.ViewPdf("", "Print", ptw);

            return View(ex);
        }

        public ActionResult RadPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            RadEntity rad = new RadEntity(id, user);

            //return this.ViewPdf("", "Print", ptw);

            return View(rad);
        }

        public ActionResult LotoPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            LotoEntity loto = new LotoEntity(id, user);

            //return this.ViewPdf("", "Print", ptw);

            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;
            ViewBag.listUser = listUser;

            return View(loto);
        }

        public ActionResult GlarfPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);

            //return this.ViewPdf("", "Print", ptw);

            return View(glarf);
        }

        public ActionResult SafetyBriefingPrint(int id, int user_id, string user_token)
        {
            UserEntity user = new UserEntity(user_id, user_token);
            SafetyBriefingEntity sb = new SafetyBriefingEntity(id, user);

            //return this.ViewPdf("", "Print", ptw);

            return View(sb);
        }
    }
}
