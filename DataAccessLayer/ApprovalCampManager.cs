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
                query = query.Where(c => c.ManagedByID == this._user.UserID);
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

            campmodel.ModelListcampcomment = new List<approvalcampcommentModel>();

            if (campInDb != null)
            {
                foreach (approvalcampcomment campcomInDB in campInDb.Listapprovalcampcomment)
                {
                    var campcommentmodel = new approvalcampcommentModel();
                    ModelConverter.Convert<approvalcampcomment, approvalcampcommentModel>(campcomInDB, campcommentmodel);
                    campmodel.ModelListcampcomment.Add(campcommentmodel);
                }
            }

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
                var efCampPhost = dc.approvalcamphosts.FirstOrDefault(c => c.UserID == this._user.UserID);
                if (efCampPhost != null)
                {
                    ModelConverter.Convert<approvalcamphost, approvalcamphostModel>(efCampPhost, camphostmodel);

                    camphostmodel.ModelListcamphostlanguage = new List<approvalcamphostlanguageModel>();
                    var efCampPhostLang = dc.approvalcamphostlanguages.Where(c => c.CampHostID == efCampPhost.CampHostID).ToList();
                    foreach (var languageInDb in efCampPhostLang)
                    {
                        var languagemodel = new approvalcamphostlanguageModel();
                        ModelConverter.Convert<approvalcamphostlanguage, approvalcamphostlanguageModel>(languageInDb, languagemodel);
                        camphostmodel.ModelListcamphostlanguage.Add(languagemodel);
                    }
                    campmodel.ModelListcamphost.Add(camphostmodel);
                }
            }

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

                    #region Approval Camp Comment
                    if (info.ModelListcampcomment != null)
                    {
                        foreach (var nInfo in info.ModelListcampcomment)
                        {
                            var efInfo = new approvalcampcomment();
                            ModelConverter.Convert(nInfo, efInfo);
                            if (efInfo.CampCommentID > 0)
                            {
                                var origAppCampComment = dc.approvalcampcomments.Where(c => c.CampCommentID == nInfo.CampCommentID).ToList();
                                if (origAppCampComment != null)
                                {
                                    dc.Entry(origAppCampComment).CurrentValues.SetValues(efInfo);
                                }
                            }
                            else
                            {
                                dc.approvalcampcomments.Add(efInfo);
                            }
                        }
                        var origComment = dc.approvalcampcomments.Where(c => c.CampID == info.CampID).ToList();
                        var newIds = info.ModelListcampcomment.Where(c => c.CampCommentID > 0).Select(c => c.CampCommentID);
                        foreach (var comm in origComment.Where(c => !newIds.Any(d => c.CampCommentID == d)))
                        {
                            dc.approvalcampcomments.Remove(comm);
                        }
                    }
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

            #region Approcal Camp Comment
            info.Listapprovalcampcomment = new List<approvalcampcomment>();
            if (campmodel.ModelListcampcomment != null)
            {
                foreach (approvalcampcommentModel md in campmodel.ModelListcampcomment)
                {
                    var campcomment = new approvalcampcomment();
                    ModelConverter.Convert<approvalcampcommentModel, approvalcampcomment>(md, campcomment);
                    info.Listapprovalcampcomment.Add(campcomment);
                }
            }
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

            #region Approcal Camp Comment
            info.Listcampcomment = new List<campcomment>();
            if (campmodel.ModelListcampcomment != null)
            {
                foreach (var md in campmodel.ModelListcampcomment)
                {
                    var ef = new campcomment();
                    ModelConverter.Convert<approvalcampcommentModel, campcomment>(md, ef);
                    info.Listcampcomment.Add(ef);
                }
            }
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
                #region Approval Camp
                dc.Entry(origAppCamp).CurrentValues.SetValues(info);
                #endregion

                #region Approval Camp Comment
                if (info.Listcampcomment != null)
                {
                    foreach (var efInfo in info.Listcampcomment)
                    {
                        if (efInfo.CampCommentID > 0)
                        {
                            var origAppCampComment = dc.approvalcampcomments.Where(c => c.CampCommentID == efInfo.CampCommentID).ToList();
                            if (origAppCampComment != null)
                            {
                                dc.Entry(origAppCampComment).CurrentValues.SetValues(efInfo);
                            }
                        }
                        else
                        {
                            dc.campcomments.Add(efInfo);
                        }
                    }
                    var origComment = dc.campcomments.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcampcomment.Where(c => c.CampCommentID > 0).Select(c => c.CampCommentID);
                    foreach (var comm in origComment.Where(c => !newIds.Any(d => c.CampCommentID == d)))
                    {
                        dc.campcomments.Remove(comm);
                    }
                }
                #endregion

                #region Approval Camp Item
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
                #endregion

                #region Approval Camp Photo
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
                #endregion

                #region Approval Camp Pile
                if (info.Listcamppile != null)
                {
                    foreach (var efInfo in info.Listcamppile)
                    {
                        if (efInfo.PileID > 0)
                        {
                            var origAppCampPile = dc.approvalcamppiles.FirstOrDefault(c => c.PileID == efInfo.PileID);
                            if (origAppCampPile != null)
                            {
                                dc.Entry(origAppCampPile).CurrentValues.SetValues(efInfo);
                            }
                        }
                        else
                        {
                            dc.camppiles.Add(efInfo);
                        }
                    }
                    var origPile = dc.approvalcamppiles.Where(c => c.CampID == info.CampID).ToList();
                    var newIds = info.Listcamppile.Where(c => c.PileID > 0).Select(c => c.PileID);
                    foreach (var photo in origPile.Where(c => !newIds.Any(d => c.PileID == d)))
                    {
                        dc.approvalcamppiles.Remove(photo);
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
    }
}
