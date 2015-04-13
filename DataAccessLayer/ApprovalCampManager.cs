﻿using System;
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
    }
}
