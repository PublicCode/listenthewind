using HDS.QMS.App_Start;
using HDS.QMS.Energizer.SignalRs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

[assembly: PreApplicationStartMethod(typeof(RegisterHubs), "Start")]

namespace HDS.QMS.App_Start
{
    public static class RegisterHubs
    {
        public static void Start()
        {
            // Register the default hubs route: ~/signalr/hubs
            //RouteTable.Routes.MapHubs("~/signalr/hubs");
            RouteTable.Routes.MapConnection<MappingProgressConnection>("mappingProgress", "/mappingprogress");

        }
    }
}