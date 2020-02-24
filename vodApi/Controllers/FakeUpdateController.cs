using Microsoft.AspNetCore.Mvc;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.SignalR.Hub.Hub;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakeUpdateController : ControllerBase
    {
        private readonly UpdateNotificationHub _hub;

        public FakeUpdateController(UpdateNotificationHub hub)
        {
            this._hub = hub;
        }

        [HttpGet]
        public void Get(int type, int moviesCount = 1)
        {
            _hub.NotifyUpdate((MovieTypes)type, moviesCount);
        }
    }
}
