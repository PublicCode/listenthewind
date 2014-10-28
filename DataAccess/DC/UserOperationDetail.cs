using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace DataAccess.DC
{
    /// <summary>
    /// This model is used for save all operation detail
    /// </summary>
    public class UserOperationDetail
    {
        [Key]
        public int DetailID { get; set; }

        public virtual Guid LogID { get; set; }

        public string ObjectType { get; set; }
        public string ChangeType { get; set; }
        public string ObjectField { get; set; }
        public string ChangeFrom { get; set; }
        public string ChangeTo { get; set; }

        public DateTime? createtime { get; set; }

    }
}
