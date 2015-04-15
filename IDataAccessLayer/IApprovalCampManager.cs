using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Camp;
using WebModel.ApprovalCamp;

namespace IDataAccessLayer
{
    public interface IApprovalCampManager
    {
        IEnumerable<approvalcamplist> GetApprovalCampList();
        approvalcampModel GetCamp(int campID, DateTime? dt);
    }
}
