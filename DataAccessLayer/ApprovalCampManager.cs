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
        public IEnumerable<approvalcamplist> GetApprovalCampList()
        {
            return dc.approvalcamplists.AsQueryable();
        }
        public approvalcampModel GetCamp(int CampID, DateTime? dt)
        {
            int userid = 0;
            if (_user != null)
            {
                userid = _user.UserID;
            }
            approvalcampModel campmodel = new approvalcampModel();
            var campInDb = GetSingleCamp(CampID);
            ModelConverter.Convert<approvalcamp, approvalcampModel>(campInDb, campmodel);

            campmodel.ModelListcampcomment = new List<approvalcampcommentModel>();

            foreach (approvalcampcomment campcomInDB in campInDb.Listapprovalcampcomment)
            {
                var campcommentmodel = new approvalcampcommentModel();
                ModelConverter.Convert<approvalcampcomment, approvalcampcommentModel>(campcomInDB, campcommentmodel);
                campmodel.ModelListcampcomment.Add(campcommentmodel);
            }

            campmodel.ModelListcamphost = new List<approvalcamphostModel>();

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

            campmodel.ModelListcampitem = new List<approvalcampitemModel>();

            foreach (var campitemInDB in campInDb.Listapprovalcampitem)
            {
                var campitemmodel = new approvalcampitemModel();
                ModelConverter.Convert<approvalcampitem, approvalcampitemModel>(campitemInDB, campitemmodel);
                campmodel.ModelListcampitem.Add(campitemmodel);
            }


            campmodel.ModelListcampphoto = new List<approvalcampphotoModel>();

            foreach (var campphotoInDB in campInDb.Listapprovalcampphoto)
            {
                var campphotomodel = new approvalcampphotoModel();
                ModelConverter.Convert<approvalcampphoto, approvalcampphotoModel>(campphotoInDB, campphotomodel);
                campmodel.ModelListcampphoto.Add(campphotomodel);
            }

            campmodel.ModelListcamppile = new List<approvalcamppileModel>();

            foreach (var camppileInDB in campInDb.Listapprovalcamppile)
            {
                var camppilemodel = new approvalcamppileModel();
                ModelConverter.Convert<approvalcamppile, approvalcamppileModel>(camppileInDB, camppilemodel);
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
                    var reserdate = dc.approvalcampreservedate.FirstOrDefault(c => c.CampReserveDate == dt && c.CampPileID == camppilemodel.PileID);
                    if (reserdate == null && camppilemodel.Active == 1)
                    {
                        camppilemodel.Flag = true;
                    }
                    else
                    {
                        var camprese = dc.approvalcampreserves.FirstOrDefault(c => c.CampReserveID == reserdate.CampReserveID);
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

            campmodel.ModelListcampprice = new List<approvalcamppriceModel>();

            foreach (var camppriceInDB in campInDb.Listapprovalcampprice)
            {
                var camppricemodel = new approvalcamppriceModel();
                ModelConverter.Convert<approvalcampprice, approvalcamppriceModel>(camppriceInDB, camppricemodel);
                campmodel.ModelListcampprice.Add(camppricemodel);
            }

            campmodel.ModelListcamptype = new List<approvalcamptypeModel>();

            foreach (var camptypeInDB in campInDb.Listapprovalcamptype)
            {
                var camptypemodel = new approvalcamptypeModel();
                ModelConverter.Convert<approvalcamptype, approvalcamptypeModel>(camptypeInDB, camptypemodel);
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
    }
}
