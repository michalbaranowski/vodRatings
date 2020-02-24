using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;
using vod.SignalR.Hub.Hub;

namespace vod.Domain.Services
{
    public class RefreshDataService : IRefreshDataService
    {
        private UpdateNotificationHub _notificationHub;
        private IVodRepositoryBackground _repositoryBackground;
        private IMapper _mapper;
        private readonly INcPlusService _ncPlusService;
        private readonly IFilmwebResultsProvider _filmwebResultsProvider;
        private readonly IRefreshStateService _refreshStateService;
        private const bool SUCCESS_STATE = true;

        public RefreshDataService(
            IRefreshStateService refreshStateService,
            IMapper mapper,
            INcPlusService ncPlusService,
            IFilmwebResultsProvider filmwebResultsProvider,
            IVodRepositoryBackground repositoryBackground,
            UpdateNotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
            _repositoryBackground = repositoryBackground;
            _mapper = mapper;
            _ncPlusService = ncPlusService;
            _filmwebResultsProvider = filmwebResultsProvider;
            _refreshStateService = refreshStateService;
        }

        public bool Refresh(MovieTypes type, Func<IEnumerable<FilmwebResult>> func)
        {
            _notificationHub.NotifyRefreshStarted(type);
            _refreshStateService.SetCurrentRefreshState(type);

            var ncPlusMovies = _ncPlusService.GetMoviesOfType(type).ToList();
            var dbResults = _repositoryBackground.GetResultsOfType((int)type);
            var blackListedMovies = _repositoryBackground.GetBlackListedMovies();

            var moviesToRemove = dbResults.Where(n => ncPlusMovies.Any(p => p.Title == n.Title) == false);

            var ncPlusMoviesToAdd = ncPlusMovies
                .Where(n => dbResults.Any(p => p.Title == n.Title) == false 
                    && blackListedMovies.Any(p=>p.Title == n.Title) == false);

            var moviesToAdd = _filmwebResultsProvider.GetFilmwebResultsByNcPlusResults(ncPlusMoviesToAdd)
                .Select(n => _mapper.Map<MovieEntity>(n))
                .FillStoredDate();

            var moviesToBlackList = ncPlusMoviesToAdd
                .Where(n => moviesToAdd.Any(p => p.Title == n.Title) == false)
                .Select(n => _mapper.Map<BlackListedMovieEntity>(n));

            _repositoryBackground.MarkAsDeleted(moviesToRemove);
            _repositoryBackground.AddMovies(moviesToAdd);
            _repositoryBackground.AddBlackListedMovies(moviesToBlackList);

            _repositoryBackground.LogUpdate((int)type);
            _refreshStateService.RemoveCurrentRefreshState();

            _notificationHub.NotifyUpdate(type, moviesToAdd.Count());

            return SUCCESS_STATE;
        }
    }
}
