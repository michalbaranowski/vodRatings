using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepositoryBackground
    {
        void RefreshData(IEnumerable<ResultModel> results, int type);
        ResultModel ResultByTitle(string movieTitle);
        IList<ResultModel> GetResultsOfType(int type);
        void RemoveMovies(IEnumerable<ResultModel> moviesToRemove);
        void AddMovies(IEnumerable<ResultModel> moviesToAdd);
    }
}
