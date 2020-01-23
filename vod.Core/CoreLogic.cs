using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MoreLinq;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;
using vod.Core.Extension;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Core
{
    public class CoreLogic : ICoreLogic
    {
        private readonly IMapper _mapper;
        private readonly IStoredDataManager _storedDataManager;
        private readonly IFilmwebResultsProvider _filmwebResultsProvider;

        public CoreLogic(
            IMapper mapper,
            IStoredDataManager storedDataManager,
            IFilmwebResultsProvider filmwebResultsProvider)
        {
            _mapper = mapper;
            _storedDataManager = storedDataManager;
            _filmwebResultsProvider = filmwebResultsProvider;
        }

        public IEnumerable<Result> GetResults(MovieTypes type)
        {
            return _filmwebResultsProvider.GetFilmwebResults(type)                
                .Select(n => _mapper.Map<Result>(n))
                .OrderByDescending(n => n.FilmwebRating)
                .AddNewFlagIfNeeded();
        }

        public IEnumerable<Result> GetResultsUsingStorage(MovieTypes type)
        {
            var cmd = new UseStorageIfPossibleCommand()
            {
                Type = type,
                Func = () => _filmwebResultsProvider.GetFilmwebResults(type)
            };

            return _storedDataManager.UseStorageIfPossible(cmd)
                    .Select(n => _mapper.Map<Result>(n))
                    .OrderByDescending(n => n.FilmwebRating)
                    .AddNewFlagIfNeeded();
        }
    }
}
