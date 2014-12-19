using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using PermitToWork.Models;
using PermitToWork.Models.Master;

namespace PermitToWork.Utilities
{
    public class ExcelReader
    {
        private star_energy_ptwEntities db = new star_energy_ptwEntities();

        #region Scaffolding Inspection Sheet

        public int LoadInspection(string filename, int? id, int? idPrev)
        {
            Excel.Application app;
            Excel.Workbook book;
            Excel.Range ShtRange;
            List<List<object>> temp;
            List<object> temp_row;
            List<string> err;
            int retVal = 0;
            int i, j = 0;
            bool add = true;
            err = new List<string>();
            app = new Excel.Application();
            book = app.Workbooks.Open(Filename: filename);
            try
            {
                foreach (Excel.Worksheet sheet in book.Sheets)
                {
                    temp = new List<List<object>>();
                    ShtRange = sheet.UsedRange;
                    string a = sheet.Name;
                    for (i = 1; i <= ShtRange.Rows.Count; i++)
                    {
                        temp_row = new List<object>();
                        for (j = 1; j <= ShtRange.Columns.Count; j++)
                        {
                            if ((ShtRange.Cells[i, j] as Excel.Range).Value2 == null)
                            {
                                temp_row.Add(null);
                            }
                            else
                            {
                                string s = (ShtRange.Cells[i, j] as Excel.Range).Value2.ToString().Trim();
                                temp_row.Add(s == "-" ? null : s);
                            }
                        }
                        temp.Add(temp_row);
                    }

                    string errTemp = "";
                    if (add) retVal = saveInspection(temp, id, idPrev);
                    add = true;
                }
            }
            catch (Exception e)
            {
                err.Add(e.StackTrace);
            }
            finally
            {
                book.Close(true, Missing.Value, Missing.Value);
                app.Quit();
            }

            return retVal;
        }

