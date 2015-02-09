using PermitToWork.Models.Hw;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using ReportManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;

namespace PermitToWork.Controllers
{
    [AuthorizeUser]
    public class HwController : PdfViewController
    {
        //
        // GET: /Hw/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult Edit(int id)
        {
            HwEntity entity = new HwEntity(id);
            UserEntity user = Session["user"] as UserEntity;
            entity.GetPtw(user);
            entity.getHiraNo();
            ViewBag.isWorkLeader = entity.isWorkLeader(user);
            if (entity.status < (int)HwEntity.statusHW.CANCEL)
            {
                ViewBag.isAccSupervisor = entity.isAccSupervisor(user);
                ViewBag.isAccFireWatch = entity.isAccFireWatch(user);
                ViewBag.isAccFO = entity.isAccFO(user);
                ViewBag.isGasTester = entity.isAccGasTester(user);
            }
            else
            {
                ViewBag.isCancel = true;
                ViewBag.isCanSupervisor = entity.isAccSupervisor(user);
                ViewBag.isCanFireWatch = entity.isAccFireWatch(user);
                ViewBag.isCanFO = entity.isAccFO(user);
            }

            if (entity.status >= (int)HwEntity.statusHW.ACCFO && entity.status <= (int)HwEntity.statusHW.EXTACCFO7)
            {
                ViewBag.ptwStatus = new PtwEntity(entity.id_ptw.Value, user).status;
                ViewBag.isCanAddExt = true;
                ViewBag.isGasTesterExt1 = entity.isExtGasTester(user, 1);
                ViewBag.isFOExt1 = entity.isExtFO(user, 1);
                ViewBag.isGasTesterExt2 = entity.isExtGasTester(user, 2);
                ViewBag.isFOExt2 = entity.isExtFO(user, 2);
                ViewBag.isGasTesterExt3 = entity.isExtGasTester(user, 3);
                ViewBag.isFOExt3 = entity.isExtFO(user, 3);
                ViewBag.isGasTesterExt4 = entity.isExtGasTester(user, 4);
                ViewBag.isFOExt4 = entity.isExtFO(user, 4);
                ViewBag.isGasTesterExt5 = entity.isExtGasTester(user, 5);
                ViewBag.isFOExt5 = entity.isExtFO(user, 5);
                ViewBag.isGasTesterExt6 = entity.isExtGasTester(user, 6);
                ViewBag.isFOExt6 = entity.isExtFO(user, 6);
                ViewBag.isGasTesterExt7 = entity.isExtGasTester(user, 7);
                ViewBag.isFOExt7 = entity.isExtFO(user, 7);
            }
            ViewBag.isCanEdit = entity.isCanEdit(user);

            ViewBag.isCanEditExt1 = entity.isCanEditExt(user, 1);
            ViewBag.isCanEditExt2 = entity.isCanEditExt(user, 2);
            ViewBag.isCanEditExt3 = entity.isCanEditExt(user, 3);
            ViewBag.isCanEditExt4 = entity.isCanEditExt(user, 4);
            ViewBag.isCanEditExt5 = entity.isCanEditExt(user, 5);
            ViewBag.isCanEditExt6 = entity.isCanEditExt(user, 6);
            ViewBag.isCanEditExt7 = entity.isCanEditExt(user, 7);

            ViewBag.position = "Edit";
            ViewBag.listUser = new ListUser(user.token, user.id);
            ViewBag.listGasTester = (ViewBag.listUser as ListUser).GetHotWorkGasTester();
            return PartialView("create", entity);
        }

        [HttpPost]
        public JsonResult editHw(HwEntity hw)
        {
            UserEntity user = Session["user"] as UserEntity;
            int ret = hw.edit();
            HwEntity hw_new = new HwEntity(hw.id);

            if (hw_new.status == (int)HwEntity.statusHW.CREATE && (hw_new.isWorkLeader(user) || (hw_new.is_guest && hw_new.isAccSupervisor(user))))
            {
                // change status to SPVSCREENING
                hw_new.sendEmailRandomPIN(fullUrl(),user.token,user);
                hw_new.sendEmailSupervisorScreening(fullUrl(), user.token, user);
                // send email to facility owner (5)
            }

            if (hw_new.status == (int)HwEntity.statusHW.CREATE && hw_new.isAccSupervisor(user))
            {
                // change status to SPVSCREENING
                hw_new.setStatus((int)HwEntity.statusHW.SPVSCREENING);
                hw_new.sendEmailFOScreening(fullUrl(), user.token, user);
                // sendEmailFO(hw_new);
                // send email to facility owner (5)
            }

            if (hw_new.status == (int)HwEntity.statusHW.SPVSCREENING && hw_new.isAccFO(user))
            {
                // change status to FOSCREENING

                if (hw_new.acc_gas_tester == null)
                {
                    if (hw.acc_gas_tester != null)
                    {
                        UserEntity gasTester = new UserEntity(Int32.Parse(hw.acc_gas_tester),user.token,user);
                        hw_new.assignGasTester(gasTester);
                        hw_new.setStatus((int)HwEntity.statusHW.FOSCREENING);
                        hw_new.sendEmailGasTester(fullUrl(),user.token,user,0);
                    }
                    else
                    {
                        return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                    }
                }
                else
                {
                    if (hw.acc_gas_tester != null)
                    {
                        UserEntity gasTester = new UserEntity(Int32.Parse(hw.acc_gas_tester),user.token,user);
                        hw_new.assignGasTester(gasTester);
                    }
                }
                // check if gas tester already assign
                // if no, return error code 404
                // else
                // send email to gas tester
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

        [HttpPost]
        public JsonResult closeHw(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.closeHw(user);
            hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal, message = "There is error when saving data to database. Please check again your data." });
        }

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;
            return fullUrl;
        }

