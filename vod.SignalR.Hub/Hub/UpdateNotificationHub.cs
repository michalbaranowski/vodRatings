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
            Clients.All.SendAsync("NotifyUpdate", (int)type, newMoviesCount);
        }

        public void NotifyRefreshStarted(MovieTypes type)
        {
            Clients.All.SendAsync("RefreshStarted", (int)type);
        }

        public void NotifyRefreshProgress(int percentage)
        {
            Clients.All.SendAsync("NotifyRefreshProgress", percentage);
        }
    }
}
