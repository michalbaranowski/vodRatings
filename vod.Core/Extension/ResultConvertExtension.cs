using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Models;

namespace vod.Core.Extension
{
    public static class ResultConvertExtension
    {
        private const int _storedDatesRange = 2;

        public static IEnumerable<Result> AddNewFlagIfNeeded(this IEnumerable<Result> result)
        {
            var myResults = result.ToList();

            if (myResults.Any() == false) return myResults;

            var newestStoredDates = myResults.GroupBy(n => n.StoredDate)
                .OrderByDescending(n => n.Key)
                .Select(n => new DateTime(n.Key.Year, n.Key.Month, n.Key.Day, n.Key.Hour, n.Key.Minute, n.Key.Second))
                .Distinct()
                .Take(_storedDatesRange);

            var newestResults = myResults
                .Where(n => newestStoredDates
                    .Any(p => p.Year == n.StoredDate.Year &&
                            p.Month == n.StoredDate.Month &&
                            p.Day == n.StoredDate.Day &&
                            p.Hour == n.StoredDate.Hour &&
                            p.Minute == n.StoredDate.Minute));

            foreach (var res in myResults)
            {
                res.IsNew = newestResults.Any(n => n.Title == res.Title);
            }

            return myResults;
        }
    }
}
