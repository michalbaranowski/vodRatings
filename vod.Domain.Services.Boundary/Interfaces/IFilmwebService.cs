using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IFilmwebService
    {
        FilmwebResult GetFilmwebResult(FilmResultWithMovieType movie);
    }
}
