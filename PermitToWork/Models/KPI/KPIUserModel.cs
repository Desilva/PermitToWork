﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIUserModel
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        private int UserId;
        private int? Year;

        public KPIUserModel(int userId, int? year)
        {
            UserId = userId;
            Year = year;
        }

        #region kpi requestor

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
            //kamus
            string strUserId = UserId.ToString();
            string queryStatus = "Cancellation requested by Permit To Work Requestor";
            int count;

            //algoritma
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

        public int CalculateRequestorImpromptuPermit()
        {
            //kamus
            string strUserId = UserId.ToString();
            int count;

            //algoritma
            var query =
                from permit in db.permit_to_work
                where permit.acc_ptw_requestor == strUserId && 
                    (permit.create_date.Value.Year == permit.validity_period_start.Value.Year &&
                    permit.create_date.Value.Month == permit.validity_period_start.Value.Month &&
                    permit.create_date.Value.Day == permit.validity_period_start.Value.Day)
                select permit;

            if (Year != null)
                query = query.Where(m => m.create_date.Value.Year == Year);
            count = query.Count();

            return count;
        }

        #endregion

        #region kpi approval

        public double CalculateSupervisorAverageResponseTimeInHours()
        {
            //kamus
            string strUserId = UserId.ToString();
            double sumMinutes = 0, countData = 0;
            double sumHours, averageHours = 0;
            int lastPermitId = 0;
            KPIMaster.AppState state = KPIMaster.AppState.EMPTY;
            DateTime startTime = DateTime.Now, finishTime = DateTime.Now;
            List<permit_log> permitLogList;
            IQueryable<int> innerQuery;

            //algoritma
            if (Year == null)
            {
                innerQuery =
                    from log in db.permit_log
                    where log.user_id == UserId && log.permit_type == 0 &&
                        (log.status == KPIMaster.SupervisorApprove || log.status == KPIMaster.SupervisorReject)
                    select log.id_permit.Value;
            }
            else
            {
                innerQuery =
                    from log in db.permit_log
                    join ptw in db.permit_to_work on log.id_permit equals ptw.id
                    where log.user_id == UserId && log.permit_type == 0 &&
                        (log.status == KPIMaster.SupervisorApprove || log.status == KPIMaster.SupervisorReject) &&
                        ptw.validity_period_end.Value.Year == Year
                    select log.id_permit.Value;
            }

            var query =
                from permit in db.permit_log
                where innerQuery.Distinct().Contains(permit.id_permit.Value) &&
                    permit.permit_type == 0 &&
                    (permit.status == KPIMaster.RequestorApprove || permit.status == KPIMaster.SupervisorReject || 
                    permit.status == KPIMaster.SupervisorApprove || permit.status == KPIMaster.AssessorReject || 
                    permit.status == KPIMaster.FOReject)
                select permit
            ;

            //if (Year != null)
            //    query = query.Where(m => m.datetime.Value.Year == Year);
            query = query.OrderBy(m => m.id_permit).ThenBy(m => m.datetime);

            permitLogList = query.ToList();
            foreach (permit_log log in permitLogList)
            {
                if (state == KPIMaster.AppState.EMPTY)
                {
                    if ((log.status == KPIMaster.RequestorApprove) ||
                        (log.status == KPIMaster.FOReject) ||
                        (log.status == KPIMaster.AssessorReject))
                    {
                        state = KPIMaster.AppState.START;
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
                else if (state == KPIMaster.AppState.START)
                {
                    if ((log.status == KPIMaster.SupervisorReject) ||
                        (log.status == KPIMaster.SupervisorApprove))
                    {
                        state = KPIMaster.AppState.EMPTY;
                        finishTime = log.datetime.Value;

                        if (lastPermitId == log.id_permit)
                        {
                            sumMinutes += (finishTime - startTime).TotalMinutes;
                            ++countData;
                        }
                    }
                    else if ((log.status == KPIMaster.RequestorApprove) ||
                       (log.status == KPIMaster.FOReject) ||
                       (log.status == KPIMaster.AssessorReject))
                    {
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
            }

            sumHours = (double)sumMinutes / 60;
            if (sumHours > 0)
            {
                averageHours = (double)sumHours / countData;
            }

            return averageHours;
        }

        public double CalculateAssessorAverageResponseTimeInHours()
        {
            //kamus
            string strUserId = UserId.ToString();
            double sumMinutes = 0, countData = 0;
            double sumHours, averageHours = 0;
            int lastPermitId = 0;
            KPIMaster.AppState state = KPIMaster.AppState.EMPTY;
            DateTime startTime = DateTime.Now, finishTime = DateTime.Now;
            List<permit_log> permitList;
            IQueryable<int> innerQuery;

            //algoritma
            if (Year == null)
            {
                innerQuery =
                    from log in db.permit_log
                    where log.user_id == UserId && log.permit_type == 0 &&
                        (log.status == KPIMaster.AssessorReject || log.status == KPIMaster.AssessorApprove)
                    select log.id_permit.Value;
            }
            else
            {
                innerQuery =
                    from log in db.permit_log
                    join ptw in db.permit_to_work on log.id_permit equals ptw.id
                    where log.user_id == UserId && log.permit_type == 0 &&
                        (log.status == KPIMaster.AssessorReject || log.status == KPIMaster.AssessorApprove) &&
                        ptw.validity_period_end.Value.Year == Year
                    select log.id_permit.Value;
            }

            var query =
                from permit in db.permit_log
                where innerQuery.Distinct().Contains(permit.id_permit.Value) &&
                    permit.permit_type == 0 &&
                    (permit.status == KPIMaster.SupervisorApprove || permit.status == KPIMaster.AssessorApprove ||
                    permit.status == KPIMaster.AssessorReject)
                select permit
            ;

            query = query.OrderBy(m => m.id_permit).ThenBy(m => m.datetime);

            permitList = query.ToList();
            foreach (permit_log log in permitList)
            {
                if (state == KPIMaster.AppState.EMPTY)
                {
                    if (log.status == KPIMaster.SupervisorApprove)
                    {
                        state = KPIMaster.AppState.START;
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
                else if (state == KPIMaster.AppState.START)
                {
                    if ((log.status == KPIMaster.AssessorApprove) ||
                        (log.status == KPIMaster.AssessorReject))
                    {
                        state = KPIMaster.AppState.EMPTY;
                        finishTime = log.datetime.Value;

                        if (lastPermitId == log.id_permit)
                        {
                            sumMinutes += (finishTime - startTime).TotalMinutes;
                            ++countData;
                        }
                        else
                        {
                            //averageHours = 1;
                        }
                    }
                    else if (log.status == KPIMaster.SupervisorApprove)
                    {
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
            }

            sumHours = (double)sumMinutes / 60;
            if (sumHours > 0)
            {
                averageHours = (double)sumHours / countData;
            }

            return averageHours;
        }

        #endregion

        #region kpi fo
        
        public int CalculateFOClosing()
        {
            //kamus
            string strUserId = UserId.ToString();
            int count;

            //algoritma
            var query = db.permit_to_work
                .Where(m => m.acc_fo == strUserId && m.status == 11);

            if (Year != null)
                query = query.Where(m => m.validity_period_end.Value.Year == Year);

            count = query.Count();

            return count;
        }

        /// <summary>
        /// Cancellation approved by Supervisor s/d Cancellation rejected by Facility Owner
        /// Cancellation approved by Supervisor s/d Cancellation approved by Facility Owner
        /// -> dijumlahkan, lalu dirata-rata
        /// </summary>
        /// <returns></returns>
        public double CalculateFOAverageClosingTimeInHours()
        {
            //kamus
            string strUserId = UserId.ToString();
            double sumMinutes = 0, countData = 0;
            double sumHours, averageHours = 0;
            int lastPermitId = 0;
            KPIMaster.AppState state = KPIMaster.AppState.EMPTY;
            DateTime startTime = DateTime.Now, finishTime = DateTime.Now;
            List<permit_log> permitList;
            IQueryable<int> innerQuery;

            //algoritma
            if (Year == null)
            {
                //innerQuery =
                //    from log in db.permit_log
                //    where log.user_id == UserId && log.permit_type == 0 &&
                //        (log.status == KPIMaster.FOClosingApprove || log.status == KPIMaster.FOClosingReject)
                //    select log.id_permit.Value;

                innerQuery =
                    from ptw in db.permit_to_work
                    where ptw.acc_fo == strUserId && ptw.status == 11
                    select ptw.id;
            }
            else
            {
                //innerQuery =
                //    from log in db.permit_log
                //    join ptw in db.permit_to_work on log.id_permit equals ptw.id
                //    where log.user_id == UserId && log.permit_type == 0 &&
                //        (log.status == KPIMaster.FOClosingApprove || log.status == KPIMaster.FOClosingReject) &&
                //        ptw.validity_period_end.Value.Year == Year
                //    select log.id_permit.Value;
                innerQuery =
                    from ptw in db.permit_to_work
                    where ptw.acc_fo == strUserId && ptw.status == 11 &&
                        ptw.validity_period_end.Value.Year == Year
                    select ptw.id;
            }
            var query =
                from permit in db.permit_log
                where innerQuery.Distinct().Contains(permit.id_permit.Value) &&
                    permit.permit_type == 0 &&
                    (permit.status == KPIMaster.SupervisorClosingApprove || permit.status == KPIMaster.FOClosingApprove ||
                    permit.status == KPIMaster.FOClosingReject)
                select permit
            ;

            query = query.OrderBy(m => m.id_permit).ThenBy(m => m.datetime);

            permitList = query.ToList();
            foreach (permit_log log in permitList)
            {
                if (state == KPIMaster.AppState.EMPTY)
                {
                    if (log.status == KPIMaster.SupervisorClosingApprove)
                    {
                        state = KPIMaster.AppState.START;
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
                else if (state == KPIMaster.AppState.START)
                {
                    if ((log.status == KPIMaster.FOClosingApprove) ||
                        (log.status == KPIMaster.FOClosingReject))
                    {
                        state = KPIMaster.AppState.EMPTY;
                        finishTime = log.datetime.Value;

                        if (lastPermitId == log.id_permit)
                        {
                            sumMinutes += (finishTime - startTime).TotalMinutes;
                            ++countData;
                        }
                        else
                        {
                            //averageHours = 1;
                        }
                    }
                    else if (log.status == KPIMaster.SupervisorClosingApprove)
                    {
                        startTime = log.datetime.Value;
                        lastPermitId = log.id_permit.Value;
                    }
                }
            }

            sumHours = (double)sumMinutes / 60;
            if (sumHours > 0)
            {
                averageHours = (double)sumHours / countData;
            }

            return averageHours;
        }

        #endregion
    }
}