using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR_Chatroom
{
    public class ChatHub : Hub
    {
        static List<string> currentUserSet = new List<string>();
        public override System.Threading.Tasks.Task OnConnected()
        {
            Clients.All.user(Context.User.Identity.Name);
            var userName = Context.User.Identity.Name;
            currentUserSet.Add(userName);

            return base.OnConnected();
        }

        public void send(string message)
        {
            Clients.Caller.message("You: " + message);
            Clients.Others.message(Context.User.Identity.Name + ": " + message);
        }

        public List<string> GetAllActiveConnections()
        {
            return currentUserSet.ToList();
        }
    }
}