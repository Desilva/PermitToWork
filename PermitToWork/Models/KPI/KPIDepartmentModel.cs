using System;
using System.Collections.Generic;
using System.Data.Objects.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIDepartmentModel
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        private int? Year;
        private List<DepartmentEntity> Departments;

        public KPIDepartmentModel(int? year)
        {
            Year = year;
            Departments = new List<DepartmentEntity>();
        }

        public List<DepartmentEntity> GetDepartments()
        {
            return Departments;
        }

        #region kpi

        public void CalculateKPI()
        {
            CalculateClosing();
            CalculateAverageClosingTime();
        }

        private void CalculateClosing()
        {
            //kamus

            //algoritma
            var query = db.permit_to_work
                .Where(m => m.status == 11);

            if (Year != null)
                query = query.Where(m => m.validity_period_end.Value.Year == Year);

            Departments = query.GroupBy(m => m.ptw_no.Substring(0, (SqlFunctions.CharIndex("-", m.ptw_no).Value - 1)))
                .Select(m => new DepartmentEntity { DepartmentName = m.Key, TotalClosing = m.Count() }).ToList();
        }

        private void CalculateAverageClosingTime()
        {
            //kamus
            double sumMinutes, countData, sumHours, averageHours;
            int lastPermitId = 0;
            KPIMaster.AppState state = KPIMaster.AppState.EMPTY;
            DateTime startTime = DateTime.Now, finishTime = DateTime.Now;
            List<permit_log> permitList;

            //algoritma
            foreach (DepartmentEntity dept in Departments)
            {
                sumMinutes = 0; countData = 0;
                sumHours = 0; averageHours = 0;
                lastPermitId = 0;

                var innerQuery =
                    from ptw in db.permit_to_work
                    where ptw.status == 11 && ptw.ptw_no.StartsWith(dept.DepartmentName)
                    select ptw.id;
                var query =
                    from log in db.permit_log
                    where innerQuery.Distinct().Contains(log.id_permit.Value) &&
                        log.permit_type == 0 &&
                        (log.status == KPIMaster.SupervisorClosingApprove || log.status == KPIMaster.FOClosingApprove ||
                        log.status == KPIMaster.FOClosingReject)
                    select log
                ;

                if (Year != null)
                    query = query.Where(m => m.datetime.Value.Year == Year);
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
                                averageHours = 1;
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

                dept.AverageClosingTime = averageHours;
            }
        }

        #endregion
    }
}