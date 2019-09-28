using System;
using System.Collections.Generic;
using System.Linq;
using vod.Core.Boundary;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Enums;
using vod.Domain.Services.Boundary.Interfaces;

namespace vod.Core
{
    public class CoreLogic : ICoreLogic
    {
        private readonly IFilmwebService _filmwebService;
        private readonly INcPlusService _ncPlusService;

        public CoreLogic(
            IFilmwebService filmwebService,
            INcPlusService ncPlusService)
        {
            _filmwebService = filmwebService;
            _ncPlusService = ncPlusService;
        }

        public IEnumerable<Result> GetResults()
        {
            if (VodStorage.StorageDate > DateTime.Now.AddDays(-1) 
                && VodStorage.StoredResults != null && VodStorage.StoredResults.Any())
                return VodStorage.StoredResults;

            var ncPlusResult = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller).Result;
            var results = ncPlusResult.Select(n => _filmwebService.CheckInFilmweb(n)).Where(n=>n!=null).ToList();
            VodStorage.Store(results);
            return results;
        }
    }
}
