using DataAccess.DC;
using IDataAccessLayer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace DataAccessLayer
{
    public class ExportManager:BaseManager, IExportManager
    {
        private User u;
        public ExportManager(User user)
        {
            this.u = user;
        }
        public ExportManager()
        { }
    }
}
