using PermitToWork.Models.Hira;
using PermitToWork.Models.Ptw;
using PermitToWork.Models.User;
using PermitToWork.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;

namespace PermitToWork.Models.Hw
{
    public class HwEntity : IClearancePermitEntity
    {
        public int id { get; set; }
        public string hw_no { get; set; }
        public string fire_watch { get; set; }
        public string purpose { get; set; }
        public string work_leader { get; set; }
        public Nullable<System.DateTime> validity_period_start { get; set; }
        public Nullable<System.DateTime> validity_period_end { get; set; }
        public string[] screening_spv { get; set; }
        public string[] screening_fo { get; set; }
        public string[] screening_1 { get; set; }
        public string[] screening_2 { get; set; }
        public string[] screening_3 { get; set; }
        public string[] screening_4 { get; set; }
        public string[] screening_5 { get; set; }
        public string[] screening_6 { get; set; }
        public string[] screening_7 { get; set; }
        public string screening_spv_db { get; set; }
        public string screening_fo_db { get; set; }
        public string screening_1_db { get; set; }
        public string screening_2_db { get; set; }
        public string screening_3_db { get; set; }
        public string screening_4_db { get; set; }
        public string screening_5_db { get; set; }
        public string screening_6_db { get; set; }
        public string screening_7_db { get; set; }
        public Nullable<int> id_ptw { get; set; }
        public string notes { get; set; }
        public string methane_result { get; set; }
        public string o2_result { get; set; }
        public string h2s_result { get; set; }
        public string co_result { get; set; }
        public string other_gas { get; set; }
        public string other_result { get; set; }
        public string acc_work_leader_approve { get; set; }
        public string acc_supervisor_approve { get; set; }
        public string acc_supervisor { get; set; }
        public string acc_supervisor_delegate { get; set; }
        public string acc_fire_watch { get; set; }
        public string acc_fire_watch_delegate { get; set; }
        public string acc_fire_watch_approve { get; set; }
        public string acc_fo { get; set; }
        public string acc_fo_delegate { get; set; }
        public string acc_fo_approve { get; set; }
        public string acc_gas_tester { get; set; }
        public string acc_gas_tester_approve { get; set; }
        public string acc_work_representative_approve { get; set; }
        public string can_work_leader_approve { get; set; }
        public string can_supervisor_approve { get; set; }
        public string can_supervisor { get; set; }
        public string can_supervisor_delegate { get; set; }
        public string can_fire_watch { get; set; }
        public string can_fire_watch_delegate { get; set; }
        public string can_fire_watch_approve { get; set; }
        public string can_fo { get; set; }
        public string can_fo_delegate { get; set; }
        public string can_fo_approve { get; set; }
        public Nullable<System.DateTime> ext_datetime_1 { get; set; }
        public string ext_lel_1 { get; set; }
        public string ext_o2_1 { get; set; }
        public string ext_h2s_1 { get; set; }
        public string ext_co_1 { get; set; }
        public string ext_other_1 { get; set; }
        public string ext_gas_tester_1 { get; set; }
        public string ext_gas_tester_approve_1 { get; set; }
        public string ext_work_leader_1 { get; set; }
        public string ext_work_leader_approve_1 { get; set; }
        public string ext_fo_1 { get; set; }
        public string ext_fo_delegate_1 { get; set; }
        public string ext_fo_approve_1 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_1 { get; set; }
        public string ext_remark_1 { get; set; }
        public Nullable<System.DateTime> ext_datetime_2 { get; set; }
        public string ext_lel_2 { get; set; }
        public string ext_o2_2 { get; set; }
        public string ext_h2s_2 { get; set; }
        public string ext_co_2 { get; set; }
        public string ext_other_2 { get; set; }
        public string ext_gas_tester_2 { get; set; }
        public string ext_gas_tester_approve_2 { get; set; }
        public string ext_work_leader_2 { get; set; }
        public string ext_work_leader_approve_2 { get; set; }
        public string ext_fo_2 { get; set; }
        public string ext_fo_delegate_2 { get; set; }
        public string ext_fo_approve_2 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_2 { get; set; }
        public string ext_remark_2 { get; set; }
        public Nullable<System.DateTime> ext_datetime_3 { get; set; }
        public string ext_lel_3 { get; set; }
        public string ext_o2_3 { get; set; }
        public string ext_h2s_3 { get; set; }
        public string ext_co_3 { get; set; }
        public string ext_other_3 { get; set; }
        public string ext_gas_tester_3 { get; set; }
        public string ext_gas_tester_approve_3 { get; set; }
        public string ext_work_leader_3 { get; set; }
        public string ext_work_leader_approve_3 { get; set; }
        public string ext_fo_3 { get; set; }
        public string ext_fo_delegate_3 { get; set; }
        public string ext_fo_approve_3 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_3 { get; set; }
        public string ext_remark_3 { get; set; }
        public Nullable<System.DateTime> ext_datetime_4 { get; set; }
        public string ext_lel_4 { get; set; }
        public string ext_o2_4 { get; set; }
        public string ext_h2s_4 { get; set; }
        public string ext_co_4 { get; set; }
        public string ext_other_4 { get; set; }
        public string ext_gas_tester_4 { get; set; }
        public string ext_gas_tester_approve_4 { get; set; }
        public string ext_work_leader_4 { get; set; }
        public string ext_work_leader_approve_4 { get; set; }
        public string ext_fo_4 { get; set; }
        public string ext_fo_delegate_4 { get; set; }
        public string ext_fo_approve_4 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_4 { get; set; }
        public string ext_remark_4 { get; set; }
        public Nullable<System.DateTime> ext_datetime_5 { get; set; }
        public string ext_lel_5 { get; set; }
        public string ext_o2_5 { get; set; }
        public string ext_h2s_5 { get; set; }
        public string ext_co_5 { get; set; }
        public string ext_other_5 { get; set; }
        public string ext_gas_tester_5 { get; set; }
        public string ext_gas_tester_approve_5 { get; set; }
        public string ext_work_leader_5 { get; set; }
        public string ext_work_leader_approve_5 { get; set; }
        public string ext_fo_5 { get; set; }
        public string ext_fo_delegate_5 { get; set; }
        public string ext_fo_approve_5 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_5 { get; set; }
        public string ext_remark_5 { get; set; }
        public Nullable<System.DateTime> ext_datetime_6 { get; set; }
        public string ext_lel_6 { get; set; }
        public string ext_o2_6 { get; set; }
        public string ext_h2s_6 { get; set; }
        public string ext_co_6 { get; set; }
        public string ext_other_6 { get; set; }
        public string ext_gas_tester_6 { get; set; }
        public string ext_gas_tester_approve_6 { get; set; }
        public string ext_work_leader_6 { get; set; }
        public string ext_work_leader_approve_6 { get; set; }
        public string ext_fo_6 { get; set; }
        public string ext_fo_delegate_6 { get; set; }
        public string ext_fo_approve_6 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_6 { get; set; }
        public string ext_remark_6 { get; set; }
        public Nullable<System.DateTime> ext_datetime_7 { get; set; }
        public string ext_lel_7 { get; set; }
        public string ext_o2_7 { get; set; }
        public string ext_h2s_7 { get; set; }
        public string ext_co_7 { get; set; }
        public string ext_other_7 { get; set; }
        public string ext_gas_tester_7 { get; set; }
        public string ext_gas_tester_approve_7 { get; set; }
        public string ext_work_leader_7 { get; set; }
        public string ext_work_leader_approve_7 { get; set; }
        public string ext_fo_7 { get; set; }
        public string ext_fo_delegate_7 { get; set; }
        public string ext_fo_approve_7 { get; set; }
        public Nullable<System.DateTime> ext_new_validity_7 { get; set; }
        public string ext_remark_7 { get; set; }
        public Nullable<int> status { get; set; }
        public string random_pin { get; set; }

        private PtwEntity ptw { get; set; }
        public List<HiraEntity> hira_document { get; set; }
        public string hira_no { get; set; }

        public string hw_status { get; set; }

        public int ids { get; set; }
        public string statusText { get; set; }

        public bool is_guest { get; set; }
        public bool isUser { get; set; }
        public Dictionary<string, List<string>> listDocumentUploaded { get; set; }

        public enum statusHW
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

        public HwEntity() {
            this.db = new star_energy_ptwEntities();
        }

        public HwEntity(int id_ptw, string work_leader, string purpose, string acc_spv = null, string acc_spv_del = null, string acc_fo = null)
        {
            this.db = new star_energy_ptwEntities();
            this.id_ptw = id_ptw;
            this.work_leader = work_leader;
            this.purpose = purpose;
            this.acc_supervisor = acc_spv;
            this.acc_supervisor_delegate = acc_spv_del;
            this.can_supervisor = acc_spv;
            this.can_supervisor_delegate = acc_spv_del;
            this.acc_fo = acc_fo;
            this.can_fo = acc_fo;
        }

        public HwEntity(string id)
        {
            this.db = new star_energy_ptwEntities();
            hot_work hw = this.db.hot_work.OrderByDescending(p => p.hw_no).FirstOrDefault();
            this.hw_no = hw == null ? "" : hw.hw_no;
        }

