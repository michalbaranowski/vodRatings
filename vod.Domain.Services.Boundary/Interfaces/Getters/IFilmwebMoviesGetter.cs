using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary
{
    public interface IFilmwebMoviesGetter
    {
        IEnumerable<Movie> GetMoviesByTitle(string title);
    }
}
