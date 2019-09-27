using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var ncPlusResult = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller).Result;
            return ncPlusResult.Select(n => _filmwebService.CheckInFilmweb(n)).Where(n=>n!=null);
        }
    }
}
