using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Runtime.InteropServices;

namespace TTStatistics
{
    public class ExcelOperation
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        public static void Kill(Microsoft.Office.Interop.Excel.Application excel)
        {
            IntPtr t = new IntPtr(excel.Hwnd);   //得到这个句柄，具体作用是得到这块内存入口 
            int k = 0;
            GetWindowThreadProcessId(t, out k);   //得到本进程唯一标志k
            System.Diagnostics.Process p = System.Diagnostics.Process.GetProcessById(k);   //得到对进程k的引用
            p.Kill();     //关闭进程k
        }

        public enum ExcelOperationType
        {
            ByOleDB,
            ByExcelInterface
        }
        private OleDbConnection m_oleDBConnection;
        private string m_xlsFilePath;
        private string m_firstSheetName = "";
        private ExcelOperationType m_operType;
        private Microsoft.Office.Interop.Excel.Application m_excelApp;
        private Microsoft.Office.Interop.Excel.Workbook m_workBook;
        public ExcelOperation(ExcelOperationType type = ExcelOperationType.ByOleDB)
        {
            m_operType = type;
        }

        public string LastErrorMsg { get; private set; }

        public string FirstSheetName
        {
            get { return m_firstSheetName; }
        }
        public bool Open(string xlsFilePath)
        {
            if (Path.GetExtension(xlsFilePath).ToLower() != ".xlsx")
            {
                LastErrorMsg = "文件类型不是excel文件!";
                return false;
            }
            if (!System.IO.File.Exists(xlsFilePath))
            {
                LastErrorMsg = string.Format("{0} is not found!", xlsFilePath);
                return false;
            }

            if (m_operType == ExcelOperationType.ByOleDB)
            {
                return OpenFileWithOle(xlsFilePath);
            }
            else
            {
                return OpenFileWithOffice(xlsFilePath);
            }
        }

        private bool OpenFileWithOle(string xlsFilePath)
        {
            if (m_oleDBConnection == null)
            {
                m_oleDBConnection = new OleDbConnection();
            }
            if (m_oleDBConnection.State == ConnectionState.Open)
            {
                if (xlsFilePath == m_xlsFilePath)
                {
                    return true;
                }
                else
                {
                    m_oleDBConnection.Close();
                }
            }

            //m_oleDBConnection.ConnectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=NO;IMEX=1'", xlsFilePath);
            m_oleDBConnection.ConnectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=NO;IMEX=1'", xlsFilePath);

            try
            {
                m_oleDBConnection.Open();
            }
            catch (System.Exception e)
            {
                LastErrorMsg = e.Message;
                return false;
            }
            m_xlsFilePath = xlsFilePath;
            return true;
        }
        private bool OpenFileWithOffice(string xlsFilePath)
        {
            if (m_excelApp == null)
            {
                m_excelApp = new Microsoft.Office.Interop.Excel.Application();
                if (m_excelApp == null)
                {
                    LastErrorMsg = "Excel Application初始化失败！";
                    return false;
                }
            }
            try
            {
                m_workBook = m_excelApp.Workbooks.Open(xlsFilePath);
            }
            catch (System.Exception e)
            {
                LastErrorMsg = e.Message + e.ToString();
                return false;
            }
            m_xlsFilePath = xlsFilePath;
            return true;
        }

        public System.Data.DataTable GetSheetData(string sheetName)
        {
            if (m_operType == ExcelOperationType.ByOleDB)
            {
                return GetSheetDataByOle(sheetName);
            }
            else
            {
                return GetSheetDataByOffice(sheetName);
            }
        }

        private System.Data.DataTable GetSheetDataByOffice(string sheetName)
        {
            Worksheet sheet = m_workBook.Sheets[sheetName];
            if (sheet == null)
            {
                return null;
            }
            System.Data.DataTable dt = new System.Data.DataTable();
            int rowCount = sheet.UsedRange.Rows.Count;
            int colCount = sheet.UsedRange.Columns.Count;
            Range range;
            for (int iCol = 0; iCol < colCount; iCol++)
            {
                dt.Columns.Add("F" + (iCol + 1).ToString());
            }

            for (int iRow = 0; iRow < rowCount; iRow++)
            {
                DataRow dr = dt.NewRow();
                for (int icol = 0; icol < colCount; icol++)
                {
                    range = sheet.UsedRange.Cells[iRow + 1, icol + 1];
                    dr[icol] = (range.Value2 == null) ? "" : range.Value2.ToString();
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        private System.Data.DataTable GetSheetDataByOle(string sheetName)
        {
            if (m_oleDBConnection.State != ConnectionState.Open)
            {
                m_oleDBConnection.Open();
            }
            try
            {
                OleDbCommand oleCmd = new OleDbCommand(string.Format("SELECT * FROM [{0}]", sheetName), m_oleDBConnection);
                OleDbDataReader reader = oleCmd.ExecuteReader();
                System.Data.DataTable tb = new System.Data.DataTable();
                tb.Load(reader);
                reader.Close();
                return tb;
            }
            catch (Exception error)
            {
                LastErrorMsg = error.Message;
                return null;
            }
        }
        public List<string> GetAllSheetNames()
        {
            if (m_operType == ExcelOperationType.ByOleDB)
            {
                return GetAllSheetNamesByOle();
            }
            else
            {
                return GetAllSheetNamesByOffice();
            }
        }
        private List<string> GetAllSheetNamesByOffice()
        {
            List<string> sheetNames = new List<string>();
            if (m_workBook.Sheets.Count == 0)
            {
                LastErrorMsg = "Get sheet names in excel file error!";
                return null;
            }
            foreach (Worksheet sheet in m_workBook.Sheets)
            {
                sheetNames.Add(sheet.Name);
            }
            m_firstSheetName = sheetNames[0];
            return sheetNames;
        }


        private List<string> GetAllSheetNamesByOle()
        {
            if (m_oleDBConnection.State != ConnectionState.Open)
            {
                m_oleDBConnection.Open();
            }
            try
            {
                System.Data.DataTable dt = m_oleDBConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                if (dt == null)
                {
                    LastErrorMsg = "Get sheet names in excel file error!";
                    return null;
                }
                List<string> lstNames = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    lstNames.Add(row["TABLE_NAME"].ToString());
                }
                if (lstNames.Count >= 1)
                {
                    m_firstSheetName = lstNames[0];
                }
                else
                {
                    LastErrorMsg = "Sheet in excel file is empty!";
                    return null;
                }
                return lstNames;
            }
            catch (Exception error)
            {
                LastErrorMsg = error.Message;
                return null;
            }
        }

        public void Close()
        {
            if (m_operType == ExcelOperationType.ByOleDB)
            {
                m_oleDBConnection.Close();
            }
            else
            {
                m_workBook.Close();
                m_excelApp.Quit();
                Kill(m_excelApp);
            }

        }
    }
}
