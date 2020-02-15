using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services
{
    public class FilmwebResultsProvider : IFilmwebResultsProvider
    {
        private readonly INcPlusService _ncPlusService;
        private readonly IFilmwebService _filmwebService;

        public FilmwebResultsProvider(INcPlusService ncPlusService, IFilmwebService filmwebService)
        {
            _ncPlusService = ncPlusService;
            _filmwebService = filmwebService;
        }

        public IEnumerable<FilmwebResult> GetFilmwebResults(MovieTypes type)
        {
            var ncPlusResult = _ncPlusService.GetMoviesOfType(type);
            return GetFilmwebResultsByNcPlusResults(ncPlusResult);
        }

        public IEnumerable<FilmwebResult> GetFilmwebResultsByNcPlusResults(IEnumerable<NcPlusResult> movies)
        {
            var results = movies
                .Select(n => _filmwebService.GetFilmwebResult(n))
                .Where(n => n != null)
                .ToList();
            return results.DistinctBy(n => n.Title);
        }
    }
}
