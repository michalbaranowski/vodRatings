using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepositoryBackground
    {
        void RefreshData(IEnumerable<MovieEntity> results, int type);
        MovieEntity ResultByTitle(string movieTitle);
        IList<MovieEntity> GetResultsOfType(int type);
        void MarkAsDeleted(IEnumerable<MovieEntity> moviesToRemove);
        void AddMovies(IEnumerable<MovieEntity> moviesToAdd);
        void LogUpdate(int movieType);
        void AddBlackListedMovies(IEnumerable<BlackListedMovieEntity> moviesToBlackList);
        IList<BlackListedMovieEntity> GetBlackListedMovies();
        void RemoveDupes();
    }
}
