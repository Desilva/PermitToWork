using PermitToWork.Models;
using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Master;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using PermitToWork.WWUserService;
using System;
using System.Collections.Generic;
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
            LotoGlarfEntity entity = new LotoGlarfEntity(id, user);
            ViewBag.isCanCancel = entity.isCanCancel(user);
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

            bool[] isCanEdit = new bool[12];

            isCanEdit[0] = glarf.isCanEditForm(user);
            isCanEdit[1] = glarf.isCanSign(user);
            isCanEdit[2] = glarf.requestorCanSign(user);
            isCanEdit[3] = glarf.supervisorCanSign(user);
            isCanEdit[4] = glarf.isCanSignCancel(user);

            ViewBag.isCanEdit = isCanEdit;

            return PartialView(glarf);
        }

        public ActionResult Edit(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity entity = new LotoEntity(id, user);

            bool[] isCanEdit = new bool[16];

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


            ViewBag.isCanEdit = isCanEdit;

            ViewBag.listLotoPoint = new star_energy_ptwEntities().mst_loto_point.ToList();
            ViewBag.listUser = new ListUser(user.token, user.id).listUser;

            return PartialView("Create2", entity);
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
            if (employee_id != "")
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
            loto.sendEmailFOAgreed(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveAndInspect(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
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
        public JsonResult requestLotoChange(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;

            retVal &= loto.requestChange();
            loto.sendEmailComingHolderRequestChange(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult saveChangeApprove(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.edit();
            retVal &= loto.sendToApproveOtherHolder();
            loto.sendEmailOnComingHolderApprove(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult holderApproveChange(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            retVal &= loto.otherHolderApprove(user);
            loto.isAllOtherHolderApprove();
            loto.sendEmailOtherHolderApproveChange(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SaveAndInspectChange(LotoEntity loto)
        {
            UserEntity user = Session["user"] as UserEntity;
            int retVal = loto.sendToInspectChange();
            loto.sendEmailInspectionChange(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }
        [HttpPost]
        public JsonResult SaveApproveChange(int id, int who, string approval_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.approval_notes = approval_notes;
            if (who == 1)
            {
                retVal &= loto.approvalSupervisorChange(user);
                loto.sendEmailFOChangeApproval(fullUrl(), user);
            }
            else if (who == 2)
            {
                retVal &= loto.approvalFacilityOwnerChange(user);
                loto.sendEmailOncomingHolderChangeApproval(fullUrl(), user);
            }
            else if (who == 3)
            {
                retVal &= loto.approvalOncomingHolder(user);
            }
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult HolderCancelLoto(int id, string cancellation_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.holderCancel(user);
            loto.sendEmailLotoCancelByOncomingHolder(fullUrl(), user);
            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult SpvCancelLoto(int id, string cancellation_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.spvCancel(user);

            return Json(new { status = retVal > 0 ? "200" : "404" });
        }

        [HttpPost]
        public JsonResult FOCancelLoto(int id, string cancellation_notes)
        {
            UserEntity user = Session["user"] as UserEntity;
            LotoEntity loto = new LotoEntity(id, user);
            int retVal = 1;
            loto.cancellation_notes = cancellation_notes;
            retVal &= loto.FOCancel(user);

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
            glarf.sendToSign();
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

        #endregion

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;

            return fullUrl;
        }
    }
}
