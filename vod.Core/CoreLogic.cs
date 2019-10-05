using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AutoMapper;
using vod.Core.Boundary.Interfaces;
using vod.Core.Boundary.Model;
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
            return _storedDataManager.UseStorageIfPossible(() =>
            {
                var ncPlusResult = _ncPlusService.GetMoviesOfType(type).Result;
                var results = ncPlusResult.Select(n => _filmwebService.CheckInFilmweb(n)).Where(n => n != null).ToList();
                return results;
            }).OrderByDescending(n => n.FilmwebRating)
                .Select(n => _mapper.Map<Result>(n)); ;
        }
    }
}