        public HwEntity(int id, star_energy_ptwEntities db = null)
        {
            if (db == null)
            {
                this.db = new star_energy_ptwEntities();
            }
            else
            {
                this.db = db;
            }
            hot_work hw = this.db.hot_work.Find(id);
            this.listDocumentUploaded = new Dictionary<string, List<string>>();
            this.id = hw.id;
            this.hw_no = hw.hw_no;
            this.fire_watch = hw.fire_watch;
            this.purpose = hw.purpose;
            this.work_leader = hw.work_leader;
            this.validity_period_start = hw.validity_period_start;
            this.validity_period_end = hw.validity_period_end;
            this.screening_spv = hw.screening_spv.Split('#');
            this.screening_fo = hw.screening_fo.Split('#');
            this.screening_1 = hw.screening_1.Split('#');
            this.screening_2 = hw.screening_2.Split('#');
            this.screening_3 = hw.screening_3.Split('#');
            this.screening_4 = hw.screening_4.Split('#');
            this.screening_5 = hw.screening_5.Split('#');
            this.screening_6 = hw.screening_6.Split('#');
            this.screening_7 = hw.screening_7.Split('#');
            this.id_ptw = hw.id_ptw;
            this.notes = hw.notes;
            this.methane_result = hw.methane_result;
            this.o2_result = hw.o2_result;
            this.h2s_result = hw.h2s_result;
            this.co_result = hw.co_result;
            this.other_gas = hw.other_gas;
            this.other_result = hw.other_result;
            this.acc_work_leader_approve = hw.acc_work_leader_approve;
            this.acc_supervisor_approve = hw.acc_supervisor_approve;
            this.acc_supervisor = hw.acc_supervisor;
            this.acc_supervisor_delegate = hw.acc_supervisor_delegate;
            this.acc_fire_watch = hw.acc_fire_watch;
            this.acc_fire_watch_delegate = hw.acc_fire_watch_delegate;
            this.acc_fire_watch_approve = hw.acc_fire_watch_approve;
            this.acc_fo = hw.acc_fo;
            this.acc_fo_delegate = hw.acc_fo_delegate;
            this.acc_fo_approve = hw.acc_fo_approve;
            this.acc_gas_tester = hw.acc_gas_tester;
            this.acc_gas_tester_approve = hw.acc_gas_tester_approve;
            this.acc_work_representative_approve = hw.acc_work_representative_approve;
            this.can_work_leader_approve = hw.can_work_leader_approve;
            this.can_supervisor_approve = hw.can_supervisor_approve;
            this.can_supervisor = hw.can_supervisor;
            this.can_supervisor_delegate = hw.can_supervisor_delegate;
            this.can_fire_watch = hw.can_fire_watch;
            this.can_fire_watch_delegate = hw.can_fire_watch_delegate;
            this.can_fire_watch_approve = hw.can_fire_watch_approve;
            this.can_fo = hw.can_fo;
            this.can_fo_delegate = hw.can_fo_delegate;
            this.can_fo_approve = hw.can_fo_approve;
            this.ext_datetime_1 = hw.ext_datetime_1;
            this.ext_lel_1 = hw.ext_lel_1;
            this.ext_o2_1 = hw.ext_o2_1;
            this.ext_h2s_1 = hw.ext_h2s_1;
            this.ext_co_1 = hw.ext_co_1;
            this.ext_other_1 = hw.ext_other_1;
            this.ext_gas_tester_1 = hw.ext_gas_tester_1;
            this.ext_gas_tester_approve_1 = hw.ext_gas_tester_approve_1;
            this.ext_work_leader_1 = hw.ext_work_leader_1;
            this.ext_work_leader_approve_1 = hw.ext_work_leader_approve_1;
            this.ext_fo_1 = hw.ext_fo_1;
            this.ext_fo_delegate_1 = hw.ext_fo_delegate_1;
            this.ext_fo_approve_1 = hw.ext_fo_approve_1;
            this.ext_new_validity_1 = hw.ext_new_validity_1;
            this.ext_remark_1 = hw.ext_remark_1;
            this.ext_datetime_2 = hw.ext_datetime_2;
            this.ext_lel_2 = hw.ext_lel_2;
            this.ext_o2_2 = hw.ext_o2_2;
            this.ext_h2s_2 = hw.ext_h2s_2;
            this.ext_co_2 = hw.ext_co_2;
            this.ext_other_2 = hw.ext_other_2;
            this.ext_gas_tester_2 = hw.ext_gas_tester_2;
            this.ext_gas_tester_approve_2 = hw.ext_gas_tester_approve_2;
            this.ext_work_leader_2 = hw.ext_work_leader_2;
            this.ext_work_leader_approve_2 = hw.ext_work_leader_approve_2;
            this.ext_fo_2 = hw.ext_fo_2;
            this.ext_fo_delegate_2 = hw.ext_fo_delegate_2;
            this.ext_fo_approve_2 = hw.ext_fo_approve_2;
            this.ext_new_validity_2 = hw.ext_new_validity_2;
            this.ext_remark_2 = hw.ext_remark_2;
            this.ext_datetime_3 = hw.ext_datetime_3;
            this.ext_lel_3 = hw.ext_lel_3;
            this.ext_o2_3 = hw.ext_o2_3;
            this.ext_h2s_3 = hw.ext_h2s_3;
            this.ext_co_3 = hw.ext_co_3;
            this.ext_other_3 = hw.ext_other_3;
            this.ext_gas_tester_3 = hw.ext_gas_tester_3;
            this.ext_gas_tester_approve_3 = hw.ext_gas_tester_approve_3;
            this.ext_work_leader_3 = hw.ext_work_leader_3;
            this.ext_work_leader_approve_3 = hw.ext_work_leader_approve_3;
            this.ext_fo_3 = hw.ext_fo_3;
            this.ext_fo_delegate_3 = hw.ext_fo_delegate_3;
            this.ext_fo_approve_3 = hw.ext_fo_approve_3;
            this.ext_new_validity_3 = hw.ext_new_validity_3;
            this.ext_remark_3 = hw.ext_remark_3;
            this.ext_datetime_4 = hw.ext_datetime_4;
            this.ext_lel_4 = hw.ext_lel_4;
            this.ext_o2_4 = hw.ext_o2_4;
            this.ext_h2s_4 = hw.ext_h2s_4;
            this.ext_co_4 = hw.ext_co_4;
            this.ext_other_4 = hw.ext_other_4;
            this.ext_gas_tester_4 = hw.ext_gas_tester_4;
            this.ext_gas_tester_approve_4 = hw.ext_gas_tester_approve_4;
            this.ext_work_leader_4 = hw.ext_work_leader_4;
            this.ext_work_leader_approve_4 = hw.ext_work_leader_approve_4;
            this.ext_fo_4 = hw.ext_fo_4;
            this.ext_fo_delegate_4 = hw.ext_fo_delegate_4;
            this.ext_fo_approve_4 = hw.ext_fo_approve_4;
            this.ext_new_validity_4 = hw.ext_new_validity_4;
            this.ext_remark_4 = hw.ext_remark_4;
            this.ext_datetime_5 = hw.ext_datetime_5;
            this.ext_lel_5 = hw.ext_lel_5;
            this.ext_o2_5 = hw.ext_o2_5;
            this.ext_h2s_5 = hw.ext_h2s_5;
            this.ext_co_5 = hw.ext_co_5;
            this.ext_other_5 = hw.ext_other_5;
            this.ext_gas_tester_5 = hw.ext_gas_tester_5;
            this.ext_gas_tester_approve_5 = hw.ext_gas_tester_approve_5;
            this.ext_work_leader_5 = hw.ext_work_leader_5;
            this.ext_work_leader_approve_5 = hw.ext_work_leader_approve_5;
            this.ext_fo_5 = hw.ext_fo_5;
            this.ext_fo_delegate_5 = hw.ext_fo_delegate_5;
            this.ext_fo_approve_5 = hw.ext_fo_approve_5;
            this.ext_new_validity_5 = hw.ext_new_validity_5;
            this.ext_remark_5 = hw.ext_remark_5;
            this.ext_datetime_6 = hw.ext_datetime_6;
            this.ext_lel_6 = hw.ext_lel_6;
            this.ext_o2_6 = hw.ext_o2_6;
            this.ext_h2s_6 = hw.ext_h2s_6;
            this.ext_co_6 = hw.ext_co_6;
            this.ext_other_6 = hw.ext_other_6;
            this.ext_gas_tester_6 = hw.ext_gas_tester_6;
            this.ext_gas_tester_approve_6 = hw.ext_gas_tester_approve_6;
            this.ext_work_leader_6 = hw.ext_work_leader_6;
            this.ext_work_leader_approve_6 = hw.ext_work_leader_approve_6;
            this.ext_fo_6 = hw.ext_fo_6;
            this.ext_fo_delegate_6 = hw.ext_fo_delegate_6;
            this.ext_fo_approve_6 = hw.ext_fo_approve_6;
            this.ext_new_validity_6 = hw.ext_new_validity_6;
            this.ext_remark_6 = hw.ext_remark_6;
            this.ext_datetime_7 = hw.ext_datetime_7;
            this.ext_lel_7 = hw.ext_lel_7;
            this.ext_o2_7 = hw.ext_o2_7;
            this.ext_h2s_7 = hw.ext_h2s_7;
            this.ext_co_7 = hw.ext_co_7;
            this.ext_other_7 = hw.ext_other_7;
            this.ext_gas_tester_7 = hw.ext_gas_tester_7;
            this.ext_gas_tester_approve_7 = hw.ext_gas_tester_approve_7;
            this.ext_work_leader_7 = hw.ext_work_leader_7;
            this.ext_work_leader_approve_7 = hw.ext_work_leader_approve_7;
            this.ext_fo_7 = hw.ext_fo_7;
            this.ext_fo_delegate_7 = hw.ext_fo_delegate_7;
            this.ext_fo_approve_7 = hw.ext_fo_approve_7;
            this.ext_new_validity_7 = hw.ext_new_validity_7;
            this.ext_remark_7 = hw.ext_remark_7;
            this.status = hw.status;
            this.random_pin = hw.random_pin;

            this.is_guest = hw.permit_to_work.is_guest == 1;

            this.statusText = getStatus();

            string path = HttpContext.Current.Server.MapPath("~/Upload/HotWork/" + this.id + "");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            DirectoryInfo d = new DirectoryInfo(path);//Assuming Test is your Folder
            FileInfo[] Files = d.GetFiles(); //Getting Text files

            this.listDocumentUploaded.Add("ATTACHMENT", Files.Select(p => p.Name).ToList());

            this.hira_document = new ListHira(this.id_ptw.Value,this.db).listHira;
        }

