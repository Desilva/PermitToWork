using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIFilterFormStub
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        public int? Year { get; set; }

        public KPIFilterFormStub()
        {
            Year = DateTime.Now.Year;
        }

        #region options

        private List<SelectListItem> YearOptions;

        public void SetYearOptions()
        {
            YearOptions = new List<SelectListItem>();
            DateTime? minDate = db.permit_to_work.Where(m => m.validity_period_start != null).OrderBy(m => m.validity_period_start).Select(m => m.validity_period_start).FirstOrDefault();
            DateTime? maxDate = db.permit_to_work.Where(m => m.validity_period_end != null).OrderByDescending(m => m.validity_period_end).Select(m => m.validity_period_end).FirstOrDefault();
            int minYear, maxYear;

            if (minDate == null)
                minDate = DateTime.Now;
            if (maxDate == null)
                maxDate = DateTime.Now;

            minYear = minDate.Value.Year;
            maxYear = maxDate.Value.Year;

            for (int i = minYear; i <= maxYear; i++)
            {
                YearOptions.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString() });
            }
        }

        public List<SelectListItem> GetYearOptions()
        {
            return YearOptions;
        }

        #endregion
    }
}