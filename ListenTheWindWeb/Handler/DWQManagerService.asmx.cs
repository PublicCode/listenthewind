
using NPOI.HSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;
using DWQ;
using DWQ.ModelUtil;
using DWQ.Subject;
using T2VSoft.EnterpriseLibrary.Simulate;
using T2VSoft.DWQ;
using NPOI.SS.UserModel;
using Web.Energizer.User;
using BizLogic;
using Newtonsoft.Json;
using System.Web.Script.Serialization;


namespace Handler
{
    /// <summary>
    /// Summary description for DWQManagerService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DWQManagerService : System.Web.Services.WebService
    {

        public DWQManagerService()
        {
        }
        [WebMethod(Description = "Keeps your current session alive", EnableSession = true)]
        public void KeepSessionAlive()
        { 
            //DO nothing just want to keep session.
        }



        #region Extract Results

        [WebMethod]
        public string BuildExtractFile(string ReportItem, string ConfigData, string ExportType, string ExportFields)
        {
            string fileName = string.Empty;

            DataTable dtExportData = new DataTable();
            DataTable dtExportHead = new DataTable();
            SubjectInfo subjectInfo = new SubjectInfo(ReportItem);
            SubjectSearchArgs searchArgs = GetSearchArgs(ReportItem, ConfigData, ExportFields);


            dtExportData = GetExportData(searchArgs);
            if (dtExportData.Rows.Count > 50000 && ExportType != "CSV COMPRESS")
            {
                return "Your result set is " + string.Format("{0:0,00}", dtExportData.Rows.Count) + " rows. Are you sure you want to extract? If yes, please export with CSV COMPRESS options.";
            }

            dtExportHead = DWQSearch.GetExportHead(searchArgs);
            fileName = ExportFile(ExportType, dtExportData, dtExportHead, subjectInfo, searchArgs);


            //Delete refuse files
            DeleteRefuseFiles();

            return "../DownLoadData/" + fileName;
        }

        //Get Export Data
        private SubjectSearchArgs GetSearchArgs(string reportItem, string configData, string exportFields)
        {
            SubjectSearchArgs searchArgs = new SubjectSearchArgs();
            searchArgs.SubjectId = reportItem;
            searchArgs.CtrlValue = GetCtrlValue(reportItem, configData);
            searchArgs.ExportFields = new List<string>();
            string[] exportFieldList = exportFields.Split(',');
            foreach (string exportField in exportFieldList)
            {
                searchArgs.ExportFields.Add(exportField);
            }
            return searchArgs;
        }
        private Dictionary<string, string> GetCtrlValue(string repotItem, string configData)
        {
            Dictionary<string, string> ctrlValue = new Dictionary<string, string>();
            if (configData != string.Empty)
            {
                string[] temp = configData.Split(';');
                foreach (string tmp in temp)
                {
                    if (!string.IsNullOrEmpty(tmp))
                    {
                        string[] t = new string[2];
                        t[0] = tmp.Substring(0, tmp.IndexOf(":"));
                        t[1] = Encoding.Default.GetString(Convert.FromBase64String(tmp.Substring(tmp.IndexOf(":") + 1)));
                        if (tmp.IndexOf(":") > 0)
                        {
                            if (t[0].Trim().IndexOf("ddl") == 0)
                                ctrlValue.Add(t[0].Trim(), t[1].Replace("----Select----", "").Replace("'", "''"));
                            else
                                ctrlValue.Add(t[0].Trim(), t[1].Trim().Replace("----Select----", "").Replace("'", "''"));
                        }
                    }
                }
            }
            else
            {
                List<SubjectSearchFormInfo> list = SubjectManager.GetSearchFormInfos(repotItem);
                foreach (SubjectSearchFormInfo info in list)
                {
                    ctrlValue.Add(info.ControlId, string.Empty);
                }
            }
            return ctrlValue;
        }
        private DataTable GetExportData(SubjectSearchArgs searchArgs)
        {
            DataTable dtExport = new DataTable();
            if (searchArgs.ExportFields.Count != 0)
            {
                dtExport = DWQSearch.GetExportData(searchArgs);
            }
            return dtExport;
        }

        //Build Export File
        private string ExportFile(string exportType, DataTable dtExportData, DataTable dtExportHead, SubjectInfo subjectInfo, SubjectSearchArgs searchArgs)
        {
            string fileName = "ExportData_" + DateTime.Now.ToString("yyyy-MM-dd") + "_" + (new Random()).Next(100, 999).ToString();

            //IdentityAnalogue ia = new IdentityAnalogue();
            try
            {
                //string userName = ConfigurationManager.AppSettings["ImpersonateUserName"];
                //string password = ConfigurationManager.AppSettings["ImpersonatePassWord"];
                //if (ia.ImpersonateValidUser(userName, "", password))
                //{
                    string virtualPath = ConfigurationManager.AppSettings["DownLoadVirtualPath"];
                    string xlsFileName = Server.MapPath(virtualPath) + fileName + ".xls";
                    string csvFileName = Server.MapPath(virtualPath) + fileName + ".csv";
                    string zipFileName = Server.MapPath(virtualPath) + fileName + ".zip";

                    switch (exportType)
                    {
                        case "EXCEL":
                            ExportExcel(xlsFileName, dtExportData, dtExportHead, subjectInfo);
                            fileName += ".xls";
                            break;
                        case "CSV":
                            ExportCsv(csvFileName, dtExportData, dtExportHead, subjectInfo);
                            fileName += ".csv";
                            break;
                        case "CSV COMPRESS":
                            ExportCsv(csvFileName, dtExportData, dtExportHead, subjectInfo);
                            ExportCsvCompress(csvFileName, zipFileName, subjectInfo, searchArgs);
                            fileName += ".zip";
                            break;
                    }
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //ia.UndoImpersonation();
            }

            return "../DownLoadData/" + fileName;
        }
        private void ExportExcel(string xlsFileName, DataTable dtExportData, DataTable dtExportHead, SubjectInfo subjectInfo)
        {

            FileStream file = new FileStream(xlsFileName, FileMode.Create);
            try
            {
                //Create Sheet
                HSSFWorkbook workbook = new HSSFWorkbook();
                 ISheet sheet = workbook.CreateSheet("ExportData");   //One sheet max rows is 65535

                //Create Header
                IRow rowTitle = sheet.CreateRow(0);
                //for (int i = 0; i < dtExportData.Columns.Count; i++)
                //{
                //    rowTitle.CreateCell(i).SetCellValue(dtExportData.Columns[i].ColumnName);
                //}
                for (int i = 0; i < dtExportHead.Rows.Count; i++)
                {
                    rowTitle.CreateCell(i).SetCellValue(dtExportHead.Rows[i]["HeadText"].ToString());
                }

                //Add Row            
                for (int i = 0; i < dtExportData.Rows.Count; i++)
                {
                    DataRow dr = dtExportData.Rows[i];
                    IRow rowData = sheet.CreateRow(i + 1);
                    for (int j = 0; j < dtExportData.Columns.Count; j++)
                    {
                        DataColumn dc = dtExportData.Columns[j];
                        string tempValue = dr[dc.ColumnName].ToString();
                        ICell cell = rowData.CreateCell(j);

                        if (!string.IsNullOrEmpty(tempValue) && tempValue.Trim() != "?")
                        {
                            DbType dbType = GetDataType(dc.ColumnName, subjectInfo);
                            if (dbType == DbType.Date)
                            {
                                try
                                {
                                    cell.SetCellValue(Convert.ToDateTime(tempValue).ToString("MM/dd/yyyy"));
                                }
                                catch
                                {
                                    cell.SetCellValue(tempValue);
                                }
                            }
                            else if (dbType == DbType.DateTime)
                            {
                                try
                                {
                                    cell.SetCellValue(Convert.ToDateTime(tempValue).ToString("MM/dd/yyyy HH:mm:ss"));

                                }
                                catch
                                {
                                    cell.SetCellValue(tempValue);
                                }
                            }
                            else if (dbType == DbType.Int32)
                            {

                                cell.SetCellValue(Convert.ToInt32(Convert.ToDouble(tempValue.Replace(",", ".").Replace("\r", "0"))));
                            }
                            else if (dbType == DbType.Decimal || dbType == DbType.Double)
                            {

                                cell.SetCellValue(Convert.ToDouble(tempValue.Replace(",", ".").Replace("\r", "0")));
                            }
                            else
                            {
                                cell.SetCellValue(tempValue);
                            }
                        }
                        else
                        {
                            cell.SetCellValue(tempValue);
                        }
                    }
                }

                //Write File            
                workbook.Write(file);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                file.Close();
            }
        }
        private void ExportCsv(string csvFileName, DataTable dtExportData, DataTable dtExportHead, SubjectInfo subjectInfo)
        {
            string csvSplit = ",";

            StreamWriter sw = null;
            if (csvSplit == ";")
                sw = new StreamWriter(csvFileName, false, Encoding.Default);
            else
                sw = new StreamWriter(csvFileName);
            try
            {
                string strLine = string.Empty;
                foreach (DataRow dr in dtExportHead.Rows)
                {
                    strLine += csvSplit + dr["HeadText"].ToString();
                }
                if (!string.IsNullOrEmpty(strLine))
                {
                    sw.WriteLine(strLine.Substring(1));
                }

                foreach (DataRow dr in dtExportData.Rows)
                {
                    strLine = string.Empty;
                    foreach (DataColumn dc in dtExportData.Columns)
                    {
                        string tempValue = dr[dc.ColumnName].ToString();
                        if (!string.IsNullOrEmpty(tempValue) && tempValue.Trim() != "?")
                        {
                            DbType dbType = GetDataType(dc.ColumnName, subjectInfo);
                            if (dbType == DbType.Date)
                            {
                                try
                                {
                                    tempValue = Convert.ToDateTime(dr[dc.ColumnName].ToString()).ToString("MM/dd/yyyy");

                                    tempValue = "\"" + tempValue + "\"";
                                }
                                catch
                                {
                                    tempValue = "\"" + tempValue + "\"";
                                }
                            }
                            else if (dbType == DbType.DateTime)
                            {
                                try
                                {
                                    tempValue = Convert.ToDateTime(dr[dc.ColumnName].ToString()).ToString("MM/dd/yyyy HH:mm:ss");

                                    tempValue = "\"" + tempValue + "\"";
                                }
                                catch
                                {
                                    tempValue = "\"" + tempValue + "\"";
                                }
                            }
                            else if (dbType == DbType.Decimal || dbType == DbType.Double || dbType == DbType.Int32)
                            {
                                double a = 0;
                                if (double.TryParse(dr[dc.ColumnName].ToString(), out a))
                                {
                                    tempValue = Convert.ToDouble(dr[dc.ColumnName].ToString().Replace(",", "").Replace("\r", "0")).ToString();
                                }
                                else
                                {
                                    tempValue = dr[dc.ColumnName].ToString().Replace("\"", "\"\"");
                                    tempValue = "\"" + tempValue + "\"";
                                }
                            }
                            else
                            {
                                tempValue = dr[dc.ColumnName].ToString().Replace("\"", "\"\"");
                                tempValue = "\"" + tempValue + "\"";
                            }
                        }

                        strLine += csvSplit + tempValue;
                    }
                    if (!string.IsNullOrEmpty(strLine))
                    {
                        sw.WriteLine(strLine.Substring(1));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sw.Flush();
                sw.Close();
            }
        }
        private void ExportCsvCompress(string csvFileName, string zipFileName, SubjectInfo subjectInfo, SubjectSearchArgs searchArgs)
        {
            try
            {

                //Generate zip file
                DwqZip.ZipFile(csvFileName, zipFileName, "");
                File.Delete(csvFileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private DbType GetDataType(string gridHeadText, SubjectInfo subjectInfo)
        {
            DbType type = new DbType();
            for (int i = 0; i < subjectInfo.Details.Count; i++)
            {
                SubjectDetailInfo detail = subjectInfo.Details[i];
                if (gridHeadText == detail.FieldName)
                {
                    type = detail.FieldType;
                    break;
                }
            }
            return type;
        }
        private void DeleteRefuseFiles()
        {
            try
            {
                string virtualPath = Server.MapPath(ConfigurationManager.AppSettings["DownLoadVirtualPath"]);
                DirectoryInfo folderPath = new DirectoryInfo(virtualPath);
                foreach (FileInfo file in folderPath.GetFiles())
                {
                    if (DateTime.Now > DateTime.Parse(file.Name.Substring(file.Name.IndexOf("_") + 1, 10)).AddDays(1.0))
                    {
                        file.Delete();
                    }
                }
            }
            catch { }
        }

        #endregion
    }

}
