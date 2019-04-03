using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace WebServerEntityFramework
{
    public class HubClass : Hub
    {
        
        public static void PostToClient(string fromUser,string toUser)
        {
            try
            {
                var chat = GlobalHost.ConnectionManager.GetHubContext("HubClass");
                if (chat != null)
                {
                    chat.Clients.All.postToClient(fromUser,toUser);
                }
            }
            catch(Exception e)
            {
            }
        }

        public void Hello()
        {
            Clients.All.hello();
        }
    }
}