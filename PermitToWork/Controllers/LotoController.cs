using PermitToWork.Models;
using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Master;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using PermitToWork.WWUserService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class LotoController : Controller
    {
        //
        // GET: /Loto/

        public ActionResult Index(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            PtwEntity entity = new PtwEntity(id, user);
            ViewBag.isCanAddLoto = entity.isAccSupervisor(user) && (entity.loto_status == null || entity.loto_status < 1);
            ViewBag.isAddNewLoto = entity.lotoPermit.Where(p => p.requestor == entity.acc_ptw_requestor).Count() == 0;
            ViewBag.isCanCancel = entity.status == (int)PtwEntity.statusPtw.ACCFO && entity.isAccSupervisor(user);
            return PartialView(entity);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult Create1()
        {
            return PartialView();
        }

        public ActionResult Glarf(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);

            bool[] isCanEdit = new bool[3];

            isCanEdit[0] = glarf.isCanEditForm(user);
            isCanEdit[1] = glarf.isCanUploadCancel(user);

            ViewBag.isCanEdit = isCanEdit;

            return PartialView(glarf);
        }

        public ActionResult LotoPoint()
        {
            UserEntity user = Session["user"] as UserEntity;
            return PartialView();
        }

        public ActionResult Edit(int id, int? id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity entity = new LotoEntity(id, user);

            bool[] isCanEdit = new bool[27];

            isCanEdit[0] = entity.isCanEditFirstRequestor(user);
            isCanEdit[1] = entity.isCanAgreedApplied(user);
            isCanEdit[2] = entity.isCanInspect(user);
            isCanEdit[3] = entity.isCanApproveSpv(user);
            isCanEdit[4] = entity.isCanApproveFO(user);
            isCanEdit[5] = entity.isCanApproveChangeComingHolder(user);
            isCanEdit[6] = entity.isCanEditComingHolder(user);
            isCanEdit[7] = entity.isCanApproveOtherHolder(user);
            isCanEdit[8] = entity.isFOCanAgreedAndAppliedChange(user);
            isCanEdit[9] = entity.isCanInspectChange(user);
            isCanEdit[10] = entity.isCanApproveSpvChange(user);
            isCanEdit[11] = entity.isCanApproveFOChange(user);
            isCanEdit[12] = entity.isCanApprovingOtherHolder(user);
            isCanEdit[13] = entity.isCanCancel(user);
            isCanEdit[14] = entity.isCanCancelSpv(user);
            isCanEdit[15] = entity.isCanCancelFO(user);
            isCanEdit[16] = entity.isCanSuspend(user);
            isCanEdit[17] = entity.isCanEditOnSuspension(user);
            isCanEdit[18] = entity.isCanApproveChangeSuspension(user);
            isCanEdit[19] = entity.isCanSetAgreedRemovedFO(user);
            isCanEdit[20] = entity.isCanInspectChangeHolder(user);
            isCanEdit[21] = entity.isCanApproveFOSuspension(user);
            isCanEdit[22] = entity.isCanCompleteSuspension(user);
            isCanEdit[23] = entity.isCanAddPointOnCompleteSuspension(user);
            isCanEdit[24] = entity.isCanAgreedNewLotoPointCompleteSuspension(user);
            isCanEdit[25] = entity.isCanSetAppliedCompleteSuspension(user);
            isCanEdit[26] = entity.isCanInspectHolderCompleteSuspension(user);


            ViewBag.isCanEdit = isCanEdit;

            ViewBag.id_loto = id_loto;

            ViewBag.listLotoPoint = new star_energy_ptwEntities().mst_loto_point.ToList();
            ViewBag.listUser = new ListUser(user.token, user.id).listUser;

            return PartialView("Create2", entity);
        }

        [HttpPost]
        public JsonResult CreateNewLoto(int id_ptw)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id_ptw, userLogin);
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            LotoGlarfEntity glarf = new LotoGlarfEntity(ptw.acc_ptw_requestor, ptw.acc_supervisor);
            glarf.create();

            MstFOEntity foProd = new MstFOEntity("PROD", userLogin);
            LotoEntity loto = new LotoEntity(ptw.acc_ptw_requestor, ptw.work_location, glarf.id, ptw.acc_supervisor, foProd.user.id.ToString());
            //List<UserEntity> listHWFO = listUser.GetHotWorkFO();
            loto.generateNumber(ptw.ptw_no);
            loto.create(glarf.id);
            //loto.sendEmailFO(listHWFO, fullUrl(), userLogin.token, userLogin, 0);

            ptw.addLoto(loto);

            glarf.assignLotoForm(loto.id, loto.loto_no);

            return Json(new { status = "200", id = loto.id });
        }

        [HttpPost]
        public JsonResult FromPreviousLoto(int id, int id_prev_loto)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            PtwEntity ptw = new PtwEntity(id, userLogin);
            LotoEntity prevLoto = new LotoEntity(id_prev_loto, userLogin);
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            LotoGlarfEntity glarf = new LotoGlarfEntity(ptw.acc_ptw_requestor, ptw.acc_supervisor);
            glarf.create();

            prevLoto.addNewHolder(ptw.acc_ptw_requestor, ptw.acc_supervisor, glarf.id);
            ptw.addLoto(prevLoto);

            glarf.assignLotoForm(prevLoto.id, prevLoto.loto_no);

            return Json(new { status = "200", id = prevLoto.id });
        }

        [HttpPost]
        public JsonResult LockBox()
        {
            List<MstLockBoxEntity> listBox = new MstLockBoxEntity().getListLockBox();
            return Json(listBox);
        }

        public JsonResult ListingLOTOPoint()
        {
            List<mst_loto_point> listLotoPoint = new star_energy_ptwEntities().mst_loto_point.ToList();
            return Json(listLotoPoint, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListingEmployee()
        {
            UserEntity user = Session["user"] as UserEntity;
            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser;
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListingEmployee2()
        {
            UserEntity user = Session["user"] as UserEntity;
            List<UserEntity> listUser = new ListUser(user.token, user.id).GetListEmployeeInDepartment("Production");
            return Json(listUser, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult FindLotoPoint(string code)
        {
            mst_loto_point lotoPoint = new star_energy_ptwEntities().mst_loto_point.Find(code);
            string result = lotoPoint != null ? lotoPoint.description : "";
            return Json(result);
        }

        [HttpPost]
        public JsonResult FindEmployee(string employee_id)
        {
            UserEntity user = Session["user"] as UserEntity;
            string name = "";
            if (employee_id != "" && employee_id != "null")
            {
                UserEntity userEmployee = new UserEntity(Int32.Parse(employee_id), user.token, user);
                name = userEmployee.alpha_name;
            }
            return Json(name);
        }

        [HttpPost]
        public JsonResult countQty(string value)
        {
            List<string> list = value.Split('#').ToList();
            List<MstLockBoxEntity> listBox = new MstLockBoxEntity().getListLockBox();
            int count = 0;
            foreach (string s in list)
            {
                if (s != "")
                {
                    MstLockBoxEntity lockBox = listBox.Find(p => p.id == Int32.Parse(s));
                    count += lockBox.quantity.Value;
                }
            }
            return Json(count);
        }

        [HttpPost]
        public JsonResult SaveDraft(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = loto.edit();
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveComplete(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = loto.edit();
            retVal &= loto.sendToFO();
            loto = new LotoEntity(loto.id, user);
            if (loto.approval_facility_owner == null)
            {
                ListUser listUser = new ListUser(user.token, user.id);
                List<UserEntity> listHWFO = listUser.GetHotWorkFO();
                loto.sendEmailFO(listHWFO, fullUrl(), user.token, user, 0);
            }
            else
            {
                loto.sendEmailFOAgreed(fullUrl(), user);
            }
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult RejectToSupervisor(LotoEntity loto, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = loto.rejectToSupervisor();
            loto = new LotoEntity(loto.id, user);
            loto.sendEmailRejectSpv(fullUrl(), user, comment);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveAndInspect(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            loto.edit();
            int retVal = loto.sendToInspect();
            loto = new LotoEntity(loto.id, user);
            loto.sendEmailInspected(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveAndSendApprove(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = loto.sendToApprove();
            loto = new LotoEntity(loto.id, user);
            loto.sendEmailSupervisorApprove(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveApprove(int id, int who, string approval_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.approval_notes = approval_notes;
            if (who == 1)
            {
                retVal &= loto.approvalSupervisor(user);
                loto.sendEmailFOApprove(fullUrl(), user);
            }
            else if (who == 2)
            {
                retVal &= loto.approvalFacilityOwner(user);
            }
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveComingHolderApprove(int id, string approval_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.approval_notes = approval_notes;
            retVal &= loto.approvalOncomingHolder(user);
            loto.sendEmailOnComingHolderApprove(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult requestSuspension(int id, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.suspendLoto(user);

            // send email for comment on request
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SendApprovalSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.sendAgreePointSuspension(user, notes);
            loto.sendEmailAgreeSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult agreeSuspension(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.agreeSuspension(user);
            loto.sendEmailHolderAgreedSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult rejectSuspension(int id, string notes, string comment)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            // retVal &= loto.saveAppliedFOSuspension(user, notes);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        

        [HttpPost]
        public JsonResult saveApprovalFOSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.saveAppliedFOSuspension(user, notes);
            loto.sendEmailFoAppliedSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult approveSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.saveApprovedInspected(user, notes);
            loto.sendEmailCompleteInspectedSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult approveFOSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.foApproveSuspension(user, notes);
            loto.sendEmailFoApprovedSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult completeSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.suspensionCompletion(user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult sendCompleteSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.suspensionCompletionSend(user);
            loto.sendEmailAgreeCompletionSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult agreeCompleteSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.agreeCompleteSuspension(user);
            loto.sendEmailHolderAgreedCompletionSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult sendInspectCompleteSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.saveAppliedFOCompleteSuspension(user);
            loto.sendEmailFoAppliedCompletionSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        public JsonResult inspectCompleteSuspension(int id, string notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.saveApprovedInspectedCompleteSuspension(user);
            loto.sendEmailCompleteInspectedCompletionSuspension(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        //[HttpPost]
        //public JsonResult requestLotoChange(int id)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    LotoEntity loto = new LotoEntity(id, user);
        //    int retVal = 1;

        //    retVal &= loto.requestChange();
        //    loto.sendEmailComingHolderRequestChange(fullUrl(), user);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult saveChangeApprove(int id)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    LotoEntity loto = new LotoEntity(id, user);
        //    int retVal = 1;
        //    retVal &= loto.edit();
        //    retVal &= loto.sendToApproveOtherHolder();
        //    loto.sendEmailOnComingHolderApprove(fullUrl(), user);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult holderApproveChange(int id)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    LotoEntity loto = new LotoEntity(id, user);
        //    int retVal = 1;
        //    retVal &= loto.otherHolderApprove(user);
        //    loto.isAllOtherHolderApprove();
        //    loto.sendEmailOtherHolderApproveChange(fullUrl(), user);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        //[HttpPost]
        //public JsonResult SaveAndInspectChange(LotoEntity loto)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    int retVal = loto.sendToInspectChange();
        //    loto.sendEmailInspectionChange(fullUrl(), user);
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}
        //[HttpPost]
        //public JsonResult SaveApproveChange(int id, int who, string approval_notes)
        //{
        //    UserEntity user = Session["user"] as UserEntity;
        //    LotoEntity loto = new LotoEntity(id, user);
        //    int retVal = 1;
        //    loto.approval_notes = approval_notes;
        //    if (who == 1)
        //    {
        //        retVal &= loto.approvalSupervisorChange(user);
        //        loto.sendEmailFOChangeApproval(fullUrl(), user);
        //    }
        //    else if (who == 2)
        //    {
        //        retVal &= loto.approvalFacilityOwnerChange(user);
        //        loto.sendEmailOncomingHolderChangeApproval(fullUrl(), user);
        //    }
        //    else if (who == 3)
        //    {
        //        retVal &= loto.approvalOncomingHolder(user);
        //    }
        //    return Json(new { status = retVal > 0 ? "200" : "404" });
        //}

        [HttpPost]
        public JsonResult cancelLoto(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            int loto_glarf = 0;
            retVal = loto.lotoCancel(user, out loto_glarf);
            return Json(new { status = retVal == 2 ? "202" : (retVal > 0 ? "200" : "404"), id_glarf = loto_glarf });
        }

        [HttpPost]
        public JsonResult HolderCancelLoto(int id, string cancellation_notes, int id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.holderCancel(user, id_loto);
            loto.sendEmailFOCancellation(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SpvCancelLoto(int id, string cancellation_notes, int id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.spvCancel(user, id_loto);
            loto.sendEmailFOCancellation(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult FOCancelLoto(int id, string cancellation_notes, int id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.FOCancel(user, id_loto);

            return Json(new { status = retVal > 0 ? "200" : "404" });
        }


        public ActionResult SetFacilityOwner(string a, string b, string c)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
            string val = "emailfo";

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

                    LotoEntity loto = new LotoEntity(ptw_id, userLogin);
                    UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);

                    loto.setFacilityOwner(user, Int32.Parse(s[0]));

                    return RedirectToAction("Index", "Home", new { p = "Loto/Edit/" + loto.id });
                    // Session["user"] = user;


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

        # region LOTO point

        [HttpPost]
        public JsonResult BindingLOTOPoint(int id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            List<LotoPointEntity> result = new LotoPointEntity().getList(user, id_loto);
            return Json(result);
        }

        [HttpPost]
        public JsonResult AddLOTOPoint(LotoPointEntity lotoPoint)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.add();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditLOTOPoint(LotoPointEntity lotoPoint, int is_change)
        {
            UserEntity user = Session["user"] as UserEntity;
            if (is_change == 1)
            {
                lotoPoint.editChange();
            }
            else
            {
                lotoPoint.editLotoPoint();
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteLOTOPoint(LotoPointEntity lotoPoint)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.deleteLotoPoint();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveAgreedApplied(LotoPointEntity lotoPoint)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.agreedLotoPoint();
            lotoPoint.appliedLotoPoint();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SaveInspectedVerified(LotoPointEntity lotoPoint, int isChange)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.inspected(user);
            LotoEntity loto = new LotoEntity(lotoPoint.id_loto.Value, user);
            loto.isAllPointInspectedAndVerified(isChange);
            return Json(true);
        }

        [HttpPost]
        public JsonResult RequestRemoval(LotoPointEntity lotoPoint)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.requestRemoval();
            return Json(true);
        }

        [HttpPost]
        public JsonResult RemovePoint(LotoPointEntity lotoPoint)
        {
            UserEntity user = Session["user"] as UserEntity;
            lotoPoint.removeLotoPoint();
            return Json(true);
        }

        //[HttpPost]
        //public JsonResult EditProjectTeam(ProjectTeamModel team)
        //{
        //    UserModel user = Session["user"] as UserModel;
        //    string token = Session["tokenLogin"].ToString();
        //    team.edit();
        //    return Json(true);
        //}

        //[HttpPost]
        //public JsonResult DeleteProjectTeam(ProjectTeamModel team)
        //{
        //    UserModel user = Session["user"] as UserModel;
        //    string token = Session["tokenLogin"].ToString();
        //    team.delete();
        //    return Json(true);
        //}

        #endregion


        #region glarf

        [HttpPost]
        public JsonResult BindingGlarfUser(int id_loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            List<LotoGlarfUserEntity> result = new LotoGlarfUserEntity().listUserGlarf(id_loto, user);
            return Json(result);
        }

        [HttpPost]
        public JsonResult SaveGlarfUser(int id, string user, int glarf_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            if (id == 0)
            {
                LotoGlarfUserEntity glarf = new LotoGlarfUserEntity();
                glarf.user = user;
                glarf.id_glarf = glarf_id;
                glarf.can_edit = 1;
                glarf.create();
            }
            else
            {
                LotoGlarfUserEntity glarf = new LotoGlarfUserEntity(id, userLogin);
                glarf.user = user;
                glarf.can_edit = 1;
                glarf.edit();
            }
            return Json(true);
        }

        [HttpPost]
        public JsonResult AddGlarfUser(LotoGlarfUserEntity glarf)
        {
            UserEntity user = Session["user"] as UserEntity;
            glarf.create();
            return Json(true);
        }

        [HttpPost]
        public JsonResult EditGlarfUser(LotoGlarfUserEntity glarf)
        {
            UserEntity user = Session["user"] as UserEntity;
            glarf.edit();
            return Json(true);
        }

        [HttpPost]
        public JsonResult DeleteGlarf(LotoGlarfUserEntity glarf)
        {
            UserEntity user = Session["user"] as UserEntity;
            glarf.delete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SendToSigning(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);
            glarf.sendToSign(user);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SignRequestorGlarf(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);
            glarf.signRequestor(user);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SignSupervisorGlarf(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);
            glarf.signSupervisor(user);
            return Json(true);
        }

        [HttpPost]
        public JsonResult SignApplication(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfUserEntity glarfUser = new LotoGlarfUserEntity(id, user);
            glarfUser.applicationSignature(user);
            LotoGlarfEntity glarf = new LotoGlarfEntity(glarfUser.id_glarf.Value, user);
            glarf.signComplete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult SignCancellation(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfUserEntity glarfUser = new LotoGlarfUserEntity(id, user);
            glarfUser.cancellationSignature(user);
            LotoGlarfEntity glarf = new LotoGlarfEntity(glarfUser.id_glarf.Value, user);
            glarf.cancellationSignComplete();
            return Json(true);
        }

        [HttpPost]
        public JsonResult CancelLotoGlarf(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);
            glarf.setCancel();
            return Json(true);
        }

        public JsonResult CancellationGlarf(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoGlarfEntity glarf = new LotoGlarfEntity(id, user);
            int retVal = glarf.saveCancellation(user);
            return Json(new { status = retVal == -1 ? "403" : (retVal == 0 ? "404" : "200") });
        }

        public ActionResult saveAttachment(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            var dPath = "\\Upload\\Loto\\Glarf\\" + id;
            var pPath = "~/Upload/Loto/Glarf/" + id;

            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var dummyPath = Path.Combine(dPath, fileName);
                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // save file
                file.SaveAs(physicalPath);
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult removeAttachment(string[] fileNames, int id)
        {
            var pPath = "~/Upload/Loto/Glarf/" + id;
            // The Name of the Upload component is "attachments" 
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    //remove file
                    System.IO.File.Delete(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult saveAttachmentLoto(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            var dPath = "\\Upload\\Loto\\Attachment\\" + id + "";
            var pPath = "~/Upload/Loto/Attachment/" + id + "";

            foreach (var file in files)
            {
                // Some browsers send file names with full path. This needs to be stripped.
                var fileName = Path.GetFileName(file.FileName);
                var dummyPath = Path.Combine(dPath, fileName);
                //var physicalPath = Path.Combine(Server.MapPath("~/App_Data"), fileName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // save file
                file.SaveAs(physicalPath);
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult removeAttachmentLoto(string[] fileNames, int id)
        {
            var pPath = "~/Upload/Loto/Attachment/" + id + "";
            // The Name of the Upload component is "attachments" 
            // The parameter of the Remove action must be called "fileNames"
            foreach (var fullName in fileNames)
            {
                var fileName = Path.GetFileName(fullName);
                var physicalPath = Path.Combine(Server.MapPath(pPath), fileName);

                // TODO: Verify user permissions
                if (System.IO.File.Exists(physicalPath))
                {
                    //remove file
                    System.IO.File.Delete(physicalPath);
                }
            }
            // Return an empty string to signify success
            return Content("");
        }

        #endregion

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;

            return fullUrl;
        }

        [HttpPost]
        public JsonResult BindingListLoto()
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            List<LotoEntity> listLoto = new LotoEntity().listLoto(userLogin);
            return Json(listLoto);
        }

        [HttpPost]
        public JsonResult BindingPointRemove()
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            List<LotoEntity> listLoto = new LotoEntity().listLotoRemove(userLogin);
            return Json(listLoto);
        }
    }
}
