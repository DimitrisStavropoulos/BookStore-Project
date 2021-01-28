using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Store01.Hubs
{
    [HubName("booksHub")]
    public class BooksHub : Hub
    {
        public static void BroadcastData()
        {
            IHubContext context = GlobalHost.ConnectionManager.GetHubContext<BooksHub>();
            context.Clients.All.refreshBooksData();
        }
    }
}