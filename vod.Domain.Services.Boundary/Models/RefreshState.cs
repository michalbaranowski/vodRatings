using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary.Models
{
    public class RefreshState
    {
        public bool IsRefreshingNow { get; set; }
        public MovieTypes? RefreshingType { get; set; }
    }
}
