using DataAccess.DC;
using DataAccessLayer;
using IDataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BizLogic
{
    public class ExportBizLogic: BaseBizLogic
    {

        IExportManager exportManager = new ExportManager();
    }
}
