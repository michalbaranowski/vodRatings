using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.SignalR.Hub.Hub;

namespace vod.Domain.Services
{
    public class FilmwebResultsProvider : IFilmwebResultsProvider
    {
        private readonly INcPlusService _ncPlusService;
        private readonly IFilmwebService _filmwebService;
        private readonly UpdateNotificationHub _notificationHub;

        public FilmwebResultsProvider(
            INcPlusService ncPlusService,
            IFilmwebService filmwebService,
            UpdateNotificationHub notificationHub)
        {
            _ncPlusService = ncPlusService;
            _filmwebService = filmwebService;
            _notificationHub = notificationHub;
        }

        public IEnumerable<FilmwebResult> GetFilmwebResults(MovieTypes type)
        {
            var ncPlusResult = _ncPlusService.GetMoviesOfType(type);
            return GetFilmwebResultsByNcPlusResults(ncPlusResult);
        }

        public IEnumerable<FilmwebResult> GetFilmwebResultsByNcPlusResults(IEnumerable<NcPlusResult> movies)
        {
            return GetFilmwebResults(movies)
                .Where(n => n != null)
                .DistinctBy(n => n.Title);
        }

        private IEnumerable<FilmwebResult> GetFilmwebResults(IEnumerable<NcPlusResult> ncPlusResults)
        {
            var result = new List<FilmwebResult>();
            var count = ncPlusResults.Count();

            for (int iteration = 0; iteration < count; iteration++)
            {
                var percent = Math.Round(decimal.Divide(iteration, count) * 100);
                _notificationHub.NotifyRefreshProgress((int)percent);
                result.Add(_filmwebService.GetFilmwebResult(ncPlusResults.ElementAt(iteration)));
            }

            return result;
        }
    }
}
