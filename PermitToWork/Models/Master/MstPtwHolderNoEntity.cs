using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace PermitToWork.Models.Master
{
    public class MstPtwHolderNoEntity
    {
        public int id { get; set; }
        public Nullable<int> id_employee { get; set; }
        public string ptw_holder_no { get; set; }
        public Nullable<System.DateTime> activated_date_until { get; set; }
        private star_energy_ptwEntities db;

        public UserEntity user { get; set; }

        public MstPtwHolderNoEntity()
        {
            this.db = new star_energy_ptwEntities();
        }

        public MstPtwHolderNoEntity(int id, UserEntity user)
        {
            this.db = new star_energy_ptwEntities();
            mst_ptw_holder_no holderNo = this.db.mst_ptw_holder_no.Find(id);

            this.id = holderNo.id;
            this.id_employee = holderNo.id_employee;
            this.ptw_holder_no = holderNo.ptw_holder_no;
            this.activated_date_until = holderNo.activated_date_until;
            this.user = new UserEntity(this.id_employee.Value, user.token, user);
        }

        public MstPtwHolderNoEntity(int user_id, int a)
        {
            this.db = new star_energy_ptwEntities();
            mst_ptw_holder_no holderNo = this.db.mst_ptw_holder_no.Where(p => p.id_employee == user_id).ToList().FirstOrDefault();

            if (holderNo == null)
            {
                this.id = 0;
                this.id_employee = 0;
                this.ptw_holder_no = "";
                this.activated_date_until = null;
            }
            else
            {
                this.id = holderNo.id;
                this.id_employee = holderNo.id_employee;
                this.ptw_holder_no = holderNo.ptw_holder_no;
                this.activated_date_until = holderNo.activated_date_until;
            }
        }

        public MstPtwHolderNoEntity(mst_ptw_holder_no holderNo)
        {
            this.db = new star_energy_ptwEntities();

            this.id = holderNo.id;
            this.id_employee = holderNo.id_employee;
            this.ptw_holder_no = holderNo.ptw_holder_no;
            this.activated_date_until = holderNo.activated_date_until;
        }

        public List<MstPtwHolderNoEntity> getListMstPtwHolderNo()
        {
            var list = from a in db.mst_ptw_holder_no
                       select new MstPtwHolderNoEntity
                       {
                           id = a.id,
                           id_employee = a.id_employee,
                           ptw_holder_no = a.ptw_holder_no,
                           activated_date_until = a.activated_date_until,
                       };

            return list.ToList();
        }

        public List<MstPtwHolderNoEntity> getListMstPtwHolderNo(UserEntity user)
        {
            List<int> listId = this.db.mst_ptw_holder_no.Select(p => p.id).ToList();
            var result = new List<MstPtwHolderNoEntity>();
            foreach (int i in listId)
            {
                result.Add(new MstPtwHolderNoEntity(i, user));
            }

            return result;
        }

        public int addPtwHolderNo()
        {
            mst_ptw_holder_no holderNo = new mst_ptw_holder_no
            {
                id_employee = this.id_employee,
                ptw_holder_no = this.ptw_holder_no,
                activated_date_until = this.activated_date_until,
            };

            this.db.mst_ptw_holder_no.Add(holderNo);
            int retVal = this.db.SaveChanges();

            this.id = holderNo.id;

            return retVal;
        }

        public int editPtwHolderNo()
        {
            mst_ptw_holder_no holderNo = this.db.mst_ptw_holder_no.Find(id);
            holderNo.id_employee = this.id_employee;
            holderNo.ptw_holder_no = this.ptw_holder_no;
            holderNo.activated_date_until = this.activated_date_until;

            this.db.Entry(holderNo).State = EntityState.Modified;
            return this.db.SaveChanges();
        }

        public int deletePtwHolderNo()
        {
            mst_ptw_holder_no holderNo = this.db.mst_ptw_holder_no.Find(id);
            if (holderNo == null)
            {
                return 404;
            }
            this.db.mst_ptw_holder_no.Remove(holderNo);

            return this.db.SaveChanges();
        }
    }
}