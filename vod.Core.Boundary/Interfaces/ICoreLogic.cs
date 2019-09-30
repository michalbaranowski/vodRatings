using System.Collections.Generic;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Core.Boundary.Interfaces
{
    public interface ICoreLogic
    {
        IEnumerable<Result> GetResults(MovieTypes type);
    }
}
