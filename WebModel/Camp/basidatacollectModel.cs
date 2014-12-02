using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebModel.Camp
{
    public class basidatacollectforcampModel
    {
        public int BasicDataCollectID { get; set; }

        public string DataType { get; set; }

        public string DataName { get; set; }

        public string DataIcon { get; set; }

        public int? DataSort { get; set; }

        public bool Checked { get; set; }
    }
}
