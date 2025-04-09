using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.OleDb;

namespace FrameWork
{
    public static class ExcelReader
    {
        //public static DataTable ReadFromExcel(string filePath)
        //{
        //    if (System.IO.File.Exists(filePath))
        //    {
        //        DataSet ds = new DataSet();
        //        string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filePath + ";Extended Properties=Excel 12.0;";
        //        try
        //        {
        //            using (OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConnectionString))
        //            {
        //                conn.Open();
        //                using (DataTable dtExcelSchema = conn.GetSchema("Tables"))
        //                {
        //                    string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
        //                    string query = "SELECT * FROM [" + sheetName + "]";
        //                    OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn);
        //                    adapter.Fill(ds, "Items");
        //                    if (ds.Tables.Count > 0)
        //                    {
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            return ds.Tables[0];
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception exc)
        //        {
        //        }
        //    }
        //    return null;
        //}
    }
}