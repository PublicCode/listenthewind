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
using Newtonsoft.Json;

namespace DataAccessLayer
{
    public class HomeManager : BaseManager, IHomeManager
    {
        protected User _user;
        public HomeManager(User user)
        {
            _user = user;
        }
        public HomeManager()
        { }
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
        public List<string> GetListOfReserveForPile(int PileId, int userID)
        {
            var ret = new List<string>();
            var listOfReserve = dc.campreservedates.Where(m => m.CampPileID == PileId && (m.myReserve.ReserveStatus == 2 || m.myReserve.UserID == userID)).Select(m => m.CampReserveDate).ToList();
            foreach (DateTime dt in listOfReserve)
            {
                ret.Add(dt.ToShortDateString());
            }
            return ret;
        }

        public object SaveComments(int CampID, string CommentCon)
        {
            try
            {
                DC dc = DCLoader.GetMyDC();
                campcomment campcommentDB = new campcomment();
                campcommentDB.CampID = CampID;
                campcommentDB.CommentCon = CommentCon;
                campcommentDB.UserID = _user.UserID;//需要修改
                campcommentDB.UserName = _user.UserName;//需要修改
                campcommentDB.CommentTime = DateTime.Now;
                dc.campcomments.Add(campcommentDB);
                dc.SaveChanges();

                List<campcomment> listdb = dc.campcomments.Where(c => c.CampID == CampID).OrderByDescending(c => c.CommentTime).ToList();

                List<campcommentModel> list = new List<campcommentModel>();

                foreach (campcomment campcomInDB in listdb)
                {
                    campcommentModel campcommentmodel = new campcommentModel();
                    ModelConverter.Convert<campcomment, campcommentModel>(campcomInDB, campcommentmodel);
                    list.Add(campcommentmodel);
                }
                return new
                {
                    content = "添加评论成功！",
                    campcomments = list
                };

            }
            catch (Exception ex)
            {
                return new
                {
                    content = ex.Message
                };
            }
        }

