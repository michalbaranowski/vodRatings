using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MoreLinq;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;
using vod.Core.Extension;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Core
{
    public class CoreLogic : ICoreLogic
    {
        private readonly IFilmwebService _filmwebService;
        private readonly INcPlusService _ncPlusService;
        private readonly IMapper _mapper;
        private readonly IStoredDataManager _storedDataManager;

        public CoreLogic(
            IFilmwebService filmwebService,
            INcPlusService ncPlusService,
            IMapper mapper,
            IStoredDataManager storedDataManager)
        {
            _filmwebService = filmwebService;
            _ncPlusService = ncPlusService;
            _mapper = mapper;
            _storedDataManager = storedDataManager;
        }

        public IEnumerable<Result> GetResults(MovieTypes type)
        {
            return GetFilmwebResults(type)
                .OrderByDescending(n => n.FilmwebRating)
                .Select(n => _mapper.Map<Result>(n));
        }

        public IEnumerable<Result> GetResultsUsingStorage(MovieTypes type)
        {
            return _storedDataManager.UseStorageIfPossible(
                    type, 
                    () => GetFilmwebResults(type))
                .OrderByDescending(n => n.FilmwebRating)
                .Select(n => _mapper.Map<Result>(n))
                .ToList()
                .AddNewFlagIfNeeded();
        }

        private IEnumerable<FilmwebResult> GetFilmwebResults(MovieTypes type)
        {
            var ncPlusResult = _ncPlusService.GetMoviesOfType(type);
            var results = ncPlusResult
                .Select(n => _filmwebService.CheckInFilmweb(n, type))
                .Where(n => n != null).ToList();
            return results.DistinctBy(n => n.Title);
        }
    }
}
