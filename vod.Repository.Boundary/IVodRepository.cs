using System;
using System.Linq;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepository
    {
        IQueryable<MovieEntity> GetStoredData(int type);
        IQueryable<UserMovieEntity> GetAlreadyWatched(string userId);
        void AddAlreadyWatched(UserMovieEntity userMovie);
        void RemoveAlreadyWatched(int movieId, string userId);
        DateTime GetUpdateDateTime(int movieType);
    }
}