        private int saveInspection(List<List<object>> data, int? id, int? idPrev)
        {
            string err = "";
            Nullable<DateTime> dat = null;
            Nullable<TimeSpan> ts = null;

            scaffolding_inspection ins = new scaffolding_inspection();

            if (id != null)
            {
                ins = db.scaffolding_inspection.Find(id);
            }

            if (data[1][2].ToString().Trim().Contains("/"))
            {
                ins.date_inspection = DateTime.Parse(data[1][2].ToString().Trim());
            }
            else
            {
                ins.date_inspection = DateTime.FromOADate(Double.Parse(data[1][2].ToString().Trim()));
            }

            ins.type_scaffold = (data[2][2] != null ? data[2][2].ToString().Trim() : null);
            ins.duties_scaffold = (data[3][2] != null ? data[3][2].ToString().Trim() : null);
            ins.pupose_scaffold = (data[4][2] != null ? data[4][2].ToString().Trim() : null);
            ins.location = (data[5][2] != null ? data[5][2].ToString().Trim() : null);
            ins.name_scaffolder = (data[6][2] != null ? data[6][2].ToString().Trim() : null) + "#" + (data[6][4] != null ? data[6][4].ToString().Trim() : null) + "#" + (data[7][2] != null ? data[7][2].ToString().Trim() : null) + "#" + (data[7][4] != null ? data[7][4].ToString().Trim() : null);
            ins.base_ground_good = (data[10][3] != null ? data[10][3].ToString().Trim() : null);
            ins.base_ground_poor = (data[10][4] != null ? data[10][4].ToString().Trim() : null);
            ins.base_ground_remark = (data[10][5] != null ? data[10][5].ToString().Trim() : null);
            ins.base_concrete_good = (data[11][3] != null ? data[11][3].ToString().Trim() : null);
            ins.base_concrete_poor = (data[11][4] != null ? data[11][4].ToString().Trim() : null);
            ins.base_concrete_remark = (data[11][5] != null ? data[11][5].ToString().Trim() : null);
            ins.base_sole_plate_good = (data[12][3] != null ? data[12][3].ToString().Trim() : null);
            ins.base_sole_plate_poor = (data[12][4] != null ? data[12][4].ToString().Trim() : null);
            ins.base_sole_plate_remark = (data[12][5] != null ? data[12][5].ToString().Trim() : null);
            ins.base_base_plate_good = (data[13][3] != null ? data[13][3].ToString().Trim() : null);
            ins.base_base_plate_poor = (data[13][4] != null ? data[13][4].ToString().Trim() : null);
            ins.base_base_plate_remark = (data[13][5] != null ? data[13][5].ToString().Trim() : null);
            ins.base_adj_sole_plate_good = (data[14][3] != null ? data[14][3].ToString().Trim() : null);
            ins.base_adj_sole_plate_poor = (data[14][4] != null ? data[14][4].ToString().Trim() : null);
            ins.base_adj_sole_plate_remark = (data[14][5] != null ? data[14][5].ToString().Trim() : null);
            ins.standard_diameter_good = (data[16][3] != null ? data[16][3].ToString().Trim() : null);
            ins.standard_diameter_poor = (data[16][4] != null ? data[16][4].ToString().Trim() : null);
            ins.standard_diameter_remark = (data[16][5] != null ? data[16][5].ToString().Trim() : null);
            ins.standard_length_good = (data[17][3] != null ? data[17][3].ToString().Trim() : null);
            ins.standard_length_poor = (data[17][4] != null ? data[17][4].ToString().Trim() : null);
            ins.standard_length_remark = (data[17][5] != null ? data[17][5].ToString().Trim() : null);
            ins.standard_leveling_good = (data[18][3] != null ? data[18][3].ToString().Trim() : null);
            ins.standard_leveling_poor = (data[18][4] != null ? data[18][4].ToString().Trim() : null);
            ins.standard_leveling_remark = (data[18][5] != null ? data[18][5].ToString().Trim() : null);
            ins.standard_couplers_good = (data[19][3] != null ? data[19][3].ToString().Trim() : null);
            ins.standard_couplers_poor = (data[19][4] != null ? data[19][4].ToString().Trim() : null);
            ins.standard_couplers_remark = (data[19][5] != null ? data[19][5].ToString().Trim() : null);
            ins.standard_pin_good = (data[20][3] != null ? data[20][3].ToString().Trim() : null);
            ins.standard_pin_poor = (data[20][4] != null ? data[20][4].ToString().Trim() : null);
            ins.standard_pin_remark = (data[20][5] != null ? data[20][5].ToString().Trim() : null);
            ins.ledger_diameter_good = (data[22][3] != null ? data[22][3].ToString().Trim() : null);
            ins.ledger_diameter_poor = (data[22][4] != null ? data[22][4].ToString().Trim() : null);
            ins.ledger_diameter_remark = (data[22][5] != null ? data[22][5].ToString().Trim() : null);
            ins.ledger_length_good = (data[23][3] != null ? data[23][3].ToString().Trim() : null);
            ins.ledger_length_poor = (data[23][4] != null ? data[23][4].ToString().Trim() : null);
            ins.ledger_length_remark = (data[23][5] != null ? data[23][5].ToString().Trim() : null);
            ins.ledger_leveling_good = (data[24][3] != null ? data[24][3].ToString().Trim() : null);
            ins.ledger_leveling_poor = (data[24][4] != null ? data[24][4].ToString().Trim() : null);
            ins.ledger_leveling_remark = (data[24][5] != null ? data[24][5].ToString().Trim() : null);
            ins.ledger_couplers_good = (data[25][3] != null ? data[25][3].ToString().Trim() : null);
            ins.ledger_couplers_poor = (data[25][4] != null ? data[25][4].ToString().Trim() : null);
            ins.ledger_couplers_remark = (data[25][5] != null ? data[25][5].ToString().Trim() : null);
            ins.ledger_sleeve_couplers_good = (data[26][3] != null ? data[26][3].ToString().Trim() : null);
            ins.ledger_sleeve_couplers_poor = (data[26][4] != null ? data[26][4].ToString().Trim() : null);
            ins.ledger_sleeve_couplers_remark = (data[26][5] != null ? data[26][5].ToString().Trim() : null);
            ins.transom_diameter_good = (data[28][3] != null ? data[28][3].ToString().Trim() : null);
            ins.transom_diameter_poor = (data[28][4] != null ? data[28][4].ToString().Trim() : null);
            ins.transom_diameter_remark = (data[28][5] != null ? data[28][5].ToString().Trim() : null);
            ins.transom_length_good = (data[29][3] != null ? data[29][3].ToString().Trim() : null);
            ins.transom_length_poor = (data[29][4] != null ? data[29][4].ToString().Trim() : null);
            ins.transom_length_remark = (data[29][5] != null ? data[29][5].ToString().Trim() : null);
            ins.transom_leveling_good = (data[30][3] != null ? data[30][3].ToString().Trim() : null);
            ins.transom_leveling_poor = (data[30][4] != null ? data[30][4].ToString().Trim() : null);
            ins.transom_leveling_remark = (data[30][5] != null ? data[30][5].ToString().Trim() : null);
            ins.transom_couplers_good = (data[31][3] != null ? data[31][3].ToString().Trim() : null);
            ins.transom_couplers_poor = (data[31][4] != null ? data[31][4].ToString().Trim() : null);
            ins.transom_couplers_remark = (data[31][5] != null ? data[31][5].ToString().Trim() : null);
            ins.transom_sleeve_couplers_good = (data[32][3] != null ? data[32][3].ToString().Trim() : null);
            ins.transom_sleeve_couplers_poor = (data[32][4] != null ? data[32][4].ToString().Trim() : null);
            ins.transom_sleeve_couplers_remark = (data[32][5] != null ? data[32][5].ToString().Trim() : null);
            ins.putlog_diameter_good = (data[34][3] != null ? data[34][3].ToString().Trim() : null);
            ins.putlog_diameter_poor = (data[34][4] != null ? data[34][4].ToString().Trim() : null);
            ins.putlog_diameter_remark = (data[34][5] != null ? data[34][5].ToString().Trim() : null);
            ins.putlog_length_good = (data[35][3] != null ? data[35][3].ToString().Trim() : null);
            ins.putlog_length_poor = (data[35][4] != null ? data[35][4].ToString().Trim() : null);
            ins.putlog_length_remark = (data[35][5] != null ? data[35][5].ToString().Trim() : null);
            ins.putlog_leveling_good = (data[36][3] != null ? data[36][3].ToString().Trim() : null);
            ins.putlog_leveling_poor = (data[36][4] != null ? data[36][4].ToString().Trim() : null);
            ins.putlog_leveling_remark = (data[36][5] != null ? data[36][5].ToString().Trim() : null);
            ins.bracing_diameter_good = (data[38][3] != null ? data[38][3].ToString().Trim() : null);
            ins.bracing_diameter_poor = (data[38][4] != null ? data[38][4].ToString().Trim() : null);
            ins.bracing_diameter_remark = (data[38][5] != null ? data[38][5].ToString().Trim() : null);
            ins.bracing_length_good = (data[39][3] != null ? data[39][3].ToString().Trim() : null);
            ins.bracing_length_poor = (data[39][4] != null ? data[39][4].ToString().Trim() : null);
            ins.bracing_length_remark = (data[39][5] != null ? data[39][5].ToString().Trim() : null);
            ins.bracing_pins_good = (data[40][3] != null ? data[40][3].ToString().Trim() : null);
            ins.bracing_pins_poor = (data[40][4] != null ? data[40][4].ToString().Trim() : null);
            ins.bracing_pins_remark = (data[40][5] != null ? data[40][5].ToString().Trim() : null);
            ins.bracing_couplers_good = (data[41][3] != null ? data[41][3].ToString().Trim() : null);
            ins.bracing_couplers_poor = (data[41][4] != null ? data[41][4].ToString().Trim() : null);
            ins.bracing_couplers_remark = (data[41][5] != null ? data[41][5].ToString().Trim() : null);
            ins.guardrail_ledger_diameter_good = (data[43][3] != null ? data[43][3].ToString().Trim() : null);
            ins.guardrail_ledger_diameter_poor = (data[43][4] != null ? data[43][4].ToString().Trim() : null);
            ins.guardrail_ledger_diameter_remark = (data[43][5] != null ? data[43][5].ToString().Trim() : null);
            ins.guardrail_ledger_length_good = (data[44][3] != null ? data[44][3].ToString().Trim() : null);
            ins.guardrail_ledger_length_poor = (data[44][4] != null ? data[44][4].ToString().Trim() : null);
            ins.guardrail_ledger_length_remark = (data[44][5] != null ? data[44][5].ToString().Trim() : null);
            ins.guardrail_ledger_leveling_good = (data[45][3] != null ? data[45][3].ToString().Trim() : null);
            ins.guardrail_ledger_leveling_poor = (data[45][4] != null ? data[45][4].ToString().Trim() : null);
            ins.guardrail_ledger_leveling_remark = (data[45][5] != null ? data[45][5].ToString().Trim() : null);
            ins.guardrail_ledger_couplers_good = (data[46][3] != null ? data[46][3].ToString().Trim() : null);
            ins.guardrail_ledger_couplers_poor = (data[46][4] != null ? data[46][4].ToString().Trim() : null);
            ins.guardrail_ledger_couplers_remark = (data[46][5] != null ? data[46][5].ToString().Trim() : null);
            ins.guardrail_ledger_sleeve_couplers_good = (data[47][3] != null ? data[47][3].ToString().Trim() : null);
            ins.guardrail_ledger_sleeve_couplers_poor = (data[47][4] != null ? data[47][4].ToString().Trim() : null);
            ins.guardrail_ledger_sleeve_couplers_remark = (data[47][5] != null ? data[47][5].ToString().Trim() : null);
            ins.guardrail_transom_diameter_good = (data[49][3] != null ? data[49][3].ToString().Trim() : null);
            ins.guardrail_transom_diameter_poor = (data[49][4] != null ? data[49][4].ToString().Trim() : null);
            ins.guardrail_transom_diameter_remark = (data[49][5] != null ? data[49][5].ToString().Trim() : null);
            ins.guardrail_transom_length_good = (data[50][3] != null ? data[50][3].ToString().Trim() : null);
            ins.guardrail_transom_length_poor = (data[50][4] != null ? data[50][4].ToString().Trim() : null);
            ins.guardrail_transom_length_remark = (data[50][5] != null ? data[50][5].ToString().Trim() : null);
            ins.guardrail_transom_leveling_good = (data[51][3] != null ? data[51][3].ToString().Trim() : null);
            ins.guardrail_transom_leveling_poor = (data[51][4] != null ? data[51][4].ToString().Trim() : null);
            ins.guardrail_transom_leveling_remark = (data[51][5] != null ? data[51][5].ToString().Trim() : null);
            ins.guardrail_transom_couplers_good = (data[52][3] != null ? data[52][3].ToString().Trim() : null);
            ins.guardrail_transom_couplers_poor = (data[52][4] != null ? data[52][4].ToString().Trim() : null);
            ins.guardrail_transom_couplers_remark = (data[52][5] != null ? data[52][5].ToString().Trim() : null);
            ins.guardrail_transom_sleeve_couplers_good = (data[53][3] != null ? data[53][3].ToString().Trim() : null);
            ins.guardrail_transom_sleeve_couplers_poor = (data[53][4] != null ? data[53][4].ToString().Trim() : null);
            ins.guardrail_transom_sleeve_couplers_remark = (data[53][5] != null ? data[53][5].ToString().Trim() : null);
            ins.platform_condition_good = (data[55][3] != null ? data[55][3].ToString().Trim() : null);
            ins.platform_condition_poor = (data[55][4] != null ? data[55][4].ToString().Trim() : null);
            ins.platform_condition_remark = (data[55][5] != null ? data[55][5].ToString().Trim() : null);
            ins.platform_length_good = (data[56][3] != null ? data[56][3].ToString().Trim() : null);
            ins.platform_length_poor = (data[56][4] != null ? data[56][4].ToString().Trim() : null);
            ins.platform_length_remark = (data[56][5] != null ? data[56][5].ToString().Trim() : null);
            ins.platform_width_good = (data[57][3] != null ? data[57][3].ToString().Trim() : null);
            ins.platform_width_poor = (data[57][4] != null ? data[57][4].ToString().Trim() : null);
            ins.platform_width_remark = (data[57][5] != null ? data[57][5].ToString().Trim() : null);
            ins.platform_clearance_good = (data[58][3] != null ? data[58][3].ToString().Trim() : null);
            ins.platform_clearance_poor = (data[58][4] != null ? data[58][4].ToString().Trim() : null);
            ins.platform_clearance_remark = (data[58][5] != null ? data[58][5].ToString().Trim() : null);
            ins.platform_lashing_good = (data[59][3] != null ? data[59][3].ToString().Trim() : null);
            ins.platform_lashing_poor = (data[59][4] != null ? data[59][4].ToString().Trim() : null);
            ins.platform_lashing_remark = (data[59][5] != null ? data[59][5].ToString().Trim() : null);
            ins.platform_deflection_good = (data[60][3] != null ? data[60][3].ToString().Trim() : null);
            ins.platform_deflection_poor = (data[60][4] != null ? data[60][4].ToString().Trim() : null);
            ins.platform_deflection_remark = (data[60][5] != null ? data[60][5].ToString().Trim() : null);
            ins.toeboard_condition_good = (data[62][3] != null ? data[62][3].ToString().Trim() : null);
            ins.toeboard_condition_poor = (data[62][4] != null ? data[62][4].ToString().Trim() : null);
            ins.toeboard_condition_remark = (data[62][5] != null ? data[62][5].ToString().Trim() : null);
            ins.toeboard_length_good = (data[63][3] != null ? data[63][3].ToString().Trim() : null);
            ins.toeboard_length_poor = (data[63][4] != null ? data[63][4].ToString().Trim() : null);
            ins.toeboard_length_remark = (data[63][5] != null ? data[63][5].ToString().Trim() : null);
            ins.toeboard_height_good = (data[64][3] != null ? data[64][3].ToString().Trim() : null);
            ins.toeboard_height_poor = (data[64][4] != null ? data[64][4].ToString().Trim() : null);
            ins.toeboard_height_remark = (data[64][5] != null ? data[64][5].ToString().Trim() : null);
            ins.toeboard_clearance_good = (data[65][3] != null ? data[65][3].ToString().Trim() : null);
            ins.toeboard_clearance_poor = (data[65][4] != null ? data[65][4].ToString().Trim() : null);
            ins.toeboard_clearance_remark = (data[65][5] != null ? data[65][5].ToString().Trim() : null);
            ins.toeboard_lashing_good = (data[66][3] != null ? data[66][3].ToString().Trim() : null);
            ins.toeboard_lashing_poor = (data[66][4] != null ? data[66][4].ToString().Trim() : null);
            ins.toeboard_lashing_remark = (data[66][5] != null ? data[66][5].ToString().Trim() : null);
            ins.aluminium_ladder_condition_good = (data[68][3] != null ? data[68][3].ToString().Trim() : null);
            ins.aluminium_ladder_condition_poor = (data[68][4] != null ? data[68][4].ToString().Trim() : null);
            ins.aluminium_ladder_condition_remark = (data[68][5] != null ? data[68][5].ToString().Trim() : null);
            ins.aluminium_ladder_length_good = (data[69][3] != null ? data[69][3].ToString().Trim() : null);
            ins.aluminium_ladder_length_poor = (data[69][4] != null ? data[69][4].ToString().Trim() : null);
            ins.aluminium_ladder_length_remark = (data[69][5] != null ? data[69][5].ToString().Trim() : null);
            ins.aluminium_ladder_height_good = (data[70][3] != null ? data[70][3].ToString().Trim() : null);
            ins.aluminium_ladder_height_poor = (data[70][4] != null ? data[70][4].ToString().Trim() : null);
            ins.aluminium_ladder_height_remark = (data[70][5] != null ? data[70][5].ToString().Trim() : null);
            ins.aluminium_ladder_putlog_good = (data[71][3] != null ? data[71][3].ToString().Trim() : null);
            ins.aluminium_ladder_putlog_poor = (data[71][4] != null ? data[71][4].ToString().Trim() : null);
            ins.aluminium_ladder_putlog_remark = (data[71][5] != null ? data[71][5].ToString().Trim() : null);
            ins.aluminium_ladder_angle_good = (data[72][3] != null ? data[72][3].ToString().Trim() : null);
            ins.aluminium_ladder_angle_poor = (data[72][4] != null ? data[72][4].ToString().Trim() : null);
            ins.aluminium_ladder_angle_remark = (data[72][5] != null ? data[72][5].ToString().Trim() : null);
            ins.ladder_tube_condition_good = (data[75][3] != null ? data[75][3].ToString().Trim() : null);
            ins.ladder_tube_condition_poor = (data[75][4] != null ? data[75][4].ToString().Trim() : null);
            ins.ladder_tube_condition_remark = (data[75][5] != null ? data[75][5].ToString().Trim() : null);
            ins.ladder_tube_diameter_good = (data[74][3] != null ? data[74][3].ToString().Trim() : null);
            ins.ladder_tube_diameter_poor = (data[74][4] != null ? data[74][4].ToString().Trim() : null);
            ins.ladder_tube_diameter_remark = (data[74][5] != null ? data[74][5].ToString().Trim() : null);
            ins.ladder_tube_height_good = (data[76][3] != null ? data[76][3].ToString().Trim() : null);
            ins.ladder_tube_height_poor = (data[76][4] != null ? data[76][4].ToString().Trim() : null);
            ins.ladder_tube_height_remark = (data[76][5] != null ? data[76][5].ToString().Trim() : null);
            ins.ladder_tube_putlog_good = (data[78][3] != null ? data[78][3].ToString().Trim() : null);
            ins.ladder_tube_putlog_poor = (data[78][4] != null ? data[78][4].ToString().Trim() : null);
            ins.ladder_tube_putlog_remark = (data[78][5] != null ? data[78][5].ToString().Trim() : null);
            ins.ladder_tube_angle_good = (data[77][3] != null ? data[77][3].ToString().Trim() : null);
            ins.ladder_tube_angle_poor = (data[77][4] != null ? data[77][4].ToString().Trim() : null);
            ins.ladder_tube_angle_remark = (data[77][5] != null ? data[77][5].ToString().Trim() : null);
            ins.ladder_tube_coupler_good = (data[79][3] != null ? data[79][3].ToString().Trim() : null);
            ins.ladder_tube_coupler_poor = (data[79][4] != null ? data[79][4].ToString().Trim() : null);
            ins.ladder_tube_coupler_remark = (data[79][5] != null ? data[79][5].ToString().Trim() : null);
            ins.gin_wheel_condition_good = (data[81][3] != null ? data[81][3].ToString().Trim() : null);
            ins.gin_wheel_condition_poor = (data[81][4] != null ? data[81][4].ToString().Trim() : null);
            ins.gin_wheel_condition_remark = (data[81][5] != null ? data[81][5].ToString().Trim() : null);
            ins.gin_wheel_swl_good = (data[82][3] != null ? data[82][3].ToString().Trim() : null);
            ins.gin_wheel_swl_poor = (data[82][4] != null ? data[82][4].ToString().Trim() : null);
            ins.gin_wheel_swl_remark = (data[82][5] != null ? data[82][5].ToString().Trim() : null);
            ins.gin_wheel_hoist_good = (data[83][3] != null ? data[83][3].ToString().Trim() : null);
            ins.gin_wheel_hoist_poor = (data[83][4] != null ? data[83][4].ToString().Trim() : null);
            ins.gin_wheel_hoist_remark = (data[83][5] != null ? data[83][5].ToString().Trim() : null);
            ins.gin_wheel_catch_good = (data[84][3] != null ? data[84][3].ToString().Trim() : null);
            ins.gin_wheel_catch_poor = (data[84][4] != null ? data[84][4].ToString().Trim() : null);
            ins.gin_wheel_catch_remark = (data[84][5] != null ? data[84][5].ToString().Trim() : null);
            ins.gin_wheel_clamp_good = (data[85][3] != null ? data[85][3].ToString().Trim() : null);
            ins.gin_wheel_clamp_poor = (data[85][4] != null ? data[85][4].ToString().Trim() : null);
            ins.gin_wheel_clamp_remark = (data[85][5] != null ? data[85][5].ToString().Trim() : null);
            ins.ties_condition_good = (data[87][3] != null ? data[87][3].ToString().Trim() : null);
            ins.ties_condition_poor = (data[87][4] != null ? data[87][4].ToString().Trim() : null);
            ins.ties_condition_remark = (data[87][5] != null ? data[87][5].ToString().Trim() : null);
            ins.ties_pipes_good = (data[88][3] != null ? data[88][3].ToString().Trim() : null);
            ins.ties_pipes_poor = (data[88][4] != null ? data[88][4].ToString().Trim() : null);
            ins.ties_pipes_remark = (data[88][5] != null ? data[88][5].ToString().Trim() : null);
            ins.access_ladder_access_good = (data[90][3] != null ? data[90][3].ToString().Trim() : null);
            ins.access_ladder_access_poor = (data[90][4] != null ? data[90][4].ToString().Trim() : null);
            ins.access_ladder_access_remark = (data[90][5] != null ? data[90][5].ToString().Trim() : null);
            ins.access_ladder_landing_platform_good = (data[91][3] != null ? data[91][3].ToString().Trim() : null);
            ins.access_ladder_landing_platform_poor = (data[91][4] != null ? data[91][4].ToString().Trim() : null);
            ins.access_ladder_landing_platform_remark = (data[91][5] != null ? data[91][5].ToString().Trim() : null);
            ins.access_ladder_safety_good = (data[92][3] != null ? data[92][3].ToString().Trim() : null);
            ins.access_ladder_safety_poor = (data[92][4] != null ? data[92][4].ToString().Trim() : null);
            ins.access_ladder_safety_remark = (data[92][5] != null ? data[92][5].ToString().Trim() : null);
            ins.general_surface_leveling_good = (data[94][3] != null ? data[94][3].ToString().Trim() : null);
            ins.general_surface_leveling_poor = (data[94][4] != null ? data[94][4].ToString().Trim() : null);
            ins.general_surface_leveling_remark = (data[94][5] != null ? data[94][5].ToString().Trim() : null);
            ins.general_surface_safety_good = (data[95][3] != null ? data[95][3].ToString().Trim() : null);
            ins.general_surface_safety_poor = (data[95][4] != null ? data[95][4].ToString().Trim() : null);
            ins.general_surface_safety_remark = (data[95][5] != null ? data[95][5].ToString().Trim() : null);
            ins.others_text = (data[97][2] != null ? data[97][2].ToString().Trim() : "");
            ins.others_good = (data[97][3] != null ? data[97][3].ToString().Trim() : "");
            ins.others_poor = (data[97][4] != null ? data[97][4].ToString().Trim() : "");
            ins.others_remark = (data[97][5] != null ? data[97][5].ToString().Trim() : "");

            int j = 0;
            bool add = true;
            for (int i = 98; i < data.Count; i++)
            {
                if (data[i][0] != null && data[i][0].ToString().Trim() == "SAFETY RECOMMENDATION")
                {
                    j = i;
                }
                else if (data[i][2] != null && data[i][2].ToString().Trim() != "block" && add)
                {
                    ins.others_text += "#" + (data[i][2] != null ? data[i][2].ToString().Trim() : "");
                    ins.others_good += "#" + (data[i][3] != null ? data[i][3].ToString().Trim() : "");
                    ins.others_poor += "#" + (data[i][4] != null ? data[i][4].ToString().Trim() : "");
                    ins.others_remark += "#" + (data[i][5] != null ? data[i][5].ToString().Trim() : "");
                }
                else if (data[i][2] != null && data[i][2].ToString().Trim() == "block")
                {
                    add = false;
                }
            }

            if (j != 0)
            {
                ins.safety_recommendation = data[j + 1][0] != null ? data[j + 1][0].ToString().Trim() : null;
            }

            if (idPrev != null)
            {
                scaffolding_inspection s = db.scaffolding_inspection.Find(idPrev.Value);
                ins.type_scaffold = s.type_scaffold;
                ins.duties_scaffold = s.duties_scaffold;
                ins.pupose_scaffold = s.pupose_scaffold;
                ins.location = s.location;
                ins.name_scaffolder = s.name_scaffolder;
            }

            if (id == null)
            {
                db.scaffolding_inspection.Add(ins);
                db.SaveChanges();
            }
            else
            {
                db.Entry(ins).State = EntityState.Modified;
                db.SaveChanges();
            }
            return ins.id;

        }