        public HwEntity(hot_work hw)
        {
            this.id = hw.id;
            this.hw_no = hw.hw_no;
            this.fire_watch = hw.fire_watch;
            this.purpose = hw.purpose;
            this.work_leader = hw.work_leader;
            this.validity_period_start = hw.validity_period_start;
            this.validity_period_end = hw.validity_period_end;
            this.id_ptw = hw.id_ptw;
            this.notes = hw.notes;
            this.methane_result = hw.methane_result;
            this.o2_result = hw.o2_result;
            this.h2s_result = hw.h2s_result;
            this.co_result = hw.co_result;
            this.other_gas = hw.other_gas;
            this.other_result = hw.other_result;
            this.acc_work_leader_approve = hw.acc_work_leader_approve;
            this.acc_supervisor_approve = hw.acc_supervisor_approve;
            this.acc_supervisor = hw.acc_supervisor;
            this.acc_supervisor_delegate = hw.acc_supervisor_delegate;
            this.acc_fire_watch = hw.acc_fire_watch;
            this.acc_fire_watch_delegate = hw.acc_fire_watch_delegate;
            this.acc_fire_watch_approve = hw.acc_fire_watch_approve;
            this.acc_fo = hw.acc_fo;
            this.acc_fo_delegate = hw.acc_fo_delegate;
            this.acc_fo_approve = hw.acc_fo_approve;
            this.acc_gas_tester = hw.acc_gas_tester;
            this.acc_gas_tester_approve = hw.acc_gas_tester_approve;
            this.acc_work_representative_approve = hw.acc_work_representative_approve;
            this.can_work_leader_approve = hw.can_work_leader_approve;
            this.can_supervisor_approve = hw.can_supervisor_approve;
            this.can_supervisor = hw.can_supervisor;
            this.can_supervisor_delegate = hw.can_supervisor_delegate;
            this.can_fire_watch = hw.can_fire_watch;
            this.can_fire_watch_delegate = hw.can_fire_watch_delegate;
            this.can_fire_watch_approve = hw.can_fire_watch_approve;
            this.can_fo = hw.can_fo;
            this.can_fo_delegate = hw.can_fo_delegate;
            this.can_fo_approve = hw.can_fo_approve;
            this.ext_datetime_1 = hw.ext_datetime_1;
            this.ext_lel_1 = hw.ext_lel_1;
            this.ext_o2_1 = hw.ext_o2_1;
            this.ext_h2s_1 = hw.ext_h2s_1;
            this.ext_co_1 = hw.ext_co_1;
            this.ext_other_1 = hw.ext_other_1;
            this.ext_gas_tester_1 = hw.ext_gas_tester_1;
            this.ext_gas_tester_approve_1 = hw.ext_gas_tester_approve_1;
            this.ext_work_leader_1 = hw.ext_work_leader_1;
            this.ext_work_leader_approve_1 = hw.ext_work_leader_approve_1;
            this.ext_fo_1 = hw.ext_fo_1;
            this.ext_fo_delegate_1 = hw.ext_fo_delegate_1;
            this.ext_fo_approve_1 = hw.ext_fo_approve_1;
            this.ext_new_validity_1 = hw.ext_new_validity_1;
            this.ext_remark_1 = hw.ext_remark_1;
            this.ext_datetime_2 = hw.ext_datetime_2;
            this.ext_lel_2 = hw.ext_lel_2;
            this.ext_o2_2 = hw.ext_o2_2;
            this.ext_h2s_2 = hw.ext_h2s_2;
            this.ext_co_2 = hw.ext_co_2;
            this.ext_other_2 = hw.ext_other_2;
            this.ext_gas_tester_2 = hw.ext_gas_tester_2;
            this.ext_gas_tester_approve_2 = hw.ext_gas_tester_approve_2;
            this.ext_work_leader_2 = hw.ext_work_leader_2;
            this.ext_work_leader_approve_2 = hw.ext_work_leader_approve_2;
            this.ext_fo_2 = hw.ext_fo_2;
            this.ext_fo_delegate_2 = hw.ext_fo_delegate_2;
            this.ext_fo_approve_2 = hw.ext_fo_approve_2;
            this.ext_new_validity_2 = hw.ext_new_validity_2;
            this.ext_remark_2 = hw.ext_remark_2;
            this.ext_datetime_3 = hw.ext_datetime_3;
            this.ext_lel_3 = hw.ext_lel_3;
            this.ext_o2_3 = hw.ext_o2_3;
            this.ext_h2s_3 = hw.ext_h2s_3;
            this.ext_co_3 = hw.ext_co_3;
            this.ext_other_3 = hw.ext_other_3;
            this.ext_gas_tester_3 = hw.ext_gas_tester_3;
            this.ext_gas_tester_approve_3 = hw.ext_gas_tester_approve_3;
            this.ext_work_leader_3 = hw.ext_work_leader_3;
            this.ext_work_leader_approve_3 = hw.ext_work_leader_approve_3;
            this.ext_fo_3 = hw.ext_fo_3;
            this.ext_fo_delegate_3 = hw.ext_fo_delegate_3;
            this.ext_fo_approve_3 = hw.ext_fo_approve_3;
            this.ext_new_validity_3 = hw.ext_new_validity_3;
            this.ext_remark_3 = hw.ext_remark_3;
            this.ext_datetime_4 = hw.ext_datetime_4;
            this.ext_lel_4 = hw.ext_lel_4;
            this.ext_o2_4 = hw.ext_o2_4;
            this.ext_h2s_4 = hw.ext_h2s_4;
            this.ext_co_4 = hw.ext_co_4;
            this.ext_other_4 = hw.ext_other_4;
            this.ext_gas_tester_4 = hw.ext_gas_tester_4;
            this.ext_gas_tester_approve_4 = hw.ext_gas_tester_approve_4;
            this.ext_work_leader_4 = hw.ext_work_leader_4;
            this.ext_work_leader_approve_4 = hw.ext_work_leader_approve_4;
            this.ext_fo_4 = hw.ext_fo_4;
            this.ext_fo_delegate_4 = hw.ext_fo_delegate_4;
            this.ext_fo_approve_4 = hw.ext_fo_approve_4;
            this.ext_new_validity_4 = hw.ext_new_validity_4;
            this.ext_remark_4 = hw.ext_remark_4;
            this.ext_datetime_5 = hw.ext_datetime_5;
            this.ext_lel_5 = hw.ext_lel_5;
            this.ext_o2_5 = hw.ext_o2_5;
            this.ext_h2s_5 = hw.ext_h2s_5;
            this.ext_co_5 = hw.ext_co_5;
            this.ext_other_5 = hw.ext_other_5;
            this.ext_gas_tester_5 = hw.ext_gas_tester_5;
            this.ext_gas_tester_approve_5 = hw.ext_gas_tester_approve_5;
            this.ext_work_leader_5 = hw.ext_work_leader_5;
            this.ext_work_leader_approve_5 = hw.ext_work_leader_approve_5;
            this.ext_fo_5 = hw.ext_fo_5;
            this.ext_fo_delegate_5 = hw.ext_fo_delegate_5;
            this.ext_fo_approve_5 = hw.ext_fo_approve_5;
            this.ext_new_validity_5 = hw.ext_new_validity_5;
            this.ext_remark_5 = hw.ext_remark_5;
            this.ext_datetime_6 = hw.ext_datetime_6;
            this.ext_lel_6 = hw.ext_lel_6;
            this.ext_o2_6 = hw.ext_o2_6;
            this.ext_h2s_6 = hw.ext_h2s_6;
            this.ext_co_6 = hw.ext_co_6;
            this.ext_other_6 = hw.ext_other_6;
            this.ext_gas_tester_6 = hw.ext_gas_tester_6;
            this.ext_gas_tester_approve_6 = hw.ext_gas_tester_approve_6;
            this.ext_work_leader_6 = hw.ext_work_leader_6;
            this.ext_work_leader_approve_6 = hw.ext_work_leader_approve_6;
            this.ext_fo_6 = hw.ext_fo_6;
            this.ext_fo_delegate_6 = hw.ext_fo_delegate_6;
            this.ext_fo_approve_6 = hw.ext_fo_approve_6;
            this.ext_new_validity_6 = hw.ext_new_validity_6;
            this.ext_remark_6 = hw.ext_remark_6;
            this.ext_datetime_7 = hw.ext_datetime_7;
            this.ext_lel_7 = hw.ext_lel_7;
            this.ext_o2_7 = hw.ext_o2_7;
            this.ext_h2s_7 = hw.ext_h2s_7;
            this.ext_co_7 = hw.ext_co_7;
            this.ext_other_7 = hw.ext_other_7;
            this.ext_gas_tester_7 = hw.ext_gas_tester_7;
            this.ext_gas_tester_approve_7 = hw.ext_gas_tester_approve_7;
            this.ext_work_leader_7 = hw.ext_work_leader_7;
            this.ext_work_leader_approve_7 = hw.ext_work_leader_approve_7;
            this.ext_fo_7 = hw.ext_fo_7;
            this.ext_fo_delegate_7 = hw.ext_fo_delegate_7;
            this.ext_fo_approve_7 = hw.ext_fo_approve_7;
            this.ext_new_validity_7 = hw.ext_new_validity_7;
            this.ext_remark_7 = hw.ext_remark_7;
            this.status = hw.status;
            this.random_pin = hw.random_pin;

            this.is_guest = hw.permit_to_work.is_guest == 1;

            this.statusText = getStatus();
        }

