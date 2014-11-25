using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class camphostModel
    {
        public int CampHostID { get; set; }

        public int CampID { get; set; }

        public int UserID { get; set; }

        public string CampHostPhoto { get; set; }

        public string CampHostIntro { get; set; }

        public virtual List<camphostlanguageModel> ModelListcamphostlanguage { get; set; }
    }
}
