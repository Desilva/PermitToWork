using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using PermitToWork.Models.User;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PermitToWork.Models.KPI
{
    public class KPIReport
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        public byte[] GenerateExcelReport(int? year)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //supaya file tidak corrupt

            //kamus
            XSSFWorkbook workbook = new XSSFWorkbook();
            XSSFSheet sheet; XSSFRow row; XSSFCell cell;
            XSSFCellStyle style, styleDecimal; XSSFFont font;
            CellRangeAddressList addressList;
            XSSFRow row2; XSSFCell cell2; //untuk di header
            int colIndex; int rowIndex;
            MemoryStream ms;
            UserEntity user;
            ListUser listUser;
            KPIUserModel kpiUser;
            double doubleVal;
            KPIDepartmentModel kpiDepartment;

            //algoritma
            //USER REPORT
            sheet = (XSSFSheet)workbook.CreateSheet("Employees Report");

            //create row (header)
            row = (XSSFRow)sheet.CreateRow((short)0);

            //header style
            style = (XSSFCellStyle)workbook.CreateCellStyle();
            font = (XSSFFont)workbook.CreateFont();
            font.Boldweight = (short)FontBoldWeight.Bold; ;
            style.SetFont(font);

            //header data
            colIndex = 0;
            foreach (string title in new List<string> { "Employee", "Total Ontime Closing", "Total Overdue", "Total Impromptu", "Average Response Time Spv (hours)", "Average Response Time Assessor (hours)" })
            {
                cell = (XSSFCell)row.CreateCell(colIndex);
                cell.SetCellValue(title);
                cell.CellStyle = style;

                ++colIndex;
            }

            //user data
            user = HttpContext.Current.Session["user"] as UserEntity;
            listUser = new ListUser(user.token, user.id);
            rowIndex = 1;

            styleDecimal = (XSSFCellStyle)workbook.CreateCellStyle();
            styleDecimal.DataFormat = workbook.CreateDataFormat().GetFormat("0.00");

            foreach (UserEntity singleUser in listUser.listUser)
            {
                row = (XSSFRow)sheet.CreateRow((short)rowIndex);
                kpiUser = new KPIUserModel(singleUser.id, year);

                cell = (XSSFCell)row.CreateCell(0);
                cell.SetCellValue(singleUser.alpha_name);

                cell = (XSSFCell)row.CreateCell(1);
                cell.SetCellValue(kpiUser.CalculateRequestorOntimeClosing());

                cell = (XSSFCell)row.CreateCell(2);
                cell.SetCellValue(kpiUser.CalculateRequestorOverdueClosing());

                cell = (XSSFCell)row.CreateCell(3);
                cell.SetCellValue(kpiUser.CalculateRequestorImpromptuPermit());

                doubleVal = kpiUser.CalculateSupervisorAverageResponseTimeInHours();
                cell = (XSSFCell)row.CreateCell(4);
                cell.SetCellValue(doubleVal);
                if (doubleVal > 0)
                    cell.CellStyle = styleDecimal;

                doubleVal = kpiUser.CalculateAssessorAverageResponseTimeInHours();
                cell = (XSSFCell)row.CreateCell(5);
                cell.SetCellValue(doubleVal);
                if (doubleVal > 0)
                    cell.CellStyle = styleDecimal;

                ++rowIndex;
            }

            sheet.CreateFreezePane(0, 1);
            for (int i = 0; i < colIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            //DEPARTMENT REPORT
            sheet = (XSSFSheet)workbook.CreateSheet("Department Report");

            //create row (header)
            row = (XSSFRow)sheet.CreateRow((short)0);
            colIndex = 0;
            foreach (string title in new List<string> { "Department", "Total Closing", "Average Closing Time (hours)" })
            {
                cell = (XSSFCell)row.CreateCell(colIndex);
                cell.SetCellValue(title);
                cell.CellStyle = style;

                ++colIndex;
            }

            //department data
            kpiDepartment = new KPIDepartmentModel(year);
            kpiDepartment.CalculateKPI();
            rowIndex = 1;

            foreach (DepartmentEntity dept in kpiDepartment.GetDepartments())
            {
                row = (XSSFRow)sheet.CreateRow((short)rowIndex);

                cell = (XSSFCell)row.CreateCell(0);
                cell.SetCellValue(dept.DepartmentName);

                cell = (XSSFCell)row.CreateCell(1);
                cell.SetCellValue(dept.TotalClosing);

                doubleVal = dept.AverageClosingTime;
                cell = (XSSFCell)row.CreateCell(2);
                cell.SetCellValue(doubleVal);
                if (doubleVal > 0)
                    cell.CellStyle = styleDecimal;

                ++rowIndex;
            }

            sheet.CreateFreezePane(0, 1);
            for (int i = 0; i < colIndex; i++)
            {
                sheet.AutoSizeColumn(i);
            }

            //write to file
            ms = new MemoryStream();
            workbook.Write(ms);

            return ms.ToArray();
        }
    }
}