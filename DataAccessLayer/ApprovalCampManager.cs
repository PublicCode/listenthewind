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
using WebModel.ApprovalCamp;

namespace DataAccessLayer
{
    public class ApprovalCampManager : BaseManager, IApprovalCampManager
    {
        protected User _user;
        public ApprovalCampManager(User user)
        {
            _user = user;
        }
        public ApprovalCampManager() { }

        #region List
        public IEnumerable<approvalcamplist> GetApprovalCampList()
        {
            var query = dc.approvalcamplists.AsQueryable();
            if (this._user.UserType == "2")
                query = query.Where(c => c.CreateByID == this._user.UserID);
            else if (this._user.UserType == "3")
            {
                query = query.Where(c => c.ManagedByID == this._user.UserID && c.ApprovalStatus == "待审批");
            }
            return query;
        }
        #endregion

        #region Camp Add / Delete / Update
        public approvalcampModel GetCamp(int CampID, DateTime? dt)
        {
            int userid = 0;
            if (_user != null)
            {
                userid = _user.UserID;
            }
            approvalcampModel campmodel = new approvalcampModel();
            var campInDb = GetSingleCamp(CampID);
            if (campInDb != null)
                ModelConverter.Convert<approvalcamp, approvalcampModel>(campInDb, campmodel);

            #region Camp Comment
            campmodel.ModelListcampcomment = new List<approvalcampcommentModel>();
            if (campInDb != null)
            {
                foreach (campcomment campcomInDB in campInDb.Listapprovalcampcomment)
                {
                    var campcommentmodel = new approvalcampcommentModel();
                    ModelConverter.Convert<campcomment, approvalcampcommentModel>(campcomInDB, campcommentmodel);
                    campmodel.ModelListcampcomment.Add(campcommentmodel);
                }
            }
            #endregion

            #region Camp Host
            campmodel.ModelListcamphost = new List<approvalcamphostModel>();
            if (campInDb != null)
            {
                foreach (var camphostInDB in campInDb.Listapprovalcamphost)
                {
                    var camphostmodel = new approvalcamphostModel();
                    ModelConverter.Convert<approvalcamphost, approvalcamphostModel>(camphostInDB, camphostmodel);

                    camphostmodel.ModelListcamphostlanguage = new List<approvalcamphostlanguageModel>();

                    foreach (var languageInDb in camphostInDB.Listapprovalcamphostlanguage)
                    {
                        var languagemodel = new approvalcamphostlanguageModel();
                        ModelConverter.Convert<approvalcamphostlanguage, approvalcamphostlanguageModel>(languageInDb, languagemodel);
                        camphostmodel.ModelListcamphostlanguage.Add(languagemodel);
                    }
                    campmodel.ModelListcamphost.Add(camphostmodel);
                }
            }
            else {
                var camphostmodel = new approvalcamphostModel();
                var languagemodel = new approvalcamphostlanguageModel();
                camphostmodel.ModelListcamphostlanguage.Add(languagemodel);
                campmodel.ModelListcamphost.Add(camphostmodel);
            }
            #endregion

            #region Camp Item
            campmodel.ModelListcampitem = new List<approvalcampitemModel>();
            var campItem = dc.basicdatacollects.Where(c => c.DataType == "campitem").ToList();
            foreach (var item in campItem)
            {
                var campitemmodel = new approvalcampitemModel();
                var ci = campInDb != null ? campInDb.Listapprovalcampitem.FirstOrDefault(c => c.BasicID == item.BasicDataCollectID) : null;
                if (ci != null)
                {
                    ModelConverter.Convert<approvalcampitem, approvalcampitemModel>(ci, campitemmodel);
                    campitemmodel.Checked = true;
                }
                else
                {
                    campitemmodel.CampItemID = 0;
                    campitemmodel.CampID = campInDb != null ? campInDb.CampID : 0;
                    campitemmodel.CampItemName = item.DataName;
                    campitemmodel.CampItemIcon = item.DataIcon;
                    campitemmodel.CampItemSort = (int)item.DataSort;
                    campitemmodel.BasicID = item.BasicDataCollectID;
                    campitemmodel.Checked = false;
                }
                campmodel.ModelListcampitem.Add(campitemmodel);
            }
            #endregion

            #region Camp Photo
            campmodel.ModelListcampphoto = new List<approvalcampphotoModel>();

            if (campInDb != null)
            {
                foreach (var campphotoInDB in campInDb.Listapprovalcampphoto)
                {
                    var campphotomodel = new approvalcampphotoModel();
                    ModelConverter.Convert<approvalcampphoto, approvalcampphotoModel>(campphotoInDB, campphotomodel);
                    campmodel.ModelListcampphoto.Add(campphotomodel);
                }
            }
            #endregion

            #region Camp Pile
            campmodel.ModelListcamppile = new List<approvalcamppileModel>();

            if (campInDb != null)
            {
                foreach (var camppileInDB in campInDb.Listapprovalcamppile)
                {
                    var camppilemodel = new approvalcamppileModel();
                    ModelConverter.Convert<approvalcamppile, approvalcamppileModel>(camppileInDB, camppilemodel);
                    if (camppilemodel.Active == 1)
                    {
                        camppilemodel.Flag = true;
                    }
                    else
                    {
                        camppilemodel.Flag = false;
                    }
                    campmodel.ModelListcamppile.Add(camppilemodel);
                }
            }
            #endregion

            #region Camp Price
            campmodel.ModelListcampprice = new List<approvalcamppriceModel>();
            if (campInDb != null)
            {
                foreach (var camppriceInDB in campInDb.Listapprovalcampprice)
                {
                    var camppricemodel = new approvalcamppriceModel();
                    ModelConverter.Convert<approvalcampprice, approvalcamppriceModel>(camppriceInDB, camppricemodel);
                    campmodel.ModelListcampprice.Add(camppricemodel);
                }
            }
            #endregion

            #region Camp Type
            campmodel.ModelListcamptype = new List<approvalcamptypeModel>();
            var campType = dc.basicdatacollects.Where(c => c.DataType == "camptype").ToList();
            foreach (var item in campType)
            {
                var camptypemodel = new approvalcamptypeModel();
                var ci = campInDb != null ? campInDb.Listapprovalcamptype.FirstOrDefault(c => c.BasicID == item.BasicDataCollectID) : null;
                if (ci != null)
                {
                    ModelConverter.Convert<approvalcamptype, approvalcamptypeModel>(ci, camptypemodel);
                    camptypemodel.Checked = true;
                }
                else
                {
                    camptypemodel.CampTypeID = 0;
                    camptypemodel.CampID = campInDb != null ? campInDb.CampID : 0;
                    camptypemodel.CampTypeName = item.DataName;
                    camptypemodel.BasicID = item.BasicDataCollectID;
                    camptypemodel.Checked = false;
                }
                campmodel.ModelListcamptype.Add(camptypemodel);
            }
            #endregion

            return campmodel;
        }
        public approvalcamp GetSingleCamp(int CampID)
        {
            DC dc = DCLoader.GetMyDC();
            var campInDB = dc.approvalcamps
                .Include("Listapprovalcampcomment")
                .Include("Listapprovalcamphost")
                .Include("Listapprovalcamphost.Listapprovalcamphostlanguage")
                .Include("Listapprovalcampitem")
                .Include("Listapprovalcampphoto")
                .Include("Listapprovalcamppile")
                .Include("Listapprovalcampprice")
                .Include("Listapprovalcamptype")
                .FirstOrDefault(c => c.CampID == CampID);

            return campInDB;
        }
        public int SaveCamp(approvalcampModel info)
        {
            if (info.CampID > 0)
            {
                var origAppCamp = dc.approvalcamps.FirstOrDefault(c => c.CampID == info.CampID);
                if (origAppCamp != null)
                {
                    #region Approval Camp
                    var newAppCamp = new approvalcamp();
                    ModelConverter.Convert(info, newAppCamp);
                    dc.Entry(origAppCamp).CurrentValues.SetValues(newAppCamp);
                    #endregion

                    #region Approval Camp Item
                    if (info.ModelListcampitem != null)
                    {
                        foreach (var nInfo in info.ModelListcampitem)
                        {
                            if (nInfo.Checked)
                            {
                                var efInfo = new approvalcampitem();
                                ModelConverter.Convert(nInfo, efInfo);
                                if (efInfo.CampItemID > 0)
                                {
                                    var origAppCampItem = dc.approvalcampitems.FirstOrDefault(c => c.CampItemID == nInfo.CampItemID);
                                    if (origAppCampItem != null)
                                    {
                                        dc.Entry(origAppCampItem).CurrentValues.SetValues(efInfo);
                                    }
                                }
                                else
                                {
                                    dc.approvalcampitems.Add(efInfo);
                                }
                            }
                        }
                        var origItem = dc.approvalcampitems.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcampitem.Where(c => c.CampItemID > 0 && c.Checked).Select(c => c.CampItemID);
                        foreach (var item in origItem.Where(c => !newIds.Any(d => c.CampItemID == d)))
                        {
                            dc.approvalcampitems.Remove(item);
                        }
                    }
                    else {
                        var origItem = dc.approvalcampitems.Where(c => c.CampID == info.CampID).ToList();
                        foreach (var item in origItem)
                        {
                            dc.approvalcampitems.Remove(item);
                        }
                    }
                    #endregion

                    #region Approval Camp Photo
                    if (info.ModelListcampphoto != null)
                    {
                        foreach (var nInfo in info.ModelListcampphoto)
                        {
                            var efInfo = new approvalcampphoto();
                            ModelConverter.Convert(nInfo, efInfo);
                            if (efInfo.CampPhotoID > 0)
                            {
                                var origAppCampPhoto = dc.approvalcampphotos.FirstOrDefault(c => c.CampPhotoID == nInfo.CampPhotoID);
                                if (origAppCampPhoto != null)
                                {
                                    dc.Entry(origAppCampPhoto).CurrentValues.SetValues(efInfo);
                                }
                            }
                            else
                            {
                                dc.approvalcampphotos.Add(efInfo);
                            }
                        }
                        var origPhoto = dc.approvalcampphotos.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcampphoto.Where(c => c.CampPhotoID > 0).Select(c => c.CampPhotoID);
                        foreach (var photo in origPhoto.Where(c => !newIds.Any(d => c.CampPhotoID == d)))
                        {
                            dc.approvalcampphotos.Remove(photo);
                        }
                    }
                    else {
                        var origPhoto = dc.approvalcampphotos.Where(c => c.CampID == info.CampID).ToList();
                        foreach (var photo in origPhoto)
                        {
                            dc.approvalcampphotos.Remove(photo);
                        }
                    }
                    #endregion

                    #region Approval Camp Pile
                    if (info.ModelListcamppile != null)
                    {
                        foreach (var nInfo in info.ModelListcamppile)
                        {
                            var efInfo = new approvalcamppile();
                            ModelConverter.Convert(nInfo, efInfo);
                            if (efInfo.PileID > 0)
                            {
                                var origAppCampPile = dc.approvalcamppiles.FirstOrDefault(c => c.PileID == nInfo.PileID);
                                if (origAppCampPile != null)
                                {
                                    dc.Entry(origAppCampPile).CurrentValues.SetValues(efInfo);
                                }
                            }
                            else
                            {
                                dc.approvalcamppiles.Add(efInfo);
                            }
                        }
                        var origPile = dc.approvalcamppiles.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcamppile.Where(c => c.PileID > 0).Select(c => c.PileID);
                        foreach (var photo in origPile.Where(c => !newIds.Any(d => c.PileID == d)))
                        {
                            dc.approvalcamppiles.Remove(photo);
                        }
                    }
                    else {
                        var origPile = dc.approvalcamppiles.Where(c => c.CampID == info.CampID).ToList();
                        foreach (var photo in origPile)
                        {
                            dc.approvalcamppiles.Remove(photo);
                        }
                    }
                    #endregion

                    #region Approval Camp Price
                    if (info.ModelListcampprice != null)
                    {
                        foreach (var nInfo in info.ModelListcampprice)
                        {
                            var efInfo = new approvalcampprice();
                            ModelConverter.Convert(nInfo, efInfo);
                            if (efInfo.CampPriceID > 0)
                            {
                                var origAppCampPrice = dc.approvalcampprices.FirstOrDefault(c => c.CampPriceID == nInfo.CampPriceID);
                                if (origAppCampPrice != null)
                                {
                                    dc.Entry(origAppCampPrice).CurrentValues.SetValues(efInfo);
                                }
                            }
                            else
                            {
                                dc.approvalcampprices.Add(efInfo);
                            }
                        }
                        var origPrice = dc.approvalcampprices.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcampprice.Where(c => c.CampPriceID > 0).Select(c => c.CampPriceID);
                        foreach (var photo in origPrice.Where(c => !newIds.Any(d => c.CampPriceID == d)))
                        {
                            dc.approvalcampprices.Remove(photo);
                        }
                    }
                    else
                    {
                        var origPile = dc.approvalcampprices.Where(c => c.CampID == info.CampID).ToList();
                        foreach (var photo in origPile)
                        {
                            dc.approvalcampprices.Remove(photo);
                        }
                    }
                    #endregion

                    #region Approval Camp Type
                    if (info.ModelListcamptype != null)
                    {
                        foreach (var nInfo in info.ModelListcamptype)
                        {
                            var efInfo = new approvalcamptype();
                            ModelConverter.Convert(nInfo, efInfo);
                            if (efInfo.CampTypeID > 0 && nInfo.Checked)
                            {
                                var origAppCampType = dc.approvalcamptypes.FirstOrDefault(c => c.CampTypeID == nInfo.CampTypeID);
                                if (origAppCampType != null)
                                {
                                    dc.Entry(origAppCampType).CurrentValues.SetValues(efInfo);
                                }
                            }
                            else if (nInfo.Checked)
                            {
                                dc.approvalcamptypes.Add(efInfo);
                            }
                        }
                        var origType = dc.approvalcamptypes.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcamptype.Where(c => c.CampTypeID > 0 && c.Checked).Select(c => c.CampTypeID);
                        foreach (var camptyp in origType.Where(c => !newIds.Any(d => c.CampTypeID == d)))
                        {
                            dc.approvalcamptypes.Remove(camptyp);
                        }
                    }
                    else {
                        var origType = dc.approvalcamptypes.Where(c => c.CampID == info.CampID).ToList();
                        foreach (var camptyp in origType)
                        {
                            dc.approvalcamptypes.Remove(camptyp);
                        }
                    }
                    #endregion
                }
                dc.SaveChanges();
            }
            else
            {
                var newCamp = CreateNewApprovalCamp(info);
                dc.approvalcamps.Add(newCamp);
                dc.SaveChanges();
                info.CampID = newCamp.CampID;
            }
            return info.CampID;
        }
        private approvalcamp CreateNewApprovalCamp(approvalcampModel campmodel)
        {
            var info = new approvalcamp();
            #region Approval Camp
            ModelConverter.Convert<approvalcampModel, approvalcamp>(campmodel, info);
            #endregion
            
            #region Approcal Camp Host
            info.Listapprovalcamphost = new List<approvalcamphost>();
            if (campmodel.ModelListcamphost != null)
            {
                foreach (var md in campmodel.ModelListcamphost)
                {
                    var ef = new approvalcamphost();
                    ModelConverter.Convert<approvalcamphostModel, approvalcamphost>(md, ef);

                    ef.Listapprovalcamphostlanguage = new List<approvalcamphostlanguage>();

                    foreach (var sMD in md.ModelListcamphostlanguage)
                    {
                        var sEF = new approvalcamphostlanguage();
                        ModelConverter.Convert<approvalcamphostlanguageModel, approvalcamphostlanguage>(sMD, sEF);
                        ef.Listapprovalcamphostlanguage.Add(sEF);
                    }
                    info.Listapprovalcamphost.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Item
            info.Listapprovalcampitem = new List<approvalcampitem>();
            if (campmodel.ModelListcamphost != null)
            {
                foreach (var md in campmodel.ModelListcampitem)
                {
                    var ef = new approvalcampitem();
                    if (md.Checked)
                    {
                        ModelConverter.Convert<approvalcampitemModel, approvalcampitem>(md, ef);
                        info.Listapprovalcampitem.Add(ef);
                    }
                }
            }
            #endregion

            #region Approval Camp Photo
            info.Listapprovalcampphoto = new List<approvalcampphoto>();
            if (campmodel.ModelListcampphoto != null)
            {
                foreach (var md in campmodel.ModelListcampphoto)
                {
                    var ef = new approvalcampphoto();
                    ModelConverter.Convert<approvalcampphotoModel, approvalcampphoto>(md, ef);
                    info.Listapprovalcampphoto.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Pile
            info.Listapprovalcamppile = new List<approvalcamppile>();
            if (campmodel.ModelListcamppile != null)
            {
                foreach (var md in campmodel.ModelListcamppile)
                {
                    var ef = new approvalcamppile();
                    ModelConverter.Convert<approvalcamppileModel, approvalcamppile>(md, ef);
                    ef.Active = md.Flag ? 1 : 0;
                    info.Listapprovalcamppile.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Price
            info.Listapprovalcampprice = new List<approvalcampprice>();
            if (campmodel.ModelListcampprice != null)
            {
                foreach (var md in campmodel.ModelListcampprice)
                {
                    var ef = new approvalcampprice();
                    ModelConverter.Convert<approvalcamppriceModel, approvalcampprice>(md, ef);
                    info.Listapprovalcampprice.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Type
            info.Listapprovalcamptype = new List<approvalcamptype>();
            foreach (var md in campmodel.ModelListcamptype)
            {
                var ef = new approvalcamptype();
                if (md.Checked)
                {
                    ModelConverter.Convert<approvalcamptypeModel, approvalcamptype>(md, ef);
                    info.Listapprovalcamptype.Add(ef);
                }
            }
            #endregion

            info.CreateByID = this._user.UserID;
            info.CreateByName = this._user.UserName;

            return info;
        }

        private camp CreateNewCamp(approvalcampModel campmodel)
        {
            var info = new camp();
            #region Approval Camp
            ModelConverter.Convert<approvalcampModel, camp>(campmodel, info);
            #endregion

            #region Approcal Camp Host
            info.Listcamphost = new List<camphost>();
            if (campmodel.ModelListcamphost != null)
            {
                foreach (var md in campmodel.ModelListcamphost)
                {
                    var ef = new camphost();
                    ModelConverter.Convert<approvalcamphostModel, camphost>(md, ef);

                    ef.Listcamphostlanguage = new List<camphostlanguage>();
                    foreach (var sMD in md.ModelListcamphostlanguage)
                    {
                        var sEF = new camphostlanguage();
                        ModelConverter.Convert<approvalcamphostlanguageModel, camphostlanguage>(sMD, sEF);
                        ef.Listcamphostlanguage.Add(sEF);
                    }
                    info.Listcamphost.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Item
            info.Listcampitem = new List<campitem>();
            if (campmodel.ModelListcamphost != null)
            {
                foreach (var md in campmodel.ModelListcampitem)
                {
                    var ef = new campitem();
                    if (md.Checked)
                    {
                        ModelConverter.Convert<approvalcampitemModel, campitem>(md, ef);
                        info.Listcampitem.Add(ef);
                    }
                }
            }
            #endregion

            #region Approval Camp Photo
            info.Listcampphoto = new List<campphoto>();
            if (campmodel.ModelListcampphoto != null)
            {
                foreach (var md in campmodel.ModelListcampphoto)
                {
                    var ef = new campphoto();
                    ModelConverter.Convert<approvalcampphotoModel, campphoto>(md, ef);
                    info.Listcampphoto.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Pile
            info.Listcamppile = new List<camppile>();
            if (campmodel.ModelListcamppile != null)
            {
                foreach (var md in campmodel.ModelListcamppile)
                {
                    var ef = new camppile();
                    ModelConverter.Convert<approvalcamppileModel, camppile>(md, ef);
                    ef.Active = md.Flag ? 1 : 0;
                    info.Listcamppile.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Price
            info.Listcampprice = new List<campprice>();
            if (campmodel.ModelListcampprice != null)
            {
                foreach (var md in campmodel.ModelListcampprice)
                {
                    var ef = new campprice();
                    ModelConverter.Convert<approvalcamppriceModel, campprice>(md, ef);
                    info.Listcampprice.Add(ef);
                }
            }
            #endregion

            #region Approval Camp Type
            info.Listcamptype = new List<camptype>();
            foreach (var md in campmodel.ModelListcamptype)
            {
                var ef = new camptype();
                if (md.Checked)
                {
                    ModelConverter.Convert<approvalcamptypeModel, camptype>(md, ef);
                    info.Listcamptype.Add(ef);
                }
            }
            #endregion

            return info;
        }
        public int ApprovalCamp(approvalcampModel campInfo) {
            var info = CreateNewCamp(campInfo);
            var origAppCamp = dc.camps.FirstOrDefault(c => c.CampID == campInfo.CampID);
            if (origAppCamp != null)
            {
                #region Camp
                dc.Entry(origAppCamp).CurrentValues.SetValues(info);
                #endregion

                #region Camp Item
                if (info.Listcampitem != null)
                {
                    foreach (var efInfo in info.Listcampitem)
                    {
                        if (efInfo.CampItemID > 0)
                        {
                            var origAppCampItem = dc.campitems.FirstOrDefault(c => c.CampItemID == efInfo.CampItemID);
                            if (origAppCampItem != null)
                            {
                                dc.Entry(origAppCampItem).CurrentValues.SetValues(efInfo);
                            }
                            else {
                                dc.campitems.Add(efInfo);
                            }
                        }
                        else
                        {
                            dc.campitems.Add(efInfo);
                        }
                    }
                    var origItem = dc.campitems.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcampitem.Where(c => c.CampItemID > 0).Select(c => c.CampItemID);
                    foreach (var item in origItem.Where(c => !newIds.Any(d => c.CampItemID == d)))
                    {
                        dc.campitems.Remove(item);
                    }
                }
                else {
                    var origItem = dc.campitems.Where(c => c.CampID == info.CampID).ToList();
                    foreach (var item in origItem)
                    {
                        dc.campitems.Remove(item);
                    }
                }
                #endregion

                #region Camp Photo
                if (info.Listcampphoto != null)
                {
                    foreach (var efInfo in info.Listcampphoto)
                    {
                        if (efInfo.CampPhotoID > 0)
                        {
                            var origAppCampPhoto = dc.campphotos.FirstOrDefault(c => c.CampPhotoID == efInfo.CampPhotoID);
                            if (origAppCampPhoto != null)
                            {
                                dc.Entry(origAppCampPhoto).CurrentValues.SetValues(efInfo);
                            }
                            else
                                dc.campphotos.Add(efInfo);
                        }
                        else
                        {
                            dc.campphotos.Add(efInfo);
                        }
                    }
                    var origPhoto = dc.campphotos.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcampphoto.Where(c => c.CampPhotoID > 0).Select(c => c.CampPhotoID);
                    foreach (var photo in origPhoto.Where(c => !newIds.Any(d => c.CampPhotoID == d)))
                    {
                        dc.campphotos.Remove(photo);
                    }
                }
                else {
                    var origPhoto = dc.campphotos.Where(c => c.CampID == info.CampID).ToList();
                    foreach (var photo in origPhoto)
                    {
                        dc.campphotos.Remove(photo);
                    }
                }
                #endregion

                #region Camp Pile
                if (info.Listcamppile != null)
                {
                    foreach (var efInfo in info.Listcamppile)
                    {
                        if (efInfo.PileID > 0)
                        {
                            var origAppCampPile = dc.camppiles.FirstOrDefault(c => c.PileID == efInfo.PileID);
                            if (origAppCampPile != null)
                            {
                                dc.Entry(origAppCampPile).CurrentValues.SetValues(efInfo);
                            }
                            else
                            {
                                dc.camppiles.Add(efInfo);
                            }
                        }
                        else
                        {
                            dc.camppiles.Add(efInfo);
                        }
                    }
                    var origPile = dc.camppiles.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcamppile.Where(c => c.PileID > 0).Select(c => c.PileID);
                    foreach (var photo in origPile.Where(c => !newIds.Any(d => c.PileID == d)))
                    {
                        dc.camppiles.Remove(photo);
                    }
                }
                else {
                    var origPile = dc.camppiles.Where(c => c.CampID == info.CampID).ToList();
                    foreach (var photo in origPile)
                    {
                        dc.camppiles.Remove(photo);
                    }
                }
                #endregion

                #region Camp Price
                if (info.Listcampprice != null)
                {
                    foreach (var efInfo in info.Listcampprice)
                    {
                        if (efInfo.CampPriceID > 0)
                        {
                            var origAppCampPile = dc.campprices.FirstOrDefault(c => c.CampPriceID == efInfo.CampPriceID);
                            if (origAppCampPile != null)
                            {
                                dc.Entry(origAppCampPile).CurrentValues.SetValues(efInfo);
                            }
                            else
                            {
                                dc.campprices.Add(efInfo);
                            }
                        }
                        else
                        {
                            dc.campprices.Add(efInfo);
                        }
                    }
                    var origPile = dc.campprices.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcampprice.Where(c => c.CampPriceID > 0).Select(c => c.CampPriceID);
                    foreach (var photo in origPile.Where(c => !newIds.Any(d => c.CampPriceID == d)))
                    {
                        dc.campprices.Remove(photo);
                    }
                }
                else
                {
                    var origPrice = dc.campprices.Where(c => c.CampID == info.CampID).ToList();
                    foreach (var price in origPrice)
                    {
                        dc.campprices.Remove(price);
                    }
                }
                #endregion

                #region Camp Type
                if (info.Listcamptype != null)
                {
                    foreach (var efInfo in info.Listcamptype)
                    {
                        if (efInfo.CampTypeID > 0)
                        {
                            var origAppCampType = dc.camptypes.FirstOrDefault(c => c.CampTypeID == efInfo.CampTypeID);
                            if (origAppCampType != null)
                            {
                                dc.Entry(origAppCampType).CurrentValues.SetValues(efInfo);
                            }
                            else
                            {
                                dc.camptypes.Add(efInfo);
                            }
                        }
                        else
                        {
                            dc.camptypes.Add(efInfo);
                        }
                    }
                    var origPile = dc.camptypes.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcamptype.Where(c => c.CampTypeID > 0).Select(c => c.CampTypeID);
                    foreach (var type in origPile.Where(c => !newIds.Any(d => c.CampTypeID == d)))
                    {
                        dc.camptypes.Remove(type);
                    }
                }
                else
                {
                    var origtype = dc.camptypes.Where(c => c.CampID == info.CampID).ToList();
                    foreach (var campType in origtype)
                    {
                        dc.camptypes.Remove(campType);
                    }
                }
                #endregion
            }
            else
            {
                dc.camps.Add(info);
            }
            dc.SaveChanges();
            return info.CampID;
        }

        public int RejectCamp(approvalcampModel info)
        {
            var origAppCamp = dc.approvalcamps.FirstOrDefault(c => c.CampID == info.CampID);
            var newAppCamp = new approvalcamp();
            ModelConverter.Convert<approvalcampModel, approvalcamp>(info, newAppCamp);
            dc.Entry(origAppCamp).CurrentValues.SetValues(newAppCamp);
            dc.SaveChanges();
            return info.CampID;
        }
        #endregion

        #region Comment
        public void UpdateComment(int id, string cres, string type) {
            var ef = dc.campcomments.FirstOrDefault(c => c.CampCommentID == id);
            if (type == "DeleteRes")
            {
                if (ef != null)
                {
                    ef.CommentRes = "";
                    dc.SaveChanges();
                }
            }
            else if (type == "UpdateRes") {
                if (ef != null)
                {
                    ef.CommentRes = cres;
                    dc.SaveChanges();
                }
            }
            else if (type == "DeleteRecord") {
                dc.campcomments.Remove(ef);
                dc.SaveChanges();
            }
        }
        #endregion
    }
}
