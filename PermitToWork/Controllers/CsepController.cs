using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Controllers
{
    public class CsepController : Controller
    {
        //
        // GET: /Csep/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        public ActionResult GasTestCreate()
        {
            return PartialView();
        }

        public ActionResult Edit(int id)
        {
            UserEntity user = Session["user"] as UserEntity;
            CsepEntity entity = new CsepEntity(id, user);
            ViewBag.isWorkLeader = entity.isWorkLeader(user);
            if (entity.status < (int)CsepEntity.CsepStatus.CANCEL)
            {
                ViewBag.isAccSupervisor = entity.isAccSupervisor(user);
                ViewBag.isAccFireWatch = entity.isAccFireWatch(user);
                ViewBag.isAccFO = entity.isAccFO(user);
                ViewBag.isGasTester = entity.isAccGasTester(user);
            }
            else
            {
                ViewBag.isCancel = true;
                ViewBag.isCanSupervisor = entity.isCanSupervisor(user);
                ViewBag.isCanFireWatch = entity.isCanFireWatch(user);
                ViewBag.isCanFO = entity.isCanFO(user);
            }

            if (entity.status >= (int)CsepEntity.CsepStatus.ACCFO && entity.status <= (int)CsepEntity.CsepStatus.EXTACCFO7)
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
        public JsonResult editCsep(CsepEntity csep)
        {
            UserEntity user = Session["user"] as UserEntity;
            int ret = csep.edit();
            CsepEntity csep_new = new CsepEntity(csep.id, user);

            if (csep_new.status == (int)CsepEntity.CsepStatus.CREATE && csep_new.isWorkLeader(user))
            {
                // change status to SPVSCREENING
                csep_new.sendEmailRandomPIN(fullUrl(), user.token, user);

                // send email to facility owner (5)
            }

            if (csep_new.status == (int)CsepEntity.CsepStatus.CREATE && csep_new.isAccSupervisor(user))
            {
                // change status to SPVSCREENING
                csep_new.setStatus((int)CsepEntity.CsepStatus.SPVSCREENING);
                this.sendEmailFO(csep_new);
                // send email to facility owner (5)
            }

            if (csep_new.status == (int)CsepEntity.CsepStatus.SPVSCREENING && csep_new.isAccFO(user))
            {
                // change status to FOSCREENING

                if (csep_new.acc_gas_tester == null)
                {
                    if (csep.acc_gas_tester != null)
                    {
                        UserEntity gasTester = new UserEntity(Int32.Parse(csep.acc_gas_tester), user.token, user);
                        csep_new.assignGasTester(gasTester);
                        csep_new.setStatus((int)CsepEntity.CsepStatus.FOSCREENING);
                        csep_new.sendEmailGasTester(fullUrl(), user.token, user, 0);
                    }
                    else
                    {
                        return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                    }
                }
                else
                {
                    if (csep.acc_gas_tester != null)
                    {
                        UserEntity gasTester = new UserEntity(Int32.Parse(csep.acc_gas_tester), user.token, user);
                        csep_new.assignGasTester(gasTester);
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
        public JsonResult closeCsep(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, user);
            string retVal = csep.closeCsep(user);

            return Json(new { status = retVal, message = "There is error when saving data to database. Please check again your data." });
        }

        private string fullUrl()
        {
            string url = Request.Url.Authority;
            string applicationPath = Request.ApplicationPath;

            string fullUrl = "http://" + url + applicationPath;
            return fullUrl;
        }

        private string sendEmailFO(CsepEntity csep)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            ListUser listUser = new ListUser(userLogin.token, userLogin.id);

            List<UserEntity> listHWFO = listUser.GetHotWorkFO();

            csep.sendEmailFO(listHWFO, fullUrl(), userLogin.token, userLogin);

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

                    CsepEntity csep = new CsepEntity(ptw_id, userLogin);
                    UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);

                    if (!csep.isExistFO(user, Int32.Parse(s[0])))
                    {
                        csep.assignFO(user, Int32.Parse(s[0]));
                        PtwEntity ptw = new PtwEntity(csep.id_ptw.Value, user);
                        if (ptw.acc_fo == null)
                        {
                            ptw.assignFO(user);
                        }

                        return RedirectToAction("Index", "Home", new { p = Url.Action("Edit","Csep", new { id = csep.id}) });
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
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.gasTesterAcc(user, extension);
            csep.sendEmailRequestor(fullUrl(), userLogin.token, user, extension);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorAcc(int user_id, int id, int extension, string random_pin)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.requestorAcc(user, userLogin.token, extension, random_pin);
            if (extension == 0 && retVal == "200")
                csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin);
            else if (extension != 0)
                csep.sendEmailFOExt(fullUrl(), userLogin.token, userLogin, extension);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAcc(int id, int user_id, int? fire_watch_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            //if (fire_watch_id != null)
            //{
            //    UserEntity assesor = new UserEntity(fire_watch_id.Value);
            //    csep.assignFireWatch(assesor);
            //    csep.sendEmailFOAcc(fullUrl());
            //}
            string retVal = csep.supervisorAcc(user);
            csep.sendEmailFOAcc(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.supervisorAccReject(user, comment);
            csep.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 0, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchAcc(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fireWatchAccApproval(user);
            csep.sendEmailFOAcc(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchAccReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fireWatchAccReject(user, comment);
            csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOAcc(int id, int user_id, int extension)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fOAccApproval(user, extension);
            PtwEntity ptw = new PtwEntity(csep.id_ptw.Value, user);
            ptw.setClerancePermitStatus((int)PtwEntity.statusClearance.COMPLETE, PtwEntity.clearancePermit.CONFINEDSPACE.ToString());
            ptw.sendEmailRequestorClearance(fullUrl(), userLogin.token, userLogin, (int)PtwEntity.clearancePermit.CONFINEDSPACE, (int)PtwEntity.statusClearance.COMPLETE);
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
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fOAccReject(user, extension, comment);
            if (extension == 0)
                csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, comment);
            else
                csep.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, extension, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult requestorCan(int user_id, int id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.requestorCan(user);
            csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.supervisorCan(user);
            csep.sendEmailFOCan(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult supervisorCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.supervisorCanReject(user, comment);
            csep.sendEmailRequestor(fullUrl(), userLogin.token, userLogin, 0, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fireWatchCanApproval(user);
            if (csep.can_fo == null)
                sendEmailFO(csep);
            else
                csep.sendEmailFOCan(fullUrl(), userLogin.token, userLogin);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fireWatchCanReject(int id, int user_id, string comment)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fireWatchCanReject(user, comment);
            csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, comment);
            return Json(new { status = retVal });
        }

        [HttpPost]
        public JsonResult fOCan(int id, int user_id)
        {
            UserEntity userLogin = Session["user"] as UserEntity;
            UserEntity user = new UserEntity(user_id, userLogin.token, userLogin);
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fOCanApproval(user);
            PtwEntity ptw = new PtwEntity(csep.id_ptw.Value, user);
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
            CsepEntity csep = new CsepEntity(id, userLogin);
            string retVal = csep.fOCanReject(user, comment);
            csep.sendEmailSupervisor(fullUrl(), userLogin.token, userLogin, 1, comment);
            return Json(new { status = retVal });
        }

        #endregion

        #region extension

        [HttpPost]
        public JsonResult extendCsep(int id, int user_id)
        {
            UserEntity user = Session["user"] as UserEntity;
            CsepEntity csep = new CsepEntity(id, user);
            csep.setStatus(csep.status.Value + 1);
            csep.assignExtWorkLeader(csep.status.Value);
            return Json(new { status = "200" });
        }

        [HttpPost]
        public JsonResult editExtCsep(CsepEntity csep, int extension)
        {
            UserEntity user = Session["user"] as UserEntity;
            int ret = csep.editExtHotWork(extension);
            CsepEntity csep_new = new CsepEntity(csep.id, user);

            if ((csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE1 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE2 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE3 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE4 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE5 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE6 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE7) && csep_new.isWorkLeader(user))
            {
                sendEmailFO(csep_new);
                // send email to facility owner (5)
            }

            if ((csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE1 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE2 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE3 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE4 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE5 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE6 ||
                csep_new.status == (int)CsepEntity.CsepStatus.EXTCREATE7) && csep_new.isExtFO(user, extension))
            {
                // change status to FOSCREENING

                switch (extension)
                {
                    case 1:
                        if (csep_new.ext_gas_tester_1 == null)
                        {
                            if (csep.ext_gas_tester_1 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING1);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_1), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_1 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_1), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 2:
                        if (csep_new.ext_gas_tester_2 == null)
                        {
                            if (csep.ext_gas_tester_2 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING2);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_2), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_2 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_2), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 3:
                        if (csep_new.ext_gas_tester_3 == null)
                        {
                            if (csep.ext_gas_tester_3 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING3);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_3), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_3 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_3), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 4:
                        if (csep_new.ext_gas_tester_4 == null)
                        {
                            if (csep.ext_gas_tester_4 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING4);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_4), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_4 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_4), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 5:
                        if (csep_new.ext_gas_tester_5 == null)
                        {
                            if (csep.ext_gas_tester_5 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING5);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_5), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_5 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_5), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 6:
                        if (csep_new.ext_gas_tester_6 == null)
                        {
                            if (csep.ext_gas_tester_6 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING6);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_6), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_6 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_6), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
                            }
                        }
                        break;
                    case 7:
                        if (csep_new.ext_gas_tester_7 == null)
                        {
                            if (csep.ext_gas_tester_7 != null)
                            {
                                csep_new.setStatus((int)CsepEntity.CsepStatus.EXTFOSCREENING7);
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_7), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);

                                csep_new.sendEmailGasTester(fullUrl(), user.token, user, extension);
                            }
                            else
                            {
                                return Json(new { status = "404", message = "You haven't assign any gas tester yet, Please assign one." });
                            }
                        }
                        else
                        {
                            if (csep.ext_gas_tester_7 != null)
                            {
                                UserEntity gasTester = new UserEntity(Int32.Parse(csep.ext_gas_tester_7), user.token, user);
                                csep_new.assignExtGasTester(gasTester, csep_new.status.Value);
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

    }
}
