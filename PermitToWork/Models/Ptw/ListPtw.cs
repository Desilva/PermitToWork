using PermitToWork.Models.ClearancePermit;
using PermitToWork.Models.Hw;
using PermitToWork.Models.Radiography;
using PermitToWork.Models.User;
using PermitToWork.Models.WorkingHeight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Ptw
{
    public class ListPtw
    {
        public List<PtwEntity> listPtw { get; set; }
        private star_energy_ptwEntities db;

        public ListPtw(UserEntity user)
        {
            this.db = new star_energy_ptwEntities();
            var result = db.permit_to_work.OrderByDescending(p => p.ptw_no).Select(p => p.id).ToList();
            this.listPtw = new List<PtwEntity>();
            foreach (int i in result)
            {
                this.listPtw.Add(new PtwEntity(i, user, db));
            }
        }

        public List<PtwEntity> listPtwByUser(UserEntity user)
        {
            List<PtwEntity> listPtwUser = new List<PtwEntity>();
            ListUser listUser = new ListUser(user.token, user.id);
            List<UserEntity> listHWFO = listUser.GetHotWorkFO();
            bool state = false;
            foreach (PtwEntity ptw in this.listPtw)
            {
                state = false;
                if (ptw.isUserInPtw(user))
                {
                    listPtwUser.Add(ptw);
                }
                else
                {
                    if (ptw.hw_id != null && ((HwEntity)ptw.cPermit[PtwEntity.clearancePermit.HOTWORK.ToString()]).isUserInHw(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.fi_id != null && ((FIEntity)ptw.cPermit[PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString()]).isUserInFI(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.rad_id != null && ((RadEntity)ptw.cPermit[PtwEntity.clearancePermit.RADIOGRAPHY.ToString()]).isUserInRad(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.wh_id != null && ((WorkingHeightEntity)ptw.cPermit[PtwEntity.clearancePermit.WORKINGHEIGHT.ToString()]).isUserInWH(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.ex_id != null && ((ExcavationEntity)ptw.cPermit[PtwEntity.clearancePermit.EXCAVATION.ToString()]).userInEx(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.csep_id != null && ((CsepEntity)ptw.cPermit[PtwEntity.clearancePermit.CONFINEDSPACE.ToString()]).isUserInCSEP(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }

                    if (!state && ptw.loto_id != null && new LotoGlarfEntity(ptw.loto_id.Value, user).isUserInLOTO(user))
                    {
                        listPtwUser.Add(ptw);
                        state = true;
                    }
                    else if (!state && ptw.loto_id != null)
                    {
                        if (listHWFO.Exists(p => p.id == user.id))
                        {
                            listPtwUser.Add(ptw);
                            state = true;
                        }
                    }

                    //if (ptw.fi_id != null && ((FIEntity)ptw.cPermit[PtwEntity.clearancePermit.FIREIMPAIRMENT.ToString()]).isUserInFI(user))
                    //{
                    //    listPtwUser.Add(ptw);
                    //}
                }
            }

            return listPtwUser;
        }

        public PtwEntity getLastPtw() {
            return this.listPtw.FirstOrDefault();
        }
    }
}