using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.SignalR.Hub.Hub
{
    public class UpdateNotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public void NotifyUpdate(MovieTypes type, int newMoviesCount)
        {
            if (Clients?.All != null)
            {
                Clients.All.SendAsync("NotifyUpdate", (int)type, newMoviesCount);
            }
        }

        public void NotifyRefreshStarted(MovieTypes type)
        {
            if (Clients?.All != null)
            {
                Clients.All.SendAsync("RefreshStarted", (int)type);
            }
        }

        public void NotifyRefreshProgress(int percentage)
        {
            if (Clients?.All != null)
            {
                Clients.All.SendAsync("NotifyRefreshProgress", percentage);
            }
        }
    }
}
