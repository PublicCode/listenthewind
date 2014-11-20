using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataAccess.DC;
using IDataAccessLayer;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;

namespace DataAccessLayer
{
    public class HomeManager : IHomeManager
    {
        public string GetNewNumber(string TypeName, string strFirstChar)
        {
            DC dc = DCLoader.GetNewDC();
            TableNumberEnt qn = new TableNumberEnt();
                qn = dc.TableNumberEnt.Where(c => c.Type == TypeName && c.FirstChar == strFirstChar).FirstOrDefault();
            int newNumber = 0;
            if (qn == null)
            {
                dc.Database.ExecuteSqlCommand("insert into TableNumberEnt(Type,NumberID,FirstChar) values('" + TypeName + "','1','" + strFirstChar + "')");
                dc.SaveChanges();
                newNumber = 1;
            }
            else
            {
                dc.Database.ExecuteSqlCommand("update TableNumberEnt set NumberID=@NumberID where ID=" + qn.ID, new SqlParameter("@NumberID", qn.NumberID + 1));
                dc.SaveChanges();
                newNumber = qn.NumberID + 1;
            }

            string strNewNumber= strFirstChar + newNumber.ToString("00000");
            //check number exists
            if (TypeName == "Quote" && strFirstChar == "R")
            {

            }

            return strNewNumber;
        }
        /// <summary>
        /// Get list of history entry
        /// </summary>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public List<UserOperationLog> GetOperationLogList(string[] objectType, string objectValue)
        {
            DC dc = DCLoader.GetMyDC();
            return dc.UserOperationLogs.Include("OperationDetails").WhereIn(c => c.OperationType, objectType)
                .Where(c => c.OID == objectValue)
                .Where(m => m.OperationInfo.Contains("Add") || m.OperationInfo.Contains("Clone") || m.OperationInfo.Contains("Re quote config") || m.OperationDetails.Any(d => d.ChangeFrom != "")).OrderByDescending(m => m.OperationTime).ToList();
        }

        public string TestMysqlDB()
         {
             DC dc = DCLoader.GetMyDC();
             int num = dc.accountnumbers.Count();
             accountnumber a = dc.accountnumbers.FirstOrDefault();
             return num.ToString();
         }
    }
}
