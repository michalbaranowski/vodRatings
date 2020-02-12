using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepository
    {
        IEnumerable<ResultModel> GetStoredData(int type);
        void AddAlreadyWatched(AlreadyWatchedModel movie);
        void RemoveAlreadyWatched(AlreadyWatchedModel alreadyWatchedModel);
        IEnumerable<AlreadyWatchedModel> GetAlreadyWatched(string username);
    }
}
