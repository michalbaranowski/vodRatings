using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepository
    {
        IEnumerable<ResultModel> GetStoredData(int type);
        void AddAlreadyWatched(AlreadyWatchedModel movie);
        IEnumerable<AlreadyWatchedModel> GetAlreadyWatched(string username);
    }
}
