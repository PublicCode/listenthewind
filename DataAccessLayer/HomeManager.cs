using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using DataAccess.DC;
using IDataAccessLayer;
using ComLib.SmartLinq;
using ComLib.SmartLinq.Energizer.JqGrid;
using WebModel.Camp;
using ComLib.Extension;

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

        public camp GetSingleCamp(int CampID)
        {
            DC dc = DCLoader.GetMyDC();
            camp campInDB = dc.camps.Include("Listcampcomment").Include("Listcamphost").Include("Listcampitem").Include("Listcampphoto").Include("Listcamphost.Listcamphostlanguage").Include("Listcamppile").Include("Listcampprice").Include("Listcamptype").FirstOrDefault(c => c.CampID == CampID);
            //camp campInDB = dc.camps.FirstOrDefault(c => c.CampID == CampID);
            return campInDB;
        }

        public campModel GetCamp(int CampID)
        {
            campModel campmodel = new campModel();
            camp campInDb = GetSingleCamp(CampID);
            ModelConverter.Convert<camp, campModel>(campInDb, campmodel);

            campmodel.ModelListcampcomment = new List<campcommentModel>();

            foreach (campcomment campcomInDB in campInDb.Listcampcomment)
            {
                campcommentModel campcommentmodel = new campcommentModel();
                ModelConverter.Convert<campcomment, campcommentModel>(campcomInDB, campcommentmodel);
                campmodel.ModelListcampcomment.Add(campcommentmodel);
            }

            campmodel.ModelListcamphost = new List<camphostModel>();

            foreach (camphost camphostInDB in campInDb.Listcamphost)
            {
                camphostModel camphostmodel = new camphostModel();
                ModelConverter.Convert<camphost, camphostModel>(camphostInDB, camphostmodel);

                camphostmodel.ModelListcamphostlanguage = new List<camphostlanguageModel>();

                foreach (camphostlanguage languageInDb in camphostInDB.Listcamphostlanguage)
                {
                    camphostlanguageModel languagemodel = new camphostlanguageModel();
                    ModelConverter.Convert<camphostlanguage, camphostlanguageModel>(languageInDb, languagemodel);
                    camphostmodel.ModelListcamphostlanguage.Add(languagemodel);
                }
                campmodel.ModelListcamphost.Add(camphostmodel);
            }

            campmodel.ModelListcampitem = new List<campitemModel>();

            foreach (campitem campitemInDB in campInDb.Listcampitem)
            {
                campitemModel campitemmodel = new campitemModel();
                ModelConverter.Convert<campitem, campitemModel>(campitemInDB, campitemmodel);
                campmodel.ModelListcampitem.Add(campitemmodel);
            }


            campmodel.ModelListcampphoto = new List<campphotoModel>();

            foreach (campphoto campphotoInDB in campInDb.Listcampphoto)
            {
                campphotoModel campphotomodel = new campphotoModel();
                ModelConverter.Convert<campphoto, campphotoModel>(campphotoInDB, campphotomodel);
                campmodel.ModelListcampphoto.Add(campphotomodel);
            }

            campmodel.ModelListcamppile = new List<camppileModel>();

            foreach (camppile camppileInDB in campInDb.Listcamppile)
            {
                camppileModel camppilemodel = new camppileModel();
                ModelConverter.Convert<camppile, camppileModel>(camppileInDB, camppilemodel);
                campmodel.ModelListcamppile.Add(camppilemodel);
            }

            campmodel.ModelListcampprice = new List<camppriceModel>();

            foreach (campprice camppriceInDB in campInDb.Listcampprice)
            {
                camppriceModel camppricemodel = new camppriceModel();
                ModelConverter.Convert<campprice, camppriceModel>(camppriceInDB, camppricemodel);
                campmodel.ModelListcampprice.Add(camppricemodel);
            }

            campmodel.ModelListcamptype = new List<camptypeModel>();

            foreach (camptype camptypeInDB in campInDb.Listcamptype)
            {
                camptypeModel camptypemodel = new camptypeModel();
                ModelConverter.Convert<camptype, camptypeModel>(camptypeInDB, camptypemodel);
                campmodel.ModelListcamptype.Add(camptypemodel);
            }


            return campmodel;
        }
    }
}