        private string sendEmailFO(HwEntity hw)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            List<UserEntity> listHWFO = listUser.GetHotWorkFO();

            hw.sendEmailFO(listHWFO, fullUrl(), userLogin.token, userLogin);

            return "200";
        }

        #region set facility owner

        // url to set who is the supervisor
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

                    HwEntity hw = new HwEntity(ptw_id);
                    UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);

                    if (!hw.isExistFO(user, Int32.Parse(s[0])))
                    {
                        hw.assignFO(user, Int32.Parse(s[0]));
                        PtwEntity ptw = new PtwEntity(hw.id_ptw.Value, user);
                        if (ptw.acc_fo == null)
                        {
                            ptw.assignFO(user);
                        }

                        return RedirectToAction("Index", "Home", new { p = "Hw/Edit/" + hw.id });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home", new { e = "402" });
                    }
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

        #endregion

        #region approve reject

        [HttpPost]
        public JsonResult gasTesterAcc(int user_id, int id, int extension)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.gasTesterAcc(user, extension, userLogin);
            hw.sendEmailRequestor(fullUrl(), userLogin.token, user, extension);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorAcc(int user_id, int id, int extension, string random_pin)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.requestorAcc(user, userLogin.token, extension, random_pin);
            if (extension == 0 && retVal == "200")
                hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin);
            else if (extension != 0)
                hw.sendEmailFOExt(fullUrl(), userLogin.token, userLogin, extension);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAcc(int id, int user_id, int? fire_watch_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            //if (fire_watch_id != null)
            //{
            //    UserEntity assesor = new UserEntity(fire_watch_id.Value);
            //    hw.assignFireWatch(assesor);
            //    hw.sendEmailFOAcc(fullUrl());
            //}
            string retVal = hw.supervisorAcc(userLogin);
            hw.sendEmailFOAcc(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.supervisorAccReject(user, comment);
            hw.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 0, 1, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchAcc(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fireWatchAccApproval(user);
            hw.sendEmailFOAcc(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fireWatchAccReject(user, comment);
            hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAcc(int id, int user_id, int extension)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fOAccApproval(user,extension);
            PtwEntity ptw = new PtwEntity(hw.id_ptw.Value, user);
            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.HOTWORK.ToString());
            ptw.sendEmailRequestorClearance(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.clearancePermit.HOTWORK, (int)PtwEntity.statusClearance.COMPLETE);
            if (ptw.isAllClearanceComplete())
            {
                ptw.sendEmailRequestorClearanceCompleted(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.statusClearance.COMPLETE);
            }
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAccReject(int id, int user_id, string comment, int extension)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fOAccReject(user, extension, comment);
            if (extension == 0)
                hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 0, comment);
            else
                hw.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, extension, 1, 0, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorCan(int user_id, int id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.requestorCan(user);
            hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 0, 1);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.supervisorCan(user);
            PtwEntity ptw = new PtwEntity(hw.id_ptw.Value, user);
            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.REQUESTORCANCELLED, PtwEntity.clearancePermit.HOTWORK.ToString());
            ptw.sendEmailRequestorClearance(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.clearancePermit.HOTWORK, (int)PtwEntity.statusClearance.REQUESTORCANCELLED);
            if (ptw.isAllClearanceClose())
            {
                ptw.sendEmailRequestorClearanceCompleted(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.statusClearance.REQUESTORCANCELLED);
            }
            hw.sendEmailFOCan(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.supervisorCanReject(user, comment);
            hw.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 0, 1, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fireWatchCanApproval(user);
            if (hw.can_fo == null)
            {
                var a = "";
                // sendEmailFO(hw);
            }
            else
                hw.sendEmailFOCan(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fireWatchCanReject(user, comment);
            hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fOCanApproval(user);
            PtwEntity ptw = new PtwEntity(hw.id_ptw.Value, user);
            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.CLOSE, PtwEntity.clearancePermit.HOTWORK.ToString());
            ptw.sendEmailRequestorClearance(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.clearancePermit.HOTWORK, (int)PtwEntity.statusClearance.CLOSE);
            if (ptw.isAllClearanceClose())
            {
                ptw.sendEmailRequestorClearanceCompleted(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.statusClearance.CLOSE);
            }
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            HwEntity hw = new HwEntity(id);
            string retVal = hw.fOCanReject(user, comment);
            hw.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, 1, comment);
            return Json(new { status = retVal });
        }

        #endregion

        #region extension

        [HttpPost]
        public JsonResult extendHw(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            HwEntity hw = new HwEntity(id);
            hw.setStatus(hw.status.Value + 1);
            hw.assignExtWorkLeader(hw.status.Value);
            UserEntity fo = new UserEntity(Int32.Parse(hw.acc_fo), userLogin.token, userLogin);
            hw.assignFO(fo, hw.status.Value);
            return Json(new { status = "200" });
        }

        [HttpPost]
        public JsonResult editExtHw(HwEntity hw, int extension)
        {
            UserEntity user = Session["user"] as UserEntity;
            int ret = hw.editExtHotWork(extension);
            HwEntity hw_new = new HwEntity(hw.id);

            if ((hw_new.status == (int)HwEntity.statusHW.EXTCREATE1 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE2 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE3 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE4 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE5 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE6 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE7) && (hw_new.isWorkLeader(user) || (hw_new.is_guest && hw_new.isAccSupervisor(user))))
            {
                // sendEmailFO(hw_new);
                // send email to facility owner (5)
            }

            if ((hw_new.status == (int)HwEntity.statusHW.EXTCREATE1 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE2 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE3 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE4 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE5 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE6 ||
                hw_new.status == (int)HwEntity.statusHW.EXTCREATE7) && hw_new.isExtFO(user,extension))
            {
                // change status to FOSCREENING

                switch (extension)
                {
                    case 1:
                        if (hw_new.ext_gas_tester_1 == null)
                        {
                            if (hw.ext_gas_tester_1 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING1);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_1), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_1 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_1), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 2:
                        if (hw_new.ext_gas_tester_2 == null)
                        {
                            if (hw.ext_gas_tester_2 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING2);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_2), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_2 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_2), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 3:
                        if (hw_new.ext_gas_tester_3 == null)
                        {
                            if (hw.ext_gas_tester_3 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING3);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_3), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_3 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_3), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 4:
                        if (hw_new.ext_gas_tester_4 == null)
                        {
                            if (hw.ext_gas_tester_4 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING4);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_4), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_4 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_4), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 5:
                        if (hw_new.ext_gas_tester_5 == null)
                        {
                            if (hw.ext_gas_tester_5 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING5);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_5), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_5 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_5), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 6:
                        if (hw_new.ext_gas_tester_6 == null)
                        {
                            if (hw.ext_gas_tester_6 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING6);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_6), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_6 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_6), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                    case 7:
                        if (hw_new.ext_gas_tester_7 == null)
                        {
                            if (hw.ext_gas_tester_7 != null)
                            {
                                hw_new.setStatus((int)HwEntity.statusHW.EXTFOSCREENING7);
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_7), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);

                                hw_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (hw.ext_gas_tester_7 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(hw.ext_gas_tester_7), user.token, user);
                                hw_new.assignExtGasTester(gasTester, hw_new.status.Value);
                            }
                        }
                        break;
                };
                
                // check if gas tester already assign
                // if no, return error code 404
                // else
                // send email to gas tester
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

        #endregion

        #region document attachment

        public ActionResult saveAttachment(IEnumerable<HttpPostedFileBase> files, int? id)
        {
            var dPath = "\\Upload\\HotWork\\" + id + "";
            var pPath = "~/Upload/HotWork/" + id + "";

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
            var pPath = "~/Upload/HotWork/" + id + "";
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

        public ActionResult Print(int id)
        {
            HwEntity hw = new HwEntity(id);
            UserEntity user = Session["user"] as UserEntity;
            List<UserEntity> listUser = new ListUser(user.token, user.id).listUser.ToList();
            ViewBag.listUser = listUser;
            int a = Int32.Parse(hw.fire_watch);
            hw.fire_watch = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(hw.work_leader);
            hw.work_leader = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(hw.acc_supervisor);
            hw.acc_supervisor = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(hw.acc_supervisor_delegate != "" && hw.acc_supervisor_delegate != null ? hw.acc_supervisor_delegate : "0");
            hw.acc_supervisor_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.acc_fo);
            hw.acc_fo = listUser.Find(p => p.id == a).alpha_name;
            a = Int32.Parse(hw.acc_fo_delegate != "" && hw.acc_fo_delegate != null ? hw.acc_fo_delegate : "0");
            hw.acc_fo_delegate = a != 0 ? listUser.Find(p => p.id == a).alpha_name : "";
            a = Int32.Parse(hw.acc_gas_tester);
            hw.acc_gas_tester = listUser.Find(p => p.id == a).alpha_name;
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

            return this.ViewPdf("", "Print", hw);
        }
    }
}
