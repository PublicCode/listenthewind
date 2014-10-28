using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.UserOperation
{
    public class UserOperationDetailModel
    {
        public virtual string ActionBatchID { get; set; }

        public string ObjectType { get; set; }
        public string ChangeType { get; set; }
        public string ObjectField { get; set; }
        public string ChangeFrom { get; set; }
        public string ChangeTo { get; set; }

        public DateTime? createtime { get; set; }
    }
}
