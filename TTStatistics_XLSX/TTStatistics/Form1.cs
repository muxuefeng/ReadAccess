using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.OleDb;
using System.Net;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Office.Interop.Excel;

// nothing to do, just for test 2019.2.15

namespace TTStatistics
{
    public partial class TTStatistics : Form
    {
        private String m_strXLSPath;
        private System.Data.DataTable dtTable;

        private List<String[]> _statisticsList = new List<String[]>();//ALost、AWon、Statistics、BWon、BLost
        private List<String[]> _strokeList = new List<String[]>();//ALost、AWon、Statistics、BWon、BLost

        public const String g_strParsedFolderName = "ParsedScoreData";

        public TTStatistics()
        {
            InitializeComponent();
            SetDataGridViewStyle(dataGrid_players);
            SetDataGridViewStyle(dataGrid_stroke);

            tb_datacenter.Text = ConfigurationManager.GetUserSettingString("Path").Trim();
        }

        private void TTStatistics_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you sure to exit?", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
            }
        }

        private void btn_datacenter_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "xdc文件|*.xdc";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                tb_datacenter.Text = dlg.FileName;
                if (File.Exists(tb_datacenter.Text))
                {
                    string strUpdate = string.Format("Update T_Config set F_XDCFile = '{0}'", tb_datacenter.Text);
                    XDC_Update(strUpdate);
                }

                ConfigurationManager.SetUserSettingString("Path", tb_datacenter.Text);
            }
        }

        private void btn_result_Click(object sender, EventArgs e)
        {
            String strDesFile = "";

            try
            {
                if (!MoveScoreFileToSpecFolder(m_strXLSPath, ref strDesFile))
                    return;

                ExcelOperation excelOper = new ExcelOperation(ExcelOperation.ExcelOperationType.ByOleDB);
                if (!excelOper.Open(strDesFile))
                {
                    return;
                }

                if (excelOper.GetAllSheetNames() == null)
                {
                    excelOper.Close();
                    return;
                }

                dtTable = excelOper.GetSheetData("Sheet1$");
                excelOper.Close();

                if (dtTable == null || dtTable.Rows.Count < 2)
                {
                    return;
                }

                DeleteXDCTable("T_Statistics");
                DeleteXDCTable("T_Stroke");

                string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";
                strConnection += tb_datacenter.Text;

                OleDbConnection oleConnection = new OleDbConnection(strConnection);
                oleConnection.Open();
                OleDbCommand oleCommand = new OleDbCommand();
                oleCommand.Connection = oleConnection;
                oleCommand.CommandType = CommandType.Text;

                if (dtTable.Columns.Count < 5)
                    return;

                for (int i = 1; i < dtTable.Rows.Count; i++)
                {

                    int nASuc, nBSuc;
                    nASuc = GetRate(dtTable.Rows[i][1].ToString(), dtTable.Rows[i][0].ToString());
                    nBSuc = GetRate(dtTable.Rows[i][3].ToString(), dtTable.Rows[i][4].ToString());

                    string strMaxPer = GetMaxPercent(nASuc, nBSuc);

                    string strASuc, strBSuc;

                    strASuc = GetRatePercent(dtTable.Rows[i][1].ToString(), dtTable.Rows[i][0].ToString());
                    strBSuc = GetRatePercent(dtTable.Rows[i][3].ToString(), dtTable.Rows[i][4].ToString());

                    string strAPer, strBPer;

                    strAPer = GetRateLimit(dtTable.Rows[i][1].ToString(), dtTable.Rows[i][0].ToString());
                    strBPer = GetRateLimit(dtTable.Rows[i][3].ToString(), dtTable.Rows[i][4].ToString());

                    string sql = "";

                    if (i < 13)
                    {
                        sql = string.Format("INSERT INTO T_Statistics([ALost],[AWon],[Statistics],[BWon],[BLost],[ASuc],[BSuc],[CMax],[APer],[BPer]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                        dtTable.Rows[i][0].ToString(),
                        dtTable.Rows[i][1].ToString(),
                        dtTable.Rows[i][2].ToString(),
                        dtTable.Rows[i][3].ToString(),
                        dtTable.Rows[i][4].ToString(),
                        strASuc,
                        strBSuc,
                        strMaxPer,
                        strAPer,
                        strBPer
                        );
                    }
                    else
                    {
                        sql = string.Format("INSERT INTO T_Stroke([ALost],[AWon],[Statistics],[BWon],[BLost],[ASuc],[BSuc],[CMax],[APer],[BPer]) VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}')",
                        dtTable.Rows[i][0].ToString(),
                        dtTable.Rows[i][1].ToString(),
                        dtTable.Rows[i][2].ToString(),
                        dtTable.Rows[i][3].ToString(),
                        dtTable.Rows[i][4].ToString(),
                        strASuc,
                        strBSuc,
                        strMaxPer,
                        strAPer,
                        strBPer
                        );
                    }

                    oleCommand.CommandText = sql;
                    int bRet = oleCommand.ExecuteNonQuery();
                }

                oleConnection.Close();
                ResetPlayerList();
                ResetStokeList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private int ConvertStringToINT(string strValue)
        {
            int nResult = 0;
            int.TryParse(strValue, out nResult);

            return nResult;
        }

        private int GetRate(string strWon, string strLost)
        {
            int nWon, nLost, nTotal, nSuc;

            int.TryParse(strWon, out nWon);
            int.TryParse(strLost, out nLost);

            nTotal = nWon + nLost;

            if(nTotal == 0)
            {
                nSuc = 0;
            }
            else
            {
                nSuc = nWon * 100 / nTotal;
            }

            return nSuc;
        }

        private string GetRatePercent(string strWon, string strLost)
        {
            int nWon, nLost, nTotal, nSuc;
            int.TryParse(strWon, out nWon);
            int.TryParse(strLost, out nLost);

            nTotal = nWon + nLost;

            if (nTotal == 0)
            {
                nSuc = 0;
            }
            else
            {
                nSuc = nWon * 100 / nTotal;
            }


            return nSuc.ToString() + "%";
        }

        private string GetRateLimit(string strWon, string strLost)
        {
            int nWon, nLost, nTotal, nSuc;
            int.TryParse(strWon, out nWon);
            int.TryParse(strLost, out nLost);

            nTotal = nWon + nLost;

            if (nTotal == 0)
            {
                nSuc = 0;
            }
            else
            {
                nSuc = nWon * 100 / nTotal;
            }

            if(nSuc == 0)
            {
                nSuc = 1; //石墨雷达图
            }

            return nSuc.ToString() + "%";
        }


        private string GetMaxPercent(int n1, int n2)
        {
            return (1.5 * Math.Max(n1, n2)).ToString();
        }

        private string GetJObjectValue(JObject jb, string strPropertyName)
        {
            if (jb == null || jb.Property(strPropertyName) == null)
                return string.Empty;

            return jb[strPropertyName].ToString().Trim('\"');
        }

        private int XDC_Update(string sql)
        {
            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";
            strConnection += tb_datacenter.Text;

            OleDbConnection oleConnection = new OleDbConnection(strConnection);
            oleConnection.Open();

            OleDbCommand oleCommand = new OleDbCommand(sql, oleConnection);

            int bRet = 0;

            try
            {
                bRet = oleCommand.ExecuteNonQuery();
            }
            catch
            {

            }

            oleConnection.Close();

            return bRet;
        }

        private void DeleteXDCTable(string strTable)
        {
            string strsql = "delete from " + strTable;
            XDC_Update(strsql);
        }

        private void XDC_Read(string sql, out List<string> lstRow)
        {
            lstRow = new List<string>();

            if (string.IsNullOrEmpty(sql))
            {
                return;
            }

            string strConnection = "Provider=Microsoft.Jet.OleDb.4.0;Data Source=";
            strConnection += tb_datacenter.Text;

            OleDbConnection oleConnection = new OleDbConnection(strConnection);
            oleConnection.Open();

            OleDbCommand oleCommand = new OleDbCommand(sql, oleConnection);
            OleDbDataReader reader = oleCommand.ExecuteReader();

            while (reader.Read())
            {
                string line = "";
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    line += reader.GetValue(i).ToString() + ",";
                }
                lstRow.Add(line);
            }
            oleConnection.Close();

        }

        private void ResetPlayerList()
        {
            string strsql = "select ALost, AWon, Statistics, BWon, BLost from T_Statistics";

            List<string> lstPlayer = new List<string>();

            XDC_Read(strsql, out lstPlayer);

            _statisticsList.Clear();

            for (int i = 0; i < lstPlayer.Count; i++)
            {
                String[] strPlayer = new String[5];

                string[] column = lstPlayer[i].Split(',');

                strPlayer[0] = column[0];
                strPlayer[1] = column[1];
                strPlayer[2] = column[2];
                strPlayer[3] = column[3];
                strPlayer[4] = column[4];

                _statisticsList.Add(strPlayer);
            }

            FillPlayerGridview();
        }

        private void ResetStokeList()
        {
            string strsql = "select ALost, AWon, Statistics, BWon, BLost from T_Stroke";

            List<string> lstPlayer = new List<string>();

            XDC_Read(strsql, out lstPlayer);

            _strokeList.Clear();

            for (int i = 0; i < lstPlayer.Count; i++)
            {
                String[] strPlayer = new String[5];

                string[] column = lstPlayer[i].Split(',');

                strPlayer[0] = column[0];
                strPlayer[1] = column[1];
                strPlayer[2] = column[2];
                strPlayer[3] = column[3];
                strPlayer[4] = column[4];

                _strokeList.Add(strPlayer);
            }

            FillStokeGridview();
        }
        private void FillPlayerGridview()
        {
            try
            {
                if (dataGrid_players.Columns.Count != 5)
                {
                    dataGrid_players.Columns.Clear();

                    DataGridViewColumn col = null;

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "ALost";
                    col.Name = "ALost";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_players.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "AWon";
                    col.Name = "AWon";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_players.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "Statistics";
                    col.Name = "Statistics";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    col.Width = 200;
                    dataGrid_players.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "BWon";
                    col.Name = "BWon";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_players.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "BLost";
                    col.Name = "BLost";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_players.Columns.Add(col);
                }

                // Fill DataGridView
                dataGrid_players.Rows.Clear();
                for (int j = 0; j < _statisticsList.Count; j++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dr.CreateCells(dataGrid_players);
                    dr.Selected = false;
                    dr.Height = 30;

                    String[] info = _statisticsList[j];

                    for (int i = 0; i < info.Length; i++)
                    {
                        dr.Cells[i].Value = info[i];
                    }

                    dataGrid_players.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("sss");
            }
        }

        private void FillStokeGridview()
        {
            try
            {
                if (dataGrid_stroke.Columns.Count != 5)
                {
                    dataGrid_stroke.Columns.Clear();

                    DataGridViewColumn col = null;

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "ALost";
                    col.Name = "ALost";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_stroke.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "AWon";
                    col.Name = "AWon";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_stroke.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "Statistics";
                    col.Name = "Statistics";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    col.Width = 200;
                    dataGrid_stroke.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "BWon";
                    col.Name = "BWon";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_stroke.Columns.Add(col);

                    col = new DataGridViewTextBoxColumn();
                    col.HeaderText = "BLost";
                    col.Name = "BLost";
                    col.ReadOnly = true;
                    col.ReadOnly = false;
                    dataGrid_stroke.Columns.Add(col);
                }

                // Fill DataGridView
                dataGrid_stroke.Rows.Clear();
                for (int j = 0; j < _strokeList.Count; j++)
                {
                    DataGridViewRow dr = new DataGridViewRow();
                    dr.CreateCells(dataGrid_stroke);
                    dr.Selected = false;
                    dr.Height = 30;

                    String[] info = _strokeList[j];

                    for (int i = 0; i < info.Length; i++)
                    {
                        dr.Cells[i].Value = info[i];
                    }

                    dataGrid_stroke.Rows.Add(dr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("sss");
            }
        }

        public static void SetDataGridViewStyle(DataGridView dgv)
        {
            if (dgv == null) return;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(202, 221, 238);
            dgv.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(230, 239, 248);
            dgv.BackgroundColor = System.Drawing.Color.FromArgb(212, 228, 242);
            dgv.GridColor = System.Drawing.Color.FromArgb(208, 215, 229);
            dgv.BorderStyle = BorderStyle.Fixed3D;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.ColumnHeadersHeight = 40;
            dgv.Font = new System.Drawing.Font("Arial", 14);
            //dgv.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //dgv.AllowUserToAddRows = false;
            //dgv.AllowUserToDeleteRows = false;
            //dgv.AllowUserToOrderColumns = false;
            //dgv.AllowUserToResizeRows = false;
        }

        private void btn_path_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Excel文件|*.xlsx";
            DialogResult dr = ofd.ShowDialog();
            tb_resultpath.Text = ofd.FileName;

            m_strXLSPath = ofd.FileName;
        }

        private bool MoveScoreFileToSpecFolder(String strExcelFile, ref String strDesFile)
        {
            String strParsedPath = "C:\\" + g_strParsedFolderName;
            String strParsedFile = strParsedPath + strExcelFile.Substring(strExcelFile.LastIndexOf('\\'));
            strDesFile = strParsedFile;
            try
            {
                // Create the specified folder in the import file path
                if (!Directory.Exists(strParsedPath))
                {
                    Directory.CreateDirectory(strParsedPath);
                }

                if (!File.Exists(strExcelFile))
                {
                    return false;
                }

                if (File.Exists(strParsedFile)) File.Delete(strParsedFile);
                File.Copy(strExcelFile, strParsedFile, true);
            }
            catch (System.Exception errDir)
            {
                return false;
            }

            return true;
        }

    }

    public class Match_List
    {
        public string id { get; set; }
        public string game_name { get; set; }
        public string game_date { get; set; }
        public string game_time { get; set; }
        public string game_stage { get; set; }
        public string user_a { get; set; }
        public string user_b { get; set; }
        public string game_project { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string show { get; set; }
        public string add_time { get; set; }
        public string states { get; set; }
    }

    public class Statistics_List
    {
        public string ALost { get; set; }
        public string AWon { get; set; }
        public string Statistics { get; set; }
        public string BWon { get; set; }
        public string BLost { get; set; }
    }
    public class Player_List
    {
        public string user_name { get; set; }
    }
}
