using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.User;
using PermitToWork.Models.WorkingHeight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PermitToWork.Models.Master;
using System.Diagnostics;

namespace PermitToWork.Models.Ptw
{
    public class ListPtw
    {
        public List<PtwEntity> listPtw { get; set; }
        private ListUser listUser { get; set; }
        private star_energy_ptwEntities db;

        public ListPtw()
        {
            this.db = new star_energy_ptwEntities();
        }

        public ListPtw(UserEntity user)
        {
            this.db = new star_energy_ptwEntities();
            //var result = db.permit_to_work.ToList().OrderByDescending(p => p.ptw_no != null ? p.ptw_no.Split('-').ElementAt(1) : "");
            //this.listUser = new ListUser(user.token, user.id);
            //this.listPtw = new List<PtwEntity>();
            //foreach (var a in result)
            //{
            //    this.listPtw.Add(new PtwEntity(a, user, listUser));
            //}
        }

        public List<PtwEntity> listPtwByUser(UserEntity user, int type, ListUser listUsers)
        {
            List<PtwEntity> listPtwUser = new List<PtwEntity>();
            var result = db.permit_to_work.ToList().OrderByDescending(p => p.ptw_no != null ? p.ptw_no.Split('-').ElementAt(1) : "").ToList();
            this.listUser = listUsers;

            MstFOEntity prodFo = new MstFOEntity("PROD", user);
            List<MstDelegateFOEntity> listMstProdFO = new MstDelegateFOEntity().getListByFO(prodFo, user, listUser);
            bool isProd = false;
            if (user.id == prodFo.id_employee || listMstProdFO.Exists(p => p.user_delegate_id == user.id))
            {
                isProd = true;
            }
            bool state = false;
            bool admin = false;
            if (user.roles.Exists(p => p == (int)PermitToWork.Models.User.UserEntity.role.ADMINMASTERSHE))
            {
                admin = true;
            }
            if (type == 1)
            {
                result = result.Where(p => DateTime.Today.CompareTo(p.validity_period_end != null ? p.validity_period_end : p.proposed_period_end.Value) <= 0 && p.status < (int)PtwEntity.statusPtw.CANFO).ToList();
            }
            else if (type == 2)
            {
                result = result.Where(p => DateTime.Today.CompareTo(p.validity_period_end != null ? p.validity_period_end : p.proposed_period_end.Value) > 0 && p.status < (int)PtwEntity.statusPtw.CANFO).ToList();
            }
            foreach (var a in result)
            {
                Debug.WriteLine("a = " + DateTime.Now.TimeOfDay.TotalMilliseconds);
                PtwEntity ptw = new PtwEntity(a, user, listUser);

                Debug.WriteLine("b = " + DateTime.Now.TimeOfDay.TotalMilliseconds);
                state = false;
                bool find = false;
                if (type == 1 && ptw.proposed_period_end != null && DateTime.Today.CompareTo(ptw.validity_period_end != null ? ptw.validity_period_end : ptw.proposed_period_end.Value) <= 0 && ptw.status < (int)PtwEntity.statusPtw.CANFO)
                {
                    find = true;
                }
                else if (type == 2 && ptw.proposed_period_end != null && DateTime.Today.CompareTo(ptw.validity_period_end != null ? ptw.validity_period_end : ptw.proposed_period_end.Value) > 0 && ptw.status < (int)PtwEntity.statusPtw.CANFO)
                {
                    find = true;
                }
                else if (type == 0 || type == 3)
                {
                    find = true;
                }

                if (ptw.id == 241)
                {
                    state = false;
                }

                if (find) {
                    if ((type != 3 && admin) || ptw.isUserPtw || ptw.isUserInPtw(user, this.listUser))
                    {
                        listPtwUser.Add(ptw);
                    }
                    else if (user.department == ptw.department.department)
                    {
                        listPtwUser.Add(ptw);
                    }
                    else
                    {
                        if (ptw.hw_id != null)
                        {
                            if (isProd || ((HwEntity)ptw.cPermit[PtwEntity.clearancePermit.HOTWORK.ToString()]).isUserInHw(user, listUser))
                            {
                                listPtwUser.Add(ptw);
                                state = true;
                            }
                        }

                        if (!state && ptw.fi_id != null)
                        {
                            if (isProd)
                            {
                                listPtwUser.Add(ptw);
                                state = true;
                            }
                        }

                        if (!state && ptw.loto_need != null && ptw.loto_need != 0)
                        {
                            if (isProd)
                            {
                                listPtwUser.Add(ptw);
                                state = true;
                            }
                        }

                        if (!state && ptw.csep_id != null)
                        {
                            if (isProd || ((CsepEntity)ptw.cPermit[PtwEntity.clearancePermit.CONFINEDSPACE.ToString()]).isUserInCSEP(user, listUser))
                            {
                                listPtwUser.Add(ptw);
                                state = true;
                            }
                        }
                    }
                }
            }

            return listPtwUser;
        }

        public List<PtwEntity> listPtwOwn(UserEntity user)
        {
            List<PtwEntity> listPtwUser = new List<PtwEntity>();
            var result = db.permit_to_work.Where(p => p.status <= (int)PermitToWork.Models.Ptw.PtwEntity.statusPtw.ACCFO).ToList().OrderByDescending(p => p.ptw_no != null ? p.ptw_no.Split('-').ElementAt(1) : "");
            this.listUser = new ListUser(user.token, user.id);

            foreach (var a in result)
            {
                
                PtwEntity ptw = new PtwEntity(a, user, true, listUser);
                if (ptw.isNeedClose == true)
                {
                    listPtwUser.Add(ptw);
                }
            }

            return listPtwUser;
        }

        public List<PtwExcelEntity> listPtwExcel(UserEntity user)
        {
            List<PtwExcelEntity> listPtwUser = new List<PtwExcelEntity>();
            var result = db.permit_to_work.ToList().OrderByDescending(p => p.ptw_no != null ? p.ptw_no.Split('-').ElementAt(1) : "");
            this.listUser = new ListUser(user.token, user.id);

            foreach (var a in result)
            {
                PtwExcelEntity ptw = new PtwExcelEntity(a, user, listUser);
                listPtwUser.Add(ptw);
            }

            return listPtwUser;
        }

        public List<PtwEntity> ListPtwRequestedNo()
        {
            var list = this.db.permit_to_work.Where(p => p.requested_no == 1 && p.is_guest == 1);
            List<PtwEntity> listPtw = new List<PtwEntity>();

            foreach (permit_to_work ptw in list)
            {
                PtwEntity permit = new PtwEntity();
                permit.id = ptw.id;
                permit.ptw_no = ptw.ptw_no;
                permit.work_description = ptw.work_description;
                permit.acc_ptw_requestor = ptw.acc_ptw_requestor;
                permit.acc_supervisor = ptw.acc_supervisor;
                permit.guest_holder_no = ptw.guest_holder_no;
                listPtw.Add(permit);
            }

            return listPtw;
        }

        public string getLastPtw() {
            return db.permit_to_work.ToList().OrderByDescending(p => p.ptw_no != null ? p.ptw_no.Split('-').ElementAt(1) : "").Select(p => p.ptw_no).FirstOrDefault();
        }
    }
}