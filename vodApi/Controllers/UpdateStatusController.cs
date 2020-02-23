using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vod.Domain.Services.Boundary.Interfaces;
using vod.SignalR.Hub.Hub;

namespace vodApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UpdateStatusController : ControllerBase
    {
        private IRefreshStateService _refreshStateService;
        private UpdateNotificationHub _hub;

        public UpdateStatusController(
            IRefreshStateService refreshStateService,
            UpdateNotificationHub hub)
        {
            _refreshStateService = refreshStateService;
            _hub = hub;
        }

        [HttpGet]
        public void NotifyIfProcessing()
        {
            var result = _refreshStateService.GetRefreshState();

            if(result.IsRefreshingNow && result.RefreshingType.HasValue)
            {
                _hub.NotifyRefreshStarted(result.RefreshingType.Value);
            }            
        }
    }
}