        public campModel GetCamp(int CampID,DateTime? dt)
        {
            int userid = 0;
            if (_user != null)
            {
                userid = _user.UserID;
            }

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
                        if (camprese.ReserveStatus == 2 || camprese.UserID == userid)
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
        public List<CityModel> GetCitys()
        {
            DC dc = DCLoader.GetMyDC();
            var lstEF = dc.Citys.ToList();
            var lstDTO = new List<CityModel>();
            foreach (var info in lstEF)
            {
                var dto = new CityModel
                {
                    CityID = info.CityID,
                    CityName = info.CityName,
                    Locations = dc.CityLocations.Where(c => c.CityID == info.CityID).ToList().Select(c => CityLocationModel.FromEFToDTO(c)).ToList()
                };
                lstDTO.Add(dto);
            }
            return lstDTO;
        }
        public List<basicdatacollect> GetBasicData()
        {
            DC dc = DCLoader.GetMyDC();
            return dc.basicdatacollects.ToList();
        }
        public object GetCampList(CampListSeachModel info, int page, int limit)
        {
            DC dc = DCLoader.GetMyDC();
            string sql = "select CampID from Camp where 1=1";
            if (info.LocationID != 0)
            {
                sql += " and LocID = '" + info.LocationID + "'";
            }
            if (!string.IsNullOrEmpty(info.JoinCampDate))
            {
                sql += "  and (select count(*) from camppile where camppile.CampID = Camp.CampID) <> (select count(*) from campreservedate where campreservedate.CampID = Camp.CampID and CampReserveDate = '" + info.DBJoinCampDate.ToString() + "')";
            }
            if (info.PriceStart != null)
            {
                sql += " and pileprice >=" + info.PriceStart + " and pileprice <= " + info.PriceEnd;
            }
            if (!string.IsNullOrEmpty(info.CampLOD))
            {
                sql += " and camplod = '" + info.CampLOD + "'";
            }
            if (info.SpecialContents != null)
            {
                if (info.SpecialContents.Count > 0)
                {
                    var campitemids = string.Join(",", info.SpecialContents);
                    sql += " and CampID in (select distinct CampID from campitem where BasicID in (" + campitemids + "))";
                }
            }
            if (info.SpecialContents != null)
            {
                if (info.CampType.Count > 0)
                {
                    var camptypeids = string.Join(",", info.CampType);
                    sql += " and CampID in (select distinct CampID from camptype where BasicID in (" + camptypeids + "))";
                }
            }
            if (info.SpecialContents != null)
            {
                if (info.HostLang.Count > 0)
                {
                    var hostsLangids = string.Join(",", info.HostLang);
                    sql += " and CampID in (select CampID from CampHost where camphostid in (select distinct camphostid from CampHostLanguage where BasicID in (" + hostsLangids + ")))";
                }
            }
            if (!string.IsNullOrEmpty(info.KeyContent))
            {
                sql += " and (CampName like '%" + info.KeyContent + "%' or CampIntro like '%" + info.KeyContent + "%')";
            }
            var camIds = dc.Database.SqlQuery<int>(sql).ToList();
            var lstEF = dc.camps.Include("Listcampitem").Where(c => camIds.Contains(c.CampID));
            return GetCampListObj(lstEF, page, limit, info);
        }

        public object GetCampListObj(IEnumerable<camp> lstEF, int page, int limit, CampListSeachModel info)
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
                rows = res.ToList(),
                searchInfo = info
            };
        }
        public bool CheckCampCollect(int CampID)
        {
            DC dc = DCLoader.GetMyDC();
            int userid = _user.UserID;
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
                int userid = _user.UserID;
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

        public string SaveReserve(List<DateTime> SelectedDate, List<camppriceModel> SelectedItem, int CampID, int PileID)
        {
            try
            {
                DC dc = DCLoader.GetMyDC();
                int userid = _user.UserID;
                camp campDB = GetSingleCamp(CampID);
                campreserve campres = new campreserve();
                campres.CampID = CampID;
                campres.UserID = userid;
                campres.CampPileID = PileID;
                campres.PilePrice = campDB.PilePrice;
                campres.Discount = 0;
                campres.FinalPilePrice = campres.PilePrice - campres.Discount;
                campres.Days = SelectedDate.Count;
                campres.PilePriceAmt = campres.FinalPilePrice * campres.Days;
                campres.ReserveStatus = 1;
                campres.Createtime = DateTime.Now;

                if (SelectedItem != null)
                {
                    campres.Listcampreserveatt = new List<campreserveatt>();
                    foreach (camppriceModel campPrice in SelectedItem)
                    {
                        if (campPrice.Checked)
                        {
                            campprice camppriceDB = campDB.Listcampprice.FirstOrDefault(c => c.CampPriceID == campPrice.CampPriceID);
                            campreserveatt campresatt = new campreserveatt();
                            campresatt.CampItemID = campPrice.CampPriceID;
                            campresatt.CampItemName = camppriceDB.ItemName;
                            campresatt.CampItemUnitPrice = camppriceDB.ItemPrice;
                            campresatt.CampItemDiscount = 0;
                            campresatt.CampItemFinalPrice = campresatt.CampItemUnitPrice - campresatt.CampItemDiscount;
                            campresatt.Qty = campPrice.Qty;
                            campresatt.CampItemPriceAmt = campresatt.Qty * campresatt.CampItemFinalPrice;
                            campres.Listcampreserveatt.Add(campresatt);
                        }
                    }
                }

                campres.Listcampreservedate = new List<campreservedate>();
                foreach (DateTime ReserveDate in SelectedDate)
                {
                    campreservedate campresdate = new campreservedate();
                    campresdate.CampPileID = PileID;
                    campresdate.CampID = CampID;
                    campresdate.CampReserveDate = ReserveDate;
                    campres.Listcampreservedate.Add(campresdate);
                }
                dc.campreserves.Add(campres);
                dc.SaveChanges();
                return "添加预约成功";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string AddCancelRequest(int CampReserveID)
        {
            try
            {
                DC dc = DCLoader.GetMyDC();
                int userid = _user.UserID;
                campreserve reserve = dc.campreserves.FirstOrDefault(m => m.CampReserveID == CampReserveID);
                //Change status to 4 cancel request.
                reserve.ReserveStatus = 4;
                cancelreserve cancelRequest = new cancelreserve();
                cancelRequest.CampReserveID = CampReserveID;
                cancelRequest.CancelledOn = DateTime.Now;
                dc.Cancels.Add(cancelRequest);
                dc.SaveChanges();
                return "提交成功";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string CancelOrder(int CampReserveID)
        {
            try
            {
                DC dc = DCLoader.GetMyDC();
                int userid = _user.UserID;
                var reserveDates = dc.campreservedates.Where(m => m.CampReserveID == CampReserveID);
                var reserveAtt = dc.campreserveatts.Where(m => m.CampReserveID == CampReserveID);
                var reserve = dc.campreserves.FirstOrDefault(m => m.CampReserveID == CampReserveID);
                foreach (campreservedate date in reserveDates)
                {
                    dc.campreservedates.Remove(date);
                }

                foreach (campreserveatt attr in reserveAtt)
                {
                    dc.campreserveatts.Remove(attr);
                }

                dc.campreserves.Remove(reserve);
                dc.SaveChanges();
                return "Cancelled";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public IQueryable<campreserve> GetReserve(int statusId = 0)
        { 
            if(statusId !=0)
            {
                var res = dc.campreserves.Include("ReservePile").Include("Listcampreserveatt").Include("Listcampreservedate").Include("ReservePile.MyCamp").Where(c => c.ReserveStatus == statusId && c.UserID == _user.UserID);
                return res;
            }
            else
            {
                var res = dc.campreserves.Include("ReservePile").Include("Listcampreserveatt").Where(c=>c.UserID == _user.UserID);
                return res;
            }
        }
    }
}
