using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebSite.Hubs
{
    public class DataHub : Hub
    {
        /*
        public void Subscribe(string token)
        {
            Groups.Add(Context.ConnectionId, token);
        }

        public void Unsubscribe(string token)
        {
            Groups.Remove(Context.ConnectionId, token);
        }
        */
    }
}