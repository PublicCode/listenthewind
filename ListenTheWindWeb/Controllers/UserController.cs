using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using T2VSoft.MVC.Core;

namespace HDS.QMS.Controllers
{
    public class UserController : T2VController
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserPayBook()
        {
            return View();
        }

    }
}
