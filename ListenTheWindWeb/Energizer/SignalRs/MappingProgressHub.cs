using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HDS.QMS.Energizer.SignalRs
{
    public class MappingProgressHub : Hub
    {
        public override System.Threading.Tasks.Task OnConnected()
        {
            Groups.Add(Context.ConnectionId, Context.User.Identity.Name);
            return null;
        }
        public void Send(string message)
        {
            Clients.Group(Context.User.Identity.Name).send(message);
        }
        public override System.Threading.Tasks.Task OnDisconnected()
        {
            Groups.Remove(Context.ConnectionId, Context.User.Identity.Name);
            return null;
        }
    }
}