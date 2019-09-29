using System.Collections.Generic;
using System.Linq;
using vod.Core.Boundary;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Repository.Boundary;
using static vod.Core.StoredDataManager;

namespace vod.Core
{
    public class CoreLogic : ICoreLogic
    {
        private readonly IFilmwebService _filmwebService;
        private readonly INcPlusService _ncPlusService;
        private readonly IVodRepository _repository;

        public CoreLogic(
            IFilmwebService filmwebService,
            INcPlusService ncPlusService,
            IVodRepository repository)
        {
            _filmwebService = filmwebService;
            _ncPlusService = ncPlusService;
            _repository = repository;
        }

        public IEnumerable<Result> GetResults()
        {
            return UseStorageIfPossible(_repository, () =>
            {
                var ncPlusResult = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller).Result;
                var results = ncPlusResult.Select(n => _filmwebService.CheckInFilmweb(n)).Where(n => n != null).ToList();
                return results;
            });
        }
    }
}
