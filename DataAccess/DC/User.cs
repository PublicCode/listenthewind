using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;

namespace DataAccess.DC
{
    public class User : ILoggedEntity
    {
        /// <summary>
        /// Key
        /// </summary>
        public int ID { get; set; }

        public string LoggedType { get { return "User"; } }
        /// <summary>
        /// User Name
        /// </summary>
        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        
        [Required]
        [Display(Name="Password")]
        public string PassWord { get; set; }

        /// <summary>
        /// FirstName
        /// </summary>
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        /// <summary>
        /// LastName
        /// </summary>
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        /// <summary>
        /// DisplayName
        public string DisplayName { get; set; }

        /// <summary>
        /// NickName
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// Title
        /// </summary>
        public string Title { get; set; }

        [Required]
        [Display(Name="Email")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        /// <summary>
        /// Phone
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Cell phone of user
        /// </summary>
        public string CellPhone { get; set; }

        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// CreateUser
        /// </summary>
        public int? CreateUser { get; set; }

        /// <summary>
        /// ModifyDate
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// ModifyUser
        /// </summary>
        public int? UpdateUser { get; set; }

        /// <summary>
        /// Address
        /// </summary>
        public string Address { get; set; }

        public virtual ICollection<Permission> UserPermissions { get; set; }

        /// <summary>
        /// Time zone of profile
        /// </summary>
        public string TimeZone { get; set; }

        /// <summary>
        /// ActiveFlag
        /// </summary>
        public int? ActiveFlag { get; set; }

        public bool? DeleteFlag { get; set; }

        long ILoggedEntity.Id
        {
            get { return ID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
