﻿using AutoMapper;
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
        private readonly INetflixService _netflixService;
        private readonly IFilmwebResultsProvider _filmwebResultsProvider;
        private readonly IRefreshStateService _refreshStateService;
        private const bool SUCCESS_STATE = true;

        public RefreshDataService(
            IRefreshStateService refreshStateService,
            IMapper mapper,
            INcPlusService ncPlusService,
            INetflixService netflixService,
            IFilmwebResultsProvider filmwebResultsProvider,
            IVodRepositoryBackground repositoryBackground,
            UpdateNotificationHub notificationHub)
        {
            _notificationHub = notificationHub;
            _repositoryBackground = repositoryBackground;
            _mapper = mapper;
            _ncPlusService = ncPlusService;
            _netflixService = netflixService;
            _filmwebResultsProvider = filmwebResultsProvider;
            _refreshStateService = refreshStateService;
        }

        public bool Refresh(MovieTypes type)
        {
            _notificationHub.NotifyRefreshStarted(type);
            _refreshStateService.SetCurrentRefreshState(type);

            var ncPlusMovies = _ncPlusService.GetMoviesOfType(type).ToList();
            var netflixMovies = _netflixService.GetMoviesOfType(type).ToList();
            var foundMovies = ncPlusMovies.Select(n => n as Result).Concat(netflixMovies);

            var dbResults = _repositoryBackground.GetResultsOfType((int)type);
            var blackListedMovies = _repositoryBackground.GetBlackListedMovies();

            var moviesToRemove = dbResults.Where(n => foundMovies.Any(p => p.Title == n.Title) == false);

            var resultMoviesToAdd = foundMovies
                .Where(n => dbResults.Any(p => p.Title == n.Title) == false 
                    && blackListedMovies.Any(p=>p.Title == n.Title) == false);

            var moviesToAdd = _filmwebResultsProvider.GetFilmwebResultsByBaseResults(resultMoviesToAdd)
                .Select(n => _mapper.Map<MovieEntity>(n))
                .FillStoredDate();

            var moviesToBlackList = resultMoviesToAdd
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
