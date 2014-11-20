using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class accountnumber : ILoggedEntity
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public string School { get; set; }
        public string Class { get; set; }

        public string LoggedType
        {
            get { return "accountnumber"; }
        }

        [NotMapped]
        public string BatchID
        {
            get;
            set;
        }

        long ILoggedEntity.Id
        {
            get { return ID; }
        }
    }
}