        public int create()
        {
            this.db = new star_energy_ptwEntities();
            generateRandomPIN();
            hot_work hw = new hot_work
            {
                hw_no = this.hw_no,
                fire_watch = this.fire_watch,
                purpose = this.purpose,
                work_leader = this.work_leader,
                id_ptw = this.id_ptw,
                screening_spv = "0#0#0#0#0#0#0#0#0",
                screening_fo = "0#0#0#0#0#0#0#0#0",
                screening_1 = "0#0#0#0#0#0#0#0#0",
                screening_2 = "0#0#0#0#0#0#0#0#0",
                screening_3 = "0#0#0#0#0#0#0#0#0",
                screening_4 = "0#0#0#0#0#0#0#0#0",
                screening_5 = "0#0#0#0#0#0#0#0#0",
                screening_6 = "0#0#0#0#0#0#0#0#0",
                screening_7 = "0#0#0#0#0#0#0#0#0",
                status = (int)statusHW.CREATE,
                random_pin = this.random_pin,
                acc_supervisor = this.acc_supervisor,
                acc_supervisor_delegate = this.acc_supervisor_delegate,
                acc_fo = this.acc_fo,
                can_fo = this.can_fo
            };

            db.hot_work.Add(hw);
            int retVal = this.db.SaveChanges();
            this.id = hw.id;

            string path = HttpContext.Current.Server.MapPath("~/Upload/HotWork/" + this.id);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return retVal;
        }

        public int delete()
        {
            this.db = new star_energy_ptwEntities();
            hot_work hw = this.db.hot_work.Find(this.id);
            this.db.hot_work.Remove(hw);
            return this.db.SaveChanges();
        }

        public int edit()
        {
            hot_work hw = db.hot_work.Find(id);
            hw.fire_watch = this.fire_watch;
            hw.purpose = this.purpose;
            hw.validity_period_start = this.validity_period_start;
            hw.validity_period_end = this.validity_period_end;
            hw.screening_spv = this.screening_spv_db;
            hw.screening_fo = this.screening_fo_db;
            hw.screening_1 = this.screening_1_db;
            hw.screening_2 = this.screening_2_db;
            hw.screening_3 = this.screening_3_db;
            hw.screening_4 = this.screening_4_db;
            hw.screening_5 = this.screening_5_db;
            hw.screening_6 = this.screening_6_db;
            hw.screening_7 = this.screening_7_db;
            hw.notes = this.notes;
            hw.methane_result = this.methane_result;
            hw.o2_result = this.o2_result;
            hw.h2s_result = this.h2s_result;
            hw.co_result = this.co_result;
            hw.other_gas = this.other_gas;
            hw.other_result = this.other_result;

            this.db.Entry(hw).State = System.Data.EntityState.Modified;
            return this.db.SaveChanges();
        }

        #region generate hot work number

