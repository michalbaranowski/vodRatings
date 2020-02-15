using System.Collections.Generic;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Core.Boundary.Interfaces
{
    public interface ICoreLogic
    {
        IEnumerable<MovieViewModel> GetResults(MovieTypes type);
        IEnumerable<MovieViewModel> GetResultsUsingStorage(MovieTypes type, string username);
    }
}
