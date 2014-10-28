using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.UserOperation
{
    public class UserOperationLogModel
    {
        public string OperationUserName { get; set; }
        public int OperationUserID { get; set; }
        public string OperationType { get; set; }
        public string OID { get; set; }
        public string OperationInfo { get; set; }
        public string OperationInfoBody { get; set; }
        public DateTime? OperationTime { get; set; }
        public DateTime? CreationTime { get; set; }

        public List<UserOperationDetailModel> UserOperationDetails { get; set; }
    }
}
