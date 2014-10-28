using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class Country : ILoggedEntity
    {
        [Key]
        public int TID { get; set; }
        public string CountryID { get; set; }
        public string CountryDesc { get; set; }

        public string LoggedType
        {
            get { return "Part"; }
        }

        [NotMapped]
        public string BatchID
        {
            get;
            set;
        }

        long ILoggedEntity.Id
        {
            get { return TID; }
        }
    }
}
