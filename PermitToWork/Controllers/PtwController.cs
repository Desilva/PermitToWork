using PermitToWork.Models;
using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hira;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.User;
using PermitToWork.Models.WorkingHeight;
using PermitToWork.Utilities;
using ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class PtwController : PdfViewController
    {
        //
        // GET: /Ptw/
        public ActionResult Index()
        {
            return PartialView();
        }

        public ActionResult Create() {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity entity = new PtwEntity();
            ViewBag.position = "Create";
            ViewBag.listUser = new ListUser(userLogin.token, userLogin.id);
            ListPtw listPtw = new ListPtw(userLogin);
            ViewBag.listFO = new MstFOEntity().getListMstFO();
            ViewBag.listAssessor = new MstAssessorEntity().getListAssessor();

            var listDepartment = new List<SelectListItem>();
            var listDept = new MstDepartmentEntity().getListMstDepartment();
            foreach (MstDepartmentEntity dept in listDept) {
                listDepartment.Add(new SelectListItem
                {
                    Text = dept.department,
                    Value = dept.id.ToString(),
                });
            }
            ViewBag.listDepartment = listDepartment;

            var listSection = new List<SelectListItem>();
            var listSect = new MstSectionEntity().getListMstSection();
            foreach (MstSectionEntity sect in listSect)
            {
                listSection.Add(new SelectListItem
                {
                    Text = sect.section,
                    Value = sect.id.ToString(),
                });
            }
            ViewBag.listSection = listSection;

            var listTotalCrew = new List<SelectListItem>();
            for (int i = 1; i <= 100; i++)
            {
                listTotalCrew.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                });
            }
            ViewBag.listTotalCrew = listTotalCrew;

            UserEntity user = Session["user"] as UserEntity;

            entity.ptw_holder_no = new MstPtwHolderNoEntity(user.id, 1);
            entity.requestor_ptw_holder_no = entity.ptw_holder_no.id;

            entity.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "");

            return PartialView(entity);
        }

        public ActionResult Edit(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            PtwEntity entity = new PtwEntity(id, user);
            ViewBag.isRequestor = entity.isRequestor(user);
            if (entity.status < (int)PtwEntity.statusPtw.CANCEL)
            {
                ViewBag.isAccSupervisor = entity.isAccSupervisor(user);
                ViewBag.isAccAssessor = entity.isAccAssessor(user);
                ViewBag.isAccFO = entity.isAccFO(user);
            }
            else
            {
                ViewBag.isCancel = true;
                ViewBag.isCanSupervisor = entity.isCanSupervisor(user);
                ViewBag.isCanAssessor = entity.isCanAssessor(user);
                ViewBag.isCanFO = entity.isCanFO(user);
            }
            ViewBag.isCanEdit = entity.isCanEdit(user);
            ViewBag.isClearenceComplete = entity.isAllClearanceComplete();
            ViewBag.isClearenceClose = entity.isAllClearanceClose();
            ViewBag.position = "Edit";
            ViewBag.listUser = new ListUser(user.token, user.id);
            ViewBag.listFO = new MstFOEntity().getListMstFO();
            ViewBag.listAssessor = new MstAssessorEntity().getListAssessor();

            var listDepartment = new List<SelectListItem>();
            var listDept = new MstDepartmentEntity().getListMstDepartment();
            foreach (MstDepartmentEntity dept in listDept)
            {
                listDepartment.Add(new SelectListItem
                {
                    Text = dept.department,
                    Value = dept.id.ToString(),
                    Selected = entity.dept_requestor == dept.id ? true : false
                });
            }
            ViewBag.listDepartment = listDepartment;

            var listSection = new List<SelectListItem>();
            var listSect = new MstSectionEntity().getListMstSection();
            foreach (MstSectionEntity sect in listSect)
            {
                listSection.Add(new SelectListItem
                {
                    Text = sect.section,
                    Value = sect.id.ToString(),
                    Selected = entity.section == sect.id ? true : false
                });
            }
            ViewBag.listSection = listSection;
            var listTotalCrew = new List<SelectListItem>();
            for (int i = 1; i <= 100; i++)
            {
                listTotalCrew.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = entity.total_crew == i.ToString() ? true : false
                });
            }
            ViewBag.listTotalCrew = listTotalCrew;

            return PartialView("create", entity);
        }

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;

            return fullUrl;
        }

        [HttpPost]
        public JsonResult GenerateNewNumber(int user_id, UserEntity user)
        {
            List<MstFOEntity> listFo = new MstFOEntity().getListMstFO();
            MstFOEntity fo = listFo.Find(p => p.id_employee == user_id);
            ListPtw listPtw = new ListPtw(user);
            
            PtwEntity entity = new PtwEntity();
            entity.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "", fo == null ? null : fo.fo_code);

            return Json(new { status = "200", message = "", ptw_number = entity.ptw_no });
        }

        [HttpPost]
        public JsonResult Add(PtwEntity ptw, int hw_need, int fi_need, int rad_need, int wh_need, int ex_need, int csep_need, IList<string> hiras)
        {
            UserEntity user = Session["user"] as UserEntity;
            List<MstFOEntity> listFo = new MstFOEntity().getListMstFO();
            int fo_id = Int32.Parse(ptw.acc_fo);
            MstFOEntity fo = listFo.Find(p => p.id_employee == fo_id);
            ListPtw listPtw = new ListPtw(user);
            ptw.generatePtwNumber(listPtw.getLastPtw() != null ? listPtw.getLastPtw().ptw_no : "", fo == null ? null : fo.fo_code);
            int ret = ptw.addPtw();

            if (ptw.acc_fo != null)
            {
                UserEntity fos = new UserEntity(Int32.Parse(ptw.acc_fo), user.token, user);
                ptw.assignFO(fos);
            }

            ListHira listHira = new ListHira();
            if (hiras != null)
            {
                listHira.changeIdPtw(hiras.ToList(), ptw.id);
            }

            if (hw_need == 1)
            {
                HwEntity hw = (HwEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.HOTWORK.ToString(), user);
                ptw.setClearancePermit(hw.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.HOTWORK.ToString());
            }

            if (fi_need == 1)
            {
                FIEntity fi = (FIEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString(), user);
                fi.sendEmailAssign(fullUrl(), user);
                ptw.setClearancePermit(fi.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString());
            }

            if (rad_need == 1)
            {
                RadEntity radiography = (RadEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.RADIOGRAPHY.ToString(), user);
                //radiography.sendEmailAssign(fullUrl(), user);
                ptw.setClearancePermit(radiography.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());
            }

            if (wh_need == 1)
            {
                WorkingHeightEntity wh = (WorkingHeightEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString(), user);
                //radiography.sendEmailAssign(fullUrl(), user);
                ptw.setClearancePermit(wh.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString());
            }

            if (ex_need == 1)
            {
                ExcavationEntity ex = (ExcavationEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.EXCAVATION.ToString(), user);
                ptw.setClearancePermit(ex.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.EXCAVATION.ToString());
            }

            if (csep_need == 1)
            {
                CsepEntity csep = (CsepEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.CONFINEDSPACE.ToString(), user);
                ptw.setClearancePermit(csep.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.CONFINEDSPACE.ToString());
            }

            ListUser listUser = new ListUser(user.token, user.id);
            List<UserEntity> listSpv = listUser.GetSupervisor(Session["user"] as UserEntity);

            

            // send email to each supervisor
            ptw.sendEmailSpv(listSpv, fullUrl());

            if (ret == 1)
            {
                return Json(new { status = "200", message = "", id = ptw.id });
            }
            else
            {
                return Json(new { status = "400", message = "There is error when saving data to database. Please check again your data." });
            }
        }

        [HttpPost]
        public JsonResult EditPtw(PtwEntity ptw)
        {
            UserEntity user = Session["user"] as UserEntity;
            if (ptw.acc_assessor != null)
            {
                UserEntity assesor = new UserEntity(Int32.Parse(ptw.acc_assessor), user.token, user);
                ptw.assignAssessor(assesor);
                ptw.setStatus((int)PtwEntity.statusPtw.CHOOSEASS);
                ptw.sendEmailAssessor(fullUrl(), user.token, user, 0);
            }
            int ret = ptw.editPtw();
            PtwEntity ptw_new = new PtwEntity(ptw.id, user);
            if (ptw.hw_need == 1 && ptw_new.hw_id == null)
            {
                HwEntity hw = (HwEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.HOTWORK.ToString(), user);
                ptw.setClearancePermit(hw.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.HOTWORK.ToString());
            }
            else if (ptw.hw_need == 0 && ptw_new.hw_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.hw_id.Value, PtwEntity.clearancePermit.HOTWORK.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.HOTWORK.ToString());
            }

            if (ptw.fi_need == 1 && ptw_new.fi_id == null)
            {
                FIEntity fi = (FIEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString(), user);
                fi.sendEmailAssign(fullUrl(), user);
                ptw.setClearancePermit(fi.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString());
            }
            else if (ptw.fi_need == 0 && ptw_new.fi_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.fi_id.Value, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString());
            }

            if (ptw.rad_need == 1 && ptw_new.rad_id == null)
            {
                RadEntity rad = (RadEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.RADIOGRAPHY.ToString(), user);
                ptw.setClearancePermit(rad.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());
            }
            else if (ptw.rad_need == 0 && ptw_new.rad_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.rad_id.Value, PtwEntity.clearancePermit.RADIOGRAPHY.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.RADIOGRAPHY.ToString());
            }

            if (ptw.wh_need == 1 && ptw_new.wh_id == null)
            {
                WorkingHeightEntity wh = (WorkingHeightEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString(), user);
                ptw.setClearancePermit(wh.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString());
            }
            else if (ptw.wh_need == 0 && ptw_new.wh_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.wh_id.Value, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.WORKINGHEIGHT.ToString());
            }

            if (ptw.ex_need == 1 && ptw_new.ex_id == null)
            {
                ExcavationEntity ex = (ExcavationEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.EXCAVATION.ToString(), user);
                ptw.setClearancePermit(ex.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.EXCAVATION.ToString());
            }
            else if (ptw.ex_need == 0 && ptw_new.ex_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.ex_id.Value, PtwEntity.clearancePermit.EXCAVATION.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.EXCAVATION.ToString());
            }

            if (ptw.csep_need == 1 && ptw_new.csep_id == null)
            {
                CsepEntity csep = (CsepEntity)addClearancePermit(ptw.id, PtwEntity.clearancePermit.CONFINEDSPACE.ToString(), user);
                ptw.setClearancePermit(csep.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.CONFINEDSPACE.ToString());
            }
            else if (ptw.csep_need == 0 && ptw_new.csep_id != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.csep_id.Value, PtwEntity.clearancePermit.CONFINEDSPACE.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.CONFINEDSPACE.ToString());
            }

            if (ptw.loto_need == 0 && ptw_new.loto_need != null)
            {
                deleteClearancePermit(ptw.id, ptw_new.csep_id.Value, PtwEntity.clearancePermit.CONFINEDSPACE.ToString(), user);
                ptw.setClearancePermit(null, null, PtwEntity.clearancePermit.CONFINEDSPACE.ToString());
            }

            if (ret == 1)
            {
                return Json(new { status = "200", message = "" });
            }
            else
            {
                return Json(new { status = "400", message = "There is error when saving data to database. Please check again your data." });
            }
        }

        public ActionResult Print(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, user);

            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;

            int a = Int32.Parse(ptw.acc_ptw_requestor);
            ptw.acc_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_supervisor);
            ptw.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_supervisor_delegate != "" && ptw.acc_supervisor_delegate != null ? ptw.acc_supervisor_delegate : "0");
            ptw.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.acc_assessor);
            ptw.acc_assessor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_assessor_delegate != "" && ptw.acc_assessor_delegate != null ? ptw.acc_assessor_delegate : "0");
            ptw.acc_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.acc_fo);
            ptw.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.acc_fo_delegate != "" && ptw.acc_fo_delegate != null ? ptw.acc_fo_delegate : "0");
            ptw.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_ptw_requestor);
            ptw.can_ptw_requestor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_supervisor);
            ptw.can_supervisor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_supervisor_delegate != "" && ptw.can_supervisor_delegate != null ? ptw.can_supervisor_delegate : "0");
            ptw.can_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_assessor);
            ptw.can_assessor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_assessor_delegate != "" && ptw.can_assessor_delegate != null ? ptw.can_assessor_delegate : "0");
            ptw.can_assessor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(ptw.can_fo);
            ptw.can_fo = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(ptw.can_fo_delegate != "" && ptw.can_fo_delegate != null ? ptw.can_fo_delegate : "0");
            ptw.can_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";

            return this.ViewPdf("", "Print", ptw);
        }

        [HttpPost]
        public JsonResult CreateNewLOTO(int id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, userLogin);
            LotoGlarfEntity loto = addNewLoto(id, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString(), userLogin);
            ptw.setClearancePermit(loto.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
            return Json(new { status = "200", id = loto.id });
        }

        [HttpPost]
        public JsonResult FromPreviousLOTO(int id, int id_prev_loto)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, userLogin);
            LotoGlarfEntity loto = createFromPreviousLoto(id, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString(), userLogin, id_prev_loto);
            ptw.setClearancePermit(loto.id, (int)PtwEntity.statusClearance.NOTCOMPLETE, PtwEntity.clearancePermit.LOCKOUTTAGOUT.ToString());
            return Json(new { status = "200", id = loto.id });
        }

        #region approve and reject PTW and Cancellation PTW

        [HttpPost]
        public JsonResult requestorAcc(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.requestorAccApproval(user);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 0);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAcc(int id, int user_id, int? assessor_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            //if (assessor_id != null)
            //{
            //    UserEntity assesor = new UserEntity(assessor_id.Value);
            //    ptw.assignAssessor(assesor);
                
            //}
            string retVal = ptw.supervisorAccApproval(user);
            if (ptw.acc_assessor != null)
            {
                ptw.sendEmailAssessor(fullUrl(), userLogin.token, userLogin, 0);
            }
            else
            {
                ptw.sendEmailFo(fullUrl(), userLogin.token, userLogin, 0);
            }
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.supervisorAccReject(user, comment);
            ptw.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorAcc(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            //if (fo_id != null)
            //{
            //    UserEntity fo = new UserEntity(fo_id.Value);
            //    ptw.assignFO(fo);
            //}
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorAccApproval(user);
            ptw.sendEmailFo(fullUrl(), userLogin.token, userLogin, 0, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.assessorAccReject(user,comment);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 0, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAcc(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.fOAccApproval(user);
            ptw.sendEmailRequestorPermitCompleted(fullUrl(), userLogin.token, userLogin, 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.fOAccReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 0, 1, comment);
            return Json(new { status = retVal });
        }

        public JsonResult cancelPtw(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.cancelPtw(user);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.requestorCanApproval(user);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.supervisorCanApproval(user);
            ptw.sendEmailAssessor(fullUrl(), userLogin.token, userLogin, 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.supervisorCanReject(user, comment);
            ptw.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorCan(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            string retVal = ptw.assessorCanApproval(user);
            ptw.sendEmailFo(fullUrl(), userLogin.token, userLogin, 1, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult assessorCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.assessorCanReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            //ptw.acc_assessor = fo_id.ToString();
            //ptw.can_assessor = fo_id.ToString();
            //ptw.acc_assessor_delegate = fo.employee_delegate.ToString();
            //ptw.can_assessor_delegate = fo.employee_delegate.ToString();
            
            string retVal = ptw.fOCanApproval(user);
            ptw.sendEmailRequestorPermitCompleted(fullUrl(), userLogin.token, userLogin, 2);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            PtwEntity ptw = new PtwEntity(id, user);
            string retVal = ptw.fOCanReject(user, comment);
            ptw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 1, comment);
            return Json(new { status = retVal });
        }

        #endregion


        [HttpPost]
        public JsonResult Binding()
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            ListPtw listPtw = new ListPtw(userLogin);
            var result = listPtw.listPtw;
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult BindingListLoto()
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            List<LotoEntity> listLoto = new LotoEntity().listLoto(userLogin);
            return Json(listLoto);
        }

        #region extends PTW

        [HttpPost]
        public JsonResult Extends(int id)
        {
            // time
            // status PTW
            // is requestor

            return Json(new { status = "200" });
        }

        public ActionResult Extend(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            PtwEntity entity = new PtwEntity(id, user);
            PtwEntity ptw = new PtwEntity();
            ViewBag.listUser = new ListUser(user.token, user.id);
            ptw.extendPtw(entity);

            //UserEntity user = Session["user"] as UserEntity;
            //ViewBag.isRequestor = ptw.isRequestor(user);
            //if (ptw.status < (int)PtwEntity.statusPtw.CANCEL)
            //{
            //    ViewBag.isAccSupervisor = ptw.isAccSupervisor(user);
            //    ViewBag.isAccAssessor = ptw.isAccAssessor(user);
            //    ViewBag.isAccFO = ptw.isAccFO(user);
            //}
            //else
            //{
            //    ViewBag.isCancel = true;
            //    ViewBag.isCanSupervisor = ptw.isCanSupervisor(user);
            //    ViewBag.isCanAssessor = ptw.isCanAssessor(user);
            //    ViewBag.isCanFO = ptw.isCanFO(user);
            //}
            //ViewBag.isCanEdit = ptw.isCanEdit(user);
            //ViewBag.isClearenceComplete = ptw.isAllClearanceComplete();
            //ViewBag.position = "Edit";
            
            return Edit(ptw.id);
        }

        #endregion

        #region setSupervisor

        // url to set who is the supervisor
        public ActionResult SetSupervisor(string a, string b, string c)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailsupervisor";

            string countSeal = Base64.MD5Seal(a + salt + val);

            if (countSeal == b)
            {
                string decodeElement = Base64.Base64Decode(c);

                if (decodeElement.Contains(salt) && decodeElement.Contains(val))
                {
                    decodeElement = decodeElement.Replace(salt, "#");
                    decodeElement = decodeElement.Replace(val, "#");

                    string[] s = decodeElement.Split('#');

                    int user_id = Int32.Parse(s[1]);
                    int ptw_id = Int32.Parse(s[2]);

                    PtwEntity ptw = new PtwEntity(ptw_id, userLogin);
                    UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);

                    if (ptw.acc_supervisor == null)
                    {
                        ptw.assignSupervisor(user);
                        if (ptw.hw_id != null)
                        {
                            HwEntity hw = new HwEntity(ptw.hw_id.Value);
                            if (hw.acc_supervisor == null)
                                hw.assignSupervisor(user);
                        }

                        if (ptw.fi_id != null)
                        {
                            FIEntity fi = new FIEntity(ptw.fi_id.Value, user);
                            if (fi.spv == null)
                                fi.assignSupervisor(user);
                        }

                        if (ptw.rad_id != null)
                        {
                            RadEntity radiography = new RadEntity(ptw.rad_id.Value, user);
                            if (radiography.supervisor == null)
                                radiography.assignSupervisor(user);
                        }

                        if (ptw.wh_id != null)
                        {
                            WorkingHeightEntity wh = new WorkingHeightEntity(ptw.wh_id.Value, user);
                            if (wh.supervisor == null)
                                wh.assignSupervisor(user);
                        }

                        if (ptw.ex_id != null)
                        {
                            ExcavationEntity ex = new ExcavationEntity(ptw.ex_id.Value, user);
                            if (ex.supervisor == null)
                                ex.assignSpv(fullUrl(),user);
                        }

                        if (ptw.csep_id != null)
                        {
                            CsepEntity csep = new CsepEntity(ptw.csep_id.Value, user);
                            if (csep.acc_supervisor == null)
                                csep.assignSupervisor(user);
                        }

                        if (ptw.loto_id != null)
                        {
                            LotoGlarfEntity loto = new LotoGlarfEntity(ptw.loto_id.Value, user);
                            if (loto.supervisor == null)
                                loto.assignSupervisor(user);
                        }
                        return RedirectToAction("Index", "Home", new { p = "Ptw/Edit/" + ptw.id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { e = "401" });
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home", new { e = "404" });
                }
            }
            else
            {
                return RedirectToAction("Index", "Home", new { e = "404" });
            }
        }

        #endregion

        #region addClearancePermit

        private IClearancePermitEntity addClearancePermit(int id, string typePermit, UserEntity user)
        {
            PtwEntity ptw = new PtwEntity(id, user);
            IClearancePermitEntity permit = new HwEntity();
            if (typePermit == PtwEntity.clearancePermit.HOTWORK.ToString())
            {
                permit = new HwEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description);
            }
            else if (typePermit == PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString())
            {
                permit = new FIEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description, ptw.acc_fo);
            }
            else if (typePermit == PtwEntity.clearancePermit.RADIOGRAPHY.ToString())
            {
                permit = new RadEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description, ptw.acc_fo);
            }
            else if (typePermit == PtwEntity.clearancePermit.WORKINGHEIGHT.ToString())
            {
                permit = new WorkingHeightEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description, ptw.acc_fo);
            }
            else if (typePermit == PtwEntity.clearancePermit.EXCAVATION.ToString())
            {
                permit = new ExcavationEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description, ptw.acc_fo, ptw.work_location, ptw.proposed_period_start, ptw.proposed_period_end);
            }
            else if (typePermit == PtwEntity.clearancePermit.CONFINEDSPACE.ToString())
            {
                permit = new CsepEntity(ptw.id, ptw.acc_ptw_requestor, ptw.work_description);
            }

            permit.generateNumber(ptw.ptw_no);
            permit.create();

            return permit;
        }

        public LotoGlarfEntity addNewLoto(int id, string typePermit, UserEntity user)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, user);
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            LotoGlarfEntity glarf = new LotoGlarfEntity(ptw.acc_ptw_requestor, ptw.acc_supervisor);
            glarf.create();

            LotoEntity loto = new LotoEntity(ptw.acc_ptw_requestor, ptw.work_location, glarf.id, ptw.acc_supervisor);
            List<UserEntity> listHWFO = listUser.GetHotWorkFO();
            loto.generateNumber(ptw.ptw_no);
            loto.create();
            loto.sendEmailFO(listHWFO, fullUrl(), userLogin.token, user, 0);

            
            glarf.assignLotoForm(loto.id, loto.loto_no);

            return glarf;
        }

        public LotoGlarfEntity createFromPreviousLoto(int id, string typePermit, UserEntity user, int id_prev_loto)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, user);
            LotoEntity prevLoto = new LotoEntity(id_prev_loto, userLogin);
            LotoEntity loto = new LotoEntity(prevLoto, user);
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            List<UserEntity> listHWFO = listUser.GetHotWorkFO();
            loto.generateLotoReviewNumber(prevLoto.loto_no);
            loto.create();

            foreach (LotoPointEntity lotoPointPrev in prevLoto.lotoPoint)
            {
                LotoPointEntity lotoPoint = new LotoPointEntity(lotoPointPrev, loto.id);
                lotoPoint.create();
            }

            LotoGlarfEntity glarf = new LotoGlarfEntity(ptw.acc_ptw_requestor, ptw.acc_supervisor);
            glarf.create();
            glarf.assignLotoForm(loto.id, loto.loto_no);
            loto.addNewHolder(userLogin.id.ToString(), glarf.id);

            List<LotoGlarfEntity> listGlarf = new LotoGlarfEntity().listLotoGlarfWithSameLotoPermit(prevLoto.id, userLogin);
            foreach (LotoGlarfEntity gl in listGlarf)
            {
                gl.assignLotoForm(loto.id, loto.loto_no);
            }

            return glarf;
        }

        public string deleteClearancePermit(int id, int permit_id, string typePermit, UserEntity user)
        {
            PtwEntity ptw = new PtwEntity(id, user);
            IClearancePermitEntity permit = new HwEntity();
            if (typePermit == PtwEntity.clearancePermit.HOTWORK.ToString())
            {
                permit = new HwEntity(permit_id);
            }
            else if (typePermit == PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString())
            {
                permit = new FIEntity(permit_id, user);
            }
            else if (typePermit == PtwEntity.clearancePermit.RADIOGRAPHY.ToString())
            {
                permit = new RadEntity(permit_id, user);
            }
            else if (typePermit == PtwEntity.clearancePermit.WORKINGHEIGHT.ToString())
            {
                permit = new WorkingHeightEntity(permit_id, user);
            }
            else if (typePermit == PtwEntity.clearancePermit.EXCAVATION.ToString())
            {
                permit = new ExcavationEntity(permit_id, user);
            }
            else if (typePermit == PtwEntity.clearancePermit.CONFINEDSPACE.ToString())
            {
                permit = new CsepEntity(permit_id, user);
            }

            permit.delete();

            return "200";
        }

        #endregion
    }
}
