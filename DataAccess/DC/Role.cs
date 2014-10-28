using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    /// <summary>
    /// Define roles in HDS system, current there distributor, HDS - Sales, HDS - Pricing, Third Part Vendor (TPV)  
    /// </summary>
    public class Role
    {

        /// <summary>
        /// RoleID
        /// </summary>
        [Key]
        public int RoleID { get; set; }
        /// <summary>
        /// RoleCode
        /// </summary>		
        public string RoleCode { get; set; }
        /// <summary>
        /// RoleDesc
        /// </summary>		
        public string RoleDesc { get; set; }

    }
}
