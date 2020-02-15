using System.Collections.Generic;
using System.Linq;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepository
    {
        IQueryable<MovieEntity> GetStoredData(int type);
        void AddAlreadyWatched(UserMovieEntity userMovie);
        void RemoveAlreadyWatched(int movieId, string userId);
        IQueryable<UserMovieEntity> GetAlreadyWatched(string userId);
    }
}