        public void generateNumber(string ptw_no)
        {
            string result = "HW-" + ptw_no;

            this.hw_no = result;
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

        public bool isAccSupervisor(UserEntity user, ListUser listUser = null)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            int foId = 0;
            Int32.TryParse(this.acc_supervisor, out foId);
            UserEntity fo = listUser == null ? new UserEntity(foId, user.token, user) : listUser.listUser.Find(p => p.id == foId);
            if ((this.acc_supervisor == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFireWatch(UserEntity user, ListUser listUser = null)
        {
            var retVal = false;
            string user_id = user.id.ToString();
            int foId = 0;
            Int32.TryParse(this.acc_fire_watch, out foId);
            UserEntity fo = listUser == null ? new UserEntity(foId, user.token, user) : listUser.listUser.Find(p => p.id == foId);
            if ((this.acc_fire_watch == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
            {
                retVal = true;
            }

            return retVal;
        }

        public bool isAccFO(UserEntity user, ListUser listUser = null)
        {
            var retVal = false;
            int foId = 0;
            Int32.TryParse(this.acc_fo, out foId);
            UserEntity fo = listUser == null ? new UserEntity(foId, user.token, user) : listUser.listUser.Find(p => p.id == foId);
            string user_id = user.id.ToString();
            List<UserEntity> listDel = fo.GetDelegateFO(user);
            if ((this.acc_fo == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
            {
                retVal = true;
            }
            else if (listDel.Exists(p => p.id == user.id))
            {
                return true;
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
            int foId = 0;
            UserEntity fo = null;
            List<UserEntity> listDel;
            switch (extension)
            {
                case 1:
                    Int32.TryParse(this.ext_fo_1, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_1 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 2:
                    Int32.TryParse(this.ext_fo_2, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_2 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 3:
                    Int32.TryParse(this.ext_fo_3, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_3 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 4:
                    Int32.TryParse(this.ext_fo_4, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_4 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 5:
                    Int32.TryParse(this.ext_fo_5, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_5 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 6:
                    Int32.TryParse(this.ext_fo_6, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_6 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
                case 7:
                    Int32.TryParse(this.ext_fo_7, out foId);
                    fo = new UserEntity(foId, user.token, user);
                    listDel = fo.GetDelegateFO(user);
                    if ((this.ext_fo_7 == user_id || (fo.employee_delegate != null && fo.employee_delegate.ToString() == user_id)))
                    {
                        retVal = true;
                    }
                    else if (listDel.Exists(p => p.id == user.id))
                    {
                        return true;
                    }
                    break;
            }

            return retVal;
        }

        public bool isCanEdit(UserEntity user)
        {
            bool isCanEdit = false;
            if (this.ptw.is_guest == 1)
            {
                if (isAccSupervisor(user) && this.status < (int)statusHW.ACCWORKLEADER)
                {
                    isCanEdit = true;
                }
            }
            else
            {
                if (isWorkLeader(user) && this.status < (int)statusHW.ACCWORKLEADER)
                {
                    isCanEdit = true;
                }
            }

            if (isAccSupervisor(user) && (this.status == (int)statusHW.CREATE || this.status == (int)statusHW.ACCWORKLEADER))
            {
                isCanEdit = true;
            }

            //if (isAccFireWatch(user) && this.status == (int)statusHW.ACCSPV)
            //{
            //    isCanEdit = true;
            //}

            if (isAccFO(user) && this.status == (int)statusHW.ACCSPV || this.status == (int)statusHW.SPVSCREENING)
            {
                isCanEdit = true;
            }

            if ((isAccGasTester(user) || isAccFO(user)) && this.status == (int)statusHW.FOSCREENING)
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
            if (this.ptw.is_guest == 1)
            {
                if (isAccSupervisor(user))
                {
                    switch (extension)
                    {
                        case 1:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER1 && this.status >= (int)statusHW.EXTCREATE1)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 2:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER2 && this.status >= (int)statusHW.EXTCREATE2)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 3:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER3 && this.status >= (int)statusHW.EXTCREATE3)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 4:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER4 && this.status >= (int)statusHW.EXTCREATE4)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 5:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER5 && this.status >= (int)statusHW.EXTCREATE5)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 6:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER6 && this.status >= (int)statusHW.EXTCREATE6)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 7:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER7 && this.status >= (int)statusHW.EXTCREATE7)
                            {
                                isCanEdit = true;
                            }
                            break;
                    }

                }
            }
            else
            {
                if (isWorkLeader(user))
                {
                    switch (extension)
                    {
                        case 1:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER1 && this.status >= (int)statusHW.EXTCREATE1)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 2:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER2 && this.status >= (int)statusHW.EXTCREATE2)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 3:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER3 && this.status >= (int)statusHW.EXTCREATE3)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 4:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER4 && this.status >= (int)statusHW.EXTCREATE4)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 5:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER5 && this.status >= (int)statusHW.EXTCREATE5)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 6:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER6 && this.status >= (int)statusHW.EXTCREATE6)
                            {
                                isCanEdit = true;
                            }
                            break;
                        case 7:
                            if (this.status < (int)statusHW.EXTACCWORKLEADER7 && this.status >= (int)statusHW.EXTCREATE7)
                            {
                                isCanEdit = true;
                            }
                            break;
                    }

                }
            }

            if (isExtFO(user,extension))
            {
                switch (extension)
                {
                    case 1:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER1 || this.status == (int)statusHW.EXTCREATE1)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 2:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER2 || this.status == (int)statusHW.EXTCREATE2)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 3:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER3 || this.status == (int)statusHW.EXTCREATE3)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 4:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER4 || this.status == (int)statusHW.EXTCREATE4)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 5:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER5 || this.status == (int)statusHW.EXTCREATE5)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 6:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER6 || this.status == (int)statusHW.EXTCREATE6)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 7:
                        if (this.status == (int)statusHW.EXTACCWORKLEADER7 || this.status == (int)statusHW.EXTCREATE7)
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
                        if (this.status == (int)statusHW.EXTFOSCREENING1)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 2:
                        if (this.status == (int)statusHW.EXTFOSCREENING2)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 3:
                        if (this.status == (int)statusHW.EXTFOSCREENING3)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 4:
                        if (this.status == (int)statusHW.EXTFOSCREENING4)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 5:
                        if (this.status == (int)statusHW.EXTFOSCREENING5)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 6:
                        if (this.status == (int)statusHW.EXTFOSCREENING6)
                        {
                            isCanEdit = true;
                        }
                        break;
                    case 7:
                        if (this.status == (int)statusHW.EXTFOSCREENING7)
                        {
                            isCanEdit = true;
                        }
                        break;
                }
            }

            return isCanEdit;
        }

        public bool isCloseHW()
        {
            return this.status >= (int)statusHW.CANCEL;
        }

        public int assignSupervisor(UserEntity supervisor)
        {
            this.acc_supervisor = supervisor.id.ToString();
            this.can_supervisor = supervisor.id.ToString();
            this.acc_supervisor_delegate = supervisor.employee_delegate.ToString();
            this.can_supervisor_delegate = supervisor.employee_delegate.ToString();

            hot_work hw = this.db.hot_work.Find(this.id);
            hw.acc_supervisor = this.acc_supervisor;
            hw.can_supervisor = this.can_supervisor;
            hw.acc_supervisor_delegate = this.acc_supervisor_delegate;
            hw.can_supervisor_delegate = this.can_supervisor_delegate;

            this.db.Entry(hw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignFireWatch(UserEntity assessor)
        {
            this.acc_fire_watch = assessor.id.ToString();
            this.can_fire_watch = assessor.id.ToString();
            this.acc_fire_watch_delegate = assessor.employee_delegate.ToString();
            this.can_fire_watch_delegate = assessor.employee_delegate.ToString();

            hot_work hw = this.db.hot_work.Find(this.id);
            hw.acc_fire_watch = this.acc_fire_watch;
            hw.can_fire_watch = this.can_fire_watch;
            hw.acc_fire_watch_delegate = this.acc_fire_watch_delegate;
            hw.can_fire_watch_delegate = this.can_fire_watch_delegate;

            this.db.Entry(hw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignGasTester(UserEntity gasTester)
        {
            this.acc_gas_tester = gasTester.id.ToString();

            hot_work hw = this.db.hot_work.Find(this.id);
            hw.acc_gas_tester = this.acc_gas_tester;

            this.db.Entry(hw).State = EntityState.Modified;

            return this.db.SaveChanges();
        }

        public int assignExtGasTester(UserEntity gasTester, int status)
        {
            hot_work hw = this.db.hot_work.Find(this.id);
            switch (status)
            {
                case (int)statusHW.EXTFOSCREENING1 :
                    this.ext_gas_tester_1 = gasTester.id.ToString();
                    hw.ext_gas_tester_1 = this.ext_gas_tester_1;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING2:
                    this.ext_gas_tester_2 = gasTester.id.ToString();
                    hw.ext_gas_tester_2 = this.ext_gas_tester_2;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING3:
                    this.ext_gas_tester_3 = gasTester.id.ToString();
                    hw.ext_gas_tester_3 = this.ext_gas_tester_3;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING4:
                    this.ext_gas_tester_4 = gasTester.id.ToString();
                    hw.ext_gas_tester_4 = this.ext_gas_tester_4;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING5:
                    this.ext_gas_tester_5 = gasTester.id.ToString();
                    hw.ext_gas_tester_5 = this.ext_gas_tester_5;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING6:
                    this.ext_gas_tester_6 = gasTester.id.ToString();
                    hw.ext_gas_tester_6 = this.ext_gas_tester_6;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
                case (int)statusHW.EXTFOSCREENING7:
                    this.ext_gas_tester_7 = gasTester.id.ToString();
                    hw.ext_gas_tester_7 = this.ext_gas_tester_7;

                    this.db.Entry(hw).State = EntityState.Modified;
                    break;
            }

            return this.db.SaveChanges();
        }

        public int assignExtWorkLeader(int status)
        {
            if (status == (int)statusHW.EXTCREATE1)
            {
                this.ext_work_leader_1 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_1 = this.ext_work_leader_1;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE2)
            {
                this.ext_work_leader_2 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_2 = this.ext_work_leader_2;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE3)
            {
                this.ext_work_leader_3 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_3 = this.ext_work_leader_3;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE4)
            {
                this.ext_work_leader_4 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_4 = this.ext_work_leader_4;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE5)
            {
                this.ext_work_leader_5 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_5 = this.ext_work_leader_5;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE6)
            {
                this.ext_work_leader_6 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_6 = this.ext_work_leader_6;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE7)
            {
                this.ext_work_leader_7 = this.work_leader;

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_7 = this.ext_work_leader_7;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }

            return 0;
        }

        public bool isExistFO(UserEntity fo, int status)
        {
            if (status == (int)statusHW.SPVSCREENING)
            {
                return this.acc_fo != null;
            }
            else if (status == (int)statusHW.EXTCREATE1)
            {
                return this.ext_fo_1 != null;
            }
            else if (status == (int)statusHW.EXTCREATE2)
            {
                return this.ext_fo_2 != null;
            }
            else if (status == (int)statusHW.EXTCREATE3)
            {
                return this.ext_fo_3 != null;
            }
            else if (status == (int)statusHW.EXTCREATE4)
            {
                return this.ext_fo_4 != null;
            }
            else if (status == (int)statusHW.EXTCREATE5)
            {
                return this.ext_fo_5 != null;
            }
            else if (status == (int)statusHW.EXTCREATE6)
            {
                return this.ext_fo_6 != null;
            }
            else if (status == (int)statusHW.EXTCREATE7)
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
            if (status == (int)statusHW.SPVSCREENING)
            {
                this.acc_fo = fo.id.ToString();
                this.acc_fo_delegate = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.acc_fo = this.acc_fo;
                hw.acc_fo_delegate = this.acc_fo_delegate;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE1)
            {
                this.ext_fo_1 = fo.id.ToString();
                this.ext_fo_delegate_1 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_1 = this.ext_fo_1;
                hw.ext_fo_delegate_1 = this.ext_fo_delegate_1;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE2)
            {
                this.ext_fo_2 = fo.id.ToString();
                this.ext_fo_delegate_2 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_2 = this.ext_fo_2;
                hw.ext_fo_delegate_2 = this.ext_fo_delegate_2;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE3)
            {
                this.ext_fo_3 = fo.id.ToString();
                this.ext_fo_delegate_3 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_3 = this.ext_fo_3;
                hw.ext_fo_delegate_3 = this.ext_fo_delegate_3;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE4)
            {
                this.ext_fo_4 = fo.id.ToString();
                this.ext_fo_delegate_4 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_4 = this.ext_fo_4;
                hw.ext_fo_delegate_4 = this.ext_fo_delegate_4;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE5)
            {
                this.ext_fo_5 = fo.id.ToString();
                this.ext_fo_delegate_5 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_5 = this.ext_fo_5;
                hw.ext_fo_delegate_5 = this.ext_fo_delegate_5;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE6)
            {
                this.ext_fo_6 = fo.id.ToString();
                this.ext_fo_delegate_6 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_6 = this.ext_fo_6;
                hw.ext_fo_delegate_6 = this.ext_fo_delegate_6;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else if (status == (int)statusHW.EXTCREATE7)
            {
                this.ext_fo_7 = fo.id.ToString();
                this.ext_fo_delegate_7 = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_fo_7 = this.ext_fo_7;
                hw.ext_fo_delegate_7 = this.ext_fo_delegate_7;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
            else
            {
                this.can_fo = fo.id.ToString();
                this.can_fo_delegate = fo.employee_delegate.ToString();

                hot_work hw = this.db.hot_work.Find(this.id);
                hw.can_fo = this.can_fo;
                hw.can_fo_delegate = this.can_fo_delegate;

                this.db.Entry(hw).State = EntityState.Modified;

                return this.db.SaveChanges();
            }
        }

        public bool isUserInHw(UserEntity user, ListUser listUser) {
            return (isAccGasTester(user)
                || isExtGasTester(user, 1) || isExtGasTester(user, 2) || isExtGasTester(user, 3) || isExtGasTester(user, 4)
                || isExtGasTester(user, 5) || isExtGasTester(user, 6) || isExtGasTester(user, 7));
        }

        #endregion

        #region retVal

        public int setStatus(int status)
        {
            this.status = status;

            hot_work hw = this.db.hot_work.Find(this.id);
            hw.status = status;
            this.db.Entry(hw).State = EntityState.Modified;
            this.db.SaveChanges();

            return hw.status.Value;
        }

        public string getStatus()
        {
            string retVal = "";
            switch (this.status)
            {
                case (int)statusHW.CREATE: retVal = "Waiting for Supervisor Screening"; break;
                case (int)statusHW.SPVSCREENING: retVal = "Waiting for Facility Owner Screening"; break;
                case (int)statusHW.FOSCREENING: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.GASTESTER: retVal = "Waiting for Approval by Work Leader / PTW Requestor"; break;
                case (int)statusHW.ACCWORKLEADER: retVal = "Waiting for Approval by Supervisor"; break;
                case (int)statusHW.ACCSPV: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.ACCFO: retVal = "Completed. Hot Work Permit has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE1: retVal = "Hot Work extended number 1. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING1: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER1: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER1: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO1: retVal = "Hot Work extension number 1 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE2: retVal = "Hot Work extended number 2. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING2: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER2: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER2: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO2: retVal = "Hot Work extension number 2 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE3: retVal = "Hot Work extended number 3. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING3: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER3: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER3: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO3: retVal = "Hot Work extension number 3 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE4: retVal = "Hot Work extended number 4. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING4: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER4: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER4: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO4: retVal = "Hot Work extension number 4 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE5: retVal = "Hot Work extended number 5. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING5: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER5: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER5: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO5: retVal = "Hot Work extension number 5 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE6: retVal = "Hot Work extended number 6. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING6: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER6: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER6: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO6: retVal = "Hot Work extension number 6 has been approved by Facility Owner"; break;
                case (int)statusHW.EXTCREATE7: retVal = "Hot Work extended number 7. Waiting for Facility Owner Screening"; break;
                case (int)statusHW.EXTFOSCREENING7: retVal = "Waiting for Gas Testing by Gas Tester"; break;
                case (int)statusHW.EXTGASTESTER7: retVal = "Waiting for Approval by Work Leader"; break;
                case (int)statusHW.EXTACCWORKLEADER7: retVal = "Waiting for Approval by Facility Owner"; break;
                case (int)statusHW.EXTACCFO7: retVal = "Hot Work extension number 7 has been approved by Facility Owner"; break;
                case (int)statusHW.CANCEL: retVal = "Hot Work Permit has been Closed. Waiting for Approval by Work Leader"; break;
                case (int)statusHW.CANWORKLEADER: retVal = "Hot Work Permit has been Closed. Waiting for Approval by Supervisor"; break;
                case (int)statusHW.CANSPV: retVal = "Waiting for Closing Approval by Facility Owner"; break;
                case (int)statusHW.CANFO: retVal = "Closed. Hot Work Permit has been approved to closing by Facility Owner"; break;
            };

            return retVal;
        }

        #endregion

        public string closeHw(UserEntity user)
        {
            if ((!is_guest ||user.id.ToString() != this.acc_supervisor) && user.id.ToString() != this.work_leader)
            {
                return "400";
            }
            else
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                this.status = (int)statusHW.CANCEL;
                hw.can_supervisor = hw.acc_supervisor;

                hw.status = this.status;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                requestorCan(user);

                return "200";
            }
        }

        public string getHiraNo()
        {
            this.hira_no = "";
            if (this.ptw.hira_docs != null)
            {
                string[] s = this.ptw.hira_docs.Split(new string[] { "#@#" }, StringSplitOptions.None);
                foreach (string ss in s)
                {
                    if (!String.IsNullOrEmpty(ss))
                    {
                        string name = ss.Split('/').Last();
                        string fileName = name.Substring(0, name.Length - 4);
                        fileName = HttpUtility.UrlDecode(fileName);
                        this.hira_no += ", " + fileName;
                    }
                }
            }

            if (this.hira_no.Length == 0)
            {
                return this.hira_no;
            }
            else
            {
                this.hira_no = this.hira_no.Substring(2);
                return this.hira_no;
            }
        }

        public string generateRandomPIN()
        {
            Random rnd = new Random();
            int card = rnd.Next(1001,9999);

            this.random_pin = card.ToString().PadLeft(4, '0');
            return this.random_pin;
        }

        #region send email

        public string sendEmailFO(List<UserEntity> listFO, string serverUrl, string token, UserEntity user, int? ext = null)
        {
            if (ext == null) {
                
                string salt = "susahbangetmencarisaltyangpalingbaikdanbenar";
                string val = "emailfo";
                SendEmail sendEmail = new SendEmail();
                foreach (UserEntity fo in listFO)
                {
                    string timestamp = DateTime.UtcNow.Ticks.ToString();
                    List<string> s = new List<string>();
#if (!DEBUG)
                    s.Add(fo.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
                    if (fo.employee_delegate != null)
                    {
                        UserEntity del = new UserEntity(fo.employee_delegate.Value, token, user);
#if (!DEBUG)
                        s.Add(del.email);
#else
                        s.Add("septu.jamasoka@gmail.com");
#endif
                    }

                    string encodedValue = this.status + salt + fo.id + val + this.id;
                    string encodedElement = Base64.Base64Encode(encodedValue);

                    string seal = Base64.MD5Seal(timestamp + salt + val);

                    string message = serverUrl + "Hw/SetFacilityOwner?a=" + timestamp + "&b=" + seal + "&c=" + encodedElement;

                    sendEmail.Send(s, message, "hot work Facility Owner");
                }
            }

            return "200";
        }

        public string sendEmailRandomPIN(string serverUrl, string token, UserEntity user)
        {
            UserEntity requestor = is_guest ? new UserEntity(Int32.Parse(this.acc_supervisor), token, user) : new UserEntity(Int32.Parse(this.work_leader), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
#if (!DEBUG)
            s.Add(requestor.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif

            string message = this.random_pin;
            string subject = "PIN for Approving as Requestor";

            sendEmail.Send(s, message, subject);
            return "200";
        }

        public string sendEmailGasTester(string serverUrl, string token, UserEntity user, int extension)
        {
            if (extension == 0) {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.acc_gas_tester), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);
                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 1)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_1), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 2)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_2), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 3)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_3), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 4)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_4), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 5)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_5), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 6)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_6), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (extension == 7)
            {
                UserEntity gasTester = new UserEntity(Int32.Parse(this.ext_gas_tester_7), token, user);
                SendEmail sendEmail = new SendEmail();
                List<string> s = new List<string>();
                List<int> userId = new List<int>();
#if (!DEBUG)
                s.Add(gasTester.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(gasTester.id);

                string message = serverUrl + "Home?p=Hw/edit/" + this.id;

                sendEmail.Send(s, message, "Assigned as Hot Work (" + this.hw_no + ") Gas Tester");
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please gas test and submit the result for Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }

            return "200";
        }

        public string sendEmailRequestor(string serverUrl, string token, UserEntity user, int extension, int stat = 0, string comment = null)
        {
            //if (extension == 0)
            //{

            UserEntity requestor = is_guest ? new UserEntity(Int32.Parse(this.acc_supervisor), token, user) : new UserEntity(Int32.Parse(this.work_leader), token, user);
            UserEntity spv = new UserEntity(Int32.Parse(this.acc_supervisor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
            //s.Add(requestor.email);
            //s.Add(spv.email);
#if (!DEBUG)
            if (!is_guest) {
                s.Add(requestor.email);
                userId.Add(requestor.id);
            }
            s.Add(spv.email);
            userId.Add(spv.id);
#else
            s.Add("septu.jamasoka@gmail.com");
            userId.Add(requestor.id);
#endif

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id;
                subject = "Hot Work Requestor Approve";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Hot Work Approval Rejection";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit No. " + this.hw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Hw/edit/" + this.id);
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
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(supervisor.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(supervisor.id);

            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id;
                subject = "Hot Work Supervisor Approve";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Hot Work Approval Rejection";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit No. " + this.hw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Hw/edit/" + this.id);
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailSupervisorScreening(string serverUrl, string token, UserEntity user)
        {
            UserEntity supervisor = new UserEntity(Int32.Parse(this.acc_supervisor), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(supervisor.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(supervisor.id);

            if (supervisor.employee_delegate != null)
            {
                UserEntity del = new UserEntity(supervisor.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }

            string message = "";
            string subject = "";
            message = serverUrl + "Home?p=Hw/edit/" + this.id;
            subject = "Hot Work Supervisor Screening";
            sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Screening Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFOScreening(string serverUrl, string token, UserEntity user)
        {
            UserEntity fOAcc = new UserEntity(Int32.Parse(this.acc_fo), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(fOAcc.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(fOAcc.id);

            if (fOAcc.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOAcc.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }
            List<UserEntity> listDel = fOAcc.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
#if (!DEBUG)
                s.Add(u.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(u.id);
            }

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;

            sendEmail.Send(s, message, "Hot Work Facility Owner Screening");
            sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Screening Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);

            return "200";
        }

        public string sendEmailFireWatch(string serverUrl, string token, UserEntity user, int stat = 0, string comment = null)
        {
            UserEntity fireWatch = new UserEntity(Int32.Parse(this.acc_fire_watch), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(fireWatch.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(fireWatch.id);

            if (fireWatch.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fireWatch.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }

            string message = "";
            string subject = "";
            if (stat == 0)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id;
                subject = "Hot Work Fire Watch Approve Approve";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);
            }
            else if (stat == 1)
            {
                message = serverUrl + "Home?p=Hw/edit/" + this.id + "<br />" + comment;
                subject = "Hot Work Approval Rejection";
                sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Hot Work Permit No. " + this.hw_no + "is rejected with comment: " + comment, serverUrl + "Home?p=Hw/edit/" + this.id);
            }

            sendEmail.Send(s, message, subject);

            return "200";
        }

        public string sendEmailFOAcc(string serverUrl, string token, UserEntity user)
        {
            UserEntity fOAcc = new UserEntity(Int32.Parse(this.acc_fo), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(fOAcc.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(fOAcc.id);

            if (fOAcc.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOAcc.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }
            List<UserEntity> listDel = fOAcc.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
#if (!DEBUG)
                s.Add(u.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(u.id);
            }

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;

            sendEmail.Send(s, message, "Hot Work Facility Owner Approve");
            sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);

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
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(fOAcc.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(fOAcc.id);

            if (fOAcc.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOAcc.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }
            List<UserEntity> listDel = fOAcc.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
#if (!DEBUG)
                s.Add(u.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(u.id);
            }

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;

            sendEmail.Send(s, message, "Hot Work Facility Owner Approve");
            sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Extension of Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);

            return "200";
        }

        public string sendEmailFOCan(string serverUrl, string token, UserEntity user)
        {
            UserEntity fOCan = new UserEntity(Int32.Parse(this.can_fo), token, user);
            SendEmail sendEmail = new SendEmail();
            List<string> s = new List<string>();
            List<int> userId = new List<int>();
#if (!DEBUG)
            s.Add(fOCan.email);
#else
            s.Add("septu.jamasoka@gmail.com");
#endif
            userId.Add(fOCan.id);

            if (fOCan.employee_delegate != null)
            {
                UserEntity del = new UserEntity(fOCan.employee_delegate.Value, token, user);
#if (!DEBUG)
                s.Add(del.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(del.id);
            }
            List<UserEntity> listDel = fOCan.GetDelegateFO(user);
            foreach (UserEntity u in listDel)
            {
#if (!DEBUG)
                s.Add(u.email);
#else
                s.Add("septu.jamasoka@gmail.com");
#endif
                userId.Add(u.id);
            }

            string message = serverUrl + "Home?p=Hw/edit/" + this.id;

            sendEmail.Send(s, message, "hot work Facility Owner Closing");
            sendEmail.SendToNotificationCenter(userId, "Hot Work Permit", "Please Approve Cancellation of Hot Work Permit No. " + this.hw_no, serverUrl + "Home?p=Hw/edit/" + this.id);

            return "200";
        }

        #endregion

        #region approve reject

        public string gasTesterAcc(UserEntity user, int extension, UserEntity userLogin)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            hot_work hw = this.db.hot_work.Find(this.id);
            int foId = 0;
            Int32.TryParse(this.acc_fo, out foId);
            UserEntity fo = new UserEntity(foId, userLogin.token, userLogin);
            List<UserEntity> listDel = fo.GetDelegateFO(userLogin);
            if (extension == 0 && (user.id.ToString() == this.acc_gas_tester || user.id.ToString() == this.acc_fo || listDel.Exists(p => p.id == user.id)))
            {
                hw.acc_gas_tester_approve = "a" + user.signature;
                hw.status = (int)statusHW.GASTESTER;
            }
            else if (extension == 1 && user.id.ToString() == this.ext_gas_tester_1 || user.id.ToString() == this.ext_fo_1 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_1 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER1;
            }
            else if (extension == 2 && user.id.ToString() == this.ext_gas_tester_2 || user.id.ToString() == this.ext_fo_2 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_2 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER2;
            }
            else if (extension == 3 && user.id.ToString() == this.ext_gas_tester_3 || user.id.ToString() == this.ext_fo_3 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_3 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER3;
            }
            else if (extension == 4 && user.id.ToString() == this.ext_gas_tester_4 || user.id.ToString() == this.ext_fo_4 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_4 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER4;
            }
            else if (extension == 5 && user.id.ToString() == this.ext_gas_tester_5 || user.id.ToString() == this.ext_fo_5 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_5 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER5;
            }
            else if (extension == 6 && user.id.ToString() == this.ext_gas_tester_6 || user.id.ToString() == this.ext_fo_6 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_6 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER6;
            }
            else if (extension == 7 && user.id.ToString() == this.ext_gas_tester_7 || user.id.ToString() == this.ext_fo_7 || listDel.Exists(p => p.id == user.id))
            {
                hw.ext_gas_tester_approve_7 = "a" + user.signature;
                hw.status = (int)statusHW.EXTGASTESTER7;
            }
            else
            {
                return "400";
            }

            this.db.Entry(hw).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        public string requestorAcc(UserEntity user, string token, int extension, string random_pin = null)
        {
            // requestor approval
            // return code - 200 {ok}
            //               400 {not the user}
            hot_work hw = this.db.hot_work.Find(this.id);
            if (extension == 0 && user.id.ToString() == this.work_leader)
            {
                hw.acc_work_leader_approve = "a" + user.signature;
                hw.status = (int)statusHW.ACCWORKLEADER;
            }
            else if (is_guest && extension == 0 && user.id.ToString() == this.acc_supervisor)
            {
                hw.acc_work_leader_approve = hw.permit_to_work.acc_ptw_requestor_approve;
                hw.status = (int)statusHW.ACCWORKLEADER;
            } else if (extension == 0 && random_pin != null && user.id.ToString() == this.acc_supervisor)
            {
                if (random_pin == this.random_pin)
                {
                    if (is_guest)
                    {
                        hw.acc_work_leader_approve = hw.permit_to_work.acc_ptw_requestor_approve;
                    }
                    else
                    {
                        user = new UserEntity(Int32.Parse(this.work_leader), token, user);
                        hw.acc_work_leader_approve = "a" + user.signature;
                    }
                    hw.status = (int)statusHW.ACCWORKLEADER;
                }
                else
                {
                    return "402";
                }
            }
            else if (extension == 1)
            {
                if (user.id.ToString() == this.ext_work_leader_1)
                {
                    hw.ext_work_leader_approve_1 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER1;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_1 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER1;
                }
            }
            else if (extension == 2)
            {
                if (user.id.ToString() == this.ext_work_leader_2)
                {
                    hw.ext_work_leader_approve_2 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER2;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_2 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER2;
                }
            }
            else if (extension == 3)
            {
                if (user.id.ToString() == this.ext_work_leader_3)
                {
                    hw.ext_work_leader_approve_3 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER3;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_3 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER3;
                }
            }
            else if (extension == 4)
            {
                if (user.id.ToString() == this.ext_work_leader_4)
                {
                    hw.ext_work_leader_approve_4 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER4;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_4 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER4;
                }
            }
            else if (extension == 5)
            {
                if (user.id.ToString() == this.ext_work_leader_5)
                {
                    hw.ext_work_leader_approve_5 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER5;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_5 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER5;
                }
            }
            else if (extension == 6)
            {
                if (user.id.ToString() == this.ext_work_leader_6)
                {
                    hw.ext_work_leader_approve_6 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER6;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_6 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER6;
                }
            }
            else if (extension == 7)
            {
                if (user.id.ToString() == this.ext_work_leader_7)
                {
                    hw.ext_work_leader_approve_7 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCWORKLEADER7;
                }
                else if (is_guest)
                {
                    hw.ext_work_leader_approve_7 = hw.permit_to_work.acc_ptw_requestor_approve;
                    hw.status = (int)statusHW.EXTACCWORKLEADER7;
                }
            }
            else
            {
                return "400";
            }

            this.db.Entry(hw).State = EntityState.Modified;
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

            hot_work hw = this.db.hot_work.Find(this.id);
            //if (hw.acc_fire_watch == null)
            //{
            //    return "401";
            //}

            int foId = 0;
            Int32.TryParse(this.acc_supervisor, out foId);
            UserEntity fo = new UserEntity(foId, user.token, user);
            if (user.id.ToString() == this.acc_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                hw.acc_supervisor_approve = "a" + user.signature;
                hw.status = (int)statusHW.ACCSPV;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == fo.employee_delegate.ToString())
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                hw.acc_supervisor_approve = "d" + user.signature;
                hw.acc_supervisor_delegate = user.id.ToString();
                hw.status = (int)statusHW.ACCSPV;
                this.db.Entry(hw).State = EntityState.Modified;
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

            int foId = 0;
            Int32.TryParse(this.acc_supervisor, out foId);
            UserEntity fo = new UserEntity(foId, user.token, user);
            if (user.id.ToString() == this.acc_supervisor || user.id.ToString() == fo.employee_delegate.ToString())
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.acc_work_leader_approve = null;
                hw.status = (int)statusHW.GASTESTER;
                this.db.Entry(hw).State = EntityState.Modified;
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

            hot_work hw = this.db.hot_work.Find(this.id);

            if (user.id.ToString() == this.acc_fire_watch)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                hw.acc_fire_watch_approve = "a" + user.signature;
                //hw.status = (int)statusHW.ACCFIREWATCH;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.acc_fire_watch_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                hw.acc_fire_watch_approve = "d" + user.signature;
                //hw.status = (int)statusHW.ACCFIREWATCH;
                this.db.Entry(hw).State = EntityState.Modified;
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
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.acc_supervisor_approve = null;
                //hw.status = (int)statusHW.ACCWORKLEADER;
                this.db.Entry(hw).State = EntityState.Modified;
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

            hot_work hw = this.db.hot_work.Find(this.id);

            if (extension == 0)
            {
                if (user.id.ToString() == this.acc_fo)
                {
                    hw.acc_fo_approve = "a" + user.signature;
                    hw.status = (int)statusHW.ACCFO;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    
                    return "200";
                }
                else
                {
                    hw.acc_fo_approve = "d" + user.signature;
                    hw.acc_fo_delegate = user.id.ToString();
                    hw.status = (int)statusHW.ACCFO;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();

                    return "201";
                }
            }
            else if (extension == 1)
            {
                if (user.id.ToString() == this.ext_fo_1)
                {
                    hw.ext_fo_approve_1 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO1;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_1 = "d" + user.signature;
                    hw.ext_fo_delegate_1 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO1;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 2)
            {
                if (user.id.ToString() == this.ext_fo_2)
                {
                    hw.ext_fo_approve_2 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO2;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_2 = "d" + user.signature;
                    hw.ext_fo_delegate_2 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO2;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 3)
            {
                if (user.id.ToString() == this.ext_fo_3)
                {
                    hw.ext_fo_approve_3 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO3;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_3 = "d" + user.signature;
                    hw.ext_fo_delegate_3 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO3;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 4)
            {
                if (user.id.ToString() == this.ext_fo_4)
                {
                    hw.ext_fo_approve_4 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO4;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_4 = "d" + user.signature;
                    hw.ext_fo_delegate_4 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO4;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 5)
            {
                if (user.id.ToString() == this.ext_fo_5)
                {
                    hw.ext_fo_approve_5 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO5;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_5 = "d" + user.signature;
                    hw.ext_fo_delegate_5 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO5;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 6)
            {
                if (user.id.ToString() == this.ext_fo_6)
                {
                    hw.ext_fo_approve_6 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO6;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_6 = "d" + user.signature;
                    hw.ext_fo_delegate_6 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO6;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
                }
            }
            else if (extension == 7)
            {
                if (user.id.ToString() == this.ext_fo_7)
                {
                    hw.ext_fo_approve_7 = "a" + user.signature;
                    hw.status = (int)statusHW.EXTACCFO7;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "200";
                }
                else
                {
                    hw.ext_fo_approve_7 = "d" + user.signature;
                    hw.ext_fo_delegate_7 = user.id.ToString();
                    hw.status = (int)statusHW.EXTACCFO7;
                    this.db.Entry(hw).State = EntityState.Modified;
                    this.db.SaveChanges();
                    return "209";
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
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.acc_supervisor_approve = null;
                hw.status = (int)statusHW.ACCWORKLEADER;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 1)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_1 = null;
                hw.status = (int)statusHW.EXTGASTESTER1;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 2)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_2 = null;
                hw.status = (int)statusHW.EXTGASTESTER2;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 3)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_3 = null;
                hw.status = (int)statusHW.EXTGASTESTER3;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 4)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_4 = null;
                hw.status = (int)statusHW.EXTGASTESTER4;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 5)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_5 = null;
                hw.status = (int)statusHW.EXTGASTESTER5;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 6)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_6 = null;
                hw.status = (int)statusHW.EXTGASTESTER6;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (extension == 7)
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.ext_work_leader_approve_7 = null;
                hw.status = (int)statusHW.EXTGASTESTER7;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
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
            hot_work hw = this.db.hot_work.Find(this.id);
            if (is_guest && user.id.ToString() == this.acc_supervisor)
            {
                hw.can_work_leader_approve = hw.permit_to_work.acc_ptw_requestor_approve;
                hw.status = (int)statusHW.CANWORKLEADER;
            } else if (user.id.ToString() == this.work_leader)
            {
                hw.can_work_leader_approve = "a" + user.signature;
                hw.status = (int)statusHW.CANWORKLEADER;
            }
            this.db.Entry(hw).State = EntityState.Modified;
            this.db.SaveChanges();
            return "200";
        }

        public string supervisorCan(UserEntity user)
        {
            // requestor approval
            // return code - 200 {ok}
            //               201 {ok, delegation}
            //               400 {not the user}

            hot_work hw = this.db.hot_work.Find(this.id);
            int foId = 0;
            Int32.TryParse(this.can_supervisor, out foId);
            UserEntity fo = new UserEntity(foId, user.token, user);
            if (user.id.ToString() == this.can_supervisor)
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                hw.can_supervisor_approve = "a" + user.signature;
                hw.status = (int)statusHW.CANSPV;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == fo.employee_delegate.ToString())
            {
                //ptw.acc_assessor = this.acc_assessor;
                //ptw.acc_assessor_delegate = this.acc_assessor_delegate;
                //ptw.can_assessor = this.can_assessor;
                //ptw.can_assessor_delegate = this.can_assessor_delegate;
                hw.can_supervisor_approve = "d" + user.signature;
                hw.can_supervisor_delegate = user.id.ToString();
                hw.status = (int)statusHW.CANSPV;
                this.db.Entry(hw).State = EntityState.Modified;
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
            int foId = 0;
            Int32.TryParse(this.can_supervisor, out foId);
            UserEntity fo = new UserEntity(foId, user.token, user);
            if (user.id.ToString() == this.can_supervisor || user.id.ToString() == fo.employee_delegate.ToString())
            {
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.can_work_leader_approve = null;
                hw.status = (int)statusHW.CANCEL;
                this.db.Entry(hw).State = EntityState.Modified;
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

            hot_work hw = this.db.hot_work.Find(this.id);

            if (user.id.ToString() == this.can_fire_watch)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                hw.can_fire_watch_approve = "a" + user.signature;
                //hw.status = (int)statusHW.CANFIREWATCH;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();

                return "200";
            }
            else if (user.id.ToString() == this.can_fire_watch_delegate)
            {
                //ptw.acc_fo = this.acc_fo;
                //ptw.acc_fo_delegate = this.acc_fo_delegate;
                //ptw.can_fo = this.acc_fo;
                //ptw.can_fo_delegate = this.acc_fo_delegate;
                hw.can_fire_watch_approve = "d" + user.signature;
                //hw.status = (int)statusHW.CANFIREWATCH;
                this.db.Entry(hw).State = EntityState.Modified;
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
                hot_work hw = this.db.hot_work.Find(this.id);
                hw.can_supervisor_approve = null;
                hw.status = (int)statusHW.CANWORKLEADER;
                this.db.Entry(hw).State = EntityState.Modified;
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

            hot_work hw = this.db.hot_work.Find(this.id);

            if (user.id.ToString() == this.can_fo)
            {
                hw.can_fo_approve = "a" + user.signature;
                hw.status = (int)statusHW.CANFO;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();
                return "200";
            }
            else
            {
                hw.can_fo_approve = "d" + user.signature;
                hw.can_fo_delegate = user.id.ToString();
                hw.status = (int)statusHW.CANFO;
                this.db.Entry(hw).State = EntityState.Modified;
                this.db.SaveChanges();
                return "201";
            }
        }

        public string fOCanReject(UserEntity user, string comment)
        {
            // FO reject
            // return code - 200 {ok}
            //               400 {not the user}
            hot_work hw = this.db.hot_work.Find(this.id);
            hw.can_supervisor_approve = null;
            hw.status = (int)statusHW.CANWORKLEADER;
            this.db.Entry(hw).State = EntityState.Modified;
            this.db.SaveChanges();

            return "200";
        }

        #endregion

        #region is can add extension

        public bool isCanAddExtension()
        {
            return (this.status == (int)statusHW.ACCFO || this.status == (int)statusHW.EXTACCFO1
                 || this.status == (int)statusHW.EXTACCFO2
                 || this.status == (int)statusHW.EXTACCFO3
                 || this.status == (int)statusHW.EXTACCFO4
                 || this.status == (int)statusHW.EXTACCFO5
                 || this.status == (int)statusHW.EXTACCFO6);
        }

        public int getExtensionNo()
        {
            if (this.status >= (int)statusHW.EXTCREATE1 && this.status <= (int)statusHW.EXTACCFO1)
            {
                return 1;
            }
            if (this.status >= (int)statusHW.EXTCREATE2 && this.status <= (int)statusHW.EXTACCFO2)
            {
                return 2;
            }
            if (this.status >= (int)statusHW.EXTCREATE3 && this.status <= (int)statusHW.EXTACCFO3)
            {
                return 3;
            }
            if (this.status >= (int)statusHW.EXTCREATE4 && this.status <= (int)statusHW.EXTACCFO4)
            {
                return 4;
            }
            if (this.status >= (int)statusHW.EXTCREATE5 && this.status <= (int)statusHW.EXTACCFO5)
            {
                return 5;
            }
            if (this.status >= (int)statusHW.EXTCREATE6 && this.status <= (int)statusHW.EXTACCFO6)
            {
                return 6;
            }
            if (this.status >= (int)statusHW.EXTCREATE7 && this.status <= (int)statusHW.EXTACCFO7)
            {
                return 7;
            }

            return 0;
        }

        public int editExtHotWork (int extension) {
            hot_work hw = this.db.hot_work.Find(this.id);

            switch (extension)
            {
                case 1:
                    hw.ext_datetime_1 = this.ext_datetime_1;
                    hw.ext_lel_1 = this.ext_lel_1;
                    hw.ext_o2_1 = this.ext_o2_1;
                    hw.ext_h2s_1 = this.ext_h2s_1;
                    hw.ext_co_1 = this.ext_co_1;
                    hw.ext_other_1 = this.ext_other_1;
                    hw.ext_remark_1 = this.ext_remark_1;
                    hw.ext_new_validity_1 = this.ext_new_validity_1;
                    hw.screening_1 = this.screening_1_db;
                    break;
                case 2:
                    hw.ext_datetime_2 = this.ext_datetime_2;
                    hw.ext_lel_2 = this.ext_lel_2;
                    hw.ext_o2_2 = this.ext_o2_2;
                    hw.ext_h2s_2 = this.ext_h2s_2;
                    hw.ext_co_2 = this.ext_co_2;
                    hw.ext_other_2 = this.ext_other_2;
                    hw.ext_remark_2 = this.ext_remark_2;
                    hw.ext_new_validity_2 = this.ext_new_validity_2;
                    hw.screening_2 = this.screening_2_db;
                    break;
                case 3:
                    hw.ext_datetime_3 = this.ext_datetime_3;
                    hw.ext_lel_3 = this.ext_lel_3;
                    hw.ext_o2_3 = this.ext_o2_3;
                    hw.ext_h2s_3 = this.ext_h2s_3;
                    hw.ext_co_3 = this.ext_co_3;
                    hw.ext_other_3 = this.ext_other_3;
                    hw.ext_remark_3 = this.ext_remark_3;
                    hw.ext_new_validity_3 = this.ext_new_validity_3;
                    hw.screening_3 = this.screening_3_db;
                    break;
                case 4:
                    hw.ext_datetime_4 = this.ext_datetime_4;
                    hw.ext_lel_4 = this.ext_lel_4;
                    hw.ext_o2_4 = this.ext_o2_4;
                    hw.ext_h2s_4 = this.ext_h2s_4;
                    hw.ext_co_4 = this.ext_co_4;
                    hw.ext_other_4 = this.ext_other_4;
                    hw.ext_remark_4 = this.ext_remark_4;
                    hw.ext_new_validity_4 = this.ext_new_validity_4;
                    hw.screening_4 = this.screening_4_db;
                    break;
                case 5:
                    hw.ext_datetime_5 = this.ext_datetime_5;
                    hw.ext_lel_5 = this.ext_lel_5;
                    hw.ext_o2_5 = this.ext_o2_5;
                    hw.ext_h2s_5 = this.ext_h2s_5;
                    hw.ext_co_5 = this.ext_co_5;
                    hw.ext_other_5 = this.ext_other_5;
                    hw.ext_remark_5 = this.ext_remark_5;
                    hw.ext_new_validity_5 = this.ext_new_validity_5;
                    hw.screening_5 = this.screening_5_db;
                    break;
                case 6:
                    hw.ext_datetime_6 = this.ext_datetime_6;
                    hw.ext_lel_6 = this.ext_lel_6;
                    hw.ext_o2_6 = this.ext_o2_6;
                    hw.ext_h2s_6 = this.ext_h2s_6;
                    hw.ext_co_6 = this.ext_co_6;
                    hw.ext_other_6 = this.ext_other_6;
                    hw.ext_remark_6 = this.ext_remark_6;
                    hw.ext_new_validity_6 = this.ext_new_validity_6;
                    hw.screening_6 = this.screening_6_db;
                    break;
                case 7:
                    hw.ext_datetime_7 = this.ext_datetime_7;
                    hw.ext_lel_7 = this.ext_lel_7;
                    hw.ext_o2_7 = this.ext_o2_7;
                    hw.ext_h2s_7 = this.ext_h2s_7;
                    hw.ext_co_7 = this.ext_co_7;
                    hw.ext_other_7 = this.ext_other_7;
                    hw.ext_remark_7 = this.ext_remark_7;
                    hw.ext_new_validity_7 = this.ext_new_validity_7;
                    hw.screening_7 = this.screening_7_db;
                    break;
            };
            this.db.Entry(hw).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        #endregion

        public void GetPtw(UserEntity user)
        {
            this.ptw = new PtwEntity(this.id_ptw.Value, user);
            this.is_guest = this.ptw.is_guest == 1;
        }
    }
}