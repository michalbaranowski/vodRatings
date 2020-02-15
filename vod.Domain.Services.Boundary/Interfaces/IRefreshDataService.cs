using System;
using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IRefreshDataService
    {
        bool Refresh(MovieTypes type, Func<IEnumerable<FilmwebResult>> func);
    }
}
