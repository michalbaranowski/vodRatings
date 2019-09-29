using System;
using System.Collections.Generic;
using System.Linq;
using vod.Core.Boundary.Model;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Core
{
    public static class StoredDataManager
    {
        public static IEnumerable<Result> UseStorageIfPossible(IVodRepository repository, Func<IEnumerable<Result>> func)
        {
            var storedCollection = repository.GetStoredData().ToList();

            if (storedCollection.Any() &&
                storedCollection.FirstOrDefault()?.StoredDate > DateTime.Now.AddDays(-1))
                return storedCollection.Select(n => new Result() { Title = n.Title, FilmwebRating = n.FilmwebRating, FilmwebRatingCount = n.FilmwebRatingCount, ProviderName = n.ProviderName, Year = n.Year });

            var results = func().ToList();

            var entities = results.Select(n => new ResultModel() { Title = n.Title, FilmwebRating = n.FilmwebRating, FilmwebRatingCount = n.FilmwebRatingCount, ProviderName = n.ProviderName, Year = n.Year});
            repository.SaveData(entities);
            return results;
        }
    }
}