        #endregion

        #region PTW Holder No

        public List<string> LoadHolderNo(string filename)
        {
            Excel.Application app;
            Excel.Workbook book;
            Excel.Range ShtRange;
            List<object> temp;
            List<object> temp_row;
            List<string> err;
            int retVal = 0;
            int i, j = 0;
            bool add = true;
            err = new List<string>();
            app = new Excel.Application();
            book = app.Workbooks.Open(Filename: filename);
            try
            {
                foreach (Excel.Worksheet sheet in book.Sheets)
                {
                    temp = new List<object>();
                    ShtRange = sheet.UsedRange;
                    string a = sheet.Name;
                    for (i = 3; i <= ShtRange.Rows.Count; i++)
                    {
                        temp_row = new List<object>();
                        for (j = 1; j <= ShtRange.Columns.Count; j++)
                        {
                            if ((ShtRange.Cells[i, j] as Excel.Range).Value2 == null)
                            {
                                temp_row.Add(null);
                            }
                            else
                            {
                                string s = (ShtRange.Cells[i, j] as Excel.Range).Value2.ToString().Trim();
                                temp_row.Add(s == "-" ? null : s);
                            }
                        }
                        string errTemp = "";
                        if (add) errTemp = saveHolderNo(temp_row);
                        err.Add(errTemp);
                        add = true;
                    }
                }
            }
            catch (Exception e)
            {
                err.Add(e.StackTrace);
            }
            finally
            {
                book.Close(true, Missing.Value, Missing.Value);
                app.Quit();
            }

            return err;
        }

