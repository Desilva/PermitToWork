using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIModel
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        private int UserId;
        private int? Year;

        public KPIModel(int userId, int? year)
        {
            UserId = userId;
            Year = year;
        }

        #region requestor

        public int CalculateRequestorOntimeClosing()
        {
            string strUserId = UserId.ToString();
            string queryStatus = "Cancellation requested by Permit To Work Requestor";
            int count;

            //int count = db.permit_to_work
            //    .Join(db.permit_log.Where(l => l.permit_type == 0 && l.status == "Cancellation requested by Permit To Work Requestor" && l.user_id == UserId),
            //        ptw => ptw.id,
            //        log => log.id_permit,
            //        (ptw, log) => new { ptw, log }
            //    )
            //    .Where(m => m.ptw.acc_ptw_requestor == strUserId &&
            //        m.ptw.validity_period_end != null &&
            //        m.log != null &&
            //        m.ptw.validity_period_end >= m.log.datetime
            //    ).Count();

            var query =
                from permit in db.permit_to_work
                join log in
                    (
                        from log in db.permit_log.Where(m => m.permit_type == 0 && m.status == queryStatus && m.user_id == UserId)
                        .GroupBy(m => m.id_permit)
                        select new { id_permit = log.Key, maxdt = log.Max(m => m.datetime) }
                    ) on permit.id equals log.id_permit
                where permit.acc_ptw_requestor == strUserId && permit.status != 12 && System.Data.Objects.EntityFunctions.AddDays(permit.validity_period_end, 1) >= log.maxdt
                select permit;

            if (Year != null)
                query = query.Where(m => m.validity_period_end.Value.Year == Year);
            count = query.Count();

            return count;
        }

        public int CalculateRequestorOverdueClosing()
        {
            string strUserId = UserId.ToString();
            string queryStatus = "Cancellation requested by Permit To Work Requestor";
            int count;

            var query =
                from permit in db.permit_to_work
                join log in
                    (
                        from log in db.permit_log.Where(m => m.permit_type == 0 && m.status == queryStatus && m.user_id == UserId)
                        .GroupBy(m => m.id_permit)
                        select new { id_permit = log.Key, maxdt = log.Max(m => m.datetime) }
                    ) on permit.id equals log.id_permit
                where permit.acc_ptw_requestor == strUserId && permit.status != 12 && System.Data.Objects.EntityFunctions.AddDays(permit.validity_period_end, 1) < log.maxdt
                select permit;

            if (Year != null)
                query = query.Where(m => m.validity_period_end.Value.Year == Year);
            count = query.Count();

            return count;
        }

        #endregion
    }
}