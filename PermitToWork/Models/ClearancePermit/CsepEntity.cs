using PermitToWork.Models.Hira;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.ClearancePermit
{
    public class CsepEntity : confined_space, IClearancePermitEntity
    {
        public override permit_to_work permit_to_work { get { return null; } set { } }

        public string[] screening_spv_arr { get; set; }
        public string[] screening_fo_arr { get; set; }
        public string[] screening_1_arr { get; set; }
        public string[] screening_2_arr { get; set; }
        public string[] screening_3_arr { get; set; }
        public string[] screening_4_arr { get; set; }
        public string[] screening_5_arr { get; set; }
        public string[] screening_6_arr { get; set; }
        public string[] screening_7_arr { get; set; }

        // ptw entity for PTW reference
        private PtwEntity ptw { get; set; }

        public List<HiraEntity> hira_document { get; set; }
        public string hira_no { get; set; }

        public string csep_status { get; set; }

        public int ids { get; set; }
        public string statusText { get; set; }


        public Dictionary<string, UserEntity> userInCSEP { get; set; }

        public enum CsepStatus
        {
            CREATE,
            SPVSCREENING,
            FOSCREENING,
            GASTESTER,
            ACCWORKLEADER,
            ACCSPV,
            ACCFO,
            EXTCREATE1,
            EXTFOSCREENING1,
            EXTGASTESTER1,
            EXTACCWORKLEADER1,
            EXTACCFO1,
            EXTCREATE2,
            EXTFOSCREENING2,
            EXTGASTESTER2,
            EXTACCWORKLEADER2,
            EXTACCFO2,
            EXTCREATE3,
            EXTFOSCREENING3,
            EXTGASTESTER3,
            EXTACCWORKLEADER3,
            EXTACCFO3,
            EXTCREATE4,
            EXTFOSCREENING4,
            EXTGASTESTER4,
            EXTACCWORKLEADER4,
            EXTACCFO4,
            EXTCREATE5,
            EXTFOSCREENING5,
            EXTGASTESTER5,
            EXTACCWORKLEADER5,
            EXTACCFO5,
            EXTCREATE6,
            EXTFOSCREENING6,
            EXTGASTESTER6,
            EXTACCWORKLEADER6,
            EXTACCFO6,
            EXTCREATE7,
            EXTFOSCREENING7,
            EXTGASTESTER7,
            EXTACCWORKLEADER7,
            EXTACCFO7,
            CANCEL,
            CANWORKLEADER,
            CANSPV,
            CANFO,
        }

        private star_energy_ptwEntities db;

        public CsepEntity() : base()
        {
            this.db = new star_energy_ptwEntities();
        }

        // constructor with id to get object from database
        public CsepEntity(int id, UserEntity user)
            : this()
        {
            confined_space csep = this.db.confined_space.Find(id);
            // this.ptw = new PtwEntity(fi.id_ptw.Value);
            ModelUtilization.Clone(csep, this);

            this.screening_spv_arr = this.screening_spv.Split('#');
            this.screening_fo_arr = this.screening_fo.Split('#');
            this.screening_1_arr = this.screening_1.Split('#');
            this.screening_2_arr = this.screening_2.Split('#');
            this.screening_3_arr = this.screening_3.Split('#');
            this.screening_4_arr = this.screening_4.Split('#');
            this.screening_5_arr = this.screening_5.Split('#');
            this.screening_6_arr = this.screening_6.Split('#');
            this.screening_7_arr = this.screening_7.Split('#');

            this.hira_document = new ListHira(this.id_ptw.Value, this.db).listHira;
        }

        public CsepEntity(int id_ptw, string work_leader, string purpose, string acc_spv = null, string acc_spv_del = null) : this()
        {
            this.id_ptw = id_ptw;
            this.work_leader = work_leader;
            this.purpose = purpose;

            this.screening_spv = "########";
            this.screening_fo = "########";
            this.screening_1 = "########";
            this.screening_2 = "########";
            this.screening_3 = "########";
            this.screening_4 = "########";
            this.screening_5 = "########";
            this.screening_6 = "########";
            this.screening_7 = "########";
        }

        public int create()
        {
            generateRandomPIN();
            confined_space csep = new confined_space();
            this.status = (int)CsepStatus.CREATE;
            ModelUtilization.Clone(this, csep);
            this.db.confined_space.Add(csep);
            int retVal = this.db.SaveChanges();
            this.id = csep.id;
            return retVal;
        }

        public int delete()
        {
            confined_space csep = this.db.confined_space.Find(this.id);
            this.db.confined_space.Remove(csep);
            return this.db.SaveChanges();
        }

        public int edit()
        {
            confined_space csep = this.db.confined_space.Find(this.id);
            csep.attendant = this.attendant;
            csep.purpose = this.purpose;
            csep.validity_period_start = this.validity_period_start;
            csep.validity_period_end = this.validity_period_end;
            csep.screening_spv = this.screening_spv;
            csep.screening_fo = this.screening_fo;
            csep.screening_1 = this.screening_1;
            csep.screening_2 = this.screening_2;
            csep.screening_3 = this.screening_3;
            csep.screening_4 = this.screening_4;
            csep.screening_5 = this.screening_5;
            csep.screening_6 = this.screening_6;
            csep.screening_7 = this.screening_7;
            csep.notes = this.notes;
            csep.methane_result = this.methane_result;
            csep.o2_result = this.o2_result;
            csep.h2s_result = this.h2s_result;
            csep.co_result = this.co_result;
            csep.other_gas = this.other_gas;
            csep.other_result = this.other_result;

            this.db.Entry(csep).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }

        #region generate CSEP number

        public void generateNumber(string ptw_no)
        {
            string result = "CSEP-" + ptw_no;

            this.csep_no = result;
        }

        #endregion

        #region check user

        public bool isWorkLeader(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.work_leader == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_supervisor == user_id || this.acc_supervisor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFireWatch(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_fire_watch == user_id || this.acc_fire_watch_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFO(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.acc_fo == user_id || this.acc_fo_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccGasTester(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if (this.acc_gas_tester == user_id)
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanSupervisor(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_supervisor == user_id || this.can_supervisor_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanFireWatch(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_fire_watch == user_id || this.can_fire_watch_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isCanFO(UserEntity user)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            if ((this.can_fo == user_id || this.can_fo_delegate == user_id))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isExtGasTester(UserEntity user, int extension)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            switch (extension)
            {
                case 1:
                    if ((this.ext_gas_tester_1 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 2:
                    if ((this.ext_gas_tester_2 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 3:
                    if ((this.ext_gas_tester_3 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 4:
                    if ((this.ext_gas_tester_4 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 5:
                    if ((this.ext_gas_tester_5 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 6:
                    if ((this.ext_gas_tester_6 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 7:
                    if ((this.ext_gas_tester_7 == user_id))
                    {
                        retVal = true;
                    }
                    break;
            }

            return retVal;
        }

        public bool isExtFO(UserEntity user, int extension)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            switch (extension)
            {
                case 1:
                    if ((this.ext_fo_1 == user_id || this.ext_fo_delegate_1 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 2:
                    if ((this.ext_fo_2 == user_id || this.ext_fo_delegate_2 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 3:
                    if ((this.ext_fo_3 == user_id || this.ext_fo_delegate_3 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 4:
                    if ((this.ext_fo_4 == user_id || this.ext_fo_delegate_4 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 5:
                    if ((this.ext_fo_5 == user_id || this.ext_fo_delegate_5 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 6:
                    if ((this.ext_fo_6 == user_id || this.ext_fo_delegate_6 == user_id))
                    {
                        retVal = true;
                    }
                    break;
                case 7:
                    if ((this.ext_fo_7 == user_id || this.ext_fo_delegate_7 == user_id))
                    {
                        retVal = true;
                    }
                    break;
            }

            return retVal;
        }

        public bool isCanEdit(UserEntity user)
        {
            bool isCanEdit = false;
            if (isWorkLeader(user) && this.status < (int)CsepStatus.ACCWORKLEADER)
            {
                isCanEdit = true;
            }

            if (isAccSupervisor(user) && (this.status == (int)CsepStatus.CREATE || this.status == (int)CsepStatus.ACCWORKLEADER))
            {
                isCanEdit = true;
            }

            //if (isAccFireWatch(user) && this.status == (int)CsepStatus.ACCSPV)
            //{
            //    isCanEdit = true;
            //}

            if (isAccFO(user) && this.status == (int)CsepStatus.ACCSPV || this.status == (int)CsepStatus.SPVSCREENING)
            {
                isCanEdit = true;
            }

            if ((isAccGasTester(user) || isAccFO(user)) && this.status == (int)CsepStatus.FOSCREENING)
            {
                isCanEdit = true;
            }

            if (this.id == 0)
            {
                isCanEdit = true;
            }

            return isCanEdit;
        }

        public bool isCanEditExt(UserEntity user, int extension)
        {
            bool isCanEdit = false;
            if (isWorkLeader(user))
            {
                switch (extension)
                {
                    case 1:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER1 && this.status >= (int)CsepStatus.EXTCREATE1)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 2:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER2 && this.status >= (int)CsepStatus.EXTCREATE2)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 3:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER3 && this.status >= (int)CsepStatus.EXTCREATE3)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 4:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER4 && this.status >= (int)CsepStatus.EXTCREATE4)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 5:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER5 && this.status >= (int)CsepStatus.EXTCREATE5)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 6:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER6 && this.status >= (int)CsepStatus.EXTCREATE6)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 7:
                        if (this.status < (int)CsepStatus.EXTACCWORKLEADER7 && this.status >= (int)CsepStatus.EXTCREATE7)
                        {
                            isCanEdit = true;
                        }
                        break;
                }

            }

            if (isExtFO(user, extension))
            {
                switch (extension)
                {
                    case 1:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER1 || this.status == (int)CsepStatus.EXTCREATE1 || this.status == (int)CsepStatus.EXTFOSCREENING1)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 2:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER2 || this.status == (int)CsepStatus.EXTCREATE2 || this.status == (int)CsepStatus.EXTFOSCREENING2)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 3:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER3 || this.status == (int)CsepStatus.EXTCREATE3 || this.status == (int)CsepStatus.EXTFOSCREENING3)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 4:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER4 || this.status == (int)CsepStatus.EXTCREATE4 || this.status == (int)CsepStatus.EXTFOSCREENING4)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 5:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER5 || this.status == (int)CsepStatus.EXTCREATE5 || this.status == (int)CsepStatus.EXTFOSCREENING5)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 6:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER6 || this.status == (int)CsepStatus.EXTCREATE6 || this.status == (int)CsepStatus.EXTFOSCREENING6)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 7:
                        if (this.status == (int)CsepStatus.EXTACCWORKLEADER7 || this.status == (int)CsepStatus.EXTCREATE7 || this.status == (int)CsepStatus.EXTFOSCREENING7)
                        {
                            isCanEdit = true;
                        }
                        break;
                }
            }

            if (isExtGasTester(user, extension))
            {
                switch (extension)
                {
                    case 1:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING1)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 2:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING2)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 3:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING3)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 4:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING4)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 5:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING5)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 6:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING6)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 7:
                        if (this.status == (int)CsepStatus.EXTFOSCREENING7)
                        {
                            isCanEdit = true;
                        }
                        break;
                }
            }

            return isCanEdit;
        }

        public bool isCloseCSEP()
        {
            return this.status >= (int)CsepStatus.CANCEL;
        }

        public int assignSupervisor(UserEntity supervisor)
        {
            this.acc_supervisor = supervisor.id.ToString();
            this.can_supervisor = supervisor.id.ToString();
            this.acc_supervisor_delegate = supervisor.employee_delegate.ToString();
            this.can_supervisor_delegate = supervisor.employee_delegate.ToString();

            confined_space csep = this.db.confined_space.Find(this.id);
            csep.acc_supervisor = this.acc_supervisor;
            csep.can_supervisor = this.can_supervisor;
            csep.acc_supervisor_delegate = this.acc_supervisor_delegate;
            csep.can_supervisor_delegate = this.can_supervisor_delegate;

            this.db.Entry(csep).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignFireWatch(UserEntity assessor)
        {
            this.acc_fire_watch = assessor.id.ToString();
            this.can_fire_watch = assessor.id.ToString();
            this.acc_fire_watch_delegate = assessor.employee_delegate.ToString();
            this.can_fire_watch_delegate = assessor.employee_delegate.ToString();

            confined_space csep = this.db.confined_space.Find(this.id);
            csep.acc_fire_watch = this.acc_fire_watch;
            csep.can_fire_watch = this.can_fire_watch;
            csep.acc_fire_watch_delegate = this.acc_fire_watch_delegate;
            csep.can_fire_watch_delegate = this.can_fire_watch_delegate;

            this.db.Entry(csep).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignGasTester(UserEntity gasTester)
        {
            this.acc_gas_tester = gasTester.id.ToString();

            confined_space csep = this.db.confined_space.Find(this.id);
            csep.acc_gas_tester = this.acc_gas_tester;

            this.db.Entry(csep).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignExtGasTester(UserEntity gasTester, int status)
        {
            confined_space csep = this.db.confined_space.Find(this.id);
            switch (status)
            {
                case (int)CsepStatus.EXTFOSCREENING1:
                    this.ext_gas_tester_1 = gasTester.id.ToString();
                    csep.ext_gas_tester_1 = this.ext_gas_tester_1;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING2:
                    this.ext_gas_tester_2 = gasTester.id.ToString();
                    csep.ext_gas_tester_2 = this.ext_gas_tester_2;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING3:
                    this.ext_gas_tester_3 = gasTester.id.ToString();
                    csep.ext_gas_tester_3 = this.ext_gas_tester_3;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING4:
                    this.ext_gas_tester_4 = gasTester.id.ToString();
                    csep.ext_gas_tester_4 = this.ext_gas_tester_4;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING5:
                    this.ext_gas_tester_5 = gasTester.id.ToString();
                    csep.ext_gas_tester_5 = this.ext_gas_tester_5;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING6:
                    this.ext_gas_tester_6 = gasTester.id.ToString();
                    csep.ext_gas_tester_6 = this.ext_gas_tester_6;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
                case (int)CsepStatus.EXTFOSCREENING7:
                    this.ext_gas_tester_7 = gasTester.id.ToString();
                    csep.ext_gas_tester_7 = this.ext_gas_tester_7;

                    this.db.Entry(csep).State = EntityState.Modified;
                    break;
            }

            return this.db.SaveChanges();
        }

        public int assignExtWorkLeader(int status)
        {
            if (status == (int)CsepStatus.EXTCREATE1)
            {
                this.ext_work_leader_1 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_1 = this.ext_work_leader_1;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE2)
            {
                this.ext_work_leader_2 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_2 = this.ext_work_leader_2;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE3)
            {
                this.ext_work_leader_3 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_3 = this.ext_work_leader_3;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE4)
            {
                this.ext_work_leader_4 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_4 = this.ext_work_leader_4;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE5)
            {
                this.ext_work_leader_5 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_5 = this.ext_work_leader_5;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE6)
            {
                this.ext_work_leader_6 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_6 = this.ext_work_leader_6;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE7)
            {
                this.ext_work_leader_7 = this.work_leader;

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_work_leader_7 = this.ext_work_leader_7;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }

            return 0;
        }

        public bool isExistFO(UserEntity fo, int status)
        {
            if (status == (int)CsepStatus.SPVSCREENING)
            {
                return this.acc_fo != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE1)
            {
                return this.ext_fo_1 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE2)
            {
                return this.ext_fo_2 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE3)
            {
                return this.ext_fo_3 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE4)
            {
                return this.ext_fo_4 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE5)
            {
                return this.ext_fo_5 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE6)
            {
                return this.ext_fo_6 != null;
            }
            else if (status == (int)CsepStatus.EXTCREATE7)
            {
                return this.ext_fo_7 != null;
            }
            else
            {
                return this.can_fo != null;
            }
        }

        public int assignFO(UserEntity fo, int status)
        {
            if (status == (int)CsepStatus.SPVSCREENING)
            {
                this.acc_fo = fo.id.ToString();
                this.acc_fo_delegate = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.acc_fo = this.acc_fo;
                csep.acc_fo_delegate = this.acc_fo_delegate;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE1)
            {
                this.ext_fo_1 = fo.id.ToString();
                this.ext_fo_delegate_1 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_1 = this.ext_fo_1;
                csep.ext_fo_delegate_1 = this.ext_fo_delegate_1;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE2)
            {
                this.ext_fo_2 = fo.id.ToString();
                this.ext_fo_delegate_2 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_2 = this.ext_fo_2;
                csep.ext_fo_delegate_2 = this.ext_fo_delegate_2;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE3)
            {
                this.ext_fo_3 = fo.id.ToString();
                this.ext_fo_delegate_3 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_3 = this.ext_fo_3;
                csep.ext_fo_delegate_3 = this.ext_fo_delegate_3;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE4)
            {
                this.ext_fo_4 = fo.id.ToString();
                this.ext_fo_delegate_4 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_4 = this.ext_fo_4;
                csep.ext_fo_delegate_4 = this.ext_fo_delegate_4;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE5)
            {
                this.ext_fo_5 = fo.id.ToString();
                this.ext_fo_delegate_5 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_5 = this.ext_fo_5;
                csep.ext_fo_delegate_5 = this.ext_fo_delegate_5;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE6)
            {
                this.ext_fo_6 = fo.id.ToString();
                this.ext_fo_delegate_6 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_6 = this.ext_fo_6;
                csep.ext_fo_delegate_6 = this.ext_fo_delegate_6;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)CsepStatus.EXTCREATE7)
            {
                this.ext_fo_7 = fo.id.ToString();
                this.ext_fo_delegate_7 = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.ext_fo_7 = this.ext_fo_7;
                csep.ext_fo_delegate_7 = this.ext_fo_delegate_7;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else
            {
                this.can_fo = fo.id.ToString();
                this.can_fo_delegate = fo.employee_delegate.ToString();

                confined_space csep = this.db.confined_space.Find(this.id);
                csep.can_fo = this.can_fo;
                csep.can_fo_delegate = this.can_fo_delegate;

                this.db.Entry(csep).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
        }

        public bool isUserInCSEP(UserEntity user)
        {
            return (isWorkLeader(user) || isAccSupervisor(user) || isAccFireWatch(user) || isAccFO(user) || isAccGasTester(user)
                || isExtGasTester(user, 1) || isExtGasTester(user, 2) || isExtGasTester(user, 3) || isExtGasTester(user, 4)
                || isExtGasTester(user, 5) || isExtGasTester(user, 6) || isExtGasTester(user, 7) || isExtFO(user, 1)
                || isExtFO(user, 2) || isExtFO(user, 3) || isExtFO(user, 4) || isExtFO(user, 5) || isExtFO(user, 6)
                || isExtFO(user, 7) || isCanSupervisor(user) || isCanFO(user) || isCanFireWatch(user));
        }

        #endregion

        #region retVal

        public int setStatus(int status)
        {
            this.status = status;

            confined_space csep = this.db.confined_space.Find(this.id);
            csep.status = status;
            this.db.Entry(csep).State = EntityState.Modified;
            this.db.SaveChanges();

            return csep.status.Value;
        }

        public string getStatus()
        {
            string retVal = "";
            switch (this.status)
            {
                case (int)CsepStatus.CREATE: retVal = "Waiting for Supervisor Screening"; break;
                case (int)CsepStatus.SPVSCREENING: retVal = "Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.FOSCREENING: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.GASTESTER: retVal = "Waiting for Approval by Work Leader / PTW Requestor"; break;
                case (int)CsepStatus.ACCWORKLEADER: retVal = "Waiting for Approval by Supervisor"; break;
                case (int)CsepStatus.ACCSPV: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.ACCFO: retVal = "Completed. Confined Space Entry Permit has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE1: retVal = "Confined Space Entry extended number 1. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING1: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER1: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER1: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO1: retVal = "Confined Space Entry extension number 1 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE2: retVal = "Confined Space Entry extended number 2. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING2: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER2: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER2: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO2: retVal = "Confined Space Entry extension number 2 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE3: retVal = "Confined Space Entry extended number 3. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING3: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER3: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER3: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO3: retVal = "Confined Space Entry extension number 3 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE4: retVal = "Confined Space Entry extended number 4. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING4: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER4: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER4: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO4: retVal = "Confined Space Entry extension number 4 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE5: retVal = "Confined Space Entry extended number 5. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING5: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER5: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER5: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO5: retVal = "Confined Space Entry extension number 5 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE6: retVal = "Confined Space Entry extended number 6. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING6: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER6: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER6: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO6: retVal = "Confined Space Entry extension number 6 has been approved by Facility Owner"; break;
                case (int)CsepStatus.EXTCREATE7: retVal = "Confined Space Entry extended number 7. Waiting for Facility Owner Screening"; break;
                case (int)CsepStatus.EXTFOSCREENING7: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)CsepStatus.EXTGASTESTER7: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.EXTACCWORKLEADER7: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)CsepStatus.EXTACCFO7: retVal = "Confined Space Entry extension number 7 has been approved by Facility Owner"; break;
                case (int)CsepStatus.CANCEL: retVal = "Confined Space Entry Permit has been Closed. Waiting for Approval by Work Leader"; break;
                case (int)CsepStatus.CANWORKLEADER: retVal = "Confined Space Entry Permit has been Closed. Waiting for Approval by Supervisor"; break;
                case (int)CsepStatus.CANSPV: retVal = "Waiting for Closing Approval by Facility Owner"; break;
                case (int)CsepStatus.CANFO: retVal = "Closed. Confined Space Entry Permit has been approved to closing by Facility Owner"; break;
            };

            return retVal;
        }

        #endregion

        public string closeCsep(UserEntity user)
        {
            if (user.id.ToString() != this.work_leader)
            {
                return "400";
            }
            else
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                this.status = (int)CsepStatus.CANCEL;

                csep.status = this.status;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                requestorCan(user);

                return "200";
            }
        }

        public string getHiraNo()
        {
            this.hira_no = "";
            foreach (HiraEntity hira in this.hira_document)
            {
                string fileName = hira.filename.Substring(0, hira.filename.Length - 4);
                this.hira_no += ", " + fileName;
            }

            if (this.hira_no.Length == 0)
            {
                return this.hira_no;
            }
            else
            {
                return this.hira_no.Substring(2);
            }
        }

        public string generateRandomPIN()
        {
            Random rnd = new Random();
            int card = rnd.Next(1001, 9999);

            this.random_pin = card.ToString().PadLeft(4, '0');
            return this.random_pin;
        }

        #region send email

        public string sendEmailFO(List<UserEntity> listFO, string serverUrl, string token, UserEntity user, int? ext = null)
        {
            if (ext == null)
            {

                string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
                string val = "emailfo";
                SendEmail sendEmail = new SendEmail();
                foreach (UserEntity fo in listFO)
                {
                    string timestamp = DateTime.UtcNow.Ticks.ToString();
                    List<string> s = new List<string>();
                    //s.Add(fo.email);
                    s.Add("septu.jamasoka@gmail.com"); // email FO
                    if (fo.employee_delegate != null)
                    {
                        UserEntity del = new UserEntity(fo.employee_delegate.Value, token, user);
                        //s.Add(del.email);
                        s.Add("septu.jamasoka@gmail.com"); // email Delegasi FO
                    }

                    string encodedValue = this.status + salt + fo.id + val + this.id;
                    string encodedElement = Base64.Base64Encode(encodedValue);

                    string seal = Base64.MD5Seal(timestamp + salt + val);

                    string message = serverUrl + "Csep/SetFacilityOwner?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                    sendEmail.Send(s, message, "Confined Space Entry Permit Facility Owner");
                }
            }

            return "200";
        }

        public string sendEmailRandomPIN(string serverUrl, string token, UserEntity user)
        {
            UserEntity requestor = new UserEntity(Int32.Parse(this.work_leader), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = this.random_pin;
            string subject = "PIN for Approving as Requestor";

            sendEmail.Send(s, message, subject);
            return "200";
        }

        public string sendEmailGasTester(string serverUrl, string token, UserEntity user, int extension)
        {
            if (extension == 0)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.acc_gas_tester), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 1)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_1), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();

                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 2)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_2), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 3)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_3), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 4)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_4), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 5)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_5), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 6)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_6), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }
            else if (extension == 7)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_7), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                //s.Add(gasTester.email);
                s.Add("septu.jamasoka@gmail.com");

                string message = serverUrl + "Home?p=Csep/edit/" + this.id;

                sendEmail.Send(s, message, "Confined Space Entry Permit Gas Tester");
            }

            return "200";
        }

        public string sendEmailRequestor(string serverUrl, string token, UserEntity user, int extension, int stat = 0, string comment = null)
        {
            //if (extension == 0)
            //{
            UserEntity requestor = new UserEntity(Int32.Parse(this.work_leader), token, user);
            UserEntity spv = new UserEntity(Int32.Parse(this.acc_supervisor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            //s.Add(spv.email);
            s.Add("septu.jamasoka@gmail.com");

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id;
                subject = "Confined Space Entry Permit Requestor Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id + "<br />" + comment;
                subject = "Confined Space Entry Permit Approval Rejection";
            }

            sendEmail.Send(s, message, subject);
            //}

            return "200";
        }

        public string sendEmailSupervisor(string serverUrl, string token, UserEntity user, int stat = 0, string comment = null)
        {
            UserEntity supervisor = new UserEntity(Int32.Parse(this.acc_supervisor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(requestor.email);
            s.Add("septu.jamasoka@gmail.com");

            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value, token, user);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id;
                subject = "Confined Space Entry Permit Supervisor Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id + "<br />" + comment;
                subject = "Confined Space Entry Permit Approval Rejection";
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFireWatch(string serverUrl, string token, UserEntity user, int stat = 0, string comment = null)
        {
            UserEntity fireWatch = new UserEntity(Int32.Parse(this.acc_fire_watch), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            s.Add(fireWatch.email);
            s.Add("septu.jamasoka@gmail.com");

            if (fireWatch.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fireWatch.employee_delegate.Value, token, user);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id;
                subject = "Confined Space Entry Permit Fire Watch Approve Approve";
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Csep/edit/" + this.id + "<br />" + comment;
                subject = "Confined Space Entry Permit Approval Rejection";
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFOAcc(string serverUrl, string token, UserEntity user)
        {
            UserEntity fOAcc = new UserEntity(Int32.Parse(this.acc_fo), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fOAcc.email);
            s.Add("septu.jamasoka@gmail.com");

            if (fOAcc.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOAcc.employee_delegate.Value, token, user);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = serverUrl + "Home?p=Csep/edit/" + this.id;

            sendEmail.Send(s, message, "Confined Space Entry Permit Facility Owner Approve");

            return "200";
        }

        public string sendEmailFOExt(string serverUrl, string token, UserEntity user, int extension)
        {
            int fo_id = 0;
            switch (extension)
            {
                case 1:
                    fo_id = Int32.Parse(this.ext_fo_1);
                    break;
                case 2:
                    fo_id = Int32.Parse(this.ext_fo_2);
                    break;
                case 3:
                    fo_id = Int32.Parse(this.ext_fo_3);
                    break;
                case 4:
                    fo_id = Int32.Parse(this.ext_fo_4);
                    break;
                case 5:
                    fo_id = Int32.Parse(this.ext_fo_5);
                    break;
                case 6:
                    fo_id = Int32.Parse(this.ext_fo_6);
                    break;
                case 7:
                    fo_id = Int32.Parse(this.ext_fo_7);
                    break;
            }
            UserEntity fOAcc = new UserEntity(fo_id, token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fOAcc.email);
            s.Add("septu.jamasoka@gmail.com");

            if (fOAcc.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOAcc.employee_delegate.Value, token, user);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = serverUrl + "Home?p=Csep/edit/" + this.id;

            sendEmail.Send(s, message, "Confined Space Entry Permit Facility Owner Approve");

            return "200";
        }

        public string sendEmailFOCan(string serverUrl, string token, UserEntity user)
        {
            UserEntity fOCan = new UserEntity(Int32.Parse(this.can_fo), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            //s.Add(fOAcc.email);
            s.Add("fOCan.jamasoka@gmail.com");

            if (fOCan.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOCan.employee_delegate.Value, token, user);
                //s.Add(del.email);
                s.Add("septu.jamasoka@gmail.com");
            }

            string message = serverUrl + "Home?p=Csep/edit/" + this.id;

            sendEmail.Send(s, message, "Confined Space Entry Permit Facility Owner Closing");

            return "200";
        }

        #endregion

        #region approve reject

        public string gasTesterAcc(UserEntity user, int extension)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            confined_space csep = this.db.confined_space.Find(this.id);
            if (extension == 0 && (user.id.ToString() == this.acc_gas_tester || user.id.ToString() == this.acc_fo))
            {
                csep.acc_gas_tester_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.GASTESTER;
            }
            else if (extension == 1 && (user.id.ToString() == this.ext_gas_tester_1 || user.id.ToString() == this.ext_fo_1))
            {
                csep.ext_gas_tester_approve_1 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER1;
            }
            else if (extension == 2 && (user.id.ToString() == this.ext_gas_tester_2 || user.id.ToString() == this.ext_fo_2))
            {
                csep.ext_gas_tester_approve_2 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER2;
            }
            else if (extension == 3 && (user.id.ToString() == this.ext_gas_tester_3 || user.id.ToString() == this.ext_fo_3))
            {
                csep.ext_gas_tester_approve_3 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER3;
            }
            else if (extension == 4 && (user.id.ToString() == this.ext_gas_tester_4 || user.id.ToString() == this.ext_fo_4))
            {
                csep.ext_gas_tester_approve_4 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER4;
            }
            else if (extension == 5 && (user.id.ToString() == this.ext_gas_tester_5 || user.id.ToString() == this.ext_fo_5))
            {
                csep.ext_gas_tester_approve_5 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER5;
            }
            else if (extension == 6 && (user.id.ToString() == this.ext_gas_tester_6 || user.id.ToString() == this.ext_fo_6))
            {
                csep.ext_gas_tester_approve_6 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER6;
            }
            else if (extension == 7 && (user.id.ToString() == this.ext_gas_tester_7 || user.id.ToString() == this.ext_fo_7))
            {
                csep.ext_gas_tester_approve_7 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTGASTESTER7;
            }
            else
            {
                return "400";
            }

            this.db.Entry(csep).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        public string requestorAcc(UserEntity user, string token, int extension, string random_pin = null)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            confined_space csep = this.db.confined_space.Find(this.id);
            if (extension == 0 && user.id.ToString() == this.work_leader)
            {
                csep.acc_work_leader_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.ACCWORKLEADER;
            }
            else if (extension == 0 && random_pin != null && user.id.ToString() == this.acc_supervisor)
            {
                if (random_pin == this.random_pin)
                {
                    user = new UserEntity(Int32.Parse(this.work_leader), token, user);
                    csep.acc_work_leader_approve = "a" + user.signature;
                    csep.status = (int)CsepStatus.ACCWORKLEADER;
                }
                else
                {
                    return "402";
                }
            }
            else if (extension == 1 && user.id.ToString() == this.ext_work_leader_1)
            {
                csep.ext_work_leader_approve_1 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER1;
            }
            else if (extension == 2 && user.id.ToString() == this.ext_work_leader_2)
            {
                csep.ext_work_leader_approve_2 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER2;
            }
            else if (extension == 3 && user.id.ToString() == this.ext_work_leader_3)
            {
                csep.ext_work_leader_approve_3 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER3;
            }
            else if (extension == 4 && user.id.ToString() == this.ext_work_leader_4)
            {
                csep.ext_work_leader_approve_4 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER4;
            }
            else if (extension == 5 && user.id.ToString() == this.ext_work_leader_5)
            {
                csep.ext_work_leader_approve_5 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER5;
            }
            else if (extension == 6 && user.id.ToString() == this.ext_work_leader_6)
            {
                csep.ext_work_leader_approve_6 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER6;
            }
            else if (extension == 7 && user.id.ToString() == this.ext_work_leader_7)
            {
                csep.ext_work_leader_approve_7 = "a" + user.signature;
                csep.status = (int)CsepStatus.EXTACCWORKLEADER7;
            }
            else
            {
                return "400";
            }

            this.db.Entry(csep).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        public string supervisorAcc(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}
            //               401 {not select assessor}

            confined_space csep = this.db.confined_space.Find(this.id);
            //if (csep.acc_fire_watch == null)
            //{
            //    return "401";
            //}

            if (user.id.ToString() == this.acc_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                csep.acc_supervisor_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.ACCSPV;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.acc_supervisor_delegate)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                csep.acc_supervisor_approve = "d" + user.signature;
                csep.status = (int)CsepStatus.ACCSPV;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string supervisorAccReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_supervisor || user.id.ToString() == this.acc_supervisor_delegate)
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                csep.acc_work_leader_approve = null;
                csep.status = (int)CsepStatus.GASTESTER;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fireWatchAccApproval(UserEntity user)
        {
            // assessor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            confined_space csep = this.db.confined_space.Find(this.id);

            if (user.id.ToString() == this.acc_fire_watch)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                csep.acc_fire_watch_approve = "a" + user.signature;
                //csep.status = (int)CsepStatus.ACCFIREWATCH;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.acc_fire_watch_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                csep.acc_fire_watch_approve = "d" + user.signature;
                //csep.status = (int)CsepStatus.ACCFIREWATCH;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string fireWatchAccReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.acc_fire_watch || user.id.ToString() == this.acc_fire_watch_delegate)
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                csep.acc_supervisor_approve = null;
                //csep.status = (int)CsepStatus.ACCWORKLEADER;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fOAccApproval(UserEntity user, int extension)
        {
            // FO approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            confined_space csep = this.db.confined_space.Find(this.id);

            if (extension == 0)
            {
                if (user.id.ToString() == this.acc_fo)
                {
                    csep.acc_fo_approve = "a" + user.signature;
                    csep.status = (int)CsepStatus.ACCFO;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else if (user.id.ToString() == this.acc_fo_delegate)
                {
                    csep.acc_fo_approve = "d" + user.signature;
                    csep.status = (int)CsepStatus.ACCFO;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "201";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 1)
            {
                if (user.id.ToString() == this.ext_fo_1)
                {
                    csep.ext_fo_approve_1 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO1;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_1)
                {
                    csep.ext_fo_approve_1 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO1;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 2)
            {
                if (user.id.ToString() == this.ext_fo_2)
                {
                    csep.ext_fo_approve_2 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO2;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_2)
                {
                    csep.ext_fo_approve_2 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO2;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 3)
            {
                if (user.id.ToString() == this.ext_fo_3)
                {
                    csep.ext_fo_approve_3 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO3;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_3)
                {
                    csep.ext_fo_approve_3 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO3;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 4)
            {
                if (user.id.ToString() == this.ext_fo_4)
                {
                    csep.ext_fo_approve_4 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO4;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_4)
                {
                    csep.ext_fo_approve_4 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO4;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 5)
            {
                if (user.id.ToString() == this.ext_fo_5)
                {
                    csep.ext_fo_approve_5 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO5;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_5)
                {
                    csep.ext_fo_approve_5 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO5;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 6)
            {
                if (user.id.ToString() == this.ext_fo_6)
                {
                    csep.ext_fo_approve_6 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO6;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_6)
                {
                    csep.ext_fo_approve_6 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO6;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 7)
            {
                if (user.id.ToString() == this.ext_fo_7)
                {
                    csep.ext_fo_approve_7 = "a" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO7;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else if (user.id.ToString() == this.ext_fo_delegate_7)
                {
                    csep.ext_fo_approve_7 = "d" + user.signature;
                    csep.status = (int)CsepStatus.EXTACCFO7;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
                else
                {
                    return "400";
                }
            }
            else
            {
                return "200";
            }
        }

        public string fOAccReject(UserEntity user, int extension, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}
            if (extension == 0)
            {
                if (user.id.ToString() == this.acc_fo || user.id.ToString() == this.acc_fo_delegate)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.acc_supervisor_approve = null;
                    csep.status = (int)CsepStatus.ACCWORKLEADER;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 1)
            {
                if (user.id.ToString() == this.ext_fo_1 || user.id.ToString() == this.ext_fo_delegate_1)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_1 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER1;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 2)
            {
                if (user.id.ToString() == this.ext_fo_2 || user.id.ToString() == this.ext_fo_delegate_2)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_2 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER2;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 3)
            {
                if (user.id.ToString() == this.ext_fo_3 || user.id.ToString() == this.ext_fo_delegate_3)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_3 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER3;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 4)
            {
                if (user.id.ToString() == this.ext_fo_4 || user.id.ToString() == this.ext_fo_delegate_4)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_4 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER4;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 5)
            {
                if (user.id.ToString() == this.ext_fo_5 || user.id.ToString() == this.ext_fo_delegate_5)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_5 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER5;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 6)
            {
                if (user.id.ToString() == this.ext_fo_6 || user.id.ToString() == this.ext_fo_delegate_6)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_6 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER6;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else if (extension == 7)
            {
                if (user.id.ToString() == this.ext_fo_7 || user.id.ToString() == this.ext_fo_delegate_7)
                {
                    confined_space csep = this.db.confined_space.Find(this.id);
                    csep.ext_work_leader_approve_7 = null;
                    csep.status = (int)CsepStatus.EXTGASTESTER7;
                    this.db.Entry(csep).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "200";
                }
                else
                {
                    return "400";
                }
            }
            else
            {
                return "200";
            }
        }

        public string requestorCan(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            confined_space csep = this.db.confined_space.Find(this.id);
            if (user.id.ToString() == this.work_leader)
            {
                csep.can_work_leader_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.CANWORKLEADER;
            }
            this.db.Entry(csep).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        public string supervisorCan(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            confined_space csep = this.db.confined_space.Find(this.id);

            if (user.id.ToString() == this.can_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                csep.can_supervisor_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.CANSPV;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.can_supervisor_delegate)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                csep.can_supervisor_approve = "d" + user.signature;
                csep.status = (int)CsepStatus.CANSPV;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string supervisorCanReject(UserEntity user, string comment)
        {
            // supervisor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_supervisor || user.id.ToString() == this.can_supervisor_delegate)
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                csep.can_work_leader_approve = null;
                csep.status = (int)CsepStatus.CANCEL;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fireWatchCanApproval(UserEntity user)
        {
            // assessor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            confined_space csep = this.db.confined_space.Find(this.id);

            if (user.id.ToString() == this.can_fire_watch)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                csep.can_fire_watch_approve = "a" + user.signature;
                //csep.status = (int)CsepStatus.CANFIREWATCH;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.can_fire_watch_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                csep.can_fire_watch_approve = "d" + user.signature;
                //csep.status = (int)CsepStatus.CANFIREWATCH;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string fireWatchCanReject(UserEntity user, string comment)
        {
            // assessor reject
            // return code - 200 {ok}
            //               400 {not the user}

            if (user.id.ToString() == this.can_fire_watch || user.id.ToString() == this.can_fire_watch_delegate)
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                csep.can_supervisor_approve = null;
                csep.status = (int)CsepStatus.CANWORKLEADER;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        public string fOCanApproval(UserEntity user)
        {
            // FO approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            confined_space csep = this.db.confined_space.Find(this.id);

            if (user.id.ToString() == this.can_fo)
            {
                csep.can_fo_approve = "a" + user.signature;
                csep.status = (int)CsepStatus.CANFO;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();
                return "200";
            }
            else if (user.id.ToString() == this.can_fo_delegate)
            {
                csep.can_fo_approve = "d" + user.signature;
                csep.status = (int)CsepStatus.CANFO;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();
                return "201";
            }
            else
            {
                return "400";
            }
        }

        public string fOCanReject(UserEntity user, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}
            if (user.id.ToString() == this.can_fo || user.id.ToString() == this.can_fo_delegate)
            {
                confined_space csep = this.db.confined_space.Find(this.id);
                csep.can_supervisor_approve = null;
                csep.status = (int)CsepStatus.CANWORKLEADER;
                this.db.Entry(csep).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else
            {
                return "400";
            }
        }

        #endregion

        #region is can add extension

        public bool isCanAddExtension()
        {
            return (this.status == (int)CsepStatus.ACCFO || this.status == (int)CsepStatus.EXTACCFO1
                 || this.status == (int)CsepStatus.EXTACCFO2
                 || this.status == (int)CsepStatus.EXTACCFO3
                 || this.status == (int)CsepStatus.EXTACCFO4
                 || this.status == (int)CsepStatus.EXTACCFO5
                 || this.status == (int)CsepStatus.EXTACCFO6);
        }

        public int getExtensionNo()
        {
            if (this.status >= (int)CsepStatus.EXTCREATE1 && this.status <= (int)CsepStatus.EXTACCFO1)
            {
                return 1;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE2 && this.status <= (int)CsepStatus.EXTACCFO2)
            {
                return 2;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE3 && this.status <= (int)CsepStatus.EXTACCFO3)
            {
                return 3;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE4 && this.status <= (int)CsepStatus.EXTACCFO4)
            {
                return 4;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE5 && this.status <= (int)CsepStatus.EXTACCFO5)
            {
                return 5;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE6 && this.status <= (int)CsepStatus.EXTACCFO6)
            {
                return 6;
            }
            if (this.status >= (int)CsepStatus.EXTCREATE7 && this.status <= (int)CsepStatus.EXTACCFO7)
            {
                return 7;
            }

            return 0;
        }

        public int editExtHotWork(int extension)
        {
            confined_space csep = this.db.confined_space.Find(this.id);

            switch (extension)
            {
                case 1:
                    csep.ext_datetime_1 = this.ext_datetime_1;
                    csep.ext_lel_1 = this.ext_lel_1;
                    csep.ext_o2_1 = this.ext_o2_1;
                    csep.ext_h2s_1 = this.ext_h2s_1;
                    csep.ext_co_1 = this.ext_co_1;
                    csep.ext_other_1 = this.ext_other_1;
                    csep.ext_remark_1 = this.ext_remark_1;
                    csep.ext_new_validity_1 = this.ext_new_validity_1;
                    csep.screening_1 = this.screening_1;
                    break;
                case 2:
                    csep.ext_datetime_2 = this.ext_datetime_2;
                    csep.ext_lel_2 = this.ext_lel_2;
                    csep.ext_o2_2 = this.ext_o2_2;
                    csep.ext_h2s_2 = this.ext_h2s_2;
                    csep.ext_co_2 = this.ext_co_2;
                    csep.ext_other_2 = this.ext_other_2;
                    csep.ext_remark_2 = this.ext_remark_2;
                    csep.ext_new_validity_2 = this.ext_new_validity_2;
                    csep.screening_2 = this.screening_2;
                    break;
                case 3:
                    csep.ext_datetime_3 = this.ext_datetime_3;
                    csep.ext_lel_3 = this.ext_lel_3;
                    csep.ext_o2_3 = this.ext_o2_3;
                    csep.ext_h2s_3 = this.ext_h2s_3;
                    csep.ext_co_3 = this.ext_co_3;
                    csep.ext_other_3 = this.ext_other_3;
                    csep.ext_remark_3 = this.ext_remark_3;
                    csep.ext_new_validity_3 = this.ext_new_validity_3;
                    csep.screening_3 = this.screening_3;
                    break;
                case 4:
                    csep.ext_datetime_4 = this.ext_datetime_4;
                    csep.ext_lel_4 = this.ext_lel_4;
                    csep.ext_o2_4 = this.ext_o2_4;
                    csep.ext_h2s_4 = this.ext_h2s_4;
                    csep.ext_co_4 = this.ext_co_4;
                    csep.ext_other_4 = this.ext_other_4;
                    csep.ext_remark_4 = this.ext_remark_4;
                    csep.ext_new_validity_4 = this.ext_new_validity_4;
                    csep.screening_4 = this.screening_4;
                    break;
                case 5:
                    csep.ext_datetime_5 = this.ext_datetime_5;
                    csep.ext_lel_5 = this.ext_lel_5;
                    csep.ext_o2_5 = this.ext_o2_5;
                    csep.ext_h2s_5 = this.ext_h2s_5;
                    csep.ext_co_5 = this.ext_co_5;
                    csep.ext_other_5 = this.ext_other_5;
                    csep.ext_remark_5 = this.ext_remark_5;
                    csep.ext_new_validity_5 = this.ext_new_validity_5;
                    csep.screening_5 = this.screening_5;
                    break;
                case 6:
                    csep.ext_datetime_6 = this.ext_datetime_6;
                    csep.ext_lel_6 = this.ext_lel_6;
                    csep.ext_o2_6 = this.ext_o2_6;
                    csep.ext_h2s_6 = this.ext_h2s_6;
                    csep.ext_co_6 = this.ext_co_6;
                    csep.ext_other_6 = this.ext_other_6;
                    csep.ext_remark_6 = this.ext_remark_6;
                    csep.ext_new_validity_6 = this.ext_new_validity_6;
                    csep.screening_6 = this.screening_6;
                    break;
                case 7:
                    csep.ext_datetime_7 = this.ext_datetime_7;
                    csep.ext_lel_7 = this.ext_lel_7;
                    csep.ext_o2_7 = this.ext_o2_7;
                    csep.ext_h2s_7 = this.ext_h2s_7;
                    csep.ext_co_7 = this.ext_co_7;
                    csep.ext_other_7 = this.ext_other_7;
                    csep.ext_remark_7 = this.ext_remark_7;
                    csep.ext_new_validity_7 = this.ext_new_validity_7;
                    csep.screening_7 = this.screening_7;
                    break;
            };
            this.db.Entry(csep).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        #endregion
    }
}