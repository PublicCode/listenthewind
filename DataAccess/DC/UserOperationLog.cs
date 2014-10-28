using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.DC
{
    public class UserOperationLog
    {
        [Key]
        public Guid LogID { get; set; }
        public System.String OperationUserName { get; set; }
        public System.Int32 OperationUserID { get; set; }
        public System.String OperationType { get; set; }
        public string OID { get; set; }
        public System.String OperationInfo { get; set; }
        public string OperationInfoBody { get; set; }
        public DateTime? OperationTime { get; set; }
        public DateTime? CreationTime { get; set; }

        [ForeignKey("LogID")]
        public virtual ICollection<UserOperationDetail> OperationDetails { get; set; }


    }
}
