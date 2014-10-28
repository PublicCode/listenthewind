using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DataAccess.DC
{
    public class Part : ILoggedEntity
    {
        [Key]
        public int TID { get; set; }

        public string PartNumber { get; set; }

        public string PartDesc { get; set; }

        public string PartCategory { get; set; }

        public string ProdCode { get; set; }

        public string PartID { get; set; }

        public string PartGTN { get; set; }

        public string PartCategoryOther { get; set; }

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
