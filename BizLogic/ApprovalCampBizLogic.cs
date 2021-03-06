﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IDataAccessLayer;
using DataAccessLayer;
using DataAccess.DC;
using WebModel.ApprovalCamp;
using ComLib.Extension;

namespace BizLogic
{
    public class ApprovalCampBizLogic
    {
        private IApprovalCampManager appCampBase;
        public ApprovalCampBizLogic(User user = null)
        {
            appCampBase = new ApprovalCampManager(user);
        }
        public object GetApprovalCampLstModel(int page, int limit)
        {
            var lstEF = appCampBase.GetApprovalCampList();
            var res = lstEF;
            if (page > 0)
            {
                int skipPages = page - 1;
                res = res.Skip(skipPages * limit);
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
        public approvalcampModel GetCamp(int campID, DateTime? dt){
            return appCampBase.GetCamp(campID, dt);
        }
        public int SaveCamp(approvalcampModel info) {
            return appCampBase.SaveCamp(info);
        }
        public int ApprovalCamp(approvalcampModel campInfo) {
            return appCampBase.ApprovalCamp(campInfo);
        }
        public int RejectCamp(approvalcampModel info)
        {
            return appCampBase.RejectCamp(info);
        }
        public void UpdateComment(int campcommentid, string cres, string type) {
            appCampBase.UpdateComment(campcommentid, cres, type);
        }
    }
}
