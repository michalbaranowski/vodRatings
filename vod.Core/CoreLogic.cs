﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;
using vod.Core.Extension;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Core
{
    public class CoreLogic : ICoreLogic
    {
        private readonly IMapper _mapper;
        private readonly IStoredDataManager _storedDataManager;
        private readonly IFilmwebResultsProvider _filmwebResultsProvider;
        private IAlreadyWatchedFilmService _alreadyWatchedFilmService;

        public CoreLogic(
            IMapper mapper,
            IStoredDataManager storedDataManager,
            IFilmwebResultsProvider filmwebResultsProvider,
            IAlreadyWatchedFilmService alreadyWatchedFilmService)
        {
            _mapper = mapper;
            _storedDataManager = storedDataManager;
            _filmwebResultsProvider = filmwebResultsProvider;
            _alreadyWatchedFilmService = alreadyWatchedFilmService;
        }

        public IEnumerable<MovieViewModel> GetResults(MovieTypes type)
        {
            return _filmwebResultsProvider.GetFilmwebResults(type)                
                .Select(n => _mapper.Map<MovieViewModel>(n))
                .OrderByDescending(n => n.FilmwebRating)
                .AddNewFlagIfNeeded();
        }

        public IEnumerable<MovieViewModel> GetResultsUsingStorage(MovieTypes type, string userId)
        {
            var cmd = new UseStorageIfPossibleCommand()
            {
                Type = type,
                UserId = userId
            };

            return _storedDataManager.UseStorageIfPossible(cmd)
                    .Select(n => _mapper.Map<MovieViewModel>(n))
                    .OrderByDescending(n => n.FilmwebRating)
                    .AddNewFlagIfNeeded();
        }
    }
}
