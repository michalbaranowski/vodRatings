using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces.Getters
{
    public interface INcPlusMoviesGetter
    {
        IEnumerable<NcPlusResult> GetMoviesOfType(MovieTypes type);
    }
}
