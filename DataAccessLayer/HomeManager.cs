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
using DataAccessLayer.DTO;

namespace DataAccessLayer
{
    public class HomeManager : BaseManager, IHomeManager
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

            string strNewNumber = strFirstChar + newNumber.ToString("00000");
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

        public campModel GetCamp(int CampID,DateTime? dt)
        {
            int userid = 1;

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
                if (!dt.HasValue)
                {
                    if (camppilemodel.Active == 1)
                    {
                        camppilemodel.Flag = true;
                    }
                    else
                    {
                        camppilemodel.Flag = false;
                    }
                }
                else
                {
                    campreservedate reserdate = dc.campreservedates.FirstOrDefault(c => c.CampReserveDate == dt && c.CampPileID == camppilemodel.PileID);
                    if (reserdate == null && camppilemodel.Active == 1)
                    {
                        camppilemodel.Flag = true;
                    }
                    else
                    {
                        campreserve camprese = dc.campreserves.FirstOrDefault(c => c.CampReserveID == reserdate.CampReserveID);
                        //已交款 或者未交款但是自己已经预约这天的 少个验证 用户是否登陆
                        if (camprese.ReserveStatus == "2" || camprese.UserID == userid)
                        {
                            camppilemodel.Flag = false;
                        }
                        else
                        {
                            camppilemodel.Flag = true;
                        }

                    }
                }
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
        public List<CityDTO> GetCitys()
        {
            DC dc = DCLoader.GetMyDC();
            var lstEF = dc.Citys.ToList();
            var lstDTO = new List<CityDTO>();
            foreach (var info in lstEF)
            {
                var dto = new CityDTO
                {
                    CityID = info.CityID,
                    CityName = info.CityName,
                    Locations = dc.CityLocations.Where(c => c.CityID == info.CityID).ToList().Select(c => CityLocationDTO.FromEFToDTO(c)).ToList()
                };
                lstDTO.Add(dto);
            }
            return lstDTO;
        }
        public object GetCampList(int locId, DateTime dateTime, int page, int limit)
        {
            DC dc = DCLoader.GetMyDC();
            var camIds = new List<int>();
            var lstId = dc.camps.Where(c => c.LocID == locId).Select(c => c.CampID).ToList();
            foreach (var id in lstId)
            {
                var allCount = dc.camppiles.Where(c => c.CampID == id).ToList().Count();
                var freeCount = dc.campreservedates.Where(c => c.CampID == id && c.CampReserveDate == dateTime).ToList().Count();
                if (allCount != freeCount)
                {
                    camIds.Add(id);
                }
            }
            var lstEF = dc.camps.Where(c => camIds.Contains(c.CampID)).ToList();
            return GetCampListObj(lstEF, page, limit);
        }
        public object GetCampListObj(List<camp> lstEF, int page, int limit)
        {
            IEnumerable<camp> res = lstEF;
            if (page > 0)
            {
                int skipPages = page - 1;
                res = lstEF.Skip(skipPages * limit);
            }
            if (limit > 0)
            {
                res = res.Take(limit);
            }
            int count = lstEF.Count();
            return new
            {
                total = limit > 0 ? Math.Ceiling((double)count / limit) : 1,
                page = page,
                records = count,
                rows = res.ToList()
            };
        }
        public bool CheckCampCollect(int CampID)
        {
            DC dc = DCLoader.GetMyDC();
            int userid = 1;
            campcollect coll = dc.campcollects.FirstOrDefault(c => c.UserID == userid && c.CampID == CampID);
            if (coll == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string AddCampCollect(int CampID)
        {
            try
            {
                DC dc = DCLoader.GetMyDC();
                int userid = 1;
                campcollect coll = new campcollect();
                coll.CampID = CampID;
                coll.UserID = userid;
                dc.campcollects.Add(coll);
                dc.SaveChanges();
                return "添加收藏成功";
                //Save(_user);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
