using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary
{
    public interface IFilmwebResultsProvider
    {
        IEnumerable<FilmwebResult> GetFilmwebResultsByBaseResults(IEnumerable<Result> movies);
    }
}