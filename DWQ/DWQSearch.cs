using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DataAccess.Database;
using DWQ.Builder;
using DWQ.Subject;
using System.Text;


namespace DWQ
{
    public class DWQSearch
    {
        public static DataTable Search(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData,int maximumRows, int startRowIndex)
        {
            if (isReset || subjectId == "0")
                return null;
            string sql = DwqSqlBuilder.GetDwqSqlForPage(subjectId, ctrlValue, isReset, isFullTextSearch, fullTextData, maximumRows, startRowIndex);

            DataTable dt = GetDataTable(sql);
            return dt;
        }


        public static DataTable GetExport(string subjectId, Dictionary<string, string> ctrlValue)
        {
            string sql = DwqSqlBuilder.GetExportSql(subjectId, ctrlValue);
            DataTable dt = GetDataTable(sql);
            return dt;
        }
        public static int SearchCount(string subjectId, Dictionary<string, string> ctrlValue, bool isReset, bool isFullTextSearch, string fullTextData)
        {
            if (isReset)
                return 0;
            string sql = DwqSqlBuilder.GetDwqSqlForPageCount(subjectId, ctrlValue, isReset, isFullTextSearch, fullTextData);
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                int count = Convert.ToInt32(db.ExecuteScalar(command).ToString());
                return count;
            }
        }

        public static DataTable GetExprotData(SubjectSearchArgs args)
        {
            string sql = DwqSqlBuilder.GetDwqSqlForExport(args).ToString();
            return GetDataTable(sql);
        }

        public static DataTable GetCodeTable(SubjectSearchFormInfo formInfo, params string[][] prereqs)
        {
            string sql = DwqSqlBuilder.GetDwqSqlForCodeTable(formInfo, prereqs);
            return GetDataTable(sql);
        }

        private static DataTable GetDataTable(string sql)
        {
            Database db = DatabaseFactory.CreateDatabase();
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                command.CommandTimeout = 3600;
                DataTable dt = db.ExecuteDataSet(command).Tables[0];
                return dt;
            }
        }
        public static DataTable GetExportHead(SubjectSearchArgs args)
        {
            string sql = DwqSqlBuilder.GetDwqSqlForExportHead(args);

            return GetDataTable(sql);
        }
        public static DataTable GetExportData(SubjectSearchArgs args)
        {
            DataTable dt = new DataTable();
            StringBuilder sql = new StringBuilder();
            try
            {
                sql = DwqSqlBuilder.GetDwqSqlForExport(args);

                dt = GetDataTable(sql.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dt;
        }

    }
}
