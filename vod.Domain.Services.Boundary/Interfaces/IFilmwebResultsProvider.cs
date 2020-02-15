using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary
{
    public interface IFilmwebResultsProvider
    {
        IEnumerable<FilmwebResult> GetFilmwebResults(MovieTypes type);
        IEnumerable<FilmwebResult> GetFilmwebResultsByNcPlusResults(IEnumerable<NcPlusResult> movies);
    }
}