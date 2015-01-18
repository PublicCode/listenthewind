using DataAccess.DC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebModel.Camp;

namespace IDataAccessLayer
{
    public interface IUserManager
    {
        object GetCampCollectByUser();
        string DeleteCampCollect(int CampID);
    }
}