        private string saveHolderNo(List<object> data)
        {
            string err = "";
            Nullable<DateTime> dat = null;
            Nullable<TimeSpan> ts = null;

            MstPtwHolderNoEntity ptwHolder = null;
            mst_ptw_holder_no ptwHolderNo = new mst_ptw_holder_no();
            int id = Int32.Parse(data[0].ToString().Trim());
            if ((ptwHolderNo = db.mst_ptw_holder_no.Where(p => p.id_employee == id).FirstOrDefault()) != null)
            {
                if (data[3] != null)
                {
                    ptwHolderNo.ptw_holder_no = data[3].ToString().Trim();
                    ptwHolderNo.activated_date_until = DateTime.FromOADate(Double.Parse(data[4].ToString().Trim()));

                    db.Entry(ptwHolderNo).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    ptwHolderNo.ptw_holder_no = null;
                    ptwHolderNo.activated_date_until = null;

                    db.Entry(ptwHolderNo).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            else
            {
                if (data[3] != null)
                {
                    ptwHolderNo = new mst_ptw_holder_no();
                    ptwHolderNo.ptw_holder_no = data[3].ToString().Trim();
                    ptwHolderNo.activated_date_until = DateTime.FromOADate(Double.Parse(data[4].ToString().Trim()));
                    ptwHolderNo.id_employee = Int32.Parse(data[0].ToString());

                    db.mst_ptw_holder_no.Add(ptwHolderNo);
                    db.SaveChanges();
                }
            }
            
            return err;

        }

        #endregion

        public string generateError(List<string> err) {
            string html = "";
            if(err.Count > 0){
                html += "<b>LOG</b>";
                html += "<br/>";
                html += "<ul>";
                foreach (string x in err) {
                    html += "<li>";
                    html += x;
                    html += "</li>";
                }
                html += "</ul>";
            }
            return html;
        }
    }

}