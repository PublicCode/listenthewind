using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class camppileModel
    {
        public int PileID { get; set; }

        public int CampID { get; set; }

        public string PileNumber { get; set; }

        public int? Active { get; set; }
    }
}
