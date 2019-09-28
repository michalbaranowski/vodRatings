using System.Collections.Generic;
using vod.Core.Boundary.Model;

namespace vod.Core.Boundary
{
    public interface ICoreLogic
    {
        IEnumerable<Result> GetResults();
    }
}
