using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace YakShop.Mvc.Hubs
{
    public class YakShopHub : Hub
    {
        public async Task ShopUpdate(string clientId, int elapsedDays)
        {
            await Clients.All.SendAsync("ShopUpdate", clientId, elapsedDays);
        }
    }
}
