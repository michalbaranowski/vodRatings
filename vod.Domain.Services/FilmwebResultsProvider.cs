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
        private readonly IFilmwebService _filmwebService;
        private readonly UpdateNotificationHub _notificationHub;

        public FilmwebResultsProvider(
            IFilmwebService filmwebService,
            UpdateNotificationHub notificationHub)
        {
            _filmwebService = filmwebService;
            _notificationHub = notificationHub;
        }

        public IEnumerable<FilmwebResult> GetFilmwebResultsByBaseResults(IEnumerable<Result> results)
        {
            return GetFilmwebResultsByBaseResultsInternal(results)
                .Where(n => n != null)
                .DistinctBy(n => n.Title);
        }

        private IEnumerable<FilmwebResult> GetFilmwebResultsByBaseResultsInternal(IEnumerable<Result> results)
        {
            var result = new List<FilmwebResult>();
            var count = results.Count();

            for (int iteration = 0; iteration < count; iteration++)
            {
                var percent = Math.Round(decimal.Divide(iteration, count) * 100);
                _notificationHub.NotifyRefreshProgress((int)percent);
                result.Add(_filmwebService.GetFilmwebResult(results.ElementAt(iteration)));
            }

            return result;
        }
    }
}
