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
        private readonly IFilmwebResultsProvider _filmwebResultsProvider;
        private const bool SUCCESS_STATE = true;

        public RefreshDataService(
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
        }

        public bool Refresh(MovieTypes type, Func<IEnumerable<FilmwebResult>> func)
        {
            _notificationHub.NotifyRefreshStarted(type);

            var ncPlusMovies = _ncPlusService.GetMoviesOfType(type).ToList();
            var dbResults = _repositoryBackground.GetResultsOfType((int)type);

            var moviesToRemove = dbResults.Where(n => ncPlusMovies.Any(p => p.Title == n.Title) == false);
            var ncPlusMoviesToAdd = ncPlusMovies.Where(n => dbResults.Any(p => p.Title == n.Title) == false);
            var moviesToAdd = _filmwebResultsProvider.GetFilmwebResultsByNcPlusResults(ncPlusMoviesToAdd)
                .Select(n => _mapper.Map<MovieEntity>(n))
                .FillStoredDate();

            _repositoryBackground.MarkAsDeleted(moviesToRemove);
            _repositoryBackground.AddMovies(moviesToAdd);

            _repositoryBackground.LogUpdate((int)type);
            _notificationHub.NotifyUpdate(type);
            return SUCCESS_STATE;
        }
    }
}
