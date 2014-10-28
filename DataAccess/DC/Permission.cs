using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    /// <summary>
    /// Using this class to get/set user permission
    /// </summary>
    public class Permission: ILoggedEntity
    {

        /// <summary>
        /// PermissionID
        /// </summary>
        [Key]
        public int PermissionID { get; set; }
        /// <summary>
        /// UserID
        /// </summary>		
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public virtual User User { get; set; }
        /// <summary>
        /// RoleID
        /// </summary>		
        public int RoleID { get; set; }
        [ForeignKey("RoleID")]
        public virtual Role MyRole {get; set;}
        /// <summary>
        /// CreatedBy
        /// </summary>		
        public int CreatedBy { get; set; }
        /// <summary>
        /// CreatedOn
        /// </summary>		
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// UpdatedBy
        /// </summary>		
        public int UpdatedBy { get; set; }
        /// <summary>
        /// UpdatedOn
        /// </summary>		
        public DateTime UpdatedOn { get; set; }


        public long Id
        {
            get { return this.PermissionID; }
        }

        public string LoggedType
        {
            get { return "Permission"; }
        }
    }
}
