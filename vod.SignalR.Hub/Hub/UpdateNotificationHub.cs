using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.SignalR.Hub.Hub
{
    public class UpdateNotificationHub : Microsoft.AspNetCore.SignalR.Hub
    {
        public void NotifyUpdate(MovieTypes type)
        {
            Clients.All.SendAsync("NotifyUpdate", (int)type);
        }
    }
}
