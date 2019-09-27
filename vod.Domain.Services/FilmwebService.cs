using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services
{
    public class FilmwebService : IFilmwebService, IFilmwebMoviesGetter
    {
        public IEnumerable<Movie> GetMoviesByTitle(string title)
        {
            throw new NotImplementedException();
        }
    }
}
