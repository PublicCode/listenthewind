using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using DataAccess.Database;

namespace DWQ
{
    public class DWQUtil
    {
        public static DataTable GetDDlData(string tableName, Dictionary<string, string[]> inputData, string outPutField)
        {
            string sql = "select DISTINCT(" + outPutField + ") from " + tableName;

            string sqlWhere = " where ";

            foreach (KeyValuePair<string, string[]> kvp in inputData)
            {
                string[] Value = kvp.Value;
                string Key = kvp.Key;
                sqlWhere += " (";
                foreach (string str in Value)
                {
                    sqlWhere += Key + "='" + str + "' or ";
                }
                sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 3);
                sqlWhere += ") and ";
            }
            sqlWhere = sqlWhere.Substring(0, sqlWhere.Length - 4);
            Database db = DatabaseFactory.CreateDatabase();
            if (inputData.Count > 0)
                sql += sqlWhere;
            using (DbCommand command = db.GetSqlStringCommand(sql))
            {
                return db.ExecuteDataSet(command).Tables[0];
            }
        }
    }
}
