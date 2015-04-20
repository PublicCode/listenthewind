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
        public int UserID { get; set; }

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
        public string Pwd { get; set; }


        /// <summary>
        /// LastName
        /// </summary>
        [Display(Name = "Last Name")]
        public string Name { get; set; }


        [Display(Name="Email")]
        [Column("Mail")]
        [RegularExpression(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$",ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }

        /// <summary>
        /// Cell phone of user
        /// </summary>
        public string Mobile { get; set; }

        public string UserIntegral { get; set; }

        /// <summary>
        /// CreateDate
        /// </summary>
        public DateTime? CreateTime { get; set; }

        public string HeadPhoto { get; set; }

        public string Sex { get; set; }

        public string Birth { get; set; }


        public string Intro { get; set; }

        public string UserType { get; set; }

        [NotMapped]
        public string UserTypeShow
        {
            get
            {
                if (UserType == "0")
                {
                    return "一般用户";
                }
                else if (UserType == "1")
                {
                    return "营主";
                }
                else if (UserType == "2")
                {
                    return "营长";
                }
                else if (UserType == "3")
                {
                    return "管理员";
                }
                else
                {
                    return "一般用户";
                }
            }
            set { }
        }

        public int? Active { get; set; }

        public int? MailFlag { get; set; }
        public int? MobileFlag { get; set; }

        public string IDNumber { get; set; }


        public string IDNumberImg1 { get; set; }

        public string IDNumberImg2 { get; set; }

        public int? IDNumberFlag { get; set; }

        long ILoggedEntity.Id
        {
            get { return UserID; }
        }
        /// <summary>
        /// This field is used for link operation history summary and detail.
        /// </summary>
        [NotMapped]
        public string BatchID { get; set; }

    }
}
