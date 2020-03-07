using System;
using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Commands
{
    public class UseStorageIfPossibleCommand
    {
        public MovieTypes Type { get; set; }
        public string UserId { get; set; }
    }
}